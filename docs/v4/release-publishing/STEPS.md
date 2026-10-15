# R4: release publishing steps

**Status:** R4-01 done; R4-02 … R4-07 pending. **Contract:** [roadmap](ROADMAP.md).
Use the [owner questions in ADR-0002](../../adr/0002-release-signing-license.md)
for the conditional license and SignPath decision.

Follow the steps in order. The package can be developed independently of visual
effects, but the first public release must pass the v3 Step 8 release gate.
Every task has its own `task.md`, `plan.md`, and `acceptance.md` in its step
folder; each step below links to them.

## Steps

### R4-01: create a versioned draft release

**Do:** Use a verified `vX.Y.Z` tag as the sole version source. Run tests and
self-contained win-x64 publish, retain the portable ZIP, create a draft GitHub
Release, and let the owner inspect generated notes and assets before publishing.

**Done when:** A tag creates a draft only, the version and asset names match,
tests pass, the ZIP contains the complete publish output, and no workflow step
publishes the release automatically.

[Task](step-01-draft-release/task.md) · [Plan](step-01-draft-release/plan.md) ·
[Acceptance](step-01-draft-release/acceptance.md)

### R4-02: publish checksums and provenance

**Do:** Generate `SHA256SUMS.txt` and a GitHub build-provenance attestation for
each download users are expected to verify.

**Done when:** The portable ZIP has a matching checksum and attestation;
`gh attestation verify` succeeds with `maxViolet/drawEM`; a modified file fails
verification. R4-03 extends this to Velopack downloads.

[Task](step-02-checksums-provenance/task.md) · [Plan](step-02-checksums-provenance/plan.md) ·
[Acceptance](step-02-checksums-provenance/acceptance.md)

### R4-03: package a per-user installer

**Do:** Add Velopack packaging while keeping the portable ZIP. Add the WPF
startup hook required by Velopack and keep settings in `%LOCALAPPDATA%\drawEM`.

**Done when:** A standard user installs from `Setup.exe` without elevation,
starts drawEM in the tray, sees a Start menu/uninstall entry, uninstall keeps
settings in `%LOCALAPPDATA%\drawEM`, the portable ZIP remains usable, and each
new Velopack download is hashed and attested.

[Task](step-03-velopack-installer/task.md) · [Plan](step-03-velopack-installer/plan.md) ·
[Acceptance](step-03-velopack-installer/acceptance.md)

### R4-04: add consent-based tray updates

**Do:** Check for updates at startup and no more than once each 24 hours;
download in the background and ask before applying an update or restarting.

**Done when:** Automated tests prove cadence, deferral, consent, and error
handling. A Windows upgrade preserves settings and resumes the tray app only
after the user accepts the restart.

[Task](step-04-tray-updates/task.md) · [Plan](step-04-tray-updates/plan.md) ·
[Acceptance](step-04-tray-updates/acceptance.md)

### R4-05: pass first-release acceptance

**Do:** Record exact build and artifact identities, complete release-blocking
v3 Step 8 checks, test installation/update/portable paths on Windows 10/11 x64,
and publish user instructions. The first v3.x GitHub release may be unsigned.

**Done when:** Every v3 Step 8 check is resolved, release CI passes, all
required Windows results are recorded, and the owner publishes the reviewed
draft manually. Record any SmartScreen/Defender warning; do not claim a
universal warning-free result.

[Task](step-05-first-release-acceptance/task.md) · [Plan](step-05-first-release-acceptance/plan.md) ·
[Acceptance](step-05-first-release-acceptance/acceptance.md)

### R4-06: add conditional SignPath signing

**Do:** After the owner resolves ADR-0002 and SignPath approves the project,
integrate the signing request into Velopack packaging. Store the API token in a
review-protected GitHub Environment, never in the repository.

**Done when:** The approved workflow signs every drawEM PE executable and DLL,
Velopack Setup.exe, and any MSI if one is produced; signature validation passes;
every release request receives its required human approval. If eligibility or
approval is denied, record that decision and keep the unsigned path explicit.

[Task](step-06-signpath-signing/task.md) · [Plan](step-06-signpath-signing/plan.md) ·
[Acceptance](step-06-signpath-signing/acceptance.md)

### R4-07: list stable releases in WinGet

**Do:** Submit the first manifest manually after a stable GitHub release. Add
release-workflow automation for later version-specific manifest pull requests.

**Done when:** The manifest validates locally, references the official stable
installer URL and matching SHA-256, and the accepted package installs from
WinGet. Record catalog review separately; it does not block GitHub releases.

[Task](step-07-winget/task.md) · [Plan](step-07-winget/plan.md) ·
[Acceptance](step-07-winget/acceptance.md)

## First action

Start [R4-02](step-02-checksums-provenance/task.md): add failing tests for the
`SHA256SUMS.txt` writer, then add the manifest and attestation to the release
workflow.
