# R4-02: acceptance criteria

**Task:** [checksums and provenance](task.md). **Plan:** [implementation](plan.md).

- [x] The portable ZIP is listed exactly once in `SHA256SUMS.txt`; the
  checksum file itself is excluded from its own list.
- [x] Recomputed SHA-256 values match the published manifest.
- [x] `actions/attest@v4` creates an attestation for the portable ZIP from the
  expected repository workflow.
- [x] `gh attestation verify <zip> -R maxViolet/drawEM` exits successfully;
  wrong-repository verification fails.
- [x] Changing one byte in a downloaded file makes checksum verification fail.
- [x] User instructions distinguish hashes, provenance, Authenticode, and
  software safety.

R4-06 owns checksum and attestation coverage for Velopack downloads; it is not
an R4-02 acceptance criterion.

## Evidence

- Workflow run: tag `v0.0.1` on `c7e20b7c6deefef32d90bcf0f98b63fdd3e490d3`
  (R4-02.2 branch head), run
  [38879888059](https://github.com/maxViolet/drawEM/actions/runs/38879888059)
  on 2026-10-16: `build` and `release` passed in about 2 minutes. The draft
  had exactly `drawEM-0.0.1-win-x64.zip` and `SHA256SUMS.txt`. The draft and
  tag were deleted afterwards (plan step 4 cleanup). Pinned actions:
  `actions/attest` v4.2.2 `1e69f48acb82d1966a394da916b4c1698aa569d6`,
  `actions/upload-artifact` v7.0.1 `043fb46d1a93c77aae656e7c1c64a875d1fc6a0a`,
  `actions/download-artifact` v8.0.1 `3e5f45b2cfb9172054b4087a40e8e0b5a5461e7c`
  (each the newest release at least two weeks old on 2026-10-16).
- Artifact/hash list: `SHA256SUMS.txt` held one line,
  `947ff35d2efedb22f2bb520722e0d7afcf07338eb200d8f2150b11d7bd9da40d  drawEM-0.0.1-win-x64.zip`.
  `Get-FileHash` of the downloaded ZIP returned the same value in upper case.
- Attestation verification output (gh 2.100.0):
  - `gh attestation verify .\drawEM-0.0.1-win-x64.zip -R maxViolet/drawEM
    --signer-workflow maxViolet/drawEM/.github/workflows/release.yml`: exit 0.
  - With the downloaded bundle
    `sha256-947ff35d2efedb22f2bb520722e0d7afcf07338eb200d8f2150b11d7bd9da40d.jsonl`:
    `-R maxViolet/drawEM` exits 0; `-R maxViolet/not-drawEM` exits 1 with
    `Error: verifying with issuer "sigstore.dev"`. Only the repository
    differs and the bundle is local, so the failure is the identity check, not
    a fetch error. gh 2.100.0 does not print a "mismatch" text.
- Mutation check: one byte flipped at offset 100 in a copy changed the hash to
  `606cc04c62ae0dac47d03905b832e723f2674be99ed3299e0410dd0129e27a77`, which no
  longer matches the manifest. `gh attestation verify` on the copy exits 1
  with `HTTP 404: Not Found`: no attestation exists for that digest.

## Validation

- Automated: 4 `Write-Sha256Sums.ps1` cases in `ReleaseScriptsTests`.
  Without the script, the sorted-output case failed; the 3 failure cases
  passed trivially, because a missing script also exits non-zero. With it,
  all pass.
- CI: PR #89 `build-test-publish` and `naming` passed; tag run above passed.
- Manual: plan step 5 commands run on Windows 11 on 2026-10-16; results above.
