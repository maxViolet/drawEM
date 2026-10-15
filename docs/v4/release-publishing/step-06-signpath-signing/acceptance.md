# R4-06: acceptance criteria

**Task:** [SignPath signing](task.md). **Plan:** [implementation](plan.md).

- [ ] The owner-selected license is present and applies to all project
  components as required by SignPath Foundation; no license is inferred by CI.
- [ ] A prior public release in the form to be signed exists and SignPath
  Foundation approval is recorded.
- [ ] SignPath publisher identity, policy, team roles, MFA, and manual approval
  requirements are documented in the project code-signing policy.
- [ ] A tested prototype proves signing can be integrated at the correct
  Velopack packaging stage without breaking package metadata or updates.
- [ ] `SIGNPATH_API_TOKEN` exists only as a reviewer-protected `release`
  environment secret; no secret appears in repository files or logs.
- [ ] Final drawEM PE files (application EXEs/DLLs, Setup.exe, and Update.exe
  where applicable) have valid signatures from the approved publisher.
- [ ] Velopack update packages install and update from signed artifacts;
  hashes and attestations identify the final signed outputs.
- [ ] Every release waits for SignPath's required manual signing approval.
- [ ] If prerequisites fail, the accepted unsigned path and unresolved owner
  decision are recorded; no step claims signing is complete.

## Evidence

- Owner license decision and file:
- SignPath application/approval:
- Publisher and policy:
- Environment secret verification (no value):
- Signature and update validation:

## Validation

- Automated:
- CI:
- External SignPath:
- Manual Windows:
