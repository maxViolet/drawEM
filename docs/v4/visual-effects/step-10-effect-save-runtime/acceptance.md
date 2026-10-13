# v4 / S4-10: acceptance criteria

**Task:** [apply settings safely](task.md). **Plan:** [implementation](plan.md).

- [ ] After Save, a newly assigned effect of either placement works through its shortcut and survives app restart. Automated: `SettingsSaveOperationTests.SavedEffectAssignment_StartsFromItsShortcut` (Confetti/Monitor and Focus ring/Cursor through the real shortcut adapter, monitor guard, and effect channel), `RuntimeMediaTests.Save_SlotChangedFromSoundToEffect_RemovesTheCopyWithItsLastSoundReference_AndRestartBindsTheEffects` (restart from the real settings file binds both placements). Unverified: the running-app check below.
- [x] No old queued or active effect becomes visible after successful Save, even if it was dispatched before Save. `SettingsSaveOperationTests.Save_RemovesTheRunningEffect_BeforeTheNewSnapshotIsActive` (both placements; the effect is removed while the previous snapshot is still active), `EffectPressedBeforeSave_ButDispatchedAfterIt_NeverStarts` (a start queued before Save and run after it shows nothing; a fresh press works).
- [x] Validation, backup, or persistence failure leaves active configuration and actions unchanged. `SettingsSaveOperationTests.SaveThatDoesNotPersist_KeepsTheRunningEffect_AndEffectShortcutsWorkWithoutAFreshPress` (invalid draft and failed write: the running effect, a queued start, and the effect shortcuts carry on), plus the existing `FailedSave_ReportsOnce_AndKeepsSoundDrawingShortcutsAndMedia`, `InvalidDraft_ReturnsErrors_AndChangesNothing`, and `RuntimeMediaTests.Save_WhenSettingsCannotBeSaved_KeepsPreviousActiveFileAndMedia`. A backup failure reaches Save as the same `SettingsStoreException` as a failed write.
- [x] Save with an active Sample stops that Sample and does not let it reappear; a test records this. `SettingsSaveOperationTests.Save_WithAnActiveSample_RemovesIt_AndItDoesNotReappear` (a real `SettingsEditor` and `EffectSampler` over the save harness: Sample, Save, run the UI queue, Cancel).
- [x] Changing a slot from Sound to Effect and saving removes the managed sound copy only when no saved SoundAction references it; Cancel keeps it. Tests record both cases. Save: `RuntimeMediaTests.Save_SlotChangedFromSoundToEffect_RemovesTheCopyWithItsLastSoundReference_AndRestartBindsTheEffects` (real store and library; the copy stays while slot 2 still plays it and is removed with its last reference), with `ManagedSoundLibraryTests.CommitSave_SlotChangedFromSoundToEffect_RemovesCopyOnlyAfterLastSoundReference`. Cancel: `SettingsEditorTests.Cancel_AfterChangingASavedSoundToAnEffect_KeepsTheSoundAndItsCopy`.
- [x] Sound stops, drawing exits and clears on successful Save as v3 specifies; mixed-slot startup and save-race tests record results. Existing v3 tests still pass against the harness, which now also holds effect slots: `Save_DuringActiveStrokeAndSound_StopsSoundAndEndsDrawing_BeforeNewSnapshotIsActive`, `Save_ClearsStrokesOnEveryMonitor`, `KeysHeldAcrossSave_DoNothing_UntilAllReleased_ThenOnlyNewShortcutsWork`, and `SettingsSaveStuckSoundTests`. Mixed startup: `RuntimeSettingsTests.SavedSoundAndEffectSlots_BindEachSlotToItsAction`, `ManagedSoundLibraryTests.Startup_WithSavedSoundAndEffectSlots_KeepsSavedSoundCopies`. Save race: `EffectPressedBeforeSave_ButDispatchedAfterIt_NeverStarts`.

## Implementation

- `Application/Settings/SettingsSaveOperation.cs`: takes the `EffectChannelController`. After the write succeeds, Save stops sound, calls `Interrupt` (which advances the configuration generation, so every queued effect start is dropped), then stops the effect channel, then publishes the snapshot and binds shortcuts. A failed or invalid Save returns before any of these.
- Serialization: Save, the hook callbacks, the queued effect starts (generation check and `EffectChannelController.Start` in one dispatcher action, from S4-06), and Sample all run on the UI thread, so no start interleaves with Interrupt and the stop.
- `Application/Settings/SettingsEditor.cs`: the Save doc says Save removed the running effect, so effect samples are only forgotten.
- `App.xaml.cs`: passes the effect channel to the Save operation.
- Tests: `SettingsSaveOperationTests` now wires the real `EffectMonitorGuard` and `EffectChannelController` with `RecordingEffectSurface` (new shared fake in `Infrastructure/Effects/FakeEffectSources.cs`); its initial settings add Confetti on slot 3 and Focus ring on slot 4.

## Validation

- Test first: the new tests first failed to build because `SettingsSaveOperation` had no effect channel parameter. With the parameter added but unused, four failed for the expected reason (the running effect was not removed): both cases of `Save_RemovesTheRunningEffect_BeforeTheNewSnapshotIsActive`, `Save_WithAnActiveSample_RemovesIt_AndItDoesNotReappear`, and `EffectPressedBeforeSave_ButDispatchedAfterIt_NeverStarts`. All pass with the stop.
- `dotnet build DrawEM.sln --no-incremental -warnaserror`: succeeded, no warnings.
- `dotnet test DrawEM.sln --no-build`: 670 passed, 0 failed.
- Mutation checks: stopping the effect after `Publish` failed both cases of `Save_RemovesTheRunningEffect_BeforeTheNewSnapshotIsActive`; removing the generation check from `GlobalShortcutAdapter.DispatchEffect` failed `EffectPressedBeforeSave_ButDispatchedAfterIt_NeverStarts` and `EffectShortcutTests.QueuedEffect_SaveInterruptsBeforeDispatch_NeverStartsUnderTheNewConfiguration`. Both reverted.
- `/simplify` (four reviewers; efficiency found nothing): the shared `FakeDisplayChangeSource` replaces the private display fakes in `EffectMonitorGuardTests` and `EffectSamplerTests`; `RecordingEffectSurface.Channel()` builds the idle effect channel the other Save rigs need; the save harness passes one monitor source to every adapter and keeps the guard private; the `NotSaved` doc names the running effect and queued starts. Skipped: merging `RecordingEffectSurface` with the private `RecordingSurface` in `EffectMonitorGuardTests` (that fake throws on failure and completes playbacks), and a narrow `ISaveEffectChannel` port (the channel is in the same layer and thread, and `EffectMonitorGuard` and `EffectSampler` already take it directly). Build and tests rerun: 670 passed.
- Running-app check: unverified.

## Running-app check

With a build of this branch running from the tray:

1. Open Settings > Actions. Set slot 5 to Effect, Confetti, keep `Ctrl+Alt+5`, and Save. Press `Ctrl+Alt+5`. Expected: confetti covers the monitor with the cursor.
2. Set slot 6 to Effect, Focus ring, and Save. Press `Ctrl+Alt+6`. Expected: the ring appears around the cursor.
3. Press `Ctrl+Alt+5`, then open Settings and Save while the confetti plays. Expected: the confetti disappears at once and does not come back.
4. In Settings, click Sample on slot 5, then Save while it plays. Expected: the sample disappears at once and does not come back.
5. Exit drawEM from the tray and start it again. Expected: `Ctrl+Alt+5` and `Ctrl+Alt+6` still start their effects.
