# S5-01 Windows playback spike

- Run (UTC): 2026-10-23 14:15:27
- Windows: Windows 11 Professional 26H2, build 26300.9711, 10.0.26300.0
- App build: DrawEM.App 1.0.0+c469e18d3362dad8530d283ec0fccd98357ca8ef (Release)
- Playback path: WPF System.Windows.Media.MediaPlayer via DrawEM.App MediaVideoPlayer
- Audio endpoint: Headphones (AirPods Pro - Find My), volume 46%, muted False

| Fixture | SHA-256 (first 12) | Validation | Validate ms | Open ms | Opened (video/audio, size) | Screen under window (lit a/b, changed) | Loopback 440 Hz level (playing / before) | End | Released |
|---|---|---|---|---|---|---|---|---|---|
| aac-audio-only.mp4 | 49c3cfc8d73c | NoVideoTrack: The MP4 file has no video track. | 20.5 | 443 | no video/audio, 0×0 | lit 0%/0%, changed 0% | -27 dBFS (all sound -14) / -43 dBFS (all sound -15) | natural end (2446 ms) | yes |
| h264-baseline-aac.mp4 | 319b2f3af013 | supported: H.264 Constrained Baseline, 320×180, 25 fps, AAC-LC audio | 16.0 | 457 | video/audio, 320×180 | lit 100%/100%, changed 12% | -26 dBFS (all sound -13) / -43 dBFS (all sound -13) | natural end (2512 ms) | yes |
| h264-high-1080p60-aac.mp4 | b1f0fdf45aee | supported: H.264 High, 1920×1080, 60 fps, AAC-LC audio | 14.0 | 395 | video/audio, 1920×1080 | lit 100%/100%, changed 2% | -27 dBFS (all sound -13) / -47 dBFS (all sound -15) | natural end (1455 ms) | yes |
| h264-high-aac-fragmented.mp4 | 116ccfd6fd5d | UnsupportedContainer: Fragmented MP4 files are not supported. | 11.6 | 385 | video/audio, 320×180 | lit 100%/100%, changed 11% | -26 dBFS (all sound -14) / -49 dBFS (all sound -15) | natural end (2492 ms) | yes |
| h264-high-aac.mp4 | bc2b9ee16350 | supported: H.264 High, 320×180, 25 fps, AAC-LC audio | 13.5 | 384 | video/audio, 320×180 | lit 100%/100%, changed 11% | -27 dBFS (all sound -14) / -37 dBFS (all sound -13) | natural end (2483 ms) | yes |
| h264-high-opus.mp4 | 42143a6c9159 | UnsupportedAudio: The audio codec Opus is not supported. drawEM plays AAC-LC audio in mono or stereo at 44.1 or 48 kHz, or video without sound. | 12.6 | 433 | video/audio, 320×180 | lit 100%/100%, changed 11% | -24 dBFS (all sound -12) / -49 dBFS (all sound -16) | natural end (2523 ms) | yes |
| h264-high-silent.mp4 | 4f245d593972 | supported: H.264 High, 320×180, 25 fps, no audio | 14.1 | 152 | video/no audio, 320×180 | lit 100%/100%, changed 11% | -38 dBFS (all sound -13) / -38 dBFS (all sound -15) | natural end (2231 ms) | yes |
| h264-high10-aac.mp4 | 1360cb0ac13f | UnsupportedVideo: H.264 High 10 video (profile 110) is not supported. drawEM plays H.264 Constrained Baseline, Main, or High video up to 1920×1080 at 60 fps. | 12.2 | - | - | - | -28 dBFS (all sound -14) / -44 dBFS (all sound -14) | failed: The video could not be played: 0xC00D11B1 (377 ms) | yes |
| h264-high10-labeled-high.mp4 | 2091c7b0343c | supported: H.264 High, 320×180, 25 fps, AAC-LC audio | 11.1 | - | - | - | -35 dBFS (all sound -14) / -34 dBFS (all sound -14) | failed: The video could not be played: 0xC00D11B1 (313 ms) | yes |
| h264-main-aac-mono44k.mp4 | 9a64dfbd7622 | supported: H.264 Main, 320×180, 25 fps, AAC-LC audio | 12.4 | 399 | video/audio, 320×180 | lit 100%/100%, changed 11% | -24 dBFS (all sound -12) / -47 dBFS (all sound -14) | natural end (2496 ms) | yes |
| hevc-main-aac.mp4 | 105b06c96640 | UnsupportedVideo: The video codec HEVC is not supported. drawEM plays H.264 Constrained Baseline, Main, or High video up to 1920×1080 at 60 fps. | 14.0 | 350 | video/audio, 320×184 | lit 100%/100%, changed 10% | -27 dBFS (all sound -13) / -40 dBFS (all sound -15) | natural end (2453 ms) | yes |
| not-mp4.mp4 | 75a986f37476 | NotMp4: The file is not an MP4 video. | 2.7 | - | - | - | -35 dBFS (all sound -14) / -46 dBFS (all sound -14) | failed: The video could not be played: Cannot find the media file. (0x80070002) (171 ms) | yes |
| truncated-moov.mp4 | 6b330350bff9 | Corrupt: The MP4 file is damaged or incomplete: it ends inside the 'mdat' box. | 13.1 | - | - | - | -38 dBFS (all sound -17) / -45 dBFS (all sound -15) | failed: The video could not be played: Cannot find the media file. (0x80070002) (65 ms) | yes |
