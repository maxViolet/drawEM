using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>The data cannot become a settings snapshot. The message is shown to the user.</summary>
public sealed class SettingsFormatException : Exception
{
    public SettingsFormatException(string message)
        : base(message)
    {
    }

    public SettingsFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Reads and writes the settings file schema described in <c>README.md</c>. Reading accepts schema versions
/// 1 through <see cref="CurrentSchemaVersion"/>; writing always produces <see cref="CurrentSchemaVersion"/>.
/// </summary>
public static class SettingsJson
{
    public const int CurrentSchemaVersion = 4;

    public static byte[] Serialize(SettingsSnapshot snapshot)
    {
        using var buffer = new MemoryStream();
        using (var json = new Utf8JsonWriter(buffer, new JsonWriterOptions
        {
            Indented = true,
            // The file is not embedded in HTML; keep '+' and non-ASCII file names readable.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }))
        {
            json.WriteStartObject();
            json.WriteNumber("schemaVersion", CurrentSchemaVersion);
            WriteDrawing(json, snapshot);
            json.WriteStartArray("slots");
            foreach (var slot in snapshot.Slots)
            {
                WriteSlot(json, slot);
            }

            json.WriteEndArray();
            json.WriteEndObject();
        }

        return buffer.ToArray();
    }

    private static void WriteDrawing(Utf8JsonWriter json, SettingsSnapshot snapshot)
    {
        json.WriteStartObject("drawing");
        json.WriteString("color", snapshot.Style.Color.ToString());
        json.WriteNumber("width", snapshot.Style.Width.Pixels);
        json.WriteString("smoothing", snapshot.Style.Smoothing == StrokeSmoothing.None ? "none" : "bezier");
        json.WriteString("drawShortcut", snapshot.DrawShortcut.ToString());
        json.WriteString("clearShortcut", snapshot.ClearShortcut.ToString());
        json.WriteEndObject();
    }

    private static void WriteSlot(Utf8JsonWriter json, ActionSlot slot)
    {
        json.WriteStartObject();
        json.WriteNumber("slot", slot.Number);
        switch (slot.Action)
        {
            case null:
                json.WriteNull("action");
                break;
            case SoundAction sound:
                WriteSound(json, sound);
                break;
            case EffectAction effect:
                WriteEffect(json, effect);
                break;
            default:
                throw new NotSupportedException($"Action type {slot.Action.GetType().Name} has no schema.");
        }

        json.WriteEndObject();
    }

    private static void WriteSound(Utf8JsonWriter json, SoundAction sound)
    {
        json.WriteStartObject("action");
        json.WriteString("type", "sound");
        json.WriteString("file", sound.Sound.LibraryFileName);
        json.WriteString("name", sound.Sound.DisplayName);
        json.WriteString("shortcut", sound.Shortcut!.ToString());
        json.WriteEndObject();
    }

    private static void WriteEffect(Utf8JsonWriter json, EffectAction effect)
    {
        json.WriteStartObject("action");
        json.WriteString("type", "effect");
        json.WriteString("effectId", effect.Effect.Value);
        json.WriteString("shortcut", effect.Shortcut!.ToString());
        json.WriteBoolean("loop", effect.Loop);
        if (effect.MaxLength is { } maxLength)
        {
            json.WriteNumber("maxSeconds", (int)maxLength.TotalSeconds);
        }
        else
        {
            json.WriteNull("maxSeconds");
        }
        json.WriteEndObject();
    }

    /// <exception cref="SettingsFormatException">
    /// The data is not JSON, has an unsupported schema version, or does not describe valid settings.
    /// </exception>
    public static SettingsSnapshot Deserialize(ReadOnlyMemory<byte> data) => Deserialize(data, out _);

    /// <summary>
    /// <c>true</c> when the data is a schema 1 file that passes every check <see cref="Deserialize(ReadOnlyMemory{byte})"/>
    /// makes, so a drawEM that reads only schema 1 can load it.
    /// </summary>
    public static bool IsValidSchema1(ReadOnlyMemory<byte> data)
    {
        try
        {
            Deserialize(data, out var version);
            return version == 1;
        }
        catch (SettingsFormatException)
        {
            return false;
        }
    }

    private static SettingsSnapshot Deserialize(ReadOnlyMemory<byte> data, out int version)
    {
        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(data);
        }
        catch (JsonException failure)
        {
            throw new SettingsFormatException("Settings file is not valid JSON.", failure);
        }

        using (document)
        {
            var root = Object(document.RootElement, "settings");
            version = ReadVersion(root);
            var drawing = Object(Property(root, "drawing"), "drawing");
            var style = ReadStyle(drawing, version);
            var draw = ShortcutProperty(drawing, "drawShortcut");
            var clear = ShortcutProperty(drawing, "clearShortcut");
            var slots = ReadSlots(root, version);
            var validation = SettingsSnapshot.Validate(style, draw, clear, slots);
            return validation.Snapshot ?? throw new SettingsFormatException(
                "Settings are invalid: " + string.Join(" ", validation.Errors));
        }
    }

    private static int ReadVersion(JsonElement root)
    {
        var version = Int(root, "schemaVersion");
        return version is < 1 or > CurrentSchemaVersion
            ? throw new SettingsFormatException(
                $"Settings schema version {version} is not supported; this drawEM reads versions 1–{CurrentSchemaVersion}.")
            : version;
    }

    private static DrawingStyle ReadStyle(JsonElement drawing, int version)
    {
        var colorText = String(drawing, "color");
        if (!HexColor.TryParse(colorText, out var color))
        {
            throw new SettingsFormatException($"drawing.color '{colorText}' is not a #RRGGBB color.");
        }

        var width = Int(drawing, "width");
        if (!StrokeWidth.IsValid(width))
        {
            throw new SettingsFormatException(
                $"drawing.width {width} is outside {StrokeWidth.Min}–{StrokeWidth.Max}.");
        }

        return new DrawingStyle(color.Value, new StrokeWidth(width), ReadSmoothing(drawing, version));
    }

    /// <summary>Files before schema 4 have no smoothing setting and draw with Bezier smoothing.</summary>
    private static StrokeSmoothing ReadSmoothing(JsonElement drawing, int version) =>
        version < 4
            ? StrokeSmoothing.Bezier
            : String(drawing, "smoothing") switch
            {
                "none" => StrokeSmoothing.None,
                "bezier" => StrokeSmoothing.Bezier,
                var value => throw new SettingsFormatException($"drawing.smoothing '{value}' is not supported."),
            };

    private static ActionSlot[] ReadSlots(JsonElement root, int version)
    {
        var slotsElement = Property(root, "slots");
        if (slotsElement.ValueKind != JsonValueKind.Array)
        {
            throw new SettingsFormatException("slots must be an array.");
        }

        return slotsElement.EnumerateArray().Select(element => ReadSlot(element, version)).ToArray();
    }

    private static ActionSlot ReadSlot(JsonElement element, int version)
    {
        var slot = Object(element, "slot entry");
        var number = ReadSlotNumber(slot);
        var action = Property(slot, "action");
        if (action.ValueKind == JsonValueKind.Null)
        {
            return ActionSlot.Empty(number);
        }

        Object(action, $"slot {number} action");
        return new ActionSlot(number, ReadAction(action, number, version));
    }

    private static int ReadSlotNumber(JsonElement slot)
    {
        var number = Int(slot, "slot");
        return number is < 1 or > ActionSlot.Count
            ? throw new SettingsFormatException($"Slot number {number} is outside 1–{ActionSlot.Count}.")
            : number;
    }

    private static SlotAction ReadAction(JsonElement action, int number, int version)
    {
        var type = String(action, "type");
        return type switch
        {
            "sound" => ReadSound(action, number),
            "effect" when version >= 2 => ReadEffect(action, number, version),
            "effect" => throw new SettingsFormatException(
                $"Slot {number} action type 'effect' is not supported in schema version {version}."),
            _ => throw new SettingsFormatException($"Slot {number} action type '{type}' is not supported."),
        };
    }

    private static SoundAction ReadSound(JsonElement action, int number)
    {
        SoundReference sound;
        try
        {
            sound = new SoundReference(String(action, "file"), String(action, "name"));
        }
        catch (ArgumentException failure)
        {
            throw new SettingsFormatException($"Slot {number} sound is invalid: {failure.Message}", failure);
        }

        return new SoundAction(sound, ShortcutProperty(action, "shortcut"));
    }

    /// <summary>An ID missing from the catalog is reported by <see cref="SettingsSnapshot.Validate"/>.</summary>
    private static EffectAction ReadEffect(JsonElement action, int number, int version)
    {
        EffectId effect;
        try
        {
            effect = new EffectId(String(action, "effectId"));
        }
        catch (ArgumentException failure)
        {
            throw new SettingsFormatException($"Slot {number} effect is invalid: {failure.Message}", failure);
        }

        var shortcut = ShortcutProperty(action, "shortcut");
        return version < 3
            ? new EffectAction(effect, shortcut)
            : new EffectAction(effect, shortcut, Boolean(action, "loop"), ReadMaxLength(action, number));
    }

    /// <summary><c>null</c> means no limit.</summary>
    private static TimeSpan? ReadMaxLength(JsonElement action, int number)
    {
        if (Property(action, "maxSeconds").ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var seconds = Int(action, "maxSeconds");
        return seconds is < EffectAction.MinSeconds or > EffectAction.MaxSeconds
            ? throw new SettingsFormatException(
                $"Slot {number} effect maxSeconds {seconds} is outside " +
                $"{EffectAction.MinSeconds}–{EffectAction.MaxSeconds}.")
            : TimeSpan.FromSeconds(seconds);
    }

    private static Shortcut ShortcutProperty(JsonElement element, string name)
    {
        var text = String(element, name);
        return Shortcut.TryParse(text, out var shortcut, out var error)
            ? shortcut
            : throw new SettingsFormatException($"{name} '{text}' is not a valid shortcut ({error}).");
    }

    private static JsonElement Object(JsonElement element, string name) =>
        element.ValueKind == JsonValueKind.Object
            ? element
            : throw new SettingsFormatException($"{name} must be an object.");

    private static JsonElement Property(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value)
            ? value
            : throw new SettingsFormatException($"Missing property '{name}'.");

    private static string String(JsonElement element, string name)
    {
        var value = Property(element, name);
        if (value.ValueKind != JsonValueKind.String)
        {
            throw new SettingsFormatException($"'{name}' must be a string.");
        }

        try
        {
            return value.GetString()!;
        }
        catch (InvalidOperationException failure)
        {
            throw new SettingsFormatException($"'{name}' is not valid UTF-8.", failure);
        }
    }

    private static int Int(JsonElement element, string name)
    {
        var value = Property(element, name);
        return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number)
            ? number
            : throw new SettingsFormatException($"'{name}' must be an integer.");
    }

    private static bool Boolean(JsonElement element, string name)
    {
        var value = Property(element, name);
        return value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => throw new SettingsFormatException($"'{name}' must be a boolean."),
        };
    }
}
