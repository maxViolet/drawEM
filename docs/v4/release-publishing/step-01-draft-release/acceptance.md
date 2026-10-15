# R4-01: acceptance criteria

**Task:** [draft release](task.md). **Plan:** [implementation](plan.md).

- [x] The tag/version test fails before the workflow change and passes after it.
- [ ] A valid `vX.Y.Z` tag produces matching product version metadata and
  `drawEM-<version>-win-x64.zip`. Local publish verified; tag run unverified.
- [ ] A malformed or missing tag fails before creating a release. Script
  verified; tag run unverified.
- [ ] The Windows x64 publish remains self-contained, not trimmed, and not
  single-file; the ZIP contains the complete publish directory. Local publish
  verified; tag run unverified.
- [ ] Tag workflow creates a draft release with generated notes and does not
  publish it. Unverified.
- [ ] Publication requires the owner to inspect and publish the draft.
  Unverified.

## Implementation

- `tools/release/Get-ReleaseVersion.ps1` turns `vX.Y.Z` into `X.Y.Z` and fails
  for any other tag: no `v`, upper-case `V`, two or four parts, leading zeros,
  pre-release or build suffixes, whitespace, or a full `refs/tags/` name.
- `tools/release/New-ReleasePackage.ps1` fails without writing a ZIP unless
  `DrawEM.App.exe` has file version exactly `X.Y.Z.0` (all four parts are
  compared) and product version `X.Y.Z` (the
  SDK's `+<commit>` suffix is allowed), and the publish directory contains
  `coreclr.dll` and `hostfxr.dll` (self-contained) and `DrawEM.App.dll` (not
  single-file). It then zips the directory and checks that the ZIP holds
  exactly the publish files.
- `.github/workflows/release.yml` checks the tag first, before tests, publish,
  or release creation. It passes the verified version to publish and package,
  and runs `gh release create --draft --verify-tag --generate-notes`, then
  fails unless the created release is a draft.
- Trimming stays off: `win-x64.pubxml` sets `PublishTrimmed=false`, and the
  .NET SDK rejects trimming for WPF apps (NETSDK1168).

## Evidence

- Workflow run: unverified; no tag pushed.
- Tag and commit: unverified.
- Artifact names and version: local publish with `-p:Version=1.2.3` (from
  `Get-ReleaseVersion.ps1 -Tag v1.2.3`) produced `DrawEM.App.exe` with file
  version `1.2.3.0` and product version
  `1.2.3+0ff049033698847784f8aa4e5e45a044fa2f0175`, and
  `drawEM-1.2.3-win-x64.zip` with 472 entries for 472 publish files.
- Draft state verified: unverified.

## Validation

- Automated: `dotnet test DrawEM.sln -c Release --filter
  FullyQualifiedName~ReleaseScriptsTests` failed 17 of 18 before the scripts
  existed (script file not found) and passed 18 of 18 after.
  Review fix: a test with an executable of file version `1.2.3.7` and tag
  version `1.2.3` failed before the four-part check (the script exited 0) and
  passes after it.
  `dotnet build DrawEM.sln -c Release -warnaserror`: 0 warnings.
  `dotnet test DrawEM.sln -c Release`: 813 passed.
- CI: PR #83 `build-test-publish` passed on `windows-latest` before the review
  fix, which runs the release script tests under `pwsh`
  ([run](https://github.com/maxViolet/drawEM/actions/runs/38734178421)).
- Manual: unverified. After merge, push a tag such as `v0.0.1` and check:
  the run passes, the release is a draft with generated notes and
  `drawEM-0.0.1-win-x64.zip`, and the release page has no published release.
  Then delete the draft and the tag. Push a malformed tag such as `v0.0.1-rc`
  and check that the run fails at "Verify tag" and creates no release.
