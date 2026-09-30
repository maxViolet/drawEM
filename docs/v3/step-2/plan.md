# v3 / Step 2: implementation plan

**Task:** [managed sound library](task.md). **Acceptance:** [criteria](acceptance.md).

1. Implement the media port from Step 1 in `Infrastructure/Sound`. Accept WAV
   and MP3, copy into a user-profile library, give each managed copy a stable
   reference, and safely reuse a copy when slots select the same content.
2. Track draft imports separately from saved references. Cancel removes only
   unreferenced draft copies; replacing or clearing a slot removes a copy only
   after a successful settings Save and only if no saved slot references it.
3. On startup, run orphan cleanup after a successful settings load. Skip it
   when settings are unreadable, including when defaults are used as fallback.
   A forced exit may leave a draft copy for the next successful startup cleanup.
4. Report copy, read, and cleanup failures without changing the active
   configuration. Test sharing, replacement, last-reference cleanup, Cancel,
   forced-exit orphan cleanup, corrupt-settings preservation, and untouched
   source files using a temporary directory.
