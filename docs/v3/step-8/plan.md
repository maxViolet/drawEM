# v3 / Step 8: verification plan

**Task:** [publish and verify](task.md). **Acceptance:** [criteria](acceptance.md).

1. Run `dotnet test DrawEM.sln --no-restore -m:1` and publish the
   self-contained Windows x64 app with the repository's publish profile.
   Record exact commands, result, build identity, Windows version, layouts,
   monitors, and DPI in a separate manual-results document.
2. Verify tray Settings, draft isolation, Save/Cancel/defaults, conflict and
   copy errors, WAV/MP3 selection and `Sample`, restart persistence, corrupt
   settings recovery, startup orphan cleanup after forced exit, and media
   preservation with unreadable settings.
3. With another app focused, test bound drawing and sound shortcuts, ordinary
   `Ctrl+C` and `Alt+F4`, already-bound capture without dispatch, Tab,
   Alt+Tab, Alt+F4, Win pass-through, and invalid `Ctrl+C` and
   `Ctrl+RightAlt+1` capture feedback. Test Right Alt/AltGr typing and Left Alt
   bindings on German (Germany) and Polish (Programmers) layouts.
4. Enable Windows `Alt+Shift` and `Ctrl+Shift` input-language hotkeys. Verify
   bound actions and captured chords with those modifier pairs do not switch
   layouts, while either bare pair still does. Treat unintended layout
   switching as a release blocker.
5. Measure line and dot physical width on 100%, 150%, and mixed-DPI monitors;
   verify color and all-monitor clear on Save. Listen to WAV/MP3 shortcut and
   sample playback, replacement, same-sound restart, and ten-second cutoff.
   Recheck v1 drawing and v2 Step 4 sound scenarios and record each result.
