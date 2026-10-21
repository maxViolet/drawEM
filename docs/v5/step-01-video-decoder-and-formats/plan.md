# v5 / S5-01: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Fixtures and behavior tests.** Add representative MP4 fixtures for the
   proposed positive and negative cases. Write tests for profile inspection,
   unsupported/corrupt results, and playback-open failure. Run them first and
   confirm they fail because the probe/validation seam does not yet exist.
2. **Windows playback spike.** Compare the candidate playback paths in the
   actual target Windows environment. Check video and embedded audio, seeking
   to start, end-of-file notification, repeat, stop/dispose, and errors. Record
   the Windows version, app build, fixture codec details, and chosen path.
3. **Support profile.** Select only the codec profiles that pass the fixture
   matrix. Record the container, video codec/profile, audio codec, and any
   resolution or frame-rate bounds that the probe establishes. Unsupported
   profiles return a user-readable result and do not modify a settings draft.
4. **Import validation seam.** Implement the smallest testable probe/validator
   required by the selected playback path. Bound the validation work; malformed
   or unsupported input must return a failure rather than hang or crash.
5. **Playback-open failure.** Force a post-import open/decode failure. Verify
   that the caller can report it, stop partial playback, dispose owned
   resources, and keep the tray application alive.
6. **Record the contract.** Update the v5 roadmap and acceptance evidence with
   the chosen path, profiles, fixture identities/hashes, results, and remaining
   target-machine limits. Do not begin S5-02 until its input contract is
   explicit.
