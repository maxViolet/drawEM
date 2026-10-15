# TODO: complete release-publishing grooming

**Status:** grooming reviewed on 2026-10-15; implementation plan is recorded.

- [x] Run the grooming research and prepare the roadmap, steps, and task folders.
- [x] Review the recommendations and record owner decisions; unresolved license and SignPath questions are in [ADR-0002](../../adr/0002-release-signing-license.md).
- [ ] Open and merge the grooming PR, then mark this TODO done or remove it.

## Grooming output

- [Roadmap](ROADMAP.md): decisions, alternatives, current state, delivery order, acceptance gates, sources.
- [Steps](STEPS.md): R4-01 through R4-07 with `Do` and `Done when` criteria.
- Each step folder contains `task.md`, `plan.md`, and `acceptance.md`.
- [Release-package README](README.md) and [v4 package index](../README.md) are updated.

## Recorded owner decisions

- First public release may be v3.x, after the v3 Step 8 release gate is closed; release publishing can be prepared in parallel with v4.
- No additional signing budget. First GitHub release may be unsigned; do not promise warning-free SmartScreen behavior.
- Keep GitHub Releases, retain portable ZIP, use Velopack Setup.exe, and install per user.
- Check updates at startup and once per 24 hours; download in background and ask before install/restart.
- Use a draft release, tag-only versioning, reviewed generated notes, SHA-256 checksums, GitHub provenance, and WinGet after the first stable release.
- Recover from a bad release by publishing a corrected higher version; automatic downgrade is out of scope.

## Owner decisions still open

See [ADR-0002](../../adr/0002-release-signing-license.md) for the future license choice, SignPath application/publisher identity, and the fallback if the project is not eligible or is declined.

## Next action

Open a grooming PR from the documented `R4-00-groom-release-publishing` branch, following [AGENTS.md](../../../AGENTS.md). Keep the PR body sections `Summary`, `Changes`, and `Validation`; state that no application tests were run because this change is documentation-only, and list the primary-source facts verified in the roadmap.
