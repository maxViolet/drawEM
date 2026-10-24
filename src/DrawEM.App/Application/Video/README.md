# Video application

`VideoSupportPolicy` is the MP4 support profile that S5-01 proved on Windows.
It turns the headers an `Mp4MediaInfo` describes into a `VideoValidationResult`:
either the accepted `SupportedVideo` profile or a `VideoValidationFailure` class
with a message the Settings window can show. It reads no file.

Ports: `IVideoValidator` checks one file before import and never throws for a
bad file. `VideoOpened` and `VideoPlaybackFailure` are what a player reports;
`VideoPlaybackException` marks a recoverable player error.
