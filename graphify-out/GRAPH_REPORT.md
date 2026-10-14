# Graph Report - worktree-silver-river-9b47  (2026-10-14)

## Corpus Check
- 290 files · ~117,417 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2782 nodes · 8300 edges · 119 communities (104 shown, 15 thin omitted)
- Extraction: 82% EXTRACTED · 18% INFERRED · 0% AMBIGUOUS · INFERRED: 1461 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `873dce00`
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
- InlineData
- SettingsViewModel
- SettingsViewModel.cs
- MonitorBounds
- drawEM architecture
- App.xaml.cs
- SettingsEditorTests
- .Validate
- LoggingSoundFailureReporter
- Fact
- SoundChannelHost
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- FakePlayback
- ScreenPoint
- SettingsEditor
- ISoundPlayer
- List
- ManagedSoundLibraryTests
- Window
- ShortcutModifiers
- Action
- GlobalShortcutAdapter
- BuiltInEffectAnimations
- Fixture
- Rig
- RuntimeMediaTests
- DrawEM.Tests.Infrastructure
- DrawEM.App.Domain.Effects
- FEAT-005: Assisted E2E Runner
- v4: simple implementation steps
- Harness
- .OnStartup
- SoundReference
- SoundChannelController
- Stage 1 Step 2 Review
- Fixture
- DrawingModeInputGate
- ManagedSoundLibrary
- SettingsSnapshot
- EffectChannelController
- App
- Harness
- DrawEM.App.Application.Sound
- SlotViewModel
- v1 roadmap: drawing
- drawEM v4 Roadmap: built-in Monitor and Cursor effects
- StartEffectCommand
- .Save
- ShortcutBindings
- ShortcutBindingsTests
- NativeMethods
- S4-11 Acceptance criteria
- EffectSurfaceWindow
- DrawEM.App.Domain.Settings
- SettingsWindow
- DrawEM.Tests.csproj
- drawEM v4: parallel work packages
- ShortcutFieldViewModel
- ManualTimeProvider
- Program.cs
- EffectSurfaceLayout
- MediaSoundPlayer
- EffectMonitorGuard
- IntPtr
- drawEM v3 Roadmap: Drawing and Action Settings
- .Deserialize
- SoundId
- SoundSlot
- .RunAll
- LoggingEffectFailureReporter
- IDisposable
- Win32MouseHookSource
- NativeFunctionBridgeRuleTests
- Sound infrastructure README
- system_runtime_interopservices
- DrawEM.App.Domain.Drawing
- .Digit
- SoundFailure
- EffectChannelController
- drawEM README
- S4-10 Acceptance criteria
- DllImport
- SettingsSnapshot.cs
- FIX-006: Single Screen Edge
- .Key
- S4-12 Acceptance criteria
- FakeSampler
- SettingsErrorCode
- Theory
- FakePlayer
- S4-09 Acceptance criteria
- EffectId
- FakeCapture
- FakePicker
- KeyDirection
- ADR 0001: Lottie built-in effects
- FakeEffectSampler
- ErrorsText
- HexBox
- Task
- F03 harness results
- Lottie Simple License (Design Barn Inc.)
- SettableCursor
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- DrawEM.App.Presentation.Sound
- Orange Brush Stroke Glyph
- Sound domain README
- Sound presentation placeholder README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 67 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `MonitorBounds` - 60 edges
7. `EffectId` - 60 edges
8. `GlobalShortcutAdapter` - 59 edges
9. `DrawEM.App.Domain.Drawing` - 56 edges
10. `Harness` - 54 edges

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

## Communities (119 total, 15 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.17
Nodes (11): DrawEM.Tests.Application.Sound, FakeRequest, IsCurrent, Fixture, Controller, Failures, Log, OnCreate (+3 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.07
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "Fixture"
Cohesion: 0.14
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 3 - "Harness"
Cohesion: 0.06
Nodes (18): Cancelled, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness (+10 more)

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
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (16): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+8 more)

### Community 9 - "InlineData"
Cohesion: 0.13
Nodes (4): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, AppIconTests

### Community 10 - "SettingsViewModel"
Cohesion: 0.09
Nodes (14): SettingsViewModel, CanSave, Clear, Color, DraftColor, Draw, Errors, Fields (+6 more)

### Community 12 - "MonitorBounds"
Cohesion: 0.13
Nodes (8): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests, EffectRenderLayoutTests

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 14 - "App.xaml.cs"
Cohesion: 0.12
Nodes (3): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 16 - ".Validate"
Cohesion: 0.17
Nodes (8): EffectAction, Effect, SoundAction, Sound, SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 17 - "LoggingSoundFailureReporter"
Cohesion: 0.14
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 19 - "SoundChannelHost"
Cohesion: 0.17
Nodes (9): SoundChannelHost, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay, Played, Player (+1 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 22 - "FakePlayback"
Cohesion: 0.07
Nodes (24): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+16 more)

