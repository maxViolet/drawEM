# Sound application

`PlaySoundCommand` names one configured sound by `SoundId`. It carries no file
path, playback, WPF, or keyboard-hook detail.

`SoundChannelController` is the global sound channel. It owns at most one
playback attempt: a new request stops and disposes the active player first,
also for the same sound. Each attempt ends at its natural end or ten seconds
after it starts. Callbacks from a replaced attempt are ignored. Failures go to
`ISoundFailureReporter` and leave the channel usable.

Ports: `ISoundPlayerFactory` opens an `ISoundPlayer` for a sound;
`SoundPlaybackException` marks a recoverable failure. Time comes from
`TimeProvider`, so tests control the deadline.

TODO (Stage 1, Step 3): route `Ctrl+Alt+1` through `Ctrl+Alt+8` to `SoundChannelHost.Play`. See
`../../../../docs/v2/ROADMAP-v2.md`.
