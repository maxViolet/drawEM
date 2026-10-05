# Sound application

`PlaySoundCommand` names one configured sound by `SoundId` and carries the path
of its managed copy. It carries no playback, WPF, or keyboard-hook detail.
`ActiveSettings` builds the commands for the active settings' sounds; a command
keeps its copy after a newer configuration is published.

`SoundChannelController` is the global sound channel. It owns at most one
playback attempt: a new request stops and disposes the active player first,
also for the same sound. Each attempt ends at its natural end or ten seconds
after it starts. Callbacks from a replaced attempt are ignored. Failures go to
`ISoundFailureReporter` and leave the channel usable.

Ports: `ISoundPlayerFactory` opens an `ISoundPlayer` for a command's copy;
`SoundPlaybackException` marks a recoverable failure. Time comes from
`TimeProvider`, so tests control the deadline.
