# R4-06: implementation plan

**Task:** [SignPath signing](task.md). **Acceptance:** [criteria](acceptance.md).

1. Resolve the questions in [ADR-0002](../../../adr/0002-release-signing-license.md).
   Do not add a license without the owner's selection.
2. Apply to SignPath Foundation after a public release exists in the form to be
   signed. Record the approval and accepted publisher identity (SignPath
   Foundation).
3. Follow the SignPath-generated GitHub integration and signing policy. Put
   `SIGNPATH_API_TOKEN` in a GitHub Environment named `release` with required
   reviewers; store organization/project identifiers as environment variables.
   Do not store secrets in the repository.
4. Prototype the SignPath-to-Velopack signing sequence before wiring it into
   release publication. Demonstrate that all drawEM `.exe` and `.dll` files,
   Setup.exe, and Update.exe where applicable can be signed without invalidating
   Velopack package metadata or updates. Sign MSI only if one is later added.
   Do not assume the returned SignPath artifact can be repacked safely.
5. If the prototype passes, follow the SignPath-generated GitHub integration
   and artifact configuration. Do not claim the ZIP or `.nupkg` container itself
   has an Authenticode signature.
6. Validate final signed output and generate checksums/attestations after
   signing. Keep per-release SignPath approval as a required gate.
7. If integration is incompatible, denied, or ineligible, record the blocker
   in ADR-0002 and keep the unsigned release behavior. Reopen distribution or
   budget only through a new owner decision.
