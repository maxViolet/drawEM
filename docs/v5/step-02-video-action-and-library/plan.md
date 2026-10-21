# v5 / S5-02: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Domain scenarios first.** Add failing tests for a video action in one of
   eight slots, one action per slot, Loop off by default, shortcut assignment,
   and settings validation. Confirm the failures describe missing behavior.
2. **Media reference and action.** Add a path-free video reference and
   `VideoAction` following the existing `SlotAction` model. Keep decoder and
   filesystem details outside the domain.
3. **Settings format.** Add a new schema version for video references and
   Loop. Migrate older sound/effect settings without changing their meaning;
   reject unknown action types and invalid video values. Document the schema
   in the settings infrastructure README. Back up settings before the first
   replacement and preserve a matching video-library snapshot. Document
   restoration of both settings and library files when rolling back to an older
   app.
4. **Managed library tests.** Add failing tests for copying supported media,
   import rollback on failure, draft discard, preservation of saved files,
   shared references, and removal after the final reference is removed and
   saved. Cover Sample stopping before draft cleanup and startup orphan removal.
   Match existing managed-sound ownership guarantees without reusing sound-only
   APIs for video playback; record cleanup failures in `video.log`.
5. **Managed video library.** Implement per-user copy/import, path-safe names,
   support-profile validation through S5-01, draft ownership, and last-reference
   cleanup. Keep the source file untouched.
6. **Settings editor and UI.** Add Video to the slot action choices, MP4 import,
   the per-slot Loop control, and Sample. Import/validation errors leave the
   previous draft slot unchanged. Sample uses the selected draft configuration
   and does not persist it. Cancel, close, and Save stop Sample before cleanup;
   Save stops any playback whose saved reference changes.
7. **Composition and migration checks.** Wire the library, settings, and
   preview seams; verify migration and cleanup with filesystem-backed tests.
   Update this step's evidence only after each criterion passes.
