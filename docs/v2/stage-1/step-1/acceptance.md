# Stage 1 / Step 1: acceptance criteria

**Task:** [command and code configuration](task.md). **Plan:** [implementation](plan.md).

- [x] A command names one configured sound and compiles without WPF or Win32 types in its interface.
- [x] One code-owned mapping covers all eight sound slots; an assigned slot resolves to exactly one command and an unassigned slot resolves to none.
- [x] Local WAV and MP3 paths remain external to the executable; this step does not import or copy them.
- [x] The log path is under the current user's `%LOCALAPPDATA%`; a failure record has time, requested slot or sound identifier, path, and reason.
- [x] Automated tests at the command-resolution boundary pass without an audio device or settings UI. Record the command and result in the implementation review.

These criteria verify the contract and mapping. They do not claim audible playback.
