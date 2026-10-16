# v4 / R4-04: add conditional SignPath signing

**Status:** pending owner prerequisites. **Source:** [roadmap](../ROADMAP.md) · [ADR-0002](../../../adr/0002-release-signing-license.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-05](../step-05-winget/task.md).

## Task

If the owner chooses a qualifying OSI license and SignPath Foundation approves
the project, sign the drawEM executables and DLLs shipped in the portable ZIP
for later GitHub releases, without exposing private keys or API tokens in the
repository. Installer and Update.exe signing is added with the installer in
[R4-06](../step-06-velopack-installer/task.md).

**Depends on:** R4-03, owner license choice, an already released artifact in
the form to be signed, and SignPath approval. This task is explicitly blocked
until those external prerequisites are met.

See [plan](plan.md) and [acceptance](acceptance.md).
