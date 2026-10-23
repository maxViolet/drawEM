# v5 / S5-01: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Fixtures and behavior tests first.** Pin and record the exact FFmpeg
   version and commands used to generate compact MP4 fixtures. Check in the
   generator and resulting positive fixtures (video with audio and silent
   video), plus unsupported and corrupt negative fixtures. Tests consume the
   checked-in files and do not require FFmpeg at runtime. Record fixture
   codecs and hashes. Add failing tests for profile inspection, validation
   results, and playback-open failure; run them first and confirm the expected
   missing probe/validation seam.
2. **Windows playback spike.** On the development Windows PC, test WPF
   `MediaPlayer` first for video rendering, embedded audio when present,
   end-of-file notification, stop/dispose, and useful failure results. Compare
   LibVLCSharp only if `MediaPlayer` fails a required fixture or cannot expose
   a useful failure result. Record OS edition/build, app build identity,
   fixture codec details, and the chosen path.
3. **Support profile.** Select only codec profiles that pass the fixture
   matrix on the recorded machine. Accept silent MP4 files; require audio
   playback when an audio track is present. Record the container, video
   codec/profile, audio codec, and any resolution or frame-rate bounds the
   probe establishes. Do not claim support beyond the tested profiles.
4. **Decoder validation seam.** Implement the smallest testable probe and
   validator required by the selected playback path. Bound malformed and
   unsupported-file validation to 30 seconds per file on the target machine.
   Return a user-readable result for S5-02; do not implement managed import,
   Settings UI, or draft mutation here.
5. **Playback-open failure.** Force a playback-open/decode failure after a
   fixture passed validation. Verify that the caller can report it, stop
   partial playback, dispose owned resources, and keep the tray application
   alive. Videos play once; natural completion releases the player and
   surface.
6. **Record the contract.** Update the v5 roadmap and acceptance evidence with
   the chosen path, supported profiles, fixture identities/hashes, FFmpeg
   version and commands, results, tested Windows/app build, and remaining
   limits. Do not begin S5-02 until its input contract is explicit.
