# R4-03: implementation plan

**Task:** [first-release acceptance](task.md). **Acceptance:** [criteria](acceptance.md).

1. Reconcile every v3 Step 8 `Unverified` result with the agreed gate. Run each
   required desktop check on hardware that supports the specified layout and
   monitor setup; record exact results and evidence.
2. Download the draft's exact assets on a clean Windows 10/11 x64 machine.
   Verify SHA-256 and provenance before running them.
3. Test the portable ZIP as a standard user: extract outside
   `%LOCALAPPDATA%\drawEM`, start, tray startup, normal input pass-through,
   and settings persistence across restart. Test a manual update: exit,
   extract a newer ZIP to a new folder, start it, and confirm the same
   settings load.
4. Record Defender and SmartScreen behavior. Document any warning and the
   hash/provenance verification steps; do not claim signatures guarantee
   warning-free execution.
5. Update root README with download, verify, manual update, and recovery
   instructions. State that the first release has no installer and no in-app
   updates. Name this package in the PR and rebase if shared files changed.
6. Review the generated GitHub notes and all draft assets. Manually publish
   only after automated, Windows, and v3 Step 8 gates are closed.
