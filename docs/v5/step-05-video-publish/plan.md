# v5 / S5-05: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Automated release gate.** Run the required solution tests, release build,
   and existing packaging checks. Record exact commands, results, commit, and
   artifact identity. Do not claim desktop behavior from these results.
2. **Published artifact.** Install or launch the exact published artifact on
   the target Windows machine. Verify all selected MP4 profiles, unsupported
   files, embedded audio, and recovery after a playback error.
3. **Settings scenarios.** Exercise video import, rejected import, slot
   assignment, Sample, Save, Cancel, clear, and cleanup of the final
   managed-file reference.
4. **Monitor and layering scenarios.** On one and two monitors, verify cursor
   targeting, transition stop, 16:9 and non-16:9 aspect fit, mixed DPI,
   negative coordinates, drawing/effect layering, input pass-through, and
   OBS Display Capture.
5. **Stop and session scenarios.** Verify Stop video across monitors, clear
   behavior, global sound coexistence, session lock, sleep/wake, and app exit.
6. **Resource cycle.** Run 100 start/restart/stop cycles, then 100 natural
   completion cycles. After each batch, confirm playback
   instances and video surfaces return to zero after all playback ends; sample
   live counts at the end of each batch and again after a second identical
   batch using the playback diagnostics. The permitted delta is zero objects
   across either batch.
7. **Evidence record.** Record Windows version, monitor topology/DPI, OBS
   version and capture method, source fixture hashes, build and artifact hash,
   outcomes, failures, and any Unverified criteria. Update the v5 roadmap only
   when its completion rule is satisfied.
