# drawEM v4: release publishing

**Status:** scope seed; not groomed, not implemented.
**Date:** 2026-10-09.
**Next action:** run the grooming prompt in [TODO.md](TODO.md).

## Goal

A user downloads drawEM from GitHub, installs it without a Windows security
warning, can verify that the file came from this repository, and receives
later versions without a manual download.

## Current release pipeline

Inspected on 2026-10-09:

- `.github/workflows/release.yml` runs on a `v*` tag push on `windows-latest`.
  It runs the tests, publishes the self-contained win-x64 app with the tag's
  version, zips the publish directory as `drawEM-<version>-win-x64.zip`, and
  creates a GitHub Release with generated notes.
- `src/DrawEM.App/Properties/PublishProfiles/win-x64.pubxml`: self-contained,
  not single-file, not trimmed.
- The app is WPF + WinForms on `net8.0-windows`. It runs only on Windows;
  macOS is out of scope for this package.

## Gaps

| Gap | User impact |
|---|---|
| No Authenticode signature | SmartScreen shows "Windows protected your PC"; antivirus false positives are more likely for an app with global input hooks |
| No checksums or build provenance | Users cannot verify the download |
| Zip of a directory, no installer | No Start menu entry, no uninstall entry, manual extraction |
| No auto-update | Users stay on old versions |
| Not in winget | No `winget install` path |
| Release is published immediately | No chance to inspect assets before users see them |

## Candidate approaches (to be decided during grooming)

- Signing: Azure Trusted Signing (Artifact Signing), SignPath Foundation (free
  for open source), or an OV certificate on a hardware or cloud HSM.
- Checksums and provenance: `SHA256SUMS.txt` and
  `actions/attest-build-provenance`, verified with `gh attestation verify`.
- Installer and updates: Velopack (Setup.exe, delta updates from GitHub
  Releases), or Inno Setup/WiX plus a separate update check.
- Distribution: winget manifest submitted from the release workflow.
- Release flow: draft release, manual inspection, then publish.

None of these is accepted yet. The grooming output decides scope, order, and
task IDs.
