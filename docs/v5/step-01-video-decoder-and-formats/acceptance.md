# v5 / S5-01: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [x] [gate] The target Windows playback path is named, with the tested OS
  edition/build and app build identity recorded.
- [x] [gate] Every supported MP4 codec profile has a checked-in fixture that
  opens, renders video, plays embedded audio when present, reaches natural
  end-of-file, and stops/disposes. A silent MP4 fixture is supported and
  covered.
- [x] [gate] The committed fixture generator, exact FFmpeg version, and
  generation commands reproduce the recorded fixture set; tests run from the
  checked-in fixture files without FFmpeg installed.
- [x] [gate] At least one unsupported or corrupt MP4 fixture is rejected with
  a diagnostic identifying the failure class and, when available, codec or
  container; this gate checks decoder validation only.
- [x] [gate] A forced playback-open/decode failure after successful validation
  releases partial player resources and does not crash the tray application.
- [x] [gate] Validation of malformed and unsupported fixtures completes
  within 30 seconds per file on the recorded target Windows machine. S5-01
  creates no managed import or Settings draft.
- [x] [record] Record fixture names, hashes, container, video/audio codecs,
  generator version and commands, playback path, Windows version/build, app
  build identity, and observed limitations.

## Evidence

Recorded 2026-10-23 on the development PC.

### Environment and playback path

| Item | Value |
|---|---|
| Playback path | WPF `System.Windows.Media.MediaPlayer`, wrapped by `Infrastructure/Video/MediaVideoPlayer`. LibVLCSharp was not compared: `MediaPlayer` passed every supported fixture and reports failures with an HRESULT. |
| Windows | Windows 11 Pro 26H2, build 26300.9711 (x64) |
| Optional codec packages present | HEVC Video Extension 2.5.33.0, AV1 Video Extension 2.0.35.0, VP9 Video Extensions 1.2.20.0, Web Media Extensions 2.1.51.0, MPEG-2 Video Extension 1.2.32.0 |
| App build | `DrawEM.App` 1.0.0+c469e18d3362dad8530d283ec0fccd98357ca8ef, Release, built by `tools/VideoPlaybackSpike` (not a published artifact) |
| Audio endpoint | AirPods Pro (Bluetooth headphones), volume 46%, not muted |
| .NET | SDK 10.0.401 building `net8.0-windows` |

### Supported profile

`Application/Video/VideoSupportPolicy` accepts exactly this; every limit is the
edge of the tested matrix:

- **Container:** non-fragmented ISO MP4; QuickTime (`qt  `) brand rejected.
- **Video:** exactly one H.264 `avc1` track, Constrained Baseline (profile 66
  with constraint_set1), Main (77), or High (100), so 8-bit 4:2:0; at most
  1920×1080 (landscape) and 60 fps.
- **Audio:** none, or exactly one AAC-LC `mp4a` track (objectType 0x40,
  audioObjectType 2), mono or stereo, 44.1 or 48 kHz.

Rejected although it played on this PC: HEVC and Opus depend on optional
Store codec packages that this PC has and other PCs may not; fragmented MP4
reported 1.08 s for a 2 s file.

### Fixtures and generator

The fixture table with codecs and expected results is in
[`tests/DrawEM.Tests/Fixtures/Video/README.md`](../../../tests/DrawEM.Tests/Fixtures/Video/README.md);
full hashes are in [`SHA256SUMS.txt`](../../../tests/DrawEM.Tests/Fixtures/Video/SHA256SUMS.txt).
[`tools/video-fixtures/New-VideoFixtures.ps1`](../../../tools/video-fixtures/New-VideoFixtures.ps1)
holds the exact FFmpeg arguments and pins FFmpeg `7.1.1-essentials_build-www.gyan.dev`
(archive SHA-256 `04861d3339c5ebe38b56c19a15cf2c0cc97f5de4fa8910e4d47e5e6404e4a2d4`).

- `New-VideoFixtures.ps1 -Verify` → `Regenerated fixtures match …SHA256SUMS.txt.`
  Run twice; both bit-identical.
- `ffmpeg` is not on `PATH` on this PC (`where ffmpeg` finds nothing); the
  test project copies the checked-in files to its output.

### Automated tests

`dotnet test DrawEM.sln -c Release` → 967 passed, 0 failed. Video tests:

| Test class | Covers |
|---|---|
| `Mp4VideoValidatorTests` | Five supported fixtures accepted with codec, profile, size, frame rate, audio; seven rejected fixtures return the expected `VideoValidationFailure` and name the codec or cause, each under 30 s; missing file → `Unreadable`; file held open by a writer is still validated |
| `VideoSupportPolicyTests` | Size, frame-rate, H.264 profile, channel and sample-rate edges; other video/audio codecs named; track counts; QuickTime brand; fragmented movie |
| `MediaVideoPlayerTests` (`Category=WindowsMedia`) | Five supported fixtures open, report tracks and size, reach natural end, and release the file after `Dispose`; forced open failure; file removed after validation; dispose during playback raises nothing later |

Before the code existed the test project failed to compile on the missing
`DrawEM.App.Application.Video` and `DrawEM.App.Infrastructure.Video` types,
which was the expected missing seam.

