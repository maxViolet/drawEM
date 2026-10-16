# v4 / R4-02: publish checksums and provenance

**Status:** in progress; `Write-Sha256Sums.ps1`, the split and attested release workflow, and the README section are implemented, and the tag run (step 4) and manual verification (step 5) are not done because they need a pushed tag. **Source:** [roadmap](../ROADMAP.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-03](../step-03-first-release-acceptance/task.md).

## Task

Attach a SHA-256 manifest and GitHub build-provenance attestation to the
portable ZIP so users can verify its integrity and repository origin. R4-06
extends coverage to Velopack installer and update packages.

**Depends on:** R4-01. **Does not wait for:** visual effects.

See [plan](plan.md) and [acceptance](acceptance.md).
