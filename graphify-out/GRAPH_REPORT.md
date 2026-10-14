# Graph Report - worktree-silver-river-9b47  (2026-10-14)

## Corpus Check
- 290 files · ~117,675 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2787 nodes · 8335 edges · 122 communities (98 shown, 24 thin omitted)
- Extraction: 82% EXTRACTED · 18% INFERRED · 0% AMBIGUOUS · INFERRED: 1476 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `648c3394`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Fixture
- StrokeRenderElement
- Fixture
- Harness
- S4-05 acceptance: save and load effect assignments
- v3 Step 8: Publish and verify on Windows (task)
- SaveHarness
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Theory
- SettingsViewModel
- .Load
- ScreenPoint
- drawEM architecture
- App.xaml.cs
- SettingsEditorTests
- .Validate
- LoggingSoundFailureReporter
- Fact
- ManualTimeProvider
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- IEffectPlayback
- DrawingSessionController
- SettingsEditor
- ISoundPlayer
- List
- ManagedSoundLibraryTests
- Window
- ShortcutModifiers
- Action
- GlobalShortcutAdapter
- .RunOnDispatcherThread
- Fixture
- Rig
- RuntimeMediaTests
- DrawEM.App.Infrastructure
- DrawEM.App.Domain.Drawing
- FEAT-005: Assisted E2E Runner
- v4: simple implementation steps
- Harness
- .OnStartup
- DrawingModeInputGate
- SoundChannelController
- Stage 1 Step 2 Review
- Fixture
- FakeMonitorBoundsSource
- ManagedSoundLibrary
- SettingsSnapshot
- EffectChannelController
- Win32SessionInterruptionSource
- Harness
- DrawEM.App.Application.Sound
- SlotViewModel
- v1 roadmap: drawing
- drawEM v4 Roadmap: built-in Monitor and Cursor effects
- StartEffectCommand
- .Save
- SoundReference
- SettingsWindow
- DllImport
- S4-11 Acceptance criteria
- LottiePlayback
- DrawEM.App.Domain.Settings
- .Window_BindsEveryFieldWithoutBindingErrors
- DrawEM.Tests.csproj
- drawEM v4: parallel work packages
- BlockingPlayerFactory
- TimeSpan
- system_io
- EffectSurfaceLayout
- RuntimeSettingsTests
- AppFailureLog
- IntPtr
- drawEM v3 Roadmap: Drawing and Action Settings
- .Deserialize
- ShortcutBindingsTests
- EffectMonitorGuard
- .ReleaseStartedResources
- EffectFailure
- IDisposable
- NativeMethods
- NativeFunctionBridgeRuleTests
- Sound infrastructure README
- system_runtime_interopservices
- DrawEM.App.Presentation.Effects
- .Create
- ActionSlot
- EffectChannelController
- drawEM README
- S4-10 Acceptance criteria
- NativeMethods
- Shortcut
- FIX-006: Single Screen Edge
- .Key
- S4-12 Acceptance criteria
- ISoundStartHold
- SettingsErrorCode
- EffectLifecycleTests
- .TryGetBounds
- S4-09 Acceptance criteria
- EffectId
- EffectSurfaceWindow
- FakePicker
- Func
- ADR 0001: Lottie built-in effects
- NotifyIconTrayHost
- TrayApplication.cs
- .TryFind_ReturnsTheEntryForAKnownId
- SingleInstanceGuardTests
- F03 harness results
- Lottie Simple License (Design Barn Inc.)
- FakeDisplayChangeSource
- NativeFunctionBridgeRule
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- DrawEM.App.Presentation.Sound
- ShortcutCaptureRejection
- Orange Brush Stroke Glyph
- Sound domain README
- RecordingNotifications
- Sound presentation placeholder README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 67 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `EffectId` - 61 edges
7. `MonitorBounds` - 60 edges
8. `GlobalShortcutAdapter` - 59 edges
9. `DrawEM.App.Domain.Drawing` - 56 edges
10. `SettingsViewModel` - 54 edges

## Surprising Connections (you probably didn't know these)
- `One fixed placement per effect` --conceptually_related_to--> `Placement (Monitor / Cursor)`  [INFERRED]
  docs/adr/0001-lottie-built-in-effects.md → CONTEXT.md
- `EffectCatalog` --semantically_similar_to--> `Domain/Effects/EffectCatalog`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-05-effect-settings-schema/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md
- `Screen sharing checks` --semantically_similar_to--> `Screen share visibility of effects`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-12-effect-publish/acceptance.md → README.md
- `Harness` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/EffectShortcutTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `Harness` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/ShortcutCaptureTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs

## Import Cycles
- None detected.

## Communities (122 total, 24 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.07
Nodes (31): DrawEM.Tests.Application.Sound, SoundId, Value, SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1 (+23 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (16): DrawingState, Stroke, DrawingStyle, HexColor, IOverlayView, Window, OverlayWindow, PhysicalToLocalTransform (+8 more)

### Community 2 - "Fixture"
Cohesion: 0.14
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 3 - "Harness"
Cohesion: 0.07
Nodes (14): Cancelled, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log (+6 more)

### Community 4 - "S4-05 acceptance: save and load effect assignments"
Cohesion: 0.05
Nodes (45): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+37 more)