### Windows playback matrix

Full report: [`playback-spike.md`](playback-spike.md), produced by
`tools/VideoPlaybackSpike/bin/Release/net8.0-windows/VideoPlaybackSpike.exe docs/v5/step-01-video-decoder-and-formats/playback-spike.md`.
The spike plays each fixture unmuted in a 640×360 topmost window, samples the
screen under it twice, and measures the 440 Hz fixture tone on the default
render device through WASAPI loopback.

| Fixture | Validation | Frames on screen | 440 Hz on endpoint (playing / before) | End | File released |
|---|---|---|---|---|---|
| `h264-high-aac.mp4` | Supported | 100% lit, 11% changed | −27 / −37 dBFS | natural, 2.5 s | yes |
| `h264-main-aac-mono44k.mp4` | Supported | 100% lit, 11% changed | −24 / −47 dBFS | natural, 2.5 s | yes |
| `h264-baseline-aac.mp4` | Supported | 100% lit, 12% changed | −26 / −43 dBFS | natural, 2.5 s | yes |
| `h264-high-silent.mp4` | Supported | 100% lit, 11% changed | −38 / −38 dBFS (no tone) | natural, 2.2 s | yes |
| `h264-high-1080p60-aac.mp4` | Supported | 100% lit, 2% changed | −27 / −47 dBFS | natural, 1.5 s | yes |

Other audio was playing on the endpoint during the run (all-sound peak about
−13 dBFS), which sets the −34 to −49 dBFS floor; each fixture with AAC audio
raised the 440 Hz level to −24…−27 dBFS. Listening by ear was not part of this
run: **Unverified**.

### Rejection diagnostics and timing

Validation times from the spike run (header reads only):

| Fixture | Result | ms |
|---|---|---|
| `hevc-main-aac.mp4` | `UnsupportedVideo`: The video codec HEVC is not supported. | 14.0 |
| `h264-high10-aac.mp4` | `UnsupportedVideo`: H.264 High 10 video (profile 110) is not supported. | 12.2 |
| `h264-high-opus.mp4` | `UnsupportedAudio`: The audio codec Opus is not supported. | 12.6 |
| `h264-high-aac-fragmented.mp4` | `UnsupportedContainer`: Fragmented MP4 files are not supported. | 11.6 |
| `aac-audio-only.mp4` | `NoVideoTrack`: The MP4 file has no video track. | 20.5 |
| `truncated-moov.mp4` | `Corrupt`: The MP4 file is damaged or incomplete: it ends inside the 'mdat' box. | 13.1 |
| `not-mp4.mp4` | `NotMp4`: The file is not an MP4 video. | 2.7 |

For comparison, `MediaPlayer` reports both `truncated-moov.mp4` and
`not-mp4.mp4` as "Cannot find the media file." (`0x80070002`), so the
validator's message is the useful one for S5-02.

### Forced playback-open failure

`h264-high10-labeled-high.mp4` passes validation (its `avcC` says High), then
`MediaPlayer` raises `MediaFailed` with `0xC00D11B1` about 0.3 s after open.
`MediaVideoPlayer` turns it into `Failed` ("The video could not be played:
0xC00D11B1"); `Dispose` closes the player, the private copy can be deleted
right after, no exception reaches the Dispatcher, and the process continues to
the next fixture. `MediaVideoPlayerTests` checks the same, plus a file deleted
between validation and open (`0x80070002`).

### S5-02 input contract

- `IVideoValidator.Validate(path)` (`Mp4VideoValidator`) never throws for a bad
  or missing file. `VideoValidationResult.IsSupported`, `Failure`
  (`Unreadable`, `NotMp4`, `Corrupt`, `UnsupportedContainer`, `NoVideoTrack`,
  `UnsupportedVideo`, `UnsupportedAudio`), and `Message` are ready to show
  before the draft slot changes; `Video` describes an accepted file.
- `MediaVideoPlayer` (UI-thread `Dispatcher`) raises `Opened`, `Completed`, or
  `Failed(VideoPlaybackFailure)`; callers dispose it after either end.

S5-01 adds no managed import, Settings UI, or Settings draft change.

### Limitations

- Support covers only the profile above. HEVC, AV1, VP9, Opus, HE-AAC,
  10-bit, 4:2:2/4:4:4, portrait or larger than 1920×1080, above 60 fps,
  fragmented MP4, and multi-track files are rejected because they were not
  tested or depend on optional codec packages.
- Validation reads headers only. A stream whose SPS contradicts its `avcC`,
  or with damaged media data, passes validation; the first fails at open (see
  above).
- During exploration, a copy of `h264-high-aac.mp4` with its `mdat` payload
  zeroed opened and then neither ended nor failed within 10 s. It is not in
  the fixture set. S5-03 needs a no-progress limit for playback.
- Tested on one PC with optional codec packages installed; a PC without
  them was not tested. H.264 and AAC decoding are built into Windows
  [INFERENCE: not checked on a clean install].
- `MediaVideoPlayerTests` need the Windows media stack; whether GitHub's
  `windows-latest` runner provides it is shown by the PR's CI run.
