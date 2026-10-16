# R4-03: acceptance criteria

**Task:** [first-release acceptance](task.md). **Plan:** [implementation](plan.md).

- [x] All v3 Step 8 release-blocking checks are `Pass` or `Skipped` by owner
  decision; no required check is `Unverified`.
- [x] Evidence identifies the exact draft tag, commit, publish artifact hashes,
  Windows version, and machine configuration.
- [x] SHA-256 and provenance verification pass before the app is run.
- [ ] Standard-user portable ZIP start, tray behavior, settings persistence,
  and manual update to a newer ZIP pass on Windows 10/11 x64.
- [x] Defender and SmartScreen outcomes are recorded, including any warning;
  no universal warning-free claim is made.
- [x] Root README explains download, checksum, attestation, signature status,
  manual update, and recovery, and states that installer and in-app updates
  are not yet available.
- [ ] The owner manually publishes the reviewed draft after all gates pass.

## Release gate

Every row must be **Pass** (or **Skipped** by owner decision) before the
owner publishes the draft. Values:
**Pass**, **Fail**, **Skipped** (owner decision), **Unverified** (not run
yet).

### v3 Step 8 checks still open

From [v3 Step 8 results](../../../v3/STEP-8-ACCEPTANCE-RESULTS.md) on
2026-10-16. **Owner decision, 2026-10-16:** the owner skipped these 7 checks
for the first portable release. They need extra keyboard layouts or monitors;
the effort is high and the impact is small. A v4 release still needs them.

| Check | Needs | Result |
| --- | --- | --- |
| 2.7 German (Germany): Right Alt/AltGr typing; Left Alt bindings | German layout installed | Skipped |
| 2.8 Polish (Programmers): Right Alt/AltGr typing; Left Alt bindings | Polish (Programmers) layout installed | Skipped |
| 4.2 Line and dot physical width at 150% | A 150% monitor | Skipped |
| 4.3 Line and dot physical width across a mixed-DPI pair | Two monitors with different DPI | Skipped |
| 4.5 Save clears drawings on all monitors | A second monitor | Skipped |
| 6.1 v1 smoke test 1–10 with saved settings | A second monitor | Skipped |
| 6.2 v2 sound scenarios 1–15 with saved settings | A second monitor (scenario 12) | Skipped |

### Draft release checks (plan steps 2–4, 6)

Run on a clean Windows 10/11 x64 machine as a standard user, with the exact
draft assets.

| Check | Result | Notes |
| --- | --- | --- |
| `Get-FileHash` of the ZIP matches `SHA256SUMS.txt` | Pass | `sha256sum -c SHA256SUMS.txt`: all 4 downloads OK |
| `gh attestation verify` for the ZIP exits 0 (README command) | Pass | Exit 0 for all 4 downloads, with `--signer-workflow` |
| Extract outside `%LOCALAPPDATA%\drawEM`; drawEM starts, tray icon appears | Pass | Extracted to `Downloads\drawEM-4.0.0-win-x64`; owner confirmed start |
| Input passes through to the app beneath when not drawing | Unverified | Not run |
| Settings saved, drawEM restarted, settings load | Pass | Run on the installed copy, which uses the same settings folder |
| Manual update: exit, extract a newer ZIP to a new folder, start, same settings load | Unverified | Needs a second release |
| Defender result recorded (warning text or none) | Pass | None |
| SmartScreen result recorded (warning text or none) | Pass | Setup.exe: "Windows protected your PC"; More info, Run anyway |
| Generated notes and every draft asset reviewed | Pass | Generated PR list replaced with a short summary |
| Owner published the draft | Pass | 2026-10-16 11:52 UTC, marked Latest |

## Evidence

- v3 Step 8 results link: [v3 Step 8 results](../../../v3/STEP-8-ACCEPTANCE-RESULTS.md)
- Release tag, commit, and asset hashes: tag `v4.0.0` on `a9b53d59be7a05ac6c98c2efab9796a1a1afbdbd`, release run [38885040389](https://github.com/maxViolet/drawEM/actions/runs/38885040389) (`build` and `release` passed). `SHA256SUMS.txt`:
  - `013fa33bbfca7f3242c495bd28eebd63707f697a70ddef7f826e09e5642f96ad  drawEM-4.0.0-win-x64-Setup.exe`
  - `a7aa9e4070edda04b8afbb846d463954f2479e6d0cfe715a36b20eee388bc276  drawEM-4.0.0-win-x64.zip`
  - `680e5b30b85ad64e2c3ff1d8430be59b374c2255653536228056d594b5b44488  drawEM.Desktop-4.0.0-full.nupkg`
  - `f67a62db18dbc0151a807f6e833520a36cfa1abb678d6fc191d75bab6d453293  releases.win.json`
- Windows environment: Windows 11 Pro 26H2 (10.0.26300), the owner's development machine, not a
  clean install. Account `max` is in the local Administrators group; every
  run was non-elevated (UAC-filtered token), and no UAC prompt appeared.
- Portable start/settings/manual-update results: the ZIP, downloaded in the
  browser (Mark of the Web, zone 3), started from `Downloads` as
  `DrawEM.App.exe`; `%LOCALAPPDATA%\drawEM\logs\update.log` was not created.
  Settings persistence passed on the installed copy. Input pass-through and
  the manual update to a newer ZIP were not run.
- Defender/SmartScreen results: Setup.exe, downloaded in the browser, showed
  SmartScreen "Windows protected your PC"; the owner chose More info, then
  Run anyway. No Defender warning. The ZIP's SmartScreen result was not
  recorded.
- Published release URL: <https://github.com/maxViolet/drawEM/releases/tag/v4.0.0>.
  The first release is `v4.0.0` and ships the installer as well, because
  R4-06 and R4-07 merged before it (owner decision, 2026-10-16).

## Validation

- Automated: none; R4-03 adds no code. Full suite on this branch on
  2026-10-16: 816 passed, 0 failed.
- CI: PR #91 and #92 checks passed; release run 38885040389 passed.
- Manual Windows: 2026-10-16, results in the gate table. Two rows are not
  run: input pass-through and the manual ZIP update.
- Owner publication action: published 2026-10-16 with those two rows
  Unverified, so the "after all gates pass" criterion stays unchecked.
