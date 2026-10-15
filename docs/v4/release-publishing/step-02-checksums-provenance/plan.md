# R4-02: implementation plan

**Task:** [checksums and provenance](task.md). **Acceptance:** [criteria](acceptance.md).

1. Add a failing test for the release asset list and generated checksum entries;
   run it and confirm missing or unexpected assets fail.
2. Generate `SHA256SUMS.txt` for the portable ZIP after it is finalized.
3. Add `actions/attest@v4` after building the portable ZIP;
   use minimum workflow permissions: `contents: read`, `id-token: write`, and
   `attestations: write` (plus release-upload permission only in the upload job).
4. Verify artifact attestations with
   `gh attestation verify <artifact> -R maxViolet/drawEM`.
5. Document Windows checksum verification with `Get-FileHash` and provenance
   verification with GitHub CLI. State that provenance proves build origin,
   not safety.

R4-03 adds installer/update artifacts and must extend both lists before the
package is considered complete. Pin third-party Actions to reviewed immutable
commit SHAs when implementing; record the selected action version and source
commit.