### Community 23 - "ScreenPoint"
Cohesion: 0.18
Nodes (9): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, RecoveringCursorPositionSource, FakeOverlayView, LastRenderedState (+1 more)

### Community 24 - "SettingsEditor"
Cohesion: 0.08
Nodes (20): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+12 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.08
Nodes (13): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, PlayerFactory, Created, ThreadRecordingPlayer (+5 more)

### Community 26 - "List"
Cohesion: 0.06
Nodes (18): IMouseHookSource, NotifyIconTrayHost, IconUri, IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime (+10 more)

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.08
Nodes (19): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, SoundImportException, ISettingsFailureReporter, JsonSettingsStore (+11 more)

### Community 28 - "Window"
Cohesion: 0.06
Nodes (35): CanSave, ChooseText, Clear, Color, Visible, Draw, Effect, Error (+27 more)

### Community 29 - "ShortcutModifiers"
Cohesion: 0.11
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 30 - "Action"
Cohesion: 0.13
Nodes (9): ISoundFailureReporter, PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingSoundFailures, IgnoringReporter, ThrowingReporter, RecordingReporter (+1 more)

### Community 31 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (8): Captured, GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, IEffectStartQueue, INeutralKeyEmitter, FailingCursorPositionSource

### Community 32 - "BuiltInEffectAnimations"
Cohesion: 0.29
Nodes (3): BuiltInEffectAnimations, BuiltInEffectAnimationsTests, BuiltInIds

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - "Rig"
Cohesion: 0.08
Nodes (24): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Release (+16 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.15
Nodes (13): ActiveSettings, Current, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig (+5 more)

### Community 36 - "DrawEM.Tests.Infrastructure"
Cohesion: 0.12
Nodes (4): DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, StaThread

### Community 37 - "DrawEM.App.Domain.Effects"
Cohesion: 0.14
Nodes (7): DrawEM.App.Domain.Effects, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects, IDisplayChangeSource, FakeDisplayChangeSource

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.13
Nodes (23): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+15 more)

### Community 39 - "v4: simple implementation steps"
Cohesion: 0.13
Nodes (27): Built-in effect catalog (Domain/Effects), Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified (+19 more)

### Community 40 - "Harness"
Cohesion: 0.14
Nodes (12): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+4 more)

### Community 41 - ".OnStartup"
Cohesion: 0.15
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 42 - "SoundReference"
Cohesion: 0.14
Nodes (7): IActiveShortcuts, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, MediaSoundPlayerFactoryTests

### Community 43 - "SoundChannelController"
Cohesion: 0.09
Nodes (16): AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed, Player (+8 more)

### Community 44 - "Stage 1 Step 2 Review"
Cohesion: 0.15
Nodes (21): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+13 more)

### Community 45 - "Fixture"
Cohesion: 0.12
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 46 - "DrawingModeInputGate"
Cohesion: 0.17
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+4 more)

### Community 47 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.12
Nodes (17): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DrawShortcut, Slots, Style (+9 more)

### Community 49 - "EffectChannelController"
Cohesion: 0.11
Nodes (16): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+8 more)

### Community 50 - "App"
Cohesion: 0.08
Nodes (7): Application, App, EffectStopRoutes, ISessionInterruptionSource, Win32DisplayChangeSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 51 - "Harness"
Cohesion: 0.12
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.14
Nodes (5): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.Tests.Application.Settings, DrawEM.App.Application.Sound, DrawEM.App.Application.Settings

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
Cohesion: 0.10
Nodes (10): Invalid, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult, IdleSoundChannel (+2 more)

### Community 58 - "ShortcutBindings"
Cohesion: 0.13
Nodes (15): SettingsStartup, StartupSettings, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects (+7 more)

### Community 60 - "NativeMethods"
Cohesion: 0.18
Nodes (6): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.24
Nodes (13): EffectMonitorGuard, Hook input ordering and monitor epoch, S4-11 Acceptance criteria, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects, TrayApplication (Stop effects / Exit), Win32DisplayChangeSource (+5 more)

### Community 62 - "EffectSurfaceWindow"
Cohesion: 0.15
Nodes (3): EffectSurfaceWindow, Layout, LottiePlayback

### Community 63 - "DrawEM.App.Domain.Settings"
Cohesion: 0.18
Nodes (10): DrawEM.App.Infrastructure, DrawEM.App.Domain.Settings, DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.App.Domain.Sound, DrawEM.Tests.Infrastructure.Drawing, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Settings (+2 more)

### Community 64 - "SettingsWindow"
Cohesion: 0.16
Nodes (6): SettingsWindow, ViewModel, FakeSettingsSave, Result, Saved, SettingsWindowTests

### Community 65 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "ShortcutFieldViewModel"
Cohesion: 0.14
Nodes (10): BindableBase, ShortcutFieldViewModel, Command, Error, IsCapturing, IsEnabled, Message, Shortcut (+2 more)

