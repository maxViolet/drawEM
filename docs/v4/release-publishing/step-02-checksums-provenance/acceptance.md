# R4-02: acceptance criteria

**Task:** [checksums and provenance](task.md). **Plan:** [implementation](plan.md).

- [ ] The portable ZIP is listed exactly once in `SHA256SUMS.txt`; the
  checksum file itself is excluded from its own list.
- [ ] Recomputed SHA-256 values match the published manifest.
- [ ] `actions/attest@v4` creates an attestation for the portable ZIP from the
  expected repository workflow.
- [ ] `gh attestation verify <zip> -R maxViolet/drawEM` exits successfully;
  wrong-repository verification fails.
- [ ] Changing one byte in a downloaded file makes checksum verification fail.
- [x] User instructions distinguish hashes, provenance, Authenticode, and
  software safety.

R4-06 owns checksum and attestation coverage for Velopack downloads; it is not
an R4-02 acceptance criterion.

## Evidence

- Workflow run and action commit: no tag run yet. Pinned actions:
  `actions/attest` v4.2.2 `1e69f48acb82d1966a394da916b4c1698aa569d6`,
  `actions/upload-artifact` v7.0.1 `043fb46d1a93c77aae656e7c1c64a875d1fc6a0a`,
  `actions/download-artifact` v8.0.1 `3e5f45b2cfb9172054b4087a40e8e0b5a5461e7c`
  (each the newest release at least two weeks old on 2026-10-16).
- Artifact/hash list:
- Attestation verification output:
- Mutation check:

## Validation

- Automated: 4 `Write-Sha256Sums.ps1` cases in `ReleaseScriptsTests`.
  Without the script, the sorted-output case failed; the 3 failure cases
  passed trivially, because a missing script also exits non-zero. With it,
  all pass.
- CI:
- Manual:
