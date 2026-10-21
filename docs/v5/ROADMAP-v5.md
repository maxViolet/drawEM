# drawEM v5 roadmap: fullscreen MP4 video

**Status:** proposed product and implementation plan. All five steps are
planned; no v5 implementation or acceptance evidence is recorded.
**Date:** 2026-10-21.
**Terms:** [v5 glossary](README.md#terms). **Step order:** [STEPS.md](STEPS.md).

## Outcome

Let a user import a local MP4 file, assign it to one of the existing eight
action slots, and play it fullscreen while another application has focus.
Playback appears on the monitor containing the cursor. It preserves the whole
video frame with black bars where needed, passes keyboard and mouse input to
the application below, and appears below drawEM drawings and effects.

This package covers only the agreed video feature. It does not add unrelated
v5 features or change the existing sound and effect actions. The Clear shortcut
has one explicit v5 lifecycle extension: it also stops video and effects on the
monitor under the cursor, while leaving global sound playing. This intentionally
supersedes v4's clear-only-drawings behavior for v5.

## Product contract

### Action slots and media

- Keep the existing eight shared action slots. A slot contains exactly one
  action: sound, effect, or video. Drawing and clear keep their separate
  bindings.
- The user chooses a local `.mp4` through Settings. drawEM copies it into an
  application-managed library; settings store a library file name, not an
  arbitrary source path.
- The copy remains while any saved slot refers to it. Removing the last
  reference and saving makes it eligible for cleanup.
- MP4 is a container, not a single codec. S5-01 must select and prove the
  supported codec profiles on the target Windows environment. Import rejects
  files outside the proven support profile with a useful error and leaves the
  draft assignment unchanged. A later playback failure is reported without
  crashing the tray application.
- Each video slot has a Loop setting, off by default. With Loop off the file
  plays once. With Loop on it restarts until stopped.

### Playback surface and input

- A shortcut key-down starts the selected video on the monitor containing the
  cursor at invocation. Holding the key does not retrigger playback.
- The video fills a borderless fullscreen surface with a black background.
  Preserve source aspect ratio and show the complete frame; do not crop it to
  fill the monitor.
- Put video above the desktop and below drawing and effect surfaces. Keep the
  surface non-activating and click-through so it does not take focus or block
  mouse, keyboard, or wheel input to the application underneath. It must remain
  above ordinary application windows, including the focused application.
- Verify that OBS Display Capture of the monitor includes the video and the
  drawEM layers above it.

### Playback and stop behavior

| Event | Result |
|---|---|
| First shortcut press | Start from the beginning on the monitor under the cursor |
| Shortcut held | Do not restart or enqueue another playback |
| Same video shortcut pressed again | Stop the current instance and restart from the beginning |
| Different video shortcut pressed during playback | Stop the current instance and start the selected video from the beginning on the invocation monitor |
| Video shortcut pressed during Settings Sample | Stop Sample and start the saved slot video; Sample never changes the saved assignment |
| Video reaches end with Loop off | Stop playback and release the player and surface resources |
| Video reaches end with Loop on | Restart from the beginning |
| Cursor enters another monitor | Stop video on the previous monitor; do not resume when the cursor returns |
| Tray > Stop video | Stop videos on every monitor |
| Clear shortcut (default `Ctrl+Alt+X`) | Clear drawings and stop video and effects on the monitor under the cursor; keep global sound playing |
| Clear shortcut with a user-configured binding | Apply the same clear behavior using the configured shortcut |
| Settings Save while video or Sample is playing | Stop affected video/Sample playback before replacing settings or deleting any now-unreferenced managed file |
| Display topology changes or removal of the playing monitor | Stop playback and release its resources; do not resume automatically |
| Exit, Windows session lock, or system sleep | Stop video and release resources; do not resume automatically |

There are no pause or resume controls in this version. The tray Stop video
command does not clear drawings, stop effects, or stop global sound.

### Terms

- **Video action:** a saved action-slot entry that references an imported MP4.
- **Managed video library:** drawEM-owned per-user copies referenced by video
  actions; cleanup may remove only copies with no saved or active preview use.
- **Loop:** a per-video-action setting that restarts playback after end-of-file.
- **Sample:** temporary playback of the Settings draft selection; it does not
  assign or persist the video in a saved action slot.

### Audio and preview

- Play the video's embedded audio at the Windows system volume.
- Embedded video audio may play at the same time as drawEM's global sound
  action. Stopping video stops its embedded audio and leaves global sound alone.
- Settings provides Sample for a draft video. It previews on the monitor under
  the cursor and does not save the assignment.

## Delivery order

1. **S5-01 — Playback feasibility and format contract.** Prove the Windows
   playback path, record the supported MP4 codec profiles, and prepare positive
   and negative fixtures before committing the rest of the implementation to a
   decoder contract.
2. **S5-02 — Video action and library.** Add the domain action, settings
   migration, managed import and cleanup, slot editor, Loop setting, and draft
   Sample entry point.
3. **S5-03 — Fullscreen playback.** Add the playback channel and surface,
   aspect-fit rendering, embedded audio, click-through behavior, and layer order.
4. **S5-04 — Controls and lifecycle.** Route shortcuts, restart and stop
   commands, monitor transitions, clear, session interruption, and app exit.
5. **S5-05 — Published-app verification.** Exercise the published artifact,
   OBS capture, multiple monitors, codecs, input pass-through, and resource
   cleanup; record automated and manual evidence separately.

## Completion

V5 is complete when every `[gate]` criterion in S5-01 through S5-05 passes and
the Windows manual checks are recorded against the tested artifact. Any
unavailable environment-dependent check stays **Unverified**; do not infer it
from unit tests or a successful build.
