# R4-01: acceptance criteria

**Task:** [draft release](task.md). **Plan:** [implementation](plan.md).

- [ ] The tag/version test fails before the workflow change and passes after it.
- [ ] A valid `vX.Y.Z` tag produces matching product version metadata and
  `drawEM-<version>-win-x64.zip`.
- [ ] A malformed or missing tag fails before creating a release.
- [ ] The Windows x64 publish remains self-contained, not trimmed, and not
  single-file; the ZIP contains the complete publish directory.
- [ ] Tag workflow creates a draft release with generated notes and does not
  publish it.
- [ ] Publication requires the owner to inspect and publish the draft.

## Evidence

- Workflow run:
- Tag and commit:
- Artifact names and version:
- Draft state verified:

## Validation

- Automated:
- CI:
- Manual:
