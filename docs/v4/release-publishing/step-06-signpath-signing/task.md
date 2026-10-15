# v4 / R4-06: add conditional SignPath signing

**Status:** pending owner prerequisites. **Source:** [roadmap](../ROADMAP.md) · [ADR-0002](../../../adr/0002-release-signing-license.md). **Plan:** [plan](plan.md). **Acceptance:** [criteria](acceptance.md). **Next:** [R4-07](../step-07-winget/task.md).

## Task

If the owner chooses a qualifying OSI license and SignPath Foundation approves
the project, add automated signing to later GitHub releases without exposing
private keys or API tokens in the repository.

**Depends on:** R4-05, owner license choice, an already released artifact in
the form to be signed, and SignPath approval. This task is explicitly blocked
until those external prerequisites are met.

See [plan](plan.md) and [acceptance](acceptance.md).