### Community 5 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.07
Nodes (53): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Settings schema/version policy, v3 Step 1: Settings snapshot and storage (task), Domain/Application free of WPF, Win32, filesystem types (+45 more)

### Community 6 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 7 - "JsonSettingsStoreTests"
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.16
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (25): IShortcutCapture, BindableBase, SettingsViewModel, CanSave, Clear, Color, DraftColor, Draw (+17 more)

### Community 11 - ".Load"
Cohesion: 0.11
Nodes (15): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+7 more)

### Community 12 - "ScreenPoint"
Cohesion: 0.11
Nodes (14): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, SettableCursor, Available, Position (+6 more)

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 14 - "App.xaml.cs"
Cohesion: 0.07
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, RenderLagHarness, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 16 - ".Validate"
Cohesion: 0.20
Nodes (6): EffectAction, Effect, SoundAction, Sound, SettingsError, SettingsSnapshotTests

### Community 17 - "LoggingSoundFailureReporter"
Cohesion: 0.12
Nodes (8): BackgroundLogWriter, SettingsSaveNotifications, LoggingSoundFailureReporter, SoundFailure, SoundFailureLog, DefaultPath, LoggingSoundFailureReporterTests, SoundFailureLogTests

### Community 19 - "ManualTimeProvider"
Cohesion: 0.10
Nodes (16): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+8 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 22 - "IEffectPlayback"
Cohesion: 0.09
Nodes (17): IEffectPlayback, IEffectSurface, LottieEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown (+9 more)

### Community 23 - "DrawingSessionController"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "SettingsEditor"
Cohesion: 0.09
Nodes (11): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+3 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.06
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 26 - "List"
Cohesion: 0.18
Nodes (7): TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable, TimedOutSound, TrayApplicationTests

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.13
Nodes (8): SoundImportException, JsonSettingsStore, DefaultDirectory, SettingsPath, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 28 - "Window"
Cohesion: 0.05
Nodes (45): CanSave, ChooseText, Clear, Color, Visible, Draw, Effect, Error (+37 more)

### Community 29 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 30 - "Action"
Cohesion: 0.19
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 31 - "GlobalShortcutAdapter"
Cohesion: 0.12
Nodes (7): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, IEffectStartQueue, INeutralKeyEmitter, FailingCursorPositionSource

### Community 32 - ".RunOnDispatcherThread"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - "Rig"
Cohesion: 0.13
Nodes (15): Release, Rig, Keyboard, New, Old, Players, Previous, Saved (+7 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 36 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 37 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (9): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+1 more)

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.13
Nodes (23): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+15 more)

### Community 39 - "v4: simple implementation steps"
Cohesion: 0.13
Nodes (27): Built-in effect catalog (Domain/Effects), Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified (+19 more)

### Community 40 - "Harness"
Cohesion: 0.12
Nodes (13): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+5 more)

### Community 41 - ".OnStartup"
Cohesion: 0.22
Nodes (4): App, ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 42 - "DrawingModeInputGate"
Cohesion: 0.15
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch (+4 more)

### Community 43 - "SoundChannelController"
Cohesion: 0.09
Nodes (12): ISoundFailureReporter, AlwaysCurrent, IsCurrent, IPlayRequest, IsCurrent, SoundChannelController, ActiveSound, Request (+4 more)

### Community 44 - "Stage 1 Step 2 Review"
Cohesion: 0.15
Nodes (21): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+13 more)

### Community 45 - "Fixture"
Cohesion: 0.12
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 46 - "FakeMonitorBoundsSource"
Cohesion: 0.27
Nodes (6): GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource, GlobalMouseInputAdapterTests

### Community 47 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (21): ISoundLibrary, SoundCleanupFailure, EffectCatalog, BuiltIn, SettingsSnapshot, ClearShortcut, Default, DrawShortcut (+13 more)

### Community 49 - "EffectChannelController"
Cohesion: 0.10
Nodes (17): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+9 more)

### Community 50 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 51 - "Harness"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.19
Nodes (4): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, RetiredSoundAssignmentsTests

### Community 53 - "SlotViewModel"
Cohesion: 0.09
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 54 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 55 - "drawEM v4 Roadmap: built-in Monitor and Cursor effects"
Cohesion: 0.14
Nodes (24): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001 Lottie built-in effects, Confetti effect (confetti), CONTEXT.md glossary (+16 more)

### Community 56 - "StartEffectCommand"
Cohesion: 0.13
Nodes (9): StartEffectCommand, EffectPlacement, Cursor, Monitor, EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures (+1 more)

### Community 57 - ".Save"
Cohesion: 0.15
Nodes (9): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult (+1 more)

### Community 58 - "SoundReference"
Cohesion: 0.16
Nodes (12): SoundReference, DisplayName, LibraryFileName, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw (+4 more)

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.24
Nodes (13): EffectMonitorGuard, Hook input ordering and monitor epoch, S4-11 Acceptance criteria, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects, TrayApplication (Stop effects / Exit), Win32DisplayChangeSource (+5 more)

