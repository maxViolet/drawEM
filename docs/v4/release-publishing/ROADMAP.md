# R4: release publishing roadmap

**Status:** groomed; implementation pending. **Decision baseline:** 2026-10-15.
**Package:** [README](README.md) · [steps](STEPS.md) · [owner questions](../../adr/0002-release-signing-license.md).

## Product decision

Publish Windows x64 releases from GitHub with both a portable ZIP and a
per-user Velopack installer. Publish checksums and build provenance so users
can verify artifact integrity and origin. Check for updates at app startup and
once every 24 hours; download in the background and ask before installing or
restarting the tray app. The first releases ship the portable ZIP only; the
Velopack installer and in-app updates are the last two steps (R4-06, R4-07).

The first public release may be unsigned because the owner set a zero
additional-cost constraint and selected GitHub distribution. Treat signing as
a later, conditional improvement through SignPath Foundation, subject to the
open owner decisions in [ADR-0002](../../adr/0002-release-signing-license.md).
Do not promise that a valid signature eliminates SmartScreen warnings. A new
signed binary can still have insufficient reputation; unsigned files can
receive stronger prompts and may be blocked by Smart App Control or enterprise
policy. [Microsoft SmartScreen guidance](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/smartscreen-reputation)
describes Microsoft Store distribution as the route that avoids SmartScreen
download warnings, but Store distribution is outside the chosen GitHub path.

Keep the current self-contained `win-x64` publish profile. Do not enable
trimming or single-file publishing unless later evidence shows Velopack
requires it and the owner approves the change. macOS and Linux remain out of
scope.

## Current state

Inspected on 2026-10-15:

- `.github/workflows/release.yml` runs on `v*` tag pushes, tests, publishes
  with the tag as version, creates `drawEM-<version>-win-x64.zip`, then
  immediately publishes a GitHub Release with generated notes. It does not
  create a draft, hash assets, attest provenance, sign, or package an installer.
- `win-x64.pubxml` is self-contained and has single-file and trimming disabled.
- The WPF + WinForms app targets `net8.0-windows`. Settings live in
  `%LOCALAPPDATA%\drawEM`; logs live in `%LOCALAPPDATA%\drawEM\logs`.
- The portable ZIP has no install step. The user chooses where to extract it,
  and nothing prevents extraction into `%LOCALAPPDATA%\drawEM`. Separation of
  application files from settings is therefore not guaranteed today. R4-06
  adds that guarantee only for the tested default Velopack install.
- The public repository currently has no detected license and no GitHub
  Releases. A SignPath Foundation application cannot be assumed eligible.
- The v3 Step 8 results still have seven unverified desktop checks. The
  accepted release gate for this package requires those checks to be resolved
  before the first public release.

## Decisions and alternatives