### Community 68 - "ManualTimeProvider"
Cohesion: 0.16
Nodes (5): ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 69 - "Program.cs"
Cohesion: 0.08
Nodes (6): RenderLagHarness, DrawEM.App.Infrastructure.Settings, ShortcutKeyKind, Digit, Function, Letter

### Community 70 - "EffectSurfaceLayout"
Cohesion: 0.22
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 71 - "MediaSoundPlayer"
Cohesion: 0.17
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 73 - "IntPtr"
Cohesion: 0.24
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 74 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.16
Nodes (12): SoundSlot, Eight Sound Slots Ctrl+Alt+1..8, v3 Additional Fixes, drawEM v3 Roadmap: Drawing and Action Settings, Eight Numbered Action Slots, Managed Sound Library, Settings Persistence and Startup Fallback, Settings Window (Drawing and Actions tabs) (+4 more)

### Community 76 - "SoundId"
Cohesion: 0.14
Nodes (8): SoundId, Value, FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop

### Community 77 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 78 - ".RunAll"
Cohesion: 0.17
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 79 - "LoggingEffectFailureReporter"
Cohesion: 0.26
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, EffectFailureLogTests

### Community 80 - "IDisposable"
Cohesion: 0.12
Nodes (7): ISoundStartHold, CompositeDisposable, StartHold, SoundChannelShutdownTimeoutException, CompositeDisposableTests, FakeDisposable, TimedOutSound

### Community 82 - "NativeFunctionBridgeRuleTests"
Cohesion: 0.18
Nodes (6): ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests, PointerToDelegateProbe

### Community 83 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 85 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.17
Nodes (4): DrawEM.App.Presentation.Effects, DrawEM.App.Domain.Drawing, DrawEM.Tests.Presentation.Effects, RetiredSoundAssignmentsTests

### Community 86 - ".Digit"
Cohesion: 0.38
Nodes (3): ShortcutKey, Kind, Value

### Community 87 - "SoundFailure"
Cohesion: 0.28
Nodes (5): SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, SoundFailureLogTests

### Community 88 - "EffectChannelController"
Cohesion: 0.24
Nodes (11): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback (+3 more)

### Community 89 - "drawEM README"
Cohesion: 0.28
Nodes (9): Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+1 more)

### Community 90 - "S4-10 Acceptance criteria"
Cohesion: 0.22
Nodes (10): GlobalShortcutAdapter, SettingsSaveOperation, Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely (+2 more)

### Community 91 - "DllImport"
Cohesion: 0.21
Nodes (4): NativeMethods, POINT, RECT, NativeMethods

### Community 92 - "SettingsSnapshot.cs"
Cohesion: 0.29
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

### Community 93 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 95 - "S4-12 Acceptance criteria"
Cohesion: 0.27
Nodes (11): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+3 more)

### Community 96 - "FakeSampler"
Cohesion: 0.25
Nodes (5): FakeSampler, Forgets, Samples, StopConfirmed, Stops

### Community 97 - "SettingsErrorCode"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 98 - "Theory"
Cohesion: 0.26
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 99 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 100 - "S4-09 Acceptance criteria"
Cohesion: 0.33
Nodes (9): SettingsEditor, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects (+1 more)

### Community 101 - "EffectId"
Cohesion: 0.21
Nodes (6): DrawEM.Tests.Domain.Effects, EffectCatalog, BuiltIn, EffectId, Value, EffectCatalogTests

### Community 102 - "FakeCapture"
Cohesion: 0.25
Nodes (4): IShortcutCapture, FakeCapture, Current, Ended

### Community 103 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 104 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "FakeEffectSampler"
Cohesion: 0.29
Nodes (4): FakeEffectSampler, Forgets, Samples, Stops

### Community 107 - "ErrorsText"
Cohesion: 0.40
Nodes (5): Errors, Status, ErrorsText, StatusText, TextBlock

### Community 108 - "HexBox"
Cohesion: 0.40
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 110 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 111 - "Lottie Simple License (Design Barn Inc.)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 112 - "SettableCursor"
Cohesion: 0.40
Nodes (4): SettableCursor, Available, Position, Reads

### Community 115 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 116 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **456 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+451 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 629 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **15 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Window` to `HexBox`, `SettingsWindow`, `ErrorsText`, `S4-05 acceptance: save and load effect assignments`?**
  _High betweenness centrality (0.113) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _456 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07058823529411765 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `drawEM v3 Roadmap: Drawing and Action Settings`, `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.113) - this node is a cross-community bridge._
- **Are the 40 inferred relationships involving `DrawingSessionController` (e.g. with `.ClearAndExitDrawMode_RemovesCompletedAndActiveStrokes()` and `.ClearMonitor_PreservesStrokesOnOtherMonitor()`) actually correct?**
  _`DrawingSessionController` has 40 INFERRED edges - model-reasoned connections that need verification._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.14204545454545456 - nodes in this community are weakly interconnected._