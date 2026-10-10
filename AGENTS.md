# drawEM rules

## Communication

- Do not use compliments.
- At the start of every session, apply the skill `C:\Users\max\.codex\skills\i-have-adhd\SKILL.md` until the user says `stop adhd mode` or `normal mode`.

## Development

- Write tests before code: first add a test that describes the required behavior and run it. Make sure it fails for the expected reason. Only then write the code, until the test passes.
- Start a bug fix with a test that reproduces the bug.
- If an automated test cannot check the behavior (for example, how a window looks on screen), describe the manual check in the PR's `## Validation` section and mark it unverified until it is done.

## Branch names

- Work outside a documented task: `<TYPE>-<NNN>-<short-description>`, for example `DOC-007-branch-naming`. Types:
  - `DOC` — documentation only: docs, plans, roadmap, rules.
  - `FIX` — a fix to existing code behavior.
  - `TEST` — only adds or changes tests.
  - `FEAT` — new behavior that no documented task covers.
- `NNN` — three digits: the highest existing number of the same type plus one. Find it with `git branch -a` and `gh pr list --state all`. The first branch of a type gets `001`. A continuation of the same work adds `.N`, for example `DOC-005.1-…`.
- Work on a documented task: `<task-id>-<short-description>`, where the ID is copied from the documentation unchanged, for example `S1-02-sound-channel`. Part of a task adds `.N`, for example `S1-02.1-…`. Such a branch does not use up a type number.
- Write the description in lowercase English, with words separated by hyphens.
- Check the name against these rules before you create a branch.
- Branches created before these rules keep their names. Rename a branch only when the user explicitly asks.

## Pull requests

- Title: the work ID from the branch name, a colon, a space, and a short summary — `<TYPE>-<NNN>: <summary>` or `<task-id>: <summary>` (with `.N` if the branch has it). Examples: `DOC-007: add branch rules and disable Claude attribution`, `S1-02: implement global sound channel`, `TEST-001: enforce layer dependencies`.
- Write the title summary in English, in the imperative mood, lowercase after the colon, with no trailing period. Do not use a Conventional Commits prefix (`docs:`, `feat:`, `fix:`) in the title; it is allowed in commit messages.
- PR body sections, in order:
  - `## Summary` — why the change is needed, one to three bullets.
  - `## Changes` — the changed files or areas and what changed in each.
  - `## Validation` — commands run and their results; mark everything not run as unverified.
  - Links to the documented task and related PRs, if any.
- Do not add the "Generated with Claude Code" attribution line to the PR body.
- Dependabot pull requests are `FIX` work. Dependabot cannot set a branch number and name, so their title is `FIX: <summary>` without a number (the prefix is set in `.github/dependabot.yml`), and the `dependabot/...` branch is not checked. The `PR lint` workflow checks only their title.
