# R4-05: implementation plan

**Task:** [first-release acceptance](task.md). **Acceptance:** [criteria](acceptance.md).

1. Reconcile every v3 Step 8 `Unverified` result with the agreed gate. Run each
   required desktop check on hardware that supports the specified layout and
   monitor setup; record exact results and evidence.
2. Download the draft's exact assets on a clean Windows 10/11 x64 machine.
   Verify SHA-256 and provenance before running them.
3. Test install, tray startup, normal input pass-through, update check, defer,
   accepted update/restart, settings persistence, portable ZIP, and uninstall.
4. Record Defender and SmartScreen behavior. Document any warning and the
   publisher/hash/provenance verification steps; do not claim signatures
   guarantee warning-free execution.
5. Update root README with install, verify, update, and recovery instructions.
   Name this package in the PR and rebase if shared files changed.
6. Review the generated GitHub notes and all draft assets. Manually publish
   only after automated, Windows, and v3 Step 8 gates are closed.
