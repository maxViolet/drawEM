# R4-01: implementation plan

**Task:** [draft release](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a failing test for tag parsing, application version metadata, and
   versioned release asset names; run it and confirm the expected failure.
2. Make the release workflow reject non-semver `v` tags and use the verified
   tag as the only version input to publish and package operations.
3. Keep `dotnet test` and self-contained `win-x64` publish. Do not enable
   trimming or single-file publishing.
4. Create a draft release with generated notes and the portable ZIP. Require an
   existing tag (`--verify-tag` or equivalent); do not let release creation
   synthesize a tag from the default branch.
5. Confirm the tag workflow ends with a draft and a separate human action is
   required to publish it.

Record workflow run URL, tag, commit, artifact names, and version metadata in
[acceptance](acceptance.md).