| Topic | Chosen approach | Rejected or deferred alternatives | Cost, setup, owner action |
|---|---|---|---|
| Code signing | Allow the first GitHub release to be unsigned; evaluate SignPath Foundation for later releases. | Microsoft Artifact Signing and OV certificate/HSM: paid, outside the zero-cost constraint. Microsoft Store MSIX: free signed distribution, but changes the chosen GitHub download path. | SignPath has no service fee for qualifying OSS under its published conditions. Owner must choose a qualifying OSI license, make a qualifying release, apply, and obtain approval. The certificate identifies SignPath Foundation as publisher and requires project policy, MFA, defined roles, and manual approval for each signing request. [SignPath terms](https://signpath.org/terms.html) |
| Microsoft Artifact Signing | Not selected under the current budget. | Reconsider only if the owner changes the no-cost constraint. | Basic is USD 9.99/account/month with 5,000 signatures/month, then USD 0.005/signature. Individual eligibility is limited to the US and Canada; organization eligibility is limited to listed countries. A paid Azure subscription is required. [Pricing](https://learn.microsoft.com/en-us/azure/artifact-signing/how-to-change-sku) · [eligibility](https://learn.microsoft.com/en-us/azure/artifact-signing/quickstart) · [subscription requirement](https://learn.microsoft.com/en-us/azure/artifact-signing/faq) |
| OV certificate on cloud HSM | Not selected. | Hardware-backed key handling can improve key custody but adds a certificate and HSM cost and operational setup. | Not estimated because it conflicts with the owner's zero-cost constraint. CA/B Forum requires publicly trusted code-signing keys to be protected by a hardware crypto module. [CA/B Forum requirements](https://cabforum.org/working-groups/code-signing/requirements/) |
| Hashes and provenance | Generate `SHA256SUMS.txt` and attest each user-downloadable release artifact with `actions/attest@v4`; users verify with `Get-FileHash` and `gh attestation verify`. | `actions/attest-build-provenance` is not the preferred action for a new integration; its maintainers direct new implementations to `actions/attest`. | No added GitHub charge for a public repository on a current GitHub plan. CI needs `id-token: write`, `attestations: write`, and read access to contents. An attestation proves origin, not that software is safe. [GitHub attestations](https://docs.github.com/en/actions/concepts/security/artifact-attestations) · [current action](https://github.com/actions/attest) |
| Installer and updates | Velopack Setup.exe and its update feed. Use per-user installation. | Inno Setup/WiX plus a separate update service creates more packaging and update code to own. | Velopack is MIT-licensed. Setup.exe installs per user without elevation. Planned package ID: `drawEM.Desktop`; it must not change after the first release that ships the installer. Verify and record the actual default install directory on Windows in R4-06; it must not be, or contain, `%LOCALAPPDATA%\drawEM`. WPF needs a custom `Main` bootstrap with `VelopackApp.Build().Run()` before WPF initialization. [License](https://github.com/velopack/velopack) · [WPF integration](https://docs.velopack.io/getting-started/wpf) · [installer](https://docs.velopack.io/packaging/operating-systems/windows) |
| Update UX | Check at startup and at most every 24 hours; download in background; user confirms install and restart. Leave settings under `%LOCALAPPDATA%\drawEM`. | Silent automatic restart is rejected because drawEM is a tray app and users may be drawing or playing audio. | No external account or service. Windows integration and update testing are required. Velopack update checks can use GitHub Releases. [Update integration](https://docs.velopack.io/integrating/overview) · [GitHub source](https://docs.velopack.io/integrating/update-sources) |
| Distribution | GitHub Releases first, WinGet after a stable release. WinGet lists the portable ZIP as a `zip` package with a nested portable executable; moving the listing to the installer later needs a separate task. Keep portable ZIP for manual updates. | Scoop and Chocolatey are deferred because they add catalog-maintenance work without a stated need. | WinGet submission is a community PR with automated validation and moderator review; a version-specific installer URL and SHA-256 are required. First submission is manual; later release automation is a separate step. [First contribution](https://github.com/microsoft/winget-pkgs/blob/master/doc/FirstContribution.md) · [validation](https://learn.microsoft.com/en-us/windows/package-manager/package/winget-validation) |
| Release safety | Use a verified `vX.Y.Z` tag as the sole version source. Build a draft with generated, owner-reviewed notes; manually publish after all gates pass. | Immediate publishing on tag push is rejected because there is no inspection window. | No added service cost. GitHub's release event `published` can trigger post-publication automation. Keep the tag immutable in practice; do not overwrite a public release. [GitHub release workflow events](https://docs.github.com/en/actions/reference/workflows-and-actions/events-that-trigger-workflows) · [`gh release create`](https://cli.github.com/manual/gh_release_create) |
| Rollback | Publish a corrected higher version; give manual recovery instructions for installs that cannot launch or update. | Do not promise automatic downgrade. Velopack's normal startup update flow advances to higher versions; downgrade requires explicit application logic and the older full package to remain available. | No external cost. Test update from the previous release to the forward fix. [Velopack specific-version update](https://docs.velopack.io/integrating/specific-version) |
| Antivirus and SmartScreen | Record real Windows results; publish a verification path and state what signing does and does not establish. | No claim of guaranteed warning-free execution. Store distribution can be reconsidered only as a new owner decision. | Requires a real Windows machine and may require Defender false-positive investigation. Global low-level input hooks can attract security scrutiny; a valid signature does not prove behavior is safe. [Microsoft SmartScreen guidance](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/smartscreen-reputation) |

## External setup and cost

Engineering estimates are in the delivery table. These external steps are not
included in those estimates; vendor review times are not published or
guaranteed.

| Option | Direct cost | Owner action outside the repository | Setup / wait estimate |
|---|---|---|---|
| SignPath Foundation | No fee for eligible OSS. | Select and publish a qualifying license; publish the code-signing policy; enable MFA; define author, reviewer, and approver roles; publish the first artifact in the form to be signed; apply and manually approve each signing request. | Allow about 1–3 hours for owner setup after prerequisites; SignPath review wait is unknown. Eligibility and approval are not guaranteed. |
| Microsoft Artifact Signing | Basic: USD 9.99/account/month, 5,000 signatures/month, then USD 0.005/signature. | Create a paid Azure subscription, pass identity validation, create an account/profile, and configure GitHub authentication. | Allow about 1–3 hours for account and CI setup after identity documents are available; identity validation wait varies. This option is outside the selected budget. |
| OV certificate with HSM | Paid certificate and HSM/cloud-HSM charges; amount depends on provider and term. | Obtain a CA quote, pass organization validation, provision hardware-backed key access, and configure a supported CI signing integration. | Procurement and validation may take days or weeks; no fixed duration or price is claimed. |
| Microsoft Store MSIX (deferred) | Individual developer registration is free in Microsoft's new flow; Store identity verification and certification still apply. | Register/verify a Partner Center account and submit an MSIX package. This changes distribution away from direct GitHub Setup.exe downloads. | Allow at least 1–3 hours for package/account preparation; Microsoft certification wait is variable. [Account registration](https://learn.microsoft.com/en-us/windows/apps/publish/partner-center/open-a-developer-account?tabs=individual) |
| SHA-256 and GitHub attestations | No added GitHub charge for a public repository on a current plan. | No external account beyond the public GitHub repository and GitHub CLI for users who verify attestations. | About 0.5 engineering day in R4-02; no owner setup. |
| Velopack | MIT-licensed; no framework fee. | No external account for packaging. GitHub Releases remain the update source. | About 1–2 engineering days for packaging; another 1–2 days for tray update UX. |
| WinGet | No catalog fee. | Owner submits the first manifest PR and responds to validation/moderator feedback. Create a narrowly scoped cross-repository token only if later automation requires one. | About 0.5–1 engineering day; community review wait varies. |

For conditional SignPath CI, store `SIGNPATH_API_TOKEN` only as a secret in the
GitHub `release` Environment with required reviewers. Store organization and
project identifiers as non-secret Environment variables. SignPath's [official
GitHub sample](https://github.com/SignPath/demo-github-actions/blob/main/.github/workflows/build-and-sign.yml)
currently uses `signpath/github-action-submit-signing-request@v3`;
recheck its supported version and pin the reviewed commit SHA when implementing.
The current release workflow already uses `actions/checkout@v7` and
`actions/setup-dotnet@v6`; new provenance uses `actions/attest@v4` (latest
release inspected: `v4.2.2`).

## Delivery order

Estimates are engineering time only; owner setup, SignPath review, WinGet review,
and access to the required Windows machines are not included.

| Order | Task | Outcome | Depends on | Estimate |
|---:|---|---|---|---:|
| 1 | [R4-01](step-01-draft-release/task.md) | Tag-versioned Windows publish produces a draft GitHub release and preserves the portable ZIP. | None | 0.5–1 day |
| 2 | [R4-02](step-02-checksums-provenance/task.md) | The portable ZIP has a SHA-256 entry and a verifiable build attestation. | R4-01 | 0.5 day |
| 3 | [R4-03](step-03-first-release-acceptance/task.md) | Windows portable-ZIP evidence and user instructions are ready; first v3.x GitHub release may be published. | R4-02, v3 Step 8 gate | 1 day plus manual run |
| 4 | [R4-04](step-04-signpath-signing/task.md) | If eligible and approved, later releases sign app binaries through SignPath. | R4-03 and owner decisions in ADR-0002 | 1–2 days plus external review |
| 5 | [R4-05](step-05-winget/task.md) | Stable portable ZIP is listed in WinGet; later updates can submit a manifest PR from release automation. | R4-03 | 0.5–1 day plus moderator review |
| 6 | [R4-06](step-06-velopack-installer/task.md) | Velopack creates the per-user installer and packages; checksums, attestations, and signing (if R4-04 is done) extend to every new download. | R4-01, R4-02, R4-03 | 1–2 days |
| 7 | [R4-07](step-07-tray-updates/task.md) | Tray app offers consent-based update installation while preserving settings. | R4-06 and a published installer release | 1–2 days |

No step waits for visual effects. The first public release cannot precede the v3 Step 8
gate. R4-06/R4-07 may touch shared `App.xaml.cs`, `DrawEM.App.csproj`,
`README.md`, or `ci.yml`; those PRs must name the release-publishing package,
check the latest visual-effects changes, and rebase when required by
[the shared-file rules](../README.md#rules-for-parallel-work).
S4-12 uses the release pipeline available when it starts and does not wait for
this package.

## Acceptance strategy

Keep evidence in four separate groups:

1. **Automated:** `dotnet test DrawEM.sln -c Release`; release workflow validates
   the tag-derived version, self-contained win-x64 publish, package contents,
   checksums, and draft-only behavior.
2. **CI and provenance:** required release checks succeed; `gh attestation
   verify <artifact> -R maxViolet/drawEM` succeeds for each published download;
   checksum verification succeeds after download and fails after a one-byte
   mutation.
3. **Real Windows:** install without elevation, launch tray app, preserve
   settings on upgrade, defer/accept updates, restart after acceptance, uninstall,
   and verify the portable ZIP still runs. Record Windows version, build/tag,
   artifact hashes, SmartScreen/Defender behavior, and exact manual results.
4. **External/manual:** v3 Step 8 gate is closed; owner decisions and SignPath
   approval are recorded if signing is enabled; WinGet acceptance is recorded
   separately from GitHub release readiness.

No automated success substitutes for the real Windows install/update checks.
The first public release gate requires all v3 Step 8 checks resolved, all
automated and CI checks passing, and all required manual results recorded.
Residual SmartScreen warnings are documented rather than treated as a
universal guarantee failure.

## Owner questions

The code license and future SignPath choice are recorded in
[ADR-0002](../../adr/0002-release-signing-license.md): select an OSI-approved
license or retain the unsigned GitHub path; decide whether to apply to SignPath
Foundation and accept its publisher identity, manual approvals, and signing
policy. Until then, do not add paid signing infrastructure or secrets.

## Sources checked 2026-10-15

- [Microsoft Artifact Signing eligibility](https://learn.microsoft.com/en-us/azure/artifact-signing/quickstart),
  [pricing](https://learn.microsoft.com/en-us/azure/artifact-signing/how-to-change-sku),
  and [subscription requirements](https://learn.microsoft.com/en-us/azure/artifact-signing/faq).
- [SignPath Foundation conditions](https://signpath.org/terms.html) and
  [GitHub Actions integration](https://docs.signpath.io/trusted-build-systems/github).
- [GitHub artifact attestations](https://docs.github.com/en/actions/concepts/security/artifact-attestations),
  [attestation action](https://github.com/actions/attest), and
  [verification command](https://docs.github.com/en/actions/how-tos/secure-your-work/use-artifact-attestations/increase-security-rating).
- [Velopack WPF setup](https://docs.velopack.io/getting-started/wpf),
  [Windows installer](https://docs.velopack.io/packaging/operating-systems/windows),
  [update integration](https://docs.velopack.io/integrating/overview),
  [GitHub update source](https://docs.velopack.io/integrating/update-sources),
  [signing coverage](https://docs.velopack.io/packaging/signing), and
  [rollback behavior](https://docs.velopack.io/integrating/specific-version).
- [WinGet first contribution](https://github.com/microsoft/winget-pkgs/blob/master/doc/FirstContribution.md),
  [manifest hashes](https://github.com/microsoft/winget-pkgs/blob/master/doc/manifest/schema/1.9.0/installer.md),
  and [moderator validation](https://learn.microsoft.com/en-us/windows/package-manager/package/winget-validation).
- [SmartScreen reputation and signing limits](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/smartscreen-reputation).
- Current workflow actions observed in this repository on 2026-10-15:
  `actions/checkout@v7` and `actions/setup-dotnet@v6`. New attestations use
  `actions/attest@v4`; the current action release inspected was `v4.2.2`.
  Recheck and pin action references when implementing each workflow step.
