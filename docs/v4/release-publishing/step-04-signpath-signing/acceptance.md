# R4-04: acceptance criteria

**Task:** [SignPath signing](task.md). **Plan:** [implementation](plan.md).

- [ ] The owner-selected license is present and applies to all project
  components as required by SignPath Foundation; no license is inferred by CI.
- [ ] A prior public release in the form to be signed exists and SignPath
  Foundation approval is recorded.
- [ ] SignPath publisher identity, policy, team roles, MFA, and manual approval
  requirements are documented in the project code-signing policy.
- [ ] `SIGNPATH_API_TOKEN` exists only as a reviewer-protected `release`
  environment secret; no secret appears in repository files or logs.
- [ ] Final drawEM PE files (application EXEs and DLLs) in the portable ZIP
  have valid signatures from the approved publisher.
- [ ] Hashes and attestations identify the final ZIP built from signed files.
- [ ] Every release waits for SignPath's required manual signing approval.
- [ ] If prerequisites fail, the accepted unsigned path and unresolved owner
  decision are recorded; no step claims signing is complete.

## Evidence

- Owner license decision and file:
- SignPath application/approval:
- Publisher and policy:
- Environment secret verification (no value):
- Signature validation:

## Validation

- Automated:
- CI:
- External SignPath:
- Manual Windows:
