# R4-01: acceptance criteria

**Task:** [draft release](task.md). **Plan:** [implementation](plan.md).

- [x] The tag/version test fails before the workflow change and passes after it.
- [x] A valid `vX.Y.Z` tag produces matching product version metadata and
  `drawEM-<version>-win-x64.zip`.
- [x] A malformed or missing tag fails before creating a release. Malformed:
  tag run. Missing: the workflow runs only on tag pushes, and the empty tag
  is covered by the script tests.
- [x] The Windows x64 publish remains self-contained, not trimmed, and not
  single-file; the ZIP contains the complete publish directory.
- [x] Tag workflow creates a draft release with generated notes and does not
  publish it.
- [x] Publication requires the owner to inspect and publish the draft.

## Implementation

- `tools/release/Get-ReleaseVersion.ps1` turns `vX.Y.Z` into `X.Y.Z` and fails
  for any other tag: no `v`, upper-case `V`, two or four parts, leading zeros,
  pre-release or build suffixes, whitespace, or a full `refs/tags/` name.
- `tools/release/New-ReleasePackage.ps1` fails without writing a ZIP unless
  `DrawEM.App.exe` has file version `X.Y.Z.0` and product version `X.Y.Z` (the
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

- Workflow run: valid tag
  [38770952896](https://github.com/maxViolet/drawEM/actions/runs/38770952896),
  passed in 3m19s; malformed tag
  [38771883964](https://github.com/maxViolet/drawEM/actions/runs/38771883964),
  failed at "Verify tag" with `Release tag 'v0.0.1-rc' is not of the form
  vX.Y.Z (for example v1.2.3).`; later steps were skipped.
- Tag and commit: `v0.0.1` and `v0.0.1-rc`, both on
  `62d376707ec4fba32f2fbee70a2768de51e8568a` (merge of PR #83).
- Artifact names and version: local publish with `-p:Version=1.2.3` (from
  `Get-ReleaseVersion.ps1 -Tag v1.2.3`) produced `DrawEM.App.exe` with file
  version `1.2.3.0` and product version
  `1.2.3+0ff049033698847784f8aa4e5e45a044fa2f0175`, and
  `drawEM-1.2.3-win-x64.zip` with 472 entries for 472 publish files.
  Tag run: the Package step passed, so `DrawEM.App.exe` carried version
  `0.0.1` and the publish directory passed the self-contained, multi-file,
  and ZIP-contents checks. The draft asset was `drawEM-0.0.1-win-x64.zip`,
  103,027,773 bytes.
- Draft state verified: `gh release view v0.0.1` returned `isDraft: true`,
  `publishedAt: null`, name `drawEM 0.0.1`, notes starting with
  `## What's Changed`. `gh release list --exclude-drafts` was empty. After the
  malformed-tag run `gh release list` was empty.

## Validation

- Automated: `dotnet test DrawEM.sln -c Release --filter
  FullyQualifiedName~ReleaseScriptsTests` failed 17 of 18 before the scripts
  existed (script file not found) and passed 18 of 18 after.
  `dotnet build DrawEM.sln -c Release -warnaserror`: 0 warnings.
  `dotnet test DrawEM.sln -c Release`: 812 passed.
- CI: PR #83 `build-test-publish` passed on `windows-latest`, which runs the
  release script tests under `pwsh`
  ([run](https://github.com/maxViolet/drawEM/actions/runs/38734178421)).
- Manual: done on 2026-10-15 with the runs above. The `v0.0.1` draft and the
  `v0.0.1` and `v0.0.1-rc` tags were deleted afterwards (local and remote).
