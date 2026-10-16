# R4-04: implementation plan

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
4. Sign every drawEM `.exe` and `.dll` in the publish folder before the
   portable ZIP is created. Do not assume the returned SignPath artifact can
   be repacked safely; verify the returned files before zipping. Do not claim
   the ZIP container itself has an Authenticode signature.
5. Validate final signed output and generate checksums/attestations after
   signing. Keep per-release SignPath approval as a required gate.
6. Record in [R4-06](../step-06-velopack-installer/task.md) that Velopack
   packaging must keep this signing stage and add Setup.exe and Update.exe.
7. If integration is incompatible, denied, or ineligible, record the blocker
   in ADR-0002 and keep the unsigned release behavior. Reopen distribution or
   budget only through a new owner decision.
