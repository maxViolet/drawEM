# Graph Report - worktree-clear-field-6052  (2026-10-19)

## Corpus Check
- 379 files · ~168,675 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3610 nodes · 10429 edges · 150 communities (129 shown, 21 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1740 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `7b0b8979`
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
- App.xaml.cs
- SettingsViewModel
- DrawEM.App.Domain.Drawing
- Shortcut
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- SoundChannelHost
- R4-07 Tray Updates Plan
- Action
- MediaSoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Effects
- ManagedSoundLibraryTests
- Fact
- .Cursor
- Fixture
- .RunOnDispatcherThread
- ScreenPoint
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- TrayApplication
- DrawEM.App.Application.Sound
- Fixture
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- EffectLifecycleTests
- Lottie Simple License
- RecordingEffectSurface
- GlobalShortcutAdapter
- SlotViewModel
- DrawEM.App.Infrastructure
- Harness
- drawEM smoke test
- SoundReference
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- ISoundPlayer
- LottiePlayback
- SaveHarness
- EffectId
- .Save
- SettingsWindow
- .Deserialize
- AppFailureLog
- JsonSettingsStore
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- MonitorBounds
- NativeMethods
- Fixture
- R4-02 Plan: Checksums and Provenance
- VelopackPackageIdentityTests
- STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- Step 4 Plan: Publish and Verify
- BlockingPlayerFactory
- LayerDependencyTests.cs
- Settings Storage README
- drawEM v4 Parallel Work Packages
- SingleInstanceGuardTests
- ManagedSoundLibrary
- drawEM v4: out of scope
- List
- StartEffectCommand
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- LoggingEffectFailureReporter
- NotifyIconTrayHost
- RenderLagHarness/Program.cs
- DrawEM.App.Application.Updates
- Sound Infrastructure README
- Stage 1 sound implementation plan
- FIX-006: Single Screen Edge
- .OnStartup
- UiThemeState
- build-test-publish job
- S4-09 Preview Draft Effects (acceptance)
- ResourceDictionary
- TrayMenuWindow
- GlobalShortcutAdapter
- S4-08 acceptance: choose effects in Settings
- ManualTimeProvider
- .Attach
- FEAT-009-effect-loop-and-limit/task.md
- Lottie built-in effects via SkiaSharp.Skottie
- R4-06 Velopack Installer Plan
- drawEM README
- Steps
- DllImport
- IEffectPlayback
- FEAT-011-glass-ui/plan.md
- BuiltInEffectAnimations.cs
- SettingsErrorCode
- VelopackUpdateService
- Size
- FakeUpdateService
- ICursorPositionSource
- UpdateController
- S4-03 Acceptance Criteria
- SoundId
- Show visual effects (Loop, Max length)
- .CreatePieces
- v3 Step 6 plan
- KeyDirection
- RenderLagHarness tool
- Win32MouseHookSource
- GlassDark
- FakePicker
- CLAUDE.md graphify rules
- TrayMenuCommand
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- DecreaseWidthButton
- SoundReference
- Sound presentation placeholder README
- S4-12 Acceptance: Publish and Verify on Windows
- DateTimeOffset
- LoggingUpdateFailureReporter
- SettingsSnapshot.cs
- .Load
- step-01-draft-release/task.md
- .ReleaseStartedResources
- EffectSurfaceWindow
- .Key
- WidthBox
- FakePlayback
- system_runtime_interopservices
- %LOCALAPPDATA%\drawEM settings directory
- graphify knowledge-graph usage rules
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 144 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `SettingsViewModel` - 62 edges
7. `Window` - 62 edges
8. `MonitorBounds` - 61 edges
9. `EffectId` - 60 edges
10. `GlobalShortcutAdapter` - 59 edges

## Surprising Connections (you probably didn't know these)
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-012: implementation plan` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs

## Import Cycles
- None detected.

## Communities (150 total, 21 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.06
Nodes (33): Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, Out of scope, Per stroke, Previous behavior, Required behavior, Settings file (+25 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (57): BezierSmoothing, CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear (+49 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.05
Nodes (57): v3 Additional Fixes, drawEM v3 Roadmap: Drawing and Action Settings, Save Applies Validated Snapshot, Settings Persistence and Startup Fallback, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X (+49 more)

### Community 7 - "Harness"
Cohesion: 0.06
Nodes (19): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeNeutralKeyEmitter (+11 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.07
Nodes (8): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Theme, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray, GlassBackdropTests, UiThemeStateTests

### Community 10 - "SettingsViewModel"
Cohesion: 0.05
Nodes (28): IShortcutCapture, BindableBase, SettingsViewModel, BezierSmoothing, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.10
Nodes (13): DrawEM.App.Domain.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+5 more)

### Community 12 - "Shortcut"
Cohesion: 0.11
Nodes (15): Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized (+7 more)

### Community 13 - "Theory"
Cohesion: 0.07
Nodes (5): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, EffectCatalogTests, ShortcutTests

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (17): EffectSample, Effect, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples (+9 more)

### Community 15 - "Fixture"
Cohesion: 0.13
Nodes (16): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure, FakeRequest, IsCurrent (+8 more)

### Community 16 - "SoundChannelHost"
Cohesion: 0.12
Nodes (13): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+5 more)

### Community 17 - "R4-07 Tray Updates Plan"
Cohesion: 0.11
Nodes (19): %LOCALAPPDATA%\drawEM settings directory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, VelopackHookExitTests, Velopack, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, DRAWEM_UPDATE_FEED local feed (+11 more)

### Community 18 - "Action"
Cohesion: 0.12
Nodes (9): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+1 more)

### Community 19 - "MediaSoundPlayer"
Cohesion: 0.17
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.13
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Domain.Effects"
Cohesion: 0.11
Nodes (9): DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.Tests.Domain.Effects, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings (+1 more)

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - ".RunOnDispatcherThread"
Cohesion: 0.22
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "ScreenPoint"
Cohesion: 0.17
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.22
Nodes (9): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource (+1 more)

### Community 29 - "Fixture"
Cohesion: 0.09
Nodes (13): EffectPlacement, Cursor, Monitor, EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays (+5 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.09
Nodes (21): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+13 more)

### Community 31 - "TrayApplication"
Cohesion: 0.13
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 32 - "DrawEM.App.Application.Sound"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.Tests.Application.Sound, SoundAssignment, SoundAssignments, Slots

### Community 33 - "Fixture"
Cohesion: 0.10
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.09
Nodes (18): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+10 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 36 - "Harness"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.08
Nodes (12): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Smoothing (+4 more)

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 39 - "EffectLifecycleTests"
Cohesion: 0.15
Nodes (6): BuiltInEffectAnimations, EffectLifecycleTests, Events, Interruptions, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "Lottie Simple License"
Cohesion: 0.15
Nodes (25): Built-in Lottie Effect, EffectCatalog, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources (+17 more)

### Community 41 - "RecordingEffectSurface"
Cohesion: 0.15
Nodes (9): Playback, DisposeFailure, HasCompletionHandler, HasFailureHandler, RecordingEffectSurface, Failures, OnRemove, Removed (+1 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.15
Nodes (3): GlobalShortcutAdapter, IsCapturing, INeutralKeyEmitter

### Community 43 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.11
Nodes (13): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+5 more)

### Community 46 - "drawEM smoke test"
Cohesion: 0.19
Nodes (12): v1 acceptance pass (step 7), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, Step 7 acceptance pass plan, Bounded-exit recheck (+4 more)

### Community 47 - "SoundReference"
Cohesion: 0.11
Nodes (16): ShortcutKey, Kind, Value, SoundReference, DisplayName, LibraryFileName, EffectBinding, KeyChord (+8 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (41): Action slot, ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+33 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.17
Nodes (20): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectFailureLog, EffectMonitorGuard.StopEffects() (+12 more)

### Community 50 - "ISoundPlayer"
Cohesion: 0.06
Nodes (19): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, PlayerFactory, Created, FakePlayer (+11 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.17
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.06
Nodes (31): ISaveSoundChannel, ISoundStartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore, Failure, OnSave (+23 more)

### Community 53 - "EffectId"
Cohesion: 0.13
Nodes (14): EffectId, Value, ActionSlot, Action, IsEmpty, Number, EffectAction, Effect (+6 more)

### Community 54 - ".Save"
Cohesion: 0.15
Nodes (9): IActiveShortcuts, Invalid, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult (+1 more)

### Community 57 - "AppFailureLog"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 58 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.16
Nodes (10): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+2 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.18
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.22
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.27
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 65 - "MonitorBounds"
Cohesion: 0.20
Nodes (4): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectCanvasPlacementTests

### Community 66 - "NativeMethods"
Cohesion: 0.18
Nodes (6): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource

### Community 67 - "Fixture"
Cohesion: 0.14
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 Plan: Checksums and Provenance"
Cohesion: 0.18
Nodes (14): SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), R4-02 Acceptance: Checksums and Provenance, One-Byte Mutation Check, R4-02 acceptance criteria, R4-02 Plan: Checksums and Provenance, actions/attest@v4 Build Provenance, GitHub Build Provenance Attestation (actions/attest), gh attestation verify (+6 more)

### Community 70 - "STEPS.md"
Cohesion: 0.14
Nodes (26): v3 Step 8 Windows Acceptance Gate, Velopack Per-User Installer, v4 Release Publishing Roadmap, R4-03 Acceptance Criteria, First Release Gate Table, SmartScreen 'Windows protected your PC' warning, drawEM v4.0.0 Release, R4-03 Implementation Plan (+18 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.10
Nodes (37): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+29 more)

### Community 72 - "Step 4 Plan: Publish and Verify"
Cohesion: 0.11
Nodes (27): drawEM v1 roadmap: drawing, Step 7 acceptance results (conditional pass), drawEM manual smoke test, Step 6 tray lifecycle and packaging plan, Lifecycle seams (ITrayHost, IOverlayLifetime, IApplicationLifetime), Step 7 acceptance pass plan, Stage 1 Acceptance Results: Sound Command, sound.log failure log (%LOCALAPPDATA%\drawEM\logs) (+19 more)

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.18
Nodes (17): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+9 more)

### Community 76 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.18
Nodes (9): drawEM v4 Parallel Work Packages, Visual Effects Package (S4), Release Publishing README, ADR-0002 Release Signing License, Portable win-x64 ZIP, R4 Release Publishing Roadmap, WinGet Portable ZIP Listing, TODO: groom the release publishing package (+1 more)

### Community 78 - "ManagedSoundLibrary"
Cohesion: 0.20
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 79 - "drawEM v4: out of scope"
Cohesion: 0.16
Nodes (9): Dependabot config (nuget, github-actions, FIX prefix), Dependabot config, PR lint workflow, PR lint workflow (branch name and title check), Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM agent rules, drawEM v4: out of scope (+1 more)

### Community 80 - "List"
Cohesion: 0.14
Nodes (10): CompositeDisposable, CompositeDisposableTests, FakeDisposable, SamplingEditor, Failures, RunningStates, Sampler, FakeDisposable (+2 more)

### Community 81 - "StartEffectCommand"
Cohesion: 0.12
Nodes (8): StartEffectCommand, IMonitorBoundsSource, EffectMonitorGuard, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, CountingMonitorBoundsSource, Queries

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (21): Stage 1 Manual Desktop Gate, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog, tools/E2ERunner black-box runner, E2ERunner.Tests drift check, JsonSettingsStore, KeyboardHookEvents.NeutralKeyTag (+13 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "LoggingEffectFailureReporter"
Cohesion: 0.17
Nodes (7): IEffectFailureReporter, LoggingEffectFailureReporter, FailingSurface, Disposed, Last, ShowFailure, Shown

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.13
Nodes (7): ITrayMenu, NotifyIconTrayHost, IconUri, FakeTrayMenu, Shown, Updates, NotifyIconTrayHostTests

### Community 86 - "RenderLagHarness/Program.cs"
Cohesion: 0.15
Nodes (3): DrawEM.Tests.Release, RenderLagHarness, VelopackHookExitTests

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.13
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.24
Nodes (19): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, Sound Application README, SoundChannelController (+11 more)

### Community 89 - "Stage 1 sound implementation plan"
Cohesion: 0.09
Nodes (48): drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, v2 roadmap: screen actions, Global sound channel, Ctrl+Alt+1..8 Sound Shortcut Slots, Ten-second playback cap, Confirmed test boundaries, Sound Command Business Test Scenarios (+40 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.19
Nodes (16): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+8 more)

### Community 91 - ".OnStartup"
Cohesion: 0.10
Nodes (6): Application, App, EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 92 - "UiThemeState"
Cohesion: 0.22
Nodes (4): v4 / FEAT-011: implementation plan, UiThemeState, Current, TrayMenuWindowTests

### Community 93 - "build-test-publish job"
Cohesion: 0.29
Nodes (6): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip)

### Community 94 - "S4-09 Preview Draft Effects (acceptance)"
Cohesion: 0.10
Nodes (27): S4-09 Preview Draft Effects (acceptance), S4-09 Acceptance criteria, EffectChannelController, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, SettingsEditor (+19 more)

### Community 95 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.18
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (30): S4-04 acceptance: add effects to action slots, S4-04 Acceptance Criteria, EffectAction, EffectCatalog / BuiltInEffect, SettingsSnapshot.Validate, StartEffectCommand, S4-04 plan: add effects to action slots, S4-04 Implementation Plan (+22 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.20
Nodes (15): S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsEditor.SelectEffect / ClearSlot (+7 more)

### Community 99 - "ManualTimeProvider"
Cohesion: 0.18
Nodes (5): ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 100 - ".Attach"
Cohesion: 0.22
Nodes (5): GlassBackdrop, Supported, UiTheme, Dark, Light

### Community 101 - "FEAT-009-effect-loop-and-limit/task.md"
Cohesion: 0.17
Nodes (11): v4 / FEAT-009: acceptance criteria, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior (+3 more)

### Community 102 - "Lottie built-in effects via SkiaSharp.Skottie"
Cohesion: 0.22
Nodes (11): Built-in effect, Effect, CONTEXT.md domain glossary, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, SkiaSharp allowed only in Presentation (+3 more)

### Community 103 - "R4-06 Velopack Installer Plan"
Cohesion: 0.16
Nodes (12): SignPath Foundation, SignPath Foundation Code Signing, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip, WinGet zip/nested-portable manifest, R4-06 Velopack Installer Plan, CI Velopack packaging smoke (+4 more)

### Community 104 - "drawEM README"
Cohesion: 0.09
Nodes (30): CI workflow (build, test, coverage, publish, Velopack smoke), Velopack packaging smoke step, Build provenance attestation, Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), SHA256SUMS.txt download manifest, Verify tag step (+22 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "DllImport"
Cohesion: 0.19
Nodes (5): NativeMethods, POINT, RECT, NativeMethods, POINT

### Community 107 - "IEffectPlayback"
Cohesion: 0.16
Nodes (8): IEffectPlayback, IEffectSurface, LottieEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown

### Community 108 - "FEAT-011-glass-ui/plan.md"
Cohesion: 0.23
Nodes (9): v4 / FEAT-011: acceptance criteria, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, Previous behavior, Required behavior (+1 more)

### Community 109 - "BuiltInEffectAnimations.cs"
Cohesion: 0.15
Nodes (6): EffectCatalog, BuiltIn, ShortcutKeyKind, Digit, Function, Letter

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 113 - "FakeUpdateService"
Cohesion: 0.13
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 114 - "ICursorPositionSource"
Cohesion: 0.17
Nodes (8): ICursorPositionSource, Win32CursorPositionSource, SettableCursor, Available, Position, Reads, FailingCursorPositionSource, RecoveringCursorPositionSource

### Community 115 - "UpdateController"
Cohesion: 0.17
Nodes (5): IUpdateFailureReporter, UpdateController, RecordingReporter, Lines, UpdateFeedTests

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.10
Nodes (25): S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement) (+17 more)

### Community 117 - "SoundId"
Cohesion: 0.22
Nodes (6): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 119 - ".CreatePieces"
Cohesion: 0.29
Nodes (6): DrawEM.Tests.Domain, Line, Quadratic, StrokePathPiece, StrokeSmoothingGeometry, StrokeSmoothingGeometryTests

### Community 120 - "v3 Step 6 plan"
Cohesion: 0.20
Nodes (12): SettingsFailureDialog, v3 Step 6 acceptance criteria, Sound start lock shared by host and Save, omp orchestrator prompt for Step 6 (PR #39), Agent split: startup, save, media-tests, herdr pane/agent orchestration, omp Orchestrator in herdr, v3 Step 6 plan (+4 more)

### Community 121 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 122 - "RenderLagHarness tool"
Cohesion: 0.36
Nodes (10): F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, F03 synthetic harness results (before/after), RenderLagHarness tool (+2 more)

### Community 124 - "GlassDark"
Cohesion: 0.25
Nodes (5): ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 125 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "TrayMenuCommand"
Cohesion: 0.33
Nodes (5): TrayMenuCommand, Exit, Settings, StopEffects, Update

### Community 128 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 131 - "DecreaseWidthButton"
Cohesion: 0.40
Nodes (5): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, RepeatButton

### Community 132 - "SoundReference"
Cohesion: 0.67
Nodes (3): Sound domain README, Sound Domain README, SoundReference

### Community 134 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.27
Nodes (11): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency (+3 more)

### Community 135 - "DateTimeOffset"
Cohesion: 0.27
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 136 - "LoggingUpdateFailureReporter"
Cohesion: 0.28
Nodes (5): LoggingUpdateFailureReporter, UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 138 - "SettingsSnapshot.cs"
Cohesion: 0.33
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

### Community 139 - ".Load"
Cohesion: 0.15
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 140 - "step-01-draft-release/task.md"
Cohesion: 0.14
Nodes (9): release.yml Release Workflow, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation, R4-01: implementation plan, Task (+1 more)

### Community 150 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 152 - "FakePlayback"
Cohesion: 0.18
Nodes (7): EffectPlaybackException, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 156 - "%LOCALAPPDATA%\drawEM settings directory"
Cohesion: 0.67
Nodes (3): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, VelopackPackageIdentityTests

## Knowledge Gaps
- **558 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+553 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 803 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **21 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `GlobalShortcutAdapter`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `drawEM v3 Roadmap: Drawing and Action Settings`, `Lottie built-in effects via SkiaSharp.Skottie`, `Lottie Simple License`, `S4-12 Acceptance: Publish and Verify on Windows`, `drawEM README`, `Settings Storage README`, `drawEM v4 Parallel Work Packages`, `drawEM v4: out of scope`, `drawEM architecture`, `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `S4-03 Acceptance Criteria`, `Stage 1 sound implementation plan`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.180) - this node is a cross-community bridge._
- **Are the 89 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 89 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _558 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.1391304347826087 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `StartEffectCommand`, `LottiePlayback`, `EffectId`?**
  _High betweenness centrality (0.097) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.05711318795430945 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `drawEM v4 Parallel Work Packages`, `FEAT-005: Assisted E2E Runner`, `v3 Step 6 plan`, `FIX-006: Single Screen Edge`?**
  _High betweenness centrality (0.095) - this node is a cross-community bridge._