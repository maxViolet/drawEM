# v3 / Step 2: acceptance criteria

**Task:** [managed sound library](task.md). **Plan:** [implementation](plan.md).

- [ ] WAV and MP3 selections are copied into the user-profile library; source
  files are never moved or deleted.
- [ ] Multiple slots can share a managed copy. Cancel and replacement preserve
  every copy still referenced by saved settings.
- [ ] Successful Save removes a copy after its last saved reference disappears.
  A failed Save does not remove or change previously saved media.
- [ ] Successful startup load removes orphan copies, including a draft import
  left by forced exit. Unreadable settings prevent startup cleanup.
- [ ] Automated filesystem tests exercise the rules above and record command
  and result. Audible playback remains for [Step 8](../step-8/task.md).
