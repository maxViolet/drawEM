# drawEM v4: parallel work packages

**Status:** two packages; visual effects is planned and S4-01 is done,
release publishing is not groomed yet.
**Date:** 2026-10-09.

v4 runs as two independent packages. Each package has its own roadmap, step
IDs, and branches, so one developer or agent can work on each at the same time.

| Package | Outcome | Task IDs | Status |
|---|---|---|---|
| [Visual effects](visual-effects/ROADMAP-v4.md) | Built-in animated effects (confetti, focus ring) triggered from action slots | `S4-01` … `S4-12` | [Steps](visual-effects/STEPS.md) planned; S4-01 done |
| [Release publishing](release-publishing/README.md) | Signed, verifiable, installable, and updatable Windows releases | `R4-01` … (assigned during grooming) | Scope seed only; see [grooming TODO](release-publishing/TODO.md) |

## Rules for parallel work

- Branch and PR names follow [AGENTS.md](../../AGENTS.md). Use the package's
  task ID: `S4-06-effect-shortcuts`, `R4-02-code-signing`.
- Each package changes only its own files where possible. Visual effects owns
  `src/DrawEM.App/**/Effects/**` and the settings/shortcut changes in its
  roadmap. Release publishing owns `.github/workflows/release.yml`, packaging
  and signing configuration, and release documentation.
- Shared files need coordination. A PR that edits one of them states in its
  body which package it serves and rebases on the other package's merged work:

  | Shared file | Visual effects touches it in | Release publishing may touch it for |
  |---|---|---|
  | `src/DrawEM.App/App.xaml.cs` | S4-01 probe, S4-11 lifecycle | Installer or updater startup hook |
  | `src/DrawEM.App/DrawEM.App.csproj` | New effect assets, if any | Version, signing, packaging properties |
  | `README.md` | S4-12 user instructions | Install, verify, and update instructions |
  | `.github/workflows/ci.yml` | Not planned | Packaging smoke checks |

- S4-12 (publish and verify effects) uses the release pipeline as it exists
  when S4-12 starts. It does not wait for release publishing, and release
  publishing does not wait for effects.
- The v3 Step 8 Windows acceptance gate still applies before any v4 release.
