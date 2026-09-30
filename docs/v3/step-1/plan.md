# v3 / Step 1: implementation plan

**Task:** [settings snapshot](task.md). **Acceptance:** [criteria](acceptance.md).

1. Define value types and one immutable, validated snapshot in `Domain` and
   `Application`. Model shortcut modifiers and candidate keys without Win32
   virtual-key values; distinguish empty action slots from filled sound slots.
2. Validate HEX color, integer physical width 1–20, required draw/clear
   shortcuts, legal keys and modifier pairs, duplicate active bindings, and
   required shortcut for every filled sound slot. Keep Right Alt out of the
   binding model: it is reserved for AltGr at runtime.
3. Define application-facing settings load/save and media import/preview ports.
   Specify schema version and how an unsupported version is reported. Implement
   profile-scoped storage with durable replacement so a failed write does not
   replace the last valid settings file or active configuration.
4. Load defaults on missing or unreadable settings. Retain unreadable bytes for
   recovery, report the error, and do not persist those fallback defaults
   automatically. Use empty action slots on first v3 launch.
5. Test validation and storage with a temporary directory: missing file,
   restart, malformed/unsupported file, failed write, and retained damaged file.
   Runtime wiring that retires `SoundAssignments.Slots` occurs in [Step 6](../step-6/task.md).
