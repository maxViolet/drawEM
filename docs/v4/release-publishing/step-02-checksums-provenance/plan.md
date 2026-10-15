# R4-02: implementation plan

**Task:** [checksums and provenance](task.md). **Acceptance:** [criteria](acceptance.md).

## Starting point

- R4-01 is merged (PR #83). `release.yml` is one `release` job with
  `contents: write`: verify tag, test, publish, package (ZIP in `artifacts/`,
  path in step output `zip`), `gh release create --draft --verify-tag`, draft
  check. R4-02 keeps these steps.
- R4-01's tag run passed on 2026-10-15: `v0.0.1` created a draft
  ([run 38770952896](https://github.com/maxViolet/drawEM/actions/runs/38770952896)),
  and `v0.0.1-rc` failed at Verify tag
  ([run 38771883964](https://github.com/maxViolet/drawEM/actions/runs/38771883964)).
  The draft and both tags are deleted, so `v0.0.1` is free for step 4.
- `SHA256SUMS.txt` is the only list of verifiable downloads: the attestation
  reads its subjects from it (`subject-checksums`). R4-03 extends coverage by
  adding files to one list.
- Follow R4-01: scripts in `tools/release/`, tests in `ReleaseScriptsTests.cs`
  through `RunScript`. The test project already copies `tools/release/*.ps1`.
  Those tests fall back to Windows PowerShell 5.1, so the script must run in
  5.1 and 7.

## Files

| File | Change |
|---|---|
| `tools/release/Write-Sha256Sums.ps1` | New. Writes the manifest for an explicit file list. |
| `tests/DrawEM.Tests/Release/ReleaseScriptsTests.cs` | Add manifest tests. |
| `.github/workflows/release.yml` | Split into `build` and `release` jobs; add manifest and attestation. |
| `README.md` | Release paragraph; new "Verify a download" section. Shared file: follow [the shared-file rules](../../README.md#rules-for-parallel-work). |

## Manifest format

One line per file, `<lowercase sha256>  <file name>` (two spaces, no
directories), LF endings, UTF-8 without BOM, sorted by name (ordinal). This is
the `sha256sum -c` format. The manifest never lists itself.

## Steps

1. **Failing tests (about 45 minutes).** In `ReleaseScriptsTests.cs`, using
   the class's temporary directory and `RunScript`:
   - Two input files passed in reverse name order give exactly two lines,
     sorted ordinally, each with the hash from `SHA256.HashData` and the file
     name. R4-03 relies on more than one file.
   - The output path given as an input fails and writes no manifest.
   - Two inputs with the same file name fail.
   - A missing input fails.

   Run `dotnet test DrawEM.sln -c Release --filter FullyQualifiedName~ReleaseScriptsTests`.
   Expected: the new tests fail because the script does not exist; R4-01's 18
   tests pass.
2. **Script (about 45 minutes).** `Write-Sha256Sums.ps1 -Path <files>
   -OutputPath <manifest>`. `$ErrorActionPreference = 'Stop'` as in R4-01;
   `Get-FileHash -Algorithm SHA256`, lowercased; write with
   `[IO.File]::WriteAllText` and `[Text.UTF8Encoding]::new($false)`. Rerun the
   filter until green, then the full `dotnet test DrawEM.sln -c Release`.
3. **Workflow (about 1 hour).** Splitting keeps `contents: write` away from
   the test and build steps.
   - Top-level `permissions: {}`.
   - `build`: `contents: read`, `id-token: write`, `attestations: write`.
     R4-01's steps up to Package, then `Write-Sha256Sums.ps1 -Path
     <zip> -OutputPath artifacts/SHA256SUMS.txt`, `actions/attest@v4` with
     `subject-checksums: artifacts/SHA256SUMS.txt`, and
     `actions/upload-artifact@v7` for `artifacts/`. Expose `version` as a job
     output.
   - `release`: `needs: build`, `contents: write`, `GH_REPO:
     ${{ github.repository }}` instead of a checkout. Download the artifact,
     then R4-01's `gh release create` and draft check with both files.
   - Leave out `artifact-metadata: write`: `actions/attest` needs it only for
     storage records, which require `push-to-registry`. If the run fails
     without it, add it to `build` and record why.
   - Pin each new action (`actions/attest`, `actions/upload-artifact`,
     `actions/download-artifact`) to a reviewed full commit SHA with a
     `# vX.Y.Z` comment, as [the roadmap](../ROADMAP.md#sources-checked-2026-10-15)
     requires. Record each version and SHA in [acceptance](acceptance.md).
     R4-01's `checkout` and `setup-dotnet` references are not changed.
4. **Tag run (about 30 minutes plus CI).** A tag push runs the workflow file
   from the tagged commit, and pushing an existing tag triggers nothing.
   - Clean up `v0.0.1` first. The release and the tag are deleted separately,
     because a failed run can leave a tag without a draft; ignore "not found"
     from any of the three commands:

     ```powershell
     gh release delete v0.0.1 -R maxViolet/drawEM --yes
     git push origin :refs/tags/v0.0.1
     git tag -d v0.0.1
     ```

     `git ls-remote --tags origin v0.0.1` must then print nothing.
   - Tag the R4-02 branch head and push it: `git tag v0.0.1` and
     `git push origin v0.0.1`.
   - Expect a draft with exactly `drawEM-0.0.1-win-x64.zip` and
     `SHA256SUMS.txt`.
5. **Manual verification (about 20 minutes).** In an empty folder:

   ```powershell
   gh release download v0.0.1 -R maxViolet/drawEM
   Get-FileHash .\drawEM-0.0.1-win-x64.zip -Algorithm SHA256
   Get-Content .\SHA256SUMS.txt
   gh attestation verify .\drawEM-0.0.1-win-x64.zip -R maxViolet/drawEM `
     --signer-workflow maxViolet/drawEM/.github/workflows/release.yml
   gh attestation download .\drawEM-0.0.1-win-x64.zip -R maxViolet/drawEM
   gh attestation verify .\drawEM-0.0.1-win-x64.zip `
     --bundle .\sha256-<digest>.jsonl -R maxViolet/not-drawEM
   ```

   Expected: hashes match (case-insensitive); the first `verify` exits 0. The
   second `verify` reads the downloaded bundle, so it makes no lookup in the
   wrong repository; it must fail with a repository identity mismatch, not a
   fetch error. Change one byte in a copy of the ZIP: the hash no longer
   matches and `verify` fails. Paste outputs into
   [acceptance](acceptance.md), then run the step 4 cleanup.
6. **README (about 30 minutes).**
   - Release paragraph: the draft also carries `SHA256SUMS.txt`, and the ZIP
     has a build attestation.
   - "Verify a download": the `Get-FileHash` and `gh attestation verify`
     commands from step 5, and what each proves. Hash: the file matches the
     published manifest. Provenance: this repository's release workflow built
     it from a specific commit. Authenticode: absent until
     [ADR-0002](../../../adr/0002-release-signing-license.md) and R4-06 decide
     otherwise. None of them proves the software is safe.

## Out of scope

- Velopack downloads: R4-03 adds them to the `Write-Sha256Sums.ps1` input.
- Signing: R4-06.
- Publishing the draft: owner action, R4-05.
