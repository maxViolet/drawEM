# ADR-001: Opaque managed sound identity

## Status

Accepted

## Date

2026-09-30

## Context

v3 stores a sound per action slot in the settings snapshot. The sound file is
a managed copy in `%LOCALAPPDATA%\drawEM\media`, created by the Step 2 sound
library. The first draft of the snapshot put `LibraryFileName` on
`SoundReference`, and Domain validated that name (no `\`, `/`, `:`, `..`).
That made Domain own the library's storage rules: a change to how copies are
named, deduplicated, or laid out on disk would change Domain types, settings
validation, and their tests.

Settings can also be edited by hand. Whatever identifies a sound in
`settings.json` must never let the app read or delete a file outside the
library.

## Decision

Identify a managed sound by an opaque `ManagedSoundId` in
`DrawEM.App.Domain.Settings`. Domain and Application store, compare, and
serialize it as text; they never interpret it. `SoundReference` pairs the ID
with the original file name shown to the user.

Only Infrastructure maps an ID to a file. `ManagedSoundLocator`
(`DrawEM.App.Infrastructure.Sound`) is that mapping today; the Step 2 library
owns it and decides how IDs are generated.

Interfaces and where they live:

| Type | Layer | Role |
|---|---|---|
| `ManagedSoundId`, `SoundReference` | Domain | Part of the validated snapshot |
| `ISoundLibrary.Import`, `ISoundSampler.Sample` | Application | Ports the Settings window uses; pass IDs and user-selected paths as strings |
| `ManagedSoundLocator` | Infrastructure | Resolves an ID to a library path |

## Adapter guarantees

The media adapter (locator and library) must:

- Resolve an ID only to a file directly inside the library directory. Reject
  paths, `.`/`..`, invalid file-name characters, leading or trailing spaces or
  dots, and Windows device names (`CON`, `NUL`, `COM1`, and so on). Check the
  resolved full path's parent directory as well.
- Treat an ID that does not resolve, or whose file is missing, as a missing
  sound. Report it through the normal sound failure path; never fall back to
  another location.
- Never move or delete a user's source file. Delete a managed copy only when no
  saved snapshot references its ID (Step 2 and Step 6 rules).
- Keep an ID valid for as long as a saved snapshot references it. Changing the
  ID format needs a settings schema migration.

## Rationale

- **File name in Domain (first draft):** rejected. Domain would change with
  every library storage decision, and the name rules would be split between
  Domain and the library.
- **Absolute path in settings:** rejected. The contract stores managed copies,
  not references to the user's files. A path would also let a hand-edited file
  point anywhere.
- **Content hash typed in Domain:** rejected. It fixes the deduplication
  strategy in the domain model before Step 2 has chosen it.

An opaque ID keeps storage changes inside the media module. The security check
lives where file access happens, so there is one place to review.

## Consequences

**Easier:**
- Step 2 can choose naming, hashing, and folder layout without touching Domain
  or settings validation.
- Settings tests use any non-blank ID; locator tests cover path safety.

**Harder:**
- A hand-edited `settings.json` with an unsafe ID now loads. It fails later,
  when the sound is resolved, instead of at load. Step 6 must report it as a
  missing sound.
- Code that needs a file path must go through the adapter; Domain objects
  alone cannot locate media.
