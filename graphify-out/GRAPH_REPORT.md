# Graph Report - worktree-new-design  (2026-10-16)

## Corpus Check
- 356 files · ~158,596 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3136 nodes · 9117 edges · 136 communities (118 shown, 18 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1548 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `90fc3d05`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Window
- drawEM v3 Roadmap: Drawing and Action Settings
- Harness
- PlaySoundCommand
- DrawEM.App.Domain.Effects
- SettingsViewModel
- App.xaml.cs
- ShortcutModifiers
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- ManualTimeProvider
- VelopackUpdateService
- SlotViewModel
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Application.Sound
- ManagedSoundLibraryTests
- Fact
- MonitorBounds
- Fixture
- FakePlayback
- ScreenPoint
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- List
- SettingsWindow
- Fixture
- SoundChannelController
- SoundReference
- Harness
- SettingsEditor
- Rig
- EffectId
- S4-12 Acceptance: Publish and Verify on Windows
- .Validate
- GlobalShortcutAdapter
- .BoundaryCrossing_RequiresShortcutReleaseBeforeDrawingAgain
- DrawEM.Tests.Infrastructure
- Harness
- Stage 1 sound implementation plan
- ShortcutBindingsTests
- drawEM architecture
- S4-11 Acceptance criteria
- ManagedSoundLibrary
- LottiePlayback
- SaveHarness
- StartEffectCommand
- Shortcut
- ActionSlot
- .RunOnDispatcherThread
- ShortcutBindings
- DrawEM.App.Domain.Drawing
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- DllImport
- S4-07 acceptance: stop effects when switching monitors
- JsonSettingsStore
- .Load
- SoundFailure
- STEPS.md
- LoggingEffectFailureReporter
- Action
- v4 Simple Implementation Steps
- FakeSoundChannel
- BlockingPlayerFactory
- system_runtime_interopservices
- Settings Storage README
- AppFailureLog
- SingleInstanceGuard
- .OnStartup
- drawEM rules (AGENTS.md)
- IDisposable
- R4-02 implementation plan
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- Func
- NotifyIconTrayHost
- SoundSlot
- Win32SessionInterruptionSource
- Sound infrastructure README
- Stage 1 Step 2 Review
- FIX-006: Single Screen Edge
- Win32KeyboardHookSource
- NativeMethods
- v1 roadmap: drawing
- drawEM v4 out-of-scope task list
- ResourceDictionary
- TrayMenuWindow
- S4-06 acceptance: launch effects through shortcuts
- S4-08 acceptance: choose effects in Settings
- .Deserialize
- .Attach
- SettingsCommand
- ADR 0001: Lottie built-in effects
- UiThemeState
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- EffectLifecycleTests
- DrawEM.App.Domain.Settings
- IntPtr
- .Digit
- UiTheme
- SettingsErrorCode
- DrawingStyle.cs
- S4-09 Acceptance criteria
- Program.cs
- v4 / FEAT-009: implementation plan
- TimeSpan
- Steps
- drawEM v4 Parallel Work Packages
- WidthBox
- FakePicker
- TrayMenuCommand
- EffectMonitorGuard
- F03 harness results
- .Default_FollowsTheWindowsAppTheme
- R4-03 First-Release Acceptance Task
- Portable win-x64 ZIP
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- WinGet manifest (zip + nested portable)
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- Orange Brush Stroke Glyph
- Sound domain README
- Sound presentation placeholder README
- NativeFunctionBridgeRule
- Show visual effects (Loop, Max length)

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `Window` - 61 edges
7. `MonitorBounds` - 60 edges
8. `EffectId` - 60 edges
9. `SettingsViewModel` - 60 edges
10. `GlobalShortcutAdapter` - 59 edges

## Surprising Connections (you probably didn't know these)
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-011: implementation plan` --references--> `ITrayMenu`  [INFERRED]
  docs/v4/out-of-scope/FEAT-011-glass-ui/plan.md → src/DrawEM.App/Infrastructure/ITrayMenu.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Effect playback pipeline** — docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap_rendering, context_placement, docs_v4_visual_effects_roadmap_v4_built_in_lottie_effect [EXTRACTED 1.00]
- **Artifact Integrity and Provenance Verification** — docs_v4_release_publishing_step_02_checksums_provenance_plan_sha256sums_manifest, tools_release_write_sha256sums, docs_v4_release_publishing_step_02_checksums_provenance_plan_actions_attest, docs_v4_release_publishing_step_02_checksums_provenance_plan_gh_attestation_verify, docs_v4_release_publishing_step_02_checksums_provenance_acceptance_one_byte_mutation_check [EXTRACTED 1.00]
- **Code Signing Options Considered** — docs_adr_0002_release_signing_license_signpath_foundation, docs_adr_0002_release_signing_license_microsoft_artifact_signing, docs_adr_0002_release_signing_license_microsoft_store_msix, docs_adr_0002_release_signing_license_unsigned_first_release [EXTRACTED 1.00]
- **Sound shortcut play and Save interlock flow** — docs_architecture_globalshortcutadapter, docs_architecture_soundchannelhost, docs_architecture_soundchannelcontroller, docs_architecture_settingssaveoperation [EXTRACTED 1.00]
- **Tray update flow** — docs_v4_release_publishing_step_07_tray_updates_plan_updatecontroller, docs_v4_release_publishing_step_07_tray_updates_plan_velopackupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_itrayhost, docs_v4_release_publishing_step_07_tray_updates_plan_trayapplication, docs_v4_release_publishing_step_07_tray_updates_plan_wait_exit_then_apply [EXTRACTED 1.00]
- **Release artifact pipeline** — docs_v4_release_publishing_step_04_signpath_signing_plan_pe_file_signing, docs_v4_release_publishing_step_06_velopack_installer_plan_new_velopackpackage_ps1, docs_v4_release_publishing_step_06_velopack_installer_plan_write_sha256sums_ps1, docs_v4_release_publishing_step_05_winget_plan_winget_manifest [INFERRED 0.85]
- **Settings directory preservation** — docs_v4_release_publishing_step_06_velopack_installer_task_package_id, docs_v4_release_publishing_step_06_velopack_installer_plan_json_settings_store, docs_v4_release_publishing_step_05_winget_acceptance_localappdata_drawem, docs_v4_release_publishing_step_06_velopack_installer_plan_velopackpackageidentitytests [INFERRED 0.85]
- **Settings schema migration and rollback** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_jsonsettingsstore, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, docs_v4_visual_effects_roadmap_v4_schema1_migration_backup, docs_v4_visual_effects_roadmap_v4_settings_schema_3, src_drawem_app_infrastructure_settings_readme_v3_rollback_procedure [INFERRED 0.85]

## Communities (136 total, 18 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (17): DrawingState, Stroke, HexColor, StrokeWidth, Pixels, IOverlayView, Window, OverlayWindow (+9 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.13
Nodes (18): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (16): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+8 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (61): CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit, ChooseText (+53 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (59): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+51 more)

### Community 7 - "Harness"
Cohesion: 0.06
Nodes (19): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeNeutralKeyEmitter (+11 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "DrawEM.App.Domain.Effects"
Cohesion: 0.14
Nodes (7): DrawEM.App.Domain.Effects, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects, IDisplayChangeSource, FakeDisplayChangeSource

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "App.xaml.cs"
Cohesion: 0.07
Nodes (8): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray, DrawEM.App

### Community 12 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.14
Nodes (4): DrawEM.Tests.Domain.Effects, LayerDependencyTests, AllowedLayerDependencies, EffectCatalogTests

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (16): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+8 more)

### Community 15 - "Fixture"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 17 - "VelopackUpdateService"
Cohesion: 0.10
Nodes (18): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, New-VelopackPackage.ps1, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, VelopackHookExitTests, VelopackPackageIdentityTests (+10 more)

### Community 18 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.14
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Application.Sound"
Cohesion: 0.13
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, RetiredSoundAssignmentsTests

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, TrayMenuPlacement, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+2 more)

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "FakePlayback"
Cohesion: 0.07
Nodes (24): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+16 more)

### Community 27 - "ScreenPoint"
Cohesion: 0.17
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.22
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 29 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (23): ISoundLibrary, SoundCleanupFailure, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut (+15 more)

### Community 31 - "List"
Cohesion: 0.13
Nodes (10): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+2 more)

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.12
Nodes (14): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+6 more)

### Community 35 - "SoundReference"
Cohesion: 0.11
Nodes (18): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, NoSettingsFailures (+10 more)

### Community 36 - "Harness"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.12
Nodes (8): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 39 - "EffectId"
Cohesion: 0.18
Nodes (7): EffectCatalog, BuiltIn, EffectId, Value, BuiltInEffectAnimations, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.33
Nodes (7): Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency, S4-12 Task: publish and verify on Windows

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.14
Nodes (4): GlobalShortcutAdapter, IsCapturing, IEffectStartQueue, INeutralKeyEmitter

### Community 43 - ".BoundaryCrossing_RequiresShortcutReleaseBeforeDrawingAgain"
Cohesion: 0.12
Nodes (10): ICursorPositionSource, Win32CursorPositionSource, SettableCursor, Available, Position, Reads, FailingCursorPositionSource, MutableCursorPositionSource (+2 more)

### Community 44 - "DrawEM.Tests.Infrastructure"
Cohesion: 0.10
Nodes (4): DrawEM.Tests.Infrastructure, DrawEM.Tests, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.09
Nodes (14): CleanupSteps, Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players (+6 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.15
Nodes (29): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+21 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (37): SkiaSharp allowed only in Presentation, Action slot, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+29 more)

### Community 49 - "S4-11 Acceptance criteria"
Cohesion: 0.15
Nodes (18): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+10 more)

### Community 50 - "ManagedSoundLibrary"
Cohesion: 0.20
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 51 - "LottiePlayback"
Cohesion: 0.14
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 53 - "StartEffectCommand"
Cohesion: 0.18
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 54 - "Shortcut"
Cohesion: 0.12
Nodes (13): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+5 more)

### Community 55 - "ActionSlot"
Cohesion: 0.13
Nodes (12): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SlotAction, SoundAction (+4 more)

### Community 57 - ".RunOnDispatcherThread"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 58 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 59 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.14
Nodes (4): DrawEM.App.Presentation.Effects, DrawEM.App.Domain.Drawing, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.11
Nodes (16): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+8 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.20
Nodes (4): DrawEM.Tests.Release, ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "DllImport"
Cohesion: 0.16
Nodes (6): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, NativeMethods, POINT

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 65 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 66 - ".Load"
Cohesion: 0.15
Nodes (12): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, FakeReporter (+4 more)

### Community 67 - "SoundFailure"
Cohesion: 0.28
Nodes (4): SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 68 - "STEPS.md"
Cohesion: 0.14
Nodes (19): Evidence, Implementation, R4-01: acceptance criteria, Validation, R4-01: implementation plan, Task, v4 / R4-01: create a versioned draft release, R4-04 SignPath Signing Acceptance (+11 more)

### Community 69 - "LoggingEffectFailureReporter"
Cohesion: 0.24
Nodes (6): EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, TextFileLogSink, EffectFailureLogTests

### Community 70 - "Action"
Cohesion: 0.11
Nodes (9): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+1 more)

### Community 71 - "v4 Simple Implementation Steps"
Cohesion: 0.10
Nodes (32): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window (+24 more)

### Community 72 - "FakeSoundChannel"
Cohesion: 0.12
Nodes (5): ISoundStartHold, StartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+7 more)

### Community 76 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.18
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 78 - ".OnStartup"
Cohesion: 0.16
Nodes (6): Application, App, ApplicationExitPolicy, StartupFailure, WpfApplicationLifetime, StartupFailureTests

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.19
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 81 - "R4-02 implementation plan"
Cohesion: 0.17
Nodes (16): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), One-Byte Mutation Check, R4-02 acceptance criteria, actions/attest@v4 Build Provenance (+8 more)

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (24): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog (+16 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 84 - "Func"
Cohesion: 0.24
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.18
Nodes (5): NotifyIconTrayHost, IconUri, FakeTrayMenu, Shown, NotifyIconTrayHostTests

### Community 86 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 87 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 88 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 89 - "Stage 1 Step 2 Review"
Cohesion: 0.10
Nodes (31): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+23 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 91 - "Win32KeyboardHookSource"
Cohesion: 0.13
Nodes (8): INPUT, InputUnion, KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 92 - "NativeMethods"
Cohesion: 0.13
Nodes (6): MONITORINFO, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource

### Community 93 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 95 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.17
Nodes (10): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, Window, TrayMenuWindow, Border (+2 more)

### Community 97 - "S4-06 acceptance: launch effects through shortcuts"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.32
Nodes (9): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings (+1 more)

### Community 100 - ".Attach"
Cohesion: 0.16
Nodes (7): GlassBackdrop, Supported, ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 101 - "SettingsCommand"
Cohesion: 0.18
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 102 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.16
Nodes (13): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, Built-in Lottie Effect, Lottie Simple License (FL 9.13.21) (+5 more)

### Community 103 - "UiThemeState"
Cohesion: 0.23
Nodes (3): UiThemeState, Current, TrayMenuWindowTests

### Community 104 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.16
Nodes (16): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, Effect Playback Contract, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X) (+8 more)

### Community 105 - "EffectLifecycleTests"
Cohesion: 0.27
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 106 - "DrawEM.App.Domain.Settings"
Cohesion: 0.15
Nodes (12): DrawEM.App.Infrastructure, DrawEM.App.Domain.Settings, DrawEM.App.Application.Drawing, DrawEM.Tests.Application.Settings, DrawEM.App.Infrastructure.Drawing, DrawEM.App.Domain.Sound, DrawEM.Tests.Infrastructure.Drawing, DrawEM.Tests.Application.Drawing (+4 more)

### Community 107 - "IntPtr"
Cohesion: 0.23
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 108 - ".Digit"
Cohesion: 0.32
Nodes (3): ShortcutKey, Kind, Value

### Community 109 - "UiTheme"
Cohesion: 0.18
Nodes (8): v4 / FEAT-011: acceptance criteria, v4 / FEAT-011: implementation plan, Previous behavior, Required behavior, v4 / FEAT-011: glass design for Settings and the tray menu, UiTheme, Dark, Light

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "DrawingStyle.cs"
Cohesion: 0.17
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 112 - "S4-09 Acceptance criteria"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 114 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 116 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 117 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.44
Nodes (9): drawEM v4: out of scope, drawEM v4 Parallel Work Packages, Visual Effects Package (S4), Release Publishing README, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, R4-07 Tray Updates Task, TODO: groom the release publishing package (+1 more)

### Community 118 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 119 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 120 - "TrayMenuCommand"
Cohesion: 0.25
Nodes (5): ITrayMenu, TrayMenuCommand, Exit, Settings, StopEffects

### Community 121 - "EffectMonitorGuard"
Cohesion: 0.22
Nodes (4): EffectPlacement, Cursor, Monitor, EffectMonitorGuard

### Community 122 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 123 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 124 - "R4-03 First-Release Acceptance Task"
Cohesion: 0.48
Nodes (6): v3 Step 8 Windows Acceptance Gate, Velopack Per-User Installer, R4-03 Acceptance Criteria, R4-03 Implementation Plan, Manual ZIP Update Procedure, R4-03 First-Release Acceptance Task

### Community 125 - "Portable win-x64 ZIP"
Cohesion: 0.33
Nodes (4): Portable win-x64 ZIP, release.yml Release Workflow, WinGet Portable ZIP Listing, Standard-User Windows Portable ZIP Test

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

### Community 128 - "WinGet manifest (zip + nested portable)"
Cohesion: 0.40
Nodes (5): GitHub release Environment, SIGNPATH_API_TOKEN, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip

## Knowledge Gaps
- **527 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+522 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 746 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **18 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-08 acceptance: choose effects in Settings`, `WidthBox`?**
  _High betweenness centrality (0.112) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _527 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07958707958707958 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `SettingsWindow` to `.Attach`, `Window`, `UiThemeState`, `SettingsViewModel`, `App.xaml.cs`, `.OnStartup`, `.Window_BindsEveryFieldWithoutBindingErrors`, `WidthBox`, `FakePicker`?**
  _High betweenness centrality (0.098) - this node is a cross-community bridge._
- **Should `R4 release publishing roadmap` be split into smaller, more focused modules?**
  _Cohesion score 0.12554112554112554 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.085) - this node is a cross-community bridge._