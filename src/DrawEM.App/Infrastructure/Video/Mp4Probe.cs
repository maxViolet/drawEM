using System.Buffers.Binary;
using System.IO;
using System.Text;
using DrawEM.App.Application.Video;

namespace DrawEM.App.Infrastructure.Video;

/// <summary>
/// Reads the ISO base media (MP4) headers that decide playback support: the <c>ftyp</c> brand and, from
/// <c>moov</c>, each video and audio track's sample entry, H.264 <c>avcC</c>, AAC <c>esds</c>, media
/// timescale/duration, and sample count. It seeks past media data, so the work is bounded by the number of
/// top-level boxes and the size of <c>moov</c>, not by the file size. It decodes no samples.
/// </summary>
internal static class Mp4Probe
{
    /// <summary>A two-hour 60 fps movie header is a few MiB; anything this large is not a file we play.</summary>
    private const int MaxMovieHeaderBytes = 64 * 1024 * 1024;

    private const int MaxTopLevelBoxes = 4096;

    /// <exception cref="Mp4FormatException">The stream is not an MP4 or its headers are damaged.</exception>
    /// <exception cref="IOException">The stream cannot be read.</exception>
    public static Mp4MediaInfo Read(Stream stream)
    {
        var length = stream.Length;
        long position = 0;
        var boxes = 0;
        string? brand = null;
        byte[]? movie = null;
        Span<byte> header = stackalloc byte[16];

        while (position < length)
        {
            if (++boxes > MaxTopLevelBoxes)
            {
                throw Corrupt($"it has more than {MaxTopLevelBoxes} top-level boxes.");
            }

            if (length - position < 8)
            {
                throw brand is null ? NotMp4() : Corrupt($"it ends inside a box header at byte {position}.");
            }

            stream.Position = position;
            stream.ReadExactly(header[..8]);
            long size = BinaryPrimitives.ReadUInt32BigEndian(header);
            var type = FourCc(header[4..8]);
            long headerSize = 8;

            // Checked before the size: a text file's first bytes are an arbitrary size and type.
            if (boxes == 1 && type != "ftyp")
            {
                throw NotMp4();
            }

            if (size == 1)
            {
                if (length - position < 16)
                {
                    throw Corrupt($"it ends inside the '{type}' box header.");
                }

                stream.ReadExactly(header[8..16]);
                var large = BinaryPrimitives.ReadUInt64BigEndian(header[8..16]);
                size = large > long.MaxValue ? long.MaxValue : (long)large;
                headerSize = 16;
            }
            else if (size == 0)
            {
                size = length - position;
            }

            if (size < headerSize)
            {
                throw Corrupt($"the '{type}' box has an invalid size.");
            }

            if (size > length - position)
            {
                throw Corrupt($"it ends inside the '{type}' box.");
            }

            var bodySize = size - headerSize;
            if (type == "ftyp")
            {
                if (bodySize < 4)
                {
                    throw Corrupt("the 'ftyp' box is too short.");
                }

                // The box header is already decoded, so its buffer holds the major brand.
                stream.ReadExactly(header[..4]);
                brand = FourCc(header[..4]);
            }
            else if (type == "moov")
            {
                if (movie is not null)
                {
                    throw Corrupt("it has two movie headers ('moov').");
                }

                if (bodySize > MaxMovieHeaderBytes)
                {
                    throw new Mp4FormatException(VideoValidationFailure.UnsupportedContainer,
                        $"The MP4 movie header is larger than {MaxMovieHeaderBytes / (1024 * 1024)} MiB.");
                }

                movie = new byte[bodySize];
                stream.ReadExactly(movie);
            }

            position += size;
        }

        if (brand is null)
        {
            throw NotMp4();
        }

        if (movie is null)
        {
            throw Corrupt("it has no movie header ('moov').");
        }

        return ReadMovie(brand, movie);
    }

    private static Mp4MediaInfo ReadMovie(string brand, byte[] movie)
    {
        var fragmented = false;
        var video = new List<Mp4VideoTrack>();
        var audio = new List<Mp4AudioTrack>();
        foreach (var box in Children(movie, "moov"))
        {
            if (box.Type == "mvex")
            {
                fragmented = true;
            }
            else if (box.Type == "trak")
            {
                ReadTrack(box.Body(movie), video, audio);
            }
        }

        return new Mp4MediaInfo(brand, fragmented, video, audio);
    }

    private static void ReadTrack(ReadOnlySpan<byte> track, List<Mp4VideoTrack> video, List<Mp4AudioTrack> audio)
    {
        var media = Required(track, "trak", "mdia");
        var handler = Required(media, "mdia", "hdlr");
        Need(handler, 12, "hdlr");
        var kind = FourCc(handler[8..12]);
        if (kind is not ("vide" or "soun"))
        {
            return;
        }

        var (timescale, duration) = MediaTime(Required(media, "mdia", "mdhd"));
        var table = Required(Required(media, "mdia", "minf"), "minf", "stbl");
        var description = Required(table, "stbl", "stsd");
        Need(description, 8, "stsd");
        var entries = Children(description[8..], "stsd");
        var trackKind = kind == "vide" ? "video" : "audio";
        if (entries.Count == 0)
        {
            throw Corrupt($"a {trackKind} track has no sample description.");
        }

        // Samples may switch to any listed description, and only one is checked against the profile.
        if (entries.Count > 1)
        {
            throw new Mp4FormatException(VideoValidationFailure.UnsupportedContainer,
                $"A {trackKind} track has {entries.Count} sample descriptions; drawEM plays tracks with one.");
        }

        var entry = entries[0];
        var entryBody = entry.Body(description[8..]);
        if (kind == "vide")
        {
            var samples = SampleCount(table);
            var frameRate = samples > 0 && duration > 0 && timescale > 0 ? samples * (double)timescale / duration : 0;
            video.Add(ReadVisualEntry(entry.Type, entryBody, frameRate));
        }
        else
        {
            audio.Add(ReadAudioEntry(entry.Type, entryBody));
        }
    }

    private static Mp4VideoTrack ReadVisualEntry(string type, ReadOnlySpan<byte> entry, double frameRate)
    {
        // VisualSampleEntry: 8 bytes SampleEntry, 16 reserved, width and height, then 50 more bytes; boxes from 78.
        Need(entry, 78, type);
        var width = BinaryPrimitives.ReadUInt16BigEndian(entry[24..]);
        var height = BinaryPrimitives.ReadUInt16BigEndian(entry[26..]);
        AvcConfiguration? avc = null;
        if (TryFind(entry[78..], type, "avcC", out var config))
        {
            Need(config, 4, "avcC");
            avc = new AvcConfiguration(config[1], config[2], config[3]);
        }

        return new Mp4VideoTrack(type, width, height, frameRate, avc);
    }

    private static Mp4AudioTrack ReadAudioEntry(string type, ReadOnlySpan<byte> entry)
    {
        // AudioSampleEntry: 8 bytes SampleEntry, version, revision, vendor, channels at 16, sample size,
        // compression ID, packet size, 16.16 sample rate at 24; boxes from 28. QuickTime version 1 adds 16
        // bytes; version 2 adds 36 and moves the rate and channel count into them.
        Need(entry, 28, type);
        var version = BinaryPrimitives.ReadUInt16BigEndian(entry[8..]);
        int channels = BinaryPrimitives.ReadUInt16BigEndian(entry[16..]);
        var sampleRate = (int)(BinaryPrimitives.ReadUInt32BigEndian(entry[24..]) >> 16);
        var boxesAt = 28;
        if (version == 1)
        {
            boxesAt = 44;
        }
        else if (version == 2)
        {
            Need(entry, 64, type);
            sampleRate = (int)Math.Round(BinaryPrimitives.ReadDoubleBigEndian(entry[32..]));
            channels = (int)BinaryPrimitives.ReadUInt32BigEndian(entry[40..]);
            boxesAt = 64;
        }

        Need(entry, boxesAt, type);
        var boxes = entry[boxesAt..];

        // QuickTime-style files may nest esds inside a 'wave' box.
        int? objectType = null;
        int? audioObjectType = null;
        if (TryFind(boxes, type, "esds", out var esds)
            || (TryFind(boxes, type, "wave", out var wave) && TryFind(wave, "wave", "esds", out esds)))
        {
            (objectType, audioObjectType) = ReadEsds(esds);
        }

        return new Mp4AudioTrack(type, channels, sampleRate, objectType, audioObjectType);
    }

    /// <summary>ES_Descriptor → DecoderConfigDescriptor → DecoderSpecificInfo (ISO/IEC 14496-1 and -3).</summary>
    private static (int ObjectType, int? AudioObjectType) ReadEsds(ReadOnlySpan<byte> esds)
    {
        Need(esds, 4, "esds");
        var data = esds[4..];
        var at = 0;
        if (Descriptor(data, ref at) != 0x03)
        {
            throw Corrupt("the 'esds' box has no ES descriptor.");
        }

        Need(data, at + 3, "esds");
        var flags = data[at + 2];
        at += 3;
        if ((flags & 0x80) != 0)
        {
            at += 2;
        }

        if ((flags & 0x40) != 0)
        {
            Need(data, at + 1, "esds");
            at += 1 + data[at];
        }

        if ((flags & 0x20) != 0)
        {
            at += 2;
        }

        if (Descriptor(data, ref at) != 0x04)
        {
            throw Corrupt("the 'esds' box has no decoder configuration.");
        }

        Need(data, at + 13, "esds");
        int objectType = data[at];
        at += 13;
        if (at >= data.Length || Descriptor(data, ref at) != 0x05)
        {
            return (objectType, null);
        }

        Need(data, at + 1, "esds");
        var audioObjectType = data[at] >> 3;
        if (audioObjectType == 31)
        {
            Need(data, at + 2, "esds");
            audioObjectType = 32 + (((data[at] & 0x07) << 3) | (data[at + 1] >> 5));
        }

        return (objectType, audioObjectType);
    }

    /// <summary>Reads a descriptor tag and its length, leaving <paramref name="at"/> on the payload.</summary>
    private static int Descriptor(ReadOnlySpan<byte> data, ref int at)
    {
        Need(data, at + 2, "esds");
        int tag = data[at++];
        for (var i = 0; i < 4; i++)
        {
            Need(data, at + 1, "esds");
            if ((data[at++] & 0x80) == 0)
            {
                break;
            }
        }

        return tag;
    }

    private static (uint Timescale, ulong Duration) MediaTime(ReadOnlySpan<byte> header)
    {
        Need(header, 4, "mdhd");
        if (header[0] == 1)
        {
            Need(header, 32, "mdhd");
            return (BinaryPrimitives.ReadUInt32BigEndian(header[20..]), BinaryPrimitives.ReadUInt64BigEndian(header[24..]));
        }

        Need(header, 20, "mdhd");
        return (BinaryPrimitives.ReadUInt32BigEndian(header[12..]), BinaryPrimitives.ReadUInt32BigEndian(header[16..]));
    }

    /// <summary>The <c>stsz</c> or <c>stz2</c> sample count; both keep it at byte 8 of the body.</summary>
    private static uint SampleCount(ReadOnlySpan<byte> table)
    {
        if (!TryFind(table, "stbl", "stsz", out var body) && !TryFind(table, "stbl", "stz2", out body))
        {
            return 0;
        }

        Need(body, 12, "stsz");
        return BinaryPrimitives.ReadUInt32BigEndian(body[8..]);
    }

    private static ReadOnlySpan<byte> Required(ReadOnlySpan<byte> parent, string parentType, string type) =>
        TryFind(parent, parentType, type, out var body) ? body : throw Corrupt($"a '{parentType}' box has no '{type}' box.");

    /// <summary>The body of the first <paramref name="type"/> child of <paramref name="parent"/>.</summary>
    private static bool TryFind(ReadOnlySpan<byte> parent, string parentType, string type, out ReadOnlySpan<byte> body)
    {
        foreach (var box in Children(parent, parentType))
        {
            if (box.Type == type)
            {
                body = box.Body(parent);
                return true;
            }
        }

        body = default;
        return false;
    }

    private static List<Box> Children(ReadOnlySpan<byte> parent, string parentType)
    {
        var boxes = new List<Box>();
        var at = 0;
        while (at < parent.Length)
        {
            if (parent.Length - at < 8)
            {
                throw Corrupt($"a '{parentType}' box ends inside a child box header.");
            }

            long size = BinaryPrimitives.ReadUInt32BigEndian(parent[at..]);
            var type = FourCc(parent.Slice(at + 4, 4));
            var headerSize = 8;
            if (size == 1)
            {
                Need(parent, at + 16, parentType);
                var large = BinaryPrimitives.ReadUInt64BigEndian(parent[(at + 8)..]);
                size = large > int.MaxValue ? long.MaxValue : (long)large;
                headerSize = 16;
            }
            else if (size == 0)
            {
                size = parent.Length - at;
            }

            if (size < headerSize || size > parent.Length - at)
            {
                throw Corrupt($"the '{type}' box inside '{parentType}' has an invalid size.");
            }

            boxes.Add(new Box(type, at + headerSize, (int)size - headerSize));
            at += (int)size;
        }

        return boxes;
    }

    private static void Need(ReadOnlySpan<byte> data, int bytes, string type)
    {
        if (data.Length < bytes)
        {
            throw Corrupt($"the '{type}' box is too short.");
        }
    }

    private static string FourCc(ReadOnlySpan<byte> bytes) => Encoding.Latin1.GetString(bytes);

    private static Mp4FormatException NotMp4() =>
        new(VideoValidationFailure.NotMp4, "The file is not an MP4 video.");

    private static Mp4FormatException Corrupt(string detail) =>
        new(VideoValidationFailure.Corrupt, "The MP4 file is damaged or incomplete: " + detail);

    private readonly record struct Box(string Type, int Offset, int Length)
    {
        public ReadOnlySpan<byte> Body(ReadOnlySpan<byte> parent) => parent.Slice(Offset, Length);
    }
}

/// <summary>An MP4 header is missing, damaged, or outside what the probe reads.</summary>
internal sealed class Mp4FormatException : Exception
{
    public Mp4FormatException(VideoValidationFailure failure, string message)
        : base(message)
    {
        Failure = failure;
    }

    public VideoValidationFailure Failure { get; }
}
