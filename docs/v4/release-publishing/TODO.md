# TODO: groom the release publishing package

- [ ] Run the grooming prompt below with a senior model (for example Claude
      Opus or Fable) in a session opened at the repository root.
- [ ] Review the produced roadmap and steps; record owner decisions on the
      open questions.
- [ ] Merge the grooming PR, then delete this file or mark it done.

## Grooming prompt

Copy everything inside the block into the session.

````text
You are grooming the "release publishing" work package of drawEM v4.
drawEM is a Windows 10/11 x64 tray app (WPF + WinForms, .NET 8,
self-contained publish) that draws annotations and plays effects over the
desktop using global low-level keyboard and mouse hooks.

Goal of the package: a user downloads drawEM from GitHub, installs it
without a Windows security warning, can verify that the download came from
this repository, and receives later versions without a manual download.

## Read first

1. AGENTS.md: branch, PR, and communication rules. Follow them.
2. docs/v4/README.md: the two parallel v4 packages and the shared-file rules.
3. docs/v4/release-publishing/README.md: current pipeline, gaps, candidates.
4. .github/workflows/release.yml, .github/workflows/ci.yml,
   .github/workflows/pr-lint.yml.
5. src/DrawEM.App/DrawEM.App.csproj,
   src/DrawEM.App/Properties/PublishProfiles/win-x64.pubxml,
   src/DrawEM.App/App.xaml.cs, src/DrawEM.App/app.manifest.
6. docs/v3/STEP-8-ACCEPTANCE-RESULTS.md: the release gate that still applies.
7. As a format reference only: docs/v4/visual-effects/ROADMAP-v4.md,
   docs/v4/visual-effects/STEPS.md, and one step folder such as
   docs/v4/visual-effects/step-02-effect-channel/ (task.md, plan.md,
   acceptance.md).

## Decide, with evidence

For each topic, compare the realistic options, recommend one, and state
cost, setup time, and what the owner must do outside the repository
(accounts, identity verification, payments, secrets). Verify current facts
(prices, eligibility, tool versions, GitHub Action versions) from primary
sources and cite them; do not rely on memory for anything that changes.

1. Code signing: Azure Trusted Signing / Artifact Signing (individual
   eligibility by country), SignPath Foundation (open-source eligibility),
   OV certificate on a cloud HSM. How signing runs in GitHub Actions without
   exposing keys; which files to sign (exe, dlls, installer, update packages).
2. Checksums and provenance: SHA256SUMS.txt, actions/attest-build-provenance,
   user-facing verification commands.
3. Installer and updates: Velopack versus Inno Setup or WiX plus a separate
   update check. Per-user versus per-machine install. Effect on a WPF app
   whose Main is generated (Velopack needs code at the start of Main).
   Update UX for a tray app: when to check, how to ask, how to restart
   without losing the user's settings. Settings location and migration
   impact (see the schema 2 migration in the visual-effects roadmap).
4. Distribution: winget (first manual submission, then automation from the
   release workflow). Scoop or Chocolatey only if justified.
5. Release flow: draft release, asset naming, version source (tag only),
   changelog generation, required checks before publishing, rollback of a
   bad release, and how the portable zip coexists with the installer.
6. Interaction with antivirus and SmartScreen reputation for an app that
   installs global input hooks; what signing does and does not fix.
7. Interaction with the visual-effects package: shared files listed in
   docs/v4/README.md, and whether release publishing ships first as a
   v3.x release or only with v4.

## Constraints

- Windows x64 only. macOS and Linux are out of scope.
- Keep the self-contained publish. Do not add trimming or single-file
  publish unless a chosen tool requires it; justify any change.
- No secrets in the repository. Name each secret and where it lives
  (GitHub Environment with required reviewers).
- Do not change application code during grooming. Output is documentation
  only.
- Each step has one outcome, can be implemented and reviewed in one PR,
  and has a completion check that can be verified. Mark anything that
  needs a real Windows machine, an external account, or a manual action
  as such.
- Steps must not wait for the visual-effects package unless a shared file
  forces it; say so explicitly where it does.

## Produce

Create these files in docs/v4/release-publishing/, matching the style and
structure of the visual-effects package:

1. ROADMAP.md: product decision, current state, chosen approach per topic
   with rejected alternatives and reasons, delivery order table, acceptance
   strategy, open questions for the owner, and links to sources.
2. STEPS.md: simple ordered steps with "Do" and "Done when" for each.
3. One folder per step, named step-NN-<short-name>/, each with task.md,
   plan.md, and acceptance.md. Task IDs are R4-01, R4-02, and so on.
4. Update docs/v4/README.md: package status and task ID range.
5. Update this TODO.md: check the first item and list the open questions
   that need an owner decision.

Then open a pull request following AGENTS.md: branch
R4-00-groom-release-publishing, title
"R4-00: groom release publishing package", body sections Summary,
Changes, Validation. In Validation, list which facts you verified from
sources and which remain unverified.

## Output in chat

End with: the recommended step order (one line per step with a time
estimate in hours or days), the external actions the owner must take
first, and the single first action to start R4-01.
````