### Community 62 - "LottiePlayback"
Cohesion: 0.13
Nodes (8): EffectPlaybackException, LottiePlayback, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 63 - "DrawEM.App.Domain.Settings"
Cohesion: 0.13
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 64 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.08
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 65 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "BlockingPlayerFactory"
Cohesion: 0.20
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 68 - "TimeSpan"
Cohesion: 0.15
Nodes (8): Attempt, Command, Deadline, OnCompleted, OnFailed, Player, ManualTimer, DueAt

### Community 69 - "system_io"
Cohesion: 0.14
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 70 - "EffectSurfaceLayout"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 71 - "RuntimeSettingsTests"
Cohesion: 0.13
Nodes (9): FakePlayer, Calls, Path, NoFailures, PlayerFactory, Created, RuntimeSettingsTests, LibraryDirectory (+1 more)

### Community 72 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 73 - "IntPtr"
Cohesion: 0.23
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 74 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.16
Nodes (12): SoundSlot, Eight Sound Slots Ctrl+Alt+1..8, v3 Additional Fixes, drawEM v3 Roadmap: Drawing and Action Settings, Eight Numbered Action Slots, Managed Sound Library, Settings Persistence and Startup Fallback, Settings Window (Drawing and Actions tabs) (+4 more)

### Community 77 - "EffectMonitorGuard"
Cohesion: 0.25
Nodes (3): IMonitorBoundsSource, Win32MonitorBoundsSource, EffectMonitorGuard

### Community 79 - "EffectFailure"
Cohesion: 0.33
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 80 - "IDisposable"
Cohesion: 0.22
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 81 - "NativeMethods"
Cohesion: 0.21
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 82 - "NativeFunctionBridgeRuleTests"
Cohesion: 0.14
Nodes (7): AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests, PointerToDelegateProbe

### Community 83 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 86 - ".Create"
Cohesion: 0.21
Nodes (3): ShortcutKey, Kind, Value

### Community 87 - "ActionSlot"
Cohesion: 0.27
Nodes (5): ActionSlot, Action, IsEmpty, Number, SlotAction

### Community 88 - "EffectChannelController"
Cohesion: 0.24
Nodes (11): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback (+3 more)

### Community 89 - "drawEM README"
Cohesion: 0.28
Nodes (9): Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+1 more)

### Community 90 - "S4-10 Acceptance criteria"
Cohesion: 0.22
Nodes (10): GlobalShortcutAdapter, SettingsSaveOperation, Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely (+2 more)

### Community 91 - "NativeMethods"
Cohesion: 0.40
Nodes (3): NativeMethods, POINT, RECT

### Community 92 - "Shortcut"
Cohesion: 0.12
Nodes (11): Captured, SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot (+3 more)

### Community 93 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 95 - "S4-12 Acceptance criteria"
Cohesion: 0.27
Nodes (11): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+3 more)

### Community 97 - "SettingsErrorCode"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 98 - "EffectLifecycleTests"
Cohesion: 0.27
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 100 - "S4-09 Acceptance criteria"
Cohesion: 0.33
Nodes (9): SettingsEditor, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects (+1 more)

### Community 101 - "EffectId"
Cohesion: 0.21
Nodes (5): EffectId, Value, BuiltInEffectAnimations, BuiltInEffectAnimationsTests, BuiltInIds

### Community 103 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 104 - "Func"
Cohesion: 0.18
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "NotifyIconTrayHost"
Cohesion: 0.22
Nodes (3): NotifyIconTrayHost, IconUri, ITrayHost

### Community 107 - "TrayApplication.cs"
Cohesion: 0.20
Nodes (4): Application, IApplicationLifetime, IOverlayLifetime, WpfApplicationLifetime

### Community 110 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 111 - "Lottie Simple License (Design Barn Inc.)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 115 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 116 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

### Community 118 - "ShortcutCaptureRejection"
Cohesion: 0.50
Nodes (4): Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers

## Knowledge Gaps
- **456 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+451 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 631 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SettingsWindow` connect `SettingsWindow` to `.Window_BindsEveryFieldWithoutBindingErrors`, `EffectSurfaceWindow`, `FakePicker`, `.OnStartup`, `SettingsViewModel`, `App.xaml.cs`, `Window`?**
  _High betweenness centrality (0.126) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _456 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.0673076923076923 - nodes in this community are weakly interconnected._
- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-05 acceptance: save and load effect assignments`?**
  _High betweenness centrality (0.123) - this node is a cross-community bridge._
- **Are the 40 inferred relationships involving `DrawingSessionController` (e.g. with `.ClearAndExitDrawMode_RemovesCompletedAndActiveStrokes()` and `.ClearMonitor_PreservesStrokesOnOtherMonitor()`) actually correct?**
  _`DrawingSessionController` has 40 INFERRED edges - model-reasoned connections that need verification._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.082365003417635 - nodes in this community are weakly interconnected._