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
- [ ] User instructions distinguish hashes, provenance, Authenticode, and
  software safety.

R4-03 owns checksum and attestation coverage for Velopack downloads; it is not
an R4-02 acceptance criterion.

## Evidence

- Workflow run and action commit:
- Artifact/hash list:
- Attestation verification output:
- Mutation check:

## Validation

- Automated:
- CI:
- Manual:
