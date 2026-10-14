# v4 / FEAT-009: acceptance criteria

See [task](task.md) and [implementation plan](plan.md).

- [ ] Each effect slot shows Loop, Max length, and No limit on a second line;
  sound and empty slots do not.
- [ ] A new or migrated effect slot shows Loop off, Max length 10 s, No limit
  off.
- [ ] The stepper stays within 1–60 seconds and disables a button only at its
  limit.
- [ ] Loop on disables Max length and No limit; No limit on disables the
  stepper.
- [ ] Loop off, limit N: the effect ends at the file end or after N seconds,
  whichever comes first.
- [ ] Loop off, No limit: the effect ends at the file end.
- [ ] Loop on: the effect repeats until stopped and never ends by itself.
- [ ] Loop on: pressing the slot's shortcut while its effect runs stops it;
  the next press starts it again.
- [ ] Loop off: pressing the slot's shortcut while its effect runs restarts it,
  as before.
- [ ] Loop on: Sample reads `Stop` while the effect runs and stops it when
  pressed; Cancel or closing Settings also stops it.
- [ ] A looping effect still stops on another effect, Tray > Stop effects,
  monitor change, topology change, session lock, suspend, Save, and exit.
- [ ] Schema 1 and 2 files load with Loop off and limit 10 s; saving writes
  schema 3 with `loop` and `maxSeconds`.
- [ ] A settings file with `maxSeconds` outside 1–60 fails validation.
- [ ] The v4 roadmap and the settings README describe the new behavior.
- [ ] `dotnet test DrawEM.sln` passes.

## Evidence

Not started.
