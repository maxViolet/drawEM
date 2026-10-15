# drawEM v4: release publishing

**Status:** in progress; R4-01 done.
**Decision baseline:** 2026-10-15. The signing license and SignPath eligibility
remain owner decisions in [ADR-0002](../../adr/0002-release-signing-license.md).
**Next action:** implement [R4-02](step-02-checksums-provenance/task.md).

## Goal

Distribute Windows x64 releases through GitHub with a portable ZIP and a
per-user installer, provide checksum and build-origin verification, and offer
opt-in in-app updates. Signing is conditional on a future zero-cost SignPath
Foundation approval. The first GitHub release may be unsigned, and no signing
choice guarantees that SmartScreen will show no warning on every machine.

## Current release pipeline

Inspected on 2026-10-15:

- `.github/workflows/release.yml` runs on a `v*` tag push on `windows-latest`.
  It rejects tags other than `vX.Y.Z`, runs the tests, publishes the
  self-contained win-x64 app with the tag's version, checks and zips the
  publish directory as `drawEM-<version>-win-x64.zip` (R4-01), and creates a
  draft GitHub Release with generated notes for the owner to publish.
- `src/DrawEM.App/Properties/PublishProfiles/win-x64.pubxml`: self-contained,
  not single-file, not trimmed.
- The app is WPF + WinForms on `net8.0-windows`. It runs only on Windows;
  macOS is out of scope for this package.

## Grooming decisions

| Area | Decision |
|---|---|
| Area | Decision |
|---|---|
| First release | GitHub v3.x after the v3 Step 8 release gate is closed; the release-publishing work can proceed in parallel with visual effects. |
| Signing | No paid service. The first release may be unsigned. Consider SignPath Foundation only after the owner chooses a qualifying license, a release in the form to be signed exists, and SignPath approves the project. |
| Artifact verification | Publish SHA-256 checksums and GitHub build-provenance attestations. |
| Installer and updates | Velopack Setup.exe, per-user install, update check at startup and every 24 hours, background download, user confirmation before install/restart. |
| Portable download | Keep the ZIP as a manually updated option. |
| Release control | Build a draft from a verified version tag; inspect it before manual publication. Tag is the version source; review generated release notes. |
| Catalog | Submit to WinGet after a stable release; catalog review does not block the GitHub release. |
| Recovery | Publish a corrected higher version. Do not promise automatic downgrade. |

## Implementation plan

See [ROADMAP.md](ROADMAP.md) for alternatives, evidence, gates, and owner
questions; [STEPS.md](STEPS.md) for the delivery order; and each linked step
folder for its task, plan, and acceptance criteria.

The package has seven steps, `R4-01` through `R4-07`. No application code is
changed during grooming. The v3 Step 8 desktop acceptance remains a release
gate; the v4 S4-12 package uses the release process available when it starts.
