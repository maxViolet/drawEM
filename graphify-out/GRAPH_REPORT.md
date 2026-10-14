# Graph Report - worktree-silver-river-9b47  (2026-10-14)

## Corpus Check
- 308 files · ~137,486 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2824 nodes · 8401 edges · 109 communities (95 shown, 14 thin omitted)
- Extraction: 82% EXTRACTED · 18% INFERRED · 0% AMBIGUOUS · INFERRED: 1479 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `01f2aa9a`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Fixture
- StrokeRenderElement
- Fixture
- Harness
- S4-05 acceptance: save and load effect assignments
- drawEM v3 Roadmap: Drawing and Action Settings
- SaveHarness
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Theory
- SettingsViewModel
- Rig
- MonitorBounds
- drawEM architecture
- SettingsViewModel.cs
- SettingsEditorTests
- .Validate
- TimeSpan
- Fact
- ManualTimeProvider
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- EffectChannelController
- ScreenPoint
- SettingsEditor
- ISoundPlayer
- TrayApplication
- ManagedSoundLibraryTests
- Window
- Shortcut
- Action
- Program.cs
- List
- Fixture
- FakeKeyboardHookSource
- RuntimeMediaTests
- DrawEM.App.Infrastructure
- DrawEM.App.Domain.Drawing
- FEAT-005: Assisted E2E Runner
- v4: simple implementation steps
- Harness
- LoggingSoundFailureReporter
- GlobalShortcutAdapter
- SoundChannelController
- Stage 1 Step 2 Review
- Fixture
- DrawingModeInputGate
- ManagedSoundLibrary
- SettingsSnapshot
- .Load
- Win32SessionInterruptionSource
- Harness
- DrawEM.App.Application.Sound
- SlotViewModel
- v1 roadmap: drawing
- drawEM v4 Roadmap: built-in Monitor and Cursor effects
- StartEffectCommand
- .Save
- .Create
- NativeFunctionBridgeRuleTests
- .Deserialize
- S4-11 Acceptance criteria
- EffectId
- RuntimeMediaTests.cs
- .Window_BindsEveryFieldWithoutBindingErrors
- DrawEM.Tests.csproj
- drawEM v4: parallel work packages
- v4 / FEAT-009: implementation plan
- FakePlayer
- EffectCatalog
- .ReleaseStartedResources
- ShortcutBindings
- SoundReference
- JsonSettingsStore
- SettingsCommand
- EffectMonitorGuard
- LottiePlayback
- S4-10 Acceptance criteria
- WidthBox
- SoundSlot
- IDisposable
- IntPtr
- Win32DisplayChangeSource
- Sound infrastructure README
- system_runtime_interopservices
- App.xaml.cs
- DrawEM.App.Domain.Effects
- SettableCursor
- EffectChannelController
- drawEM README
- FIX-006: Single Screen Edge
- S4-12 Acceptance criteria
- SettingsErrorCode
- S4-09 Acceptance criteria
- FakePicker
- Func
- ADR 0001: Lottie built-in effects
- NotifyIconTrayHost
- .OnStartup
- SingleInstanceGuard
- F03 harness results
- Lottie Simple License (Design Barn Inc.)
- FakeSoundChannel
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- DrawEM.App.Presentation.Sound
- Orange Brush Stroke Glyph
- Sound domain README
- Sound presentation placeholder README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 68 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `EffectId` - 61 edges
7. `MonitorBounds` - 60 edges
8. `GlobalShortcutAdapter` - 59 edges
9. `SettingsViewModel` - 57 edges
10. `DrawEM.App.Domain.Drawing` - 56 edges

