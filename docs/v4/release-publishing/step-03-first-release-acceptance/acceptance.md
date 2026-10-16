# R4-03: acceptance criteria

**Task:** [first-release acceptance](task.md). **Plan:** [implementation](plan.md).

- [ ] All v3 Step 8 release-blocking checks are `Pass`; no required check is
  `Unverified`.
- [ ] Evidence identifies the exact draft tag, commit, publish artifact hashes,
  Windows version, and machine configuration.
- [ ] SHA-256 and provenance verification pass before the app is run.
- [ ] Standard-user portable ZIP start, tray behavior, settings persistence,
  and manual update to a newer ZIP pass on Windows 10/11 x64.
- [ ] Defender and SmartScreen outcomes are recorded, including any warning;
  no universal warning-free claim is made.
- [x] Root README explains download, checksum, attestation, signature status,
  manual update, and recovery, and states that installer and in-app updates
  are not yet available.
- [ ] The owner manually publishes the reviewed draft after all gates pass.

## Release gate

Every row must be **Pass** before the owner publishes the draft. Values:
**Pass**, **Fail**, **Unverified** (not run yet).

### v3 Step 8 checks still open

From [v3 Step 8 results](../../../v3/STEP-8-ACCEPTANCE-RESULTS.md) on
2026-10-16. Record the result there and copy it here.

| Check | Needs | Result |
| --- | --- | --- |
| 2.7 German (Germany): Right Alt/AltGr typing; Left Alt bindings | German layout installed | Unverified |
| 2.8 Polish (Programmers): Right Alt/AltGr typing; Left Alt bindings | Polish (Programmers) layout installed | Unverified |
| 4.2 Line and dot physical width at 150% | A 150% monitor | Unverified |
| 4.3 Line and dot physical width across a mixed-DPI pair | Two monitors with different DPI | Unverified |
| 4.5 Save clears drawings on all monitors | A second monitor | Unverified |
| 6.1 v1 smoke test 1–10 with saved settings | A second monitor | Unverified |
| 6.2 v2 sound scenarios 1–15 with saved settings | A second monitor (scenario 12) | Unverified |

### Draft release checks (plan steps 2–4, 6)

Run on a clean Windows 10/11 x64 machine as a standard user, with the exact
draft assets.

| Check | Result | Notes |
| --- | --- | --- |
| `Get-FileHash` of the ZIP matches `SHA256SUMS.txt` | Unverified | |
| `gh attestation verify` for the ZIP exits 0 (README command) | Unverified | |
| Extract outside `%LOCALAPPDATA%\drawEM`; drawEM starts, tray icon appears | Unverified | |
| Input passes through to the app beneath when not drawing | Unverified | |
| Settings saved, drawEM restarted, settings load | Unverified | |
| Manual update: exit, extract a newer ZIP to a new folder, start, same settings load | Unverified | |
| Defender result recorded (warning text or none) | Unverified | |
| SmartScreen result recorded (warning text or none) | Unverified | |
| Generated notes and every draft asset reviewed | Unverified | |
| Owner published the draft | Unverified | |

## Evidence

- v3 Step 8 results link: [v3 Step 8 results](../../../v3/STEP-8-ACCEPTANCE-RESULTS.md)
- Release tag, commit, and asset hashes:
- Windows environment:
- Portable start/settings/manual-update results:
- Defender/SmartScreen results:
- Published release URL:

## Validation

- Automated: none; R4-03 adds no code. Full suite on this branch on
  2026-10-16: 816 passed, 0 failed.
- CI: not run (not pushed).
- Manual Windows: not done; every gate row above is Unverified.
- Owner publication action: not done. No tag was pushed and no release was
  created.
