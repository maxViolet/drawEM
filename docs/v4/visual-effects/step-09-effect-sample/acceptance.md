# v4 / S4-09: acceptance criteria

**Task:** [preview draft effects](task.md). **Plan:** [implementation](plan.md).

- [x] Sample starts the selected draft effect without a saved shortcut, including when the draft shortcut is missing or conflicting. `SettingsEditor.Sample` dispatches on the draft action only and never reads its shortcut. `SettingsEditorTests.Sample_StartsTheDraftEffect_WithoutChangingTheActiveSettings`, `Sample_StartsAnEffectWhoseShortcutConflicts` (conflicts with another slot and with Hold to draw), `SettingsViewModelTests.Sample_OnAnEffectSlot_StartsTheDraftEffect_AndShowsItsFailure`. The editor offers no way to leave a filled slot without a shortcut, so the missing case is covered by construction.
- [ ] Settings remains focused and the effect plays at normal size: a Monitor effect covers the active monitor and a Cursor effect appears around the cursor, never as a thumbnail. Automated: `EffectSamplerTests.Sample_StartsOnTheCursorsMonitor_WithTheEffectsPlacement` (both placements: the command has the catalog placement, the cursor's monitor, and the cursor point, exactly as a shortcut start). The surface is the shortcut surface, whose window is non-activating (`ShowActivated = false`, `WS_EX_NOACTIVATE`, `MA_NOACTIVATE`). Unverified: focus and appearance on the desktop; see [Windows check](#windows-check).
- [x] Sample -> shortcut -> old editor Cancel leaves the shortcut effect running; Sample -> Sample replacement has the same ownership rule. `EffectSamplerTests.StopSamples_AfterAShortcutReplacedTheSample_KeepsTheShortcutEffect`, `StopSamples_AfterAnotherEditorsSampleReplacedIt_KeepsTheNewerSample` (the newer sample also keeps its failure route), `StopSamples_StopsTheEditorsCurrentSample`.
- [x] A Sample rendering failure shows an error in Settings, leaves no visual residue, and does not change active bindings; an automated test forces the failure. `EffectSamplerTests.SampleThatCannotShow_ReportsInSettings_AndLeavesNothingOnScreen` (the surface throws from Show), `SampleThatFailsWhilePlaying_ReportsInSettings_AndReleasesItsWindow` (the playback raises Failed: its window is released), `ShortcutFailure_IsLoggedButNotShownInSettings`, `Sample_WithTheCursorOutsideEveryMonitor_ReportsAndStartsNothing`, `Sample_WithoutACursorPosition_ReportsAndStartsNothing`. Every failure is still written to the effect log. The editor test checks the active snapshot is unchanged and nothing was saved.
- [ ] Cancel/close stops that editor's current Sample and does not affect sound or drawings; automated ownership and Windows UI checks are recorded. Automated: `SettingsEditorTests.Cancel_StopsTheDraftsEffectSample_EvenWhenTheSoundStopIsUnconfirmed`, `SettingsViewModelTests.Close_StopsTheEditorsEffectSample`, `EffectSamplerTests.StopSamples_*`, `ForgetSamples_KeepsTheSampleRunning_ButStopsReportingIt`. The effect sampler touches only the effect channel. Unverified: the Windows UI check below.

## Implementation

- `Application/Settings/IEffectSampler.cs`: the port the editor uses to preview an effect: `Sample`, `StopSamples`, `ForgetSamples`.
- `Application/Settings/SettingsEditor.cs`: `Sample` plays a sound slot or shows an effect slot. `Cancel` stops the effect sample first, so an unconfirmed sound stop cannot skip it. A successful `Save` forgets effect samples; S4-10 makes Save remove the running effect.
- `Infrastructure/Effects/EffectSampler.cs`: `EffectSampler`, one per Settings window, takes the shortcut path: it reads the cursor at the click, enqueues the start on `EffectMonitorGuard` for the monitor epoch, resolves the monitor, and dequeues the start onto `EffectChannelController`. It keeps the started instance ID, and `StopSamples` stops only that instance. `EffectSampleFailureRouter` wraps the effect log as the channel's failure reporter and routes the failure of the latest sample, matched by command reference, to its window; a window forgets only its own route.
- `Infrastructure/Effects/IEffectStartQueue.cs`, `EffectMonitorGuard.cs`: `Dequeue` returns the started instance.
- `Presentation/Settings/SettingsWindow.xaml`: Sample is enabled for any filled slot; the help text names effects.
- `App.xaml.cs`: the effect channel reports through the router; each Settings window gets its own `EffectSampler`.

## Validation

- Test first: the new editor, view-model, and sampler tests first failed to build for the expected reason: `IEffectSampler`, `EffectSampler`, and `EffectSampleFailureRouter` did not exist. The window assertion that Sample is enabled on an effect row failed against the old `HasSound` binding (`Assert.Equal() Failure: Collections differ`) and passes with `HasAction`.
- `dotnet build DrawEM.sln --no-incremental -warnaserror`: succeeded, no warnings.
- `dotnet test DrawEM.sln --no-build`: 659 passed, 0 failed.
- Mutation checks: `StopSamples` calling `channel.Stop()` instead of `channel.Stop(id)` failed `StopSamples_AfterAShortcutReplacedTheSample_KeepsTheShortcutEffect` and `StopSamples_AfterAnotherEditorsSampleReplacedIt_KeepsTheNewerSample`; `Forget` clearing the route unconditionally failed `StopSamples_AfterAnotherEditorsSampleReplacedIt_KeepsTheNewerSample`. Both reverted.
- Published-app Windows desktop checks: unverified.

## Windows check

With a build of this branch running from the tray:

1. Open Settings > Actions. Set slot 2 to Effect, Confetti. Click Sample. Expected: confetti covers the whole monitor that contains the cursor, Settings keeps focus (its title bar stays active, typing still goes to it), and nothing is saved.
2. Choose Focus ring and click Sample. Expected: the ring appears at normal size around the cursor, over the Sample button, not as a thumbnail.
3. Give slot 2 the same shortcut as slot 1 (conflict error shown). Click Sample. Expected: the effect still plays.
4. Click Sample, then at once press a saved effect shortcut, then Cancel. Expected: the shortcut's effect keeps playing to its end.
5. Click Sample and Cancel during playback. Expected: the effect disappears at once; a playing shortcut sound and drawings on screen are untouched.
6. On two monitors: Sample on one, then move the cursor to the other. Expected: the sample stops.