## Surprising Connections (you probably didn't know these)
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-009: implementation plan` --references--> `LottiePlayback`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Presentation/Effects/LottiePlayback.cs
- `One fixed placement per effect` --conceptually_related_to--> `Placement (Monitor / Cursor)`  [INFERRED]
  docs/adr/0001-lottie-built-in-effects.md → CONTEXT.md
- `EffectCatalog` --semantically_similar_to--> `Domain/Effects/EffectCatalog`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-05-effect-settings-schema/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md

## Import Cycles
- None detected.

## Communities (109 total, 14 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.14
Nodes (13): DrawEM.Tests.Application.Sound, SoundId, Value, FakeRequest, IsCurrent, Fixture, Controller, Failures (+5 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (16): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, PhysicalToLocalTransform (+8 more)

### Community 2 - "Fixture"
Cohesion: 0.14
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 3 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 4 - "S4-05 acceptance: save and load effect assignments"
Cohesion: 0.05
Nodes (45): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+37 more)

### Community 5 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 6 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 7 - "JsonSettingsStoreTests"
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.14
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 9 - "Theory"
Cohesion: 0.07
Nodes (10): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule (+2 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.07
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "Rig"
Cohesion: 0.08
Nodes (25): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Release (+17 more)

### Community 12 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, EffectSurfaceWindow, Layout, CountingMonitorBoundsSource, Queries (+2 more)

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 15 - "SettingsEditorTests"
Cohesion: 0.16
Nodes (3): SoundAction, Sound, SettingsEditorTests

### Community 16 - ".Validate"
Cohesion: 0.13
Nodes (10): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SettingsError, SettingsValidation (+2 more)

### Community 19 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 22 - "EffectChannelController"
Cohesion: 0.06
Nodes (32): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+24 more)

### Community 23 - "ScreenPoint"
Cohesion: 0.19
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "SettingsEditor"
Cohesion: 0.10
Nodes (9): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+1 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.08
Nodes (15): ISoundPlayer, ISoundPlayerFactory, MediaSoundPlayerFactory, FakePlayer, Calls, Sound, FakePlayerFactory, Created (+7 more)

### Community 26 - "TrayApplication"
Cohesion: 0.14
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.18
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 28 - "Window"
Cohesion: 0.05
Nodes (40): CanSave, ChooseText, Clear, Color, Visible, Draw, Effect, Error (+32 more)

### Community 29 - "Shortcut"
Cohesion: 0.12
Nodes (17): SlotAction, Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers (+9 more)

### Community 30 - "Action"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 32 - "List"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - "FakeKeyboardHookSource"
Cohesion: 0.11
Nodes (10): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+2 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 36 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread

### Community 37 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.14
Nodes (10): DrawEM.App.Domain.Settings, DrawEM.App.Application.Drawing, DrawEM.Tests.Application.Settings, DrawEM.App.Infrastructure.Drawing, DrawEM.App.Domain.Sound, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Drawing (+2 more)

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 39 - "v4: simple implementation steps"
Cohesion: 0.13
Nodes (27): Built-in effect catalog (Domain/Effects), Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified (+19 more)

### Community 40 - "Harness"
Cohesion: 0.10
Nodes (16): EffectLifecycleTests, Events, Interruptions, Harness, Controller, Displays, EffectChannel, Effects (+8 more)

### Community 41 - "LoggingSoundFailureReporter"
Cohesion: 0.05
Nodes (21): AppFailureLog, DefaultPath, ApplicationExitPolicy, BackgroundLogWriter, EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter (+13 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.09
Nodes (13): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+5 more)

### Community 43 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 44 - "Stage 1 Step 2 Review"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 45 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 46 - "DrawingModeInputGate"
Cohesion: 0.20
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 47 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.09
Nodes (19): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+11 more)

### Community 49 - ".Load"
Cohesion: 0.18
Nodes (11): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, FakeReporter, Reports (+3 more)

### Community 50 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 51 - "Harness"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.17
Nodes (6): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots

### Community 53 - "SlotViewModel"
Cohesion: 0.07
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 54 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 55 - "drawEM v4 Roadmap: built-in Monitor and Cursor effects"
Cohesion: 0.14
Nodes (24): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001 Lottie built-in effects, Confetti effect (confetti), CONTEXT.md glossary (+16 more)

### Community 56 - "StartEffectCommand"
Cohesion: 0.16
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, SamplingEditor, Failures, Sampler

### Community 57 - ".Save"
Cohesion: 0.13
Nodes (8): IActiveShortcuts, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult, IdleSoundChannel

### Community 58 - ".Create"
Cohesion: 0.18
Nodes (3): ShortcutKey, Kind, Value

### Community 59 - "NativeFunctionBridgeRuleTests"
Cohesion: 0.14
Nodes (7): AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests, PointerToDelegateProbe

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.24
Nodes (13): EffectMonitorGuard, Hook input ordering and monitor epoch, S4-11 Acceptance criteria, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects, TrayApplication (Stop effects / Exit), Win32DisplayChangeSource (+5 more)

### Community 62 - "EffectId"
Cohesion: 0.14
Nodes (7): DrawEM.Tests.Domain.Effects, EffectId, Value, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 64 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.08
Nodes (17): Invalid, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops (+9 more)

### Community 65 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.18
Nodes (9): Evidence, v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Current behavior, Documentation, Playback, Required behavior, Settings file (+1 more)

### Community 68 - "FakePlayer"
Cohesion: 0.12
Nodes (8): SoundPlaybackException, MediaSoundPlayer, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 69 - "EffectCatalog"
Cohesion: 0.22
Nodes (6): EffectCatalog, BuiltIn, ShortcutKeyKind, Digit, Function, Letter

### Community 71 - "ShortcutBindings"
Cohesion: 0.09
Nodes (20): SettingsStartup, StartupSettings, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects (+12 more)

### Community 72 - "SoundReference"
Cohesion: 0.24
Nodes (5): SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, ShortcutBindingsTests

### Community 73 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 74 - "SettingsCommand"
Cohesion: 0.21
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 75 - "EffectMonitorGuard"
Cohesion: 0.22
Nodes (4): EffectPlacement, Cursor, Monitor, EffectMonitorGuard

### Community 76 - "LottiePlayback"
Cohesion: 0.11
Nodes (9): EffectPlaybackException, LottieEffectSurface, LottiePlayback, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler (+1 more)

### Community 77 - "S4-10 Acceptance criteria"
Cohesion: 0.22
Nodes (10): GlobalShortcutAdapter, SettingsSaveOperation, Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely (+2 more)

### Community 78 - "WidthBox"
Cohesion: 0.29
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 79 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 80 - "IDisposable"
Cohesion: 0.22
Nodes (5): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, TimedOutSound

### Community 81 - "IntPtr"
Cohesion: 0.07
Nodes (19): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource, INPUT (+11 more)

### Community 83 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 85 - "App.xaml.cs"
Cohesion: 0.10
Nodes (5): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Effects, DrawEM.App.Presentation.Drawing, DrawEM.Tests.Presentation.Effects, DrawEM.App

### Community 86 - "DrawEM.App.Domain.Effects"
Cohesion: 0.11
Nodes (7): DrawEM.App.Domain.Effects, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects, IDisplayChangeSource, FakeDisplayChangeSource

### Community 87 - "SettableCursor"
Cohesion: 0.40
Nodes (4): SettableCursor, Available, Position, Reads

### Community 88 - "EffectChannelController"
Cohesion: 0.24
Nodes (11): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback (+3 more)

### Community 89 - "drawEM README"
Cohesion: 0.28
Nodes (9): Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+1 more)

### Community 93 - "FIX-006: Single Screen Edge"
Cohesion: 0.38
Nodes (9): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+1 more)

### Community 95 - "S4-12 Acceptance criteria"
Cohesion: 0.27
Nodes (11): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+3 more)

### Community 97 - "SettingsErrorCode"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 100 - "S4-09 Acceptance criteria"
Cohesion: 0.33
Nodes (9): SettingsEditor, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects (+1 more)

### Community 103 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 104 - "Func"
Cohesion: 0.23
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "NotifyIconTrayHost"
Cohesion: 0.17
Nodes (4): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri, NotifyIconTrayHostTests

### Community 107 - ".OnStartup"
Cohesion: 0.10
Nodes (12): CanDecreaseWidth, CanIncreaseWidth, Application, App, WpfApplicationLifetime, Window, OverlayWindow, DecreaseWidthButton (+4 more)

### Community 109 - "SingleInstanceGuard"
Cohesion: 0.22
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 110 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 111 - "Lottie Simple License (Design Barn Inc.)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 113 - "FakeSoundChannel"
Cohesion: 0.15
Nodes (4): ISoundStartHold, StartHold, FakeSoundChannel, StopConfirmed

### Community 115 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 116 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **462 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+457 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 641 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **14 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Window` to `.OnStartup`, `S4-05 acceptance: save and load effect assignments`, `WidthBox`?**
  _High betweenness centrality (0.121) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _462 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.13900134952766532 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `.OnStartup` to `.Window_BindsEveryFieldWithoutBindingErrors`, `FakePicker`, `SettingsViewModel`, `SettingsViewModel.cs`, `WidthBox`, `SlotViewModel`, `Window`?**
  _High betweenness centrality (0.113) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.08421052631578947 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.088) - this node is a cross-community bridge._