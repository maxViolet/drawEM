# Graph Report - worktree-silver-river-9b47  (2026-10-15)

## Corpus Check
- 309 files · ~140,353 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2895 nodes · 8625 edges · 118 communities (99 shown, 19 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1502 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `88358b46`
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
- SettingsEditorTests
- .Validate
- TimeSpan
- Fact
- ManualTimeProvider
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- FakePlayback
- ScreenPoint
- SettingsEditor
- ISoundPlayer
- TrayApplication
- ManagedSoundLibraryTests
- Window
- ShortcutModifiers
- PlaySoundCommand
- EffectChannelController
- List
- Fixture
- ICursorPositionSource
- RuntimeMediaTests
- DrawEM.App.Infrastructure
- DrawEM.App.Domain.Settings
- FEAT-005: Assisted E2E Runner
- S4-03 acceptance: play built-in Lottie effects
- Harness
- EffectSurfaceLayout
- GlobalShortcutAdapter
- SoundChannelController
- Stage 1 Step 2 Review
- Fixture
- DrawingModeInputGate
- ManagedSoundLibrary
- SettingsSnapshot
- ShortcutBindingsTests
- .OnStartup
- Harness
- DrawEM.App.Application.Sound
- SlotViewModel
- v1 roadmap: drawing
- drawEM v4 Roadmap: built-in Monitor and Cursor effects
- Action
- .Save
- LoggingSoundFailureReporter
- AppFailureLog
- .Deserialize
- S4-11 Acceptance criteria
- EffectId
- NativeMethods
- .Window_BindsEveryFieldWithoutBindingErrors
- DrawEM.Tests.csproj
- drawEM v4: parallel work packages
- v4 / FEAT-009: implementation plan
- EffectLifecycleTests
- DrawEM.App.Presentation.Effects
- DllImport
- ShortcutBindings
- SoundReference
- BuiltInEffectAnimations.cs
- SettingsCommand
- StartEffectCommand
- LottiePlayback
- EffectSurfaceWindow
- .Key
- SoundSlot
- IDisposable
- IntPtr
- Sound infrastructure README
- LayerDependencyTests.cs
- App.xaml.cs
- DrawEM.App.Domain.Drawing
- .Digit
- EffectChannelController
- drawEM README
- .TryGetBounds
- SettingsErrorCode
- WidthBox
- FIX-006: Single Screen Edge
- S4-01 acceptance: prove the effect window
- S4-12 Acceptance criteria
- EffectAction
- BlockingPlayerFactory
- NativeFunctionBridgeRule
- v4: simple implementation steps
- RecordingNotifications
- FakeDisplayChangeSource
- FakePicker
- KeyDirection
- ADR 0001: Lottie built-in effects
- NotifyIconTrayHost
- SettingsWindow
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
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `MonitorBounds` - 60 edges
7. `EffectId` - 60 edges
8. `GlobalShortcutAdapter` - 59 edges
9. `SettingsViewModel` - 59 edges
10. `DrawEM.App.Domain.Drawing` - 56 edges

## Surprising Connections (you probably didn't know these)
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-009: implementation plan` --references--> `LottiePlayback`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Presentation/Effects/LottiePlayback.cs

## Import Cycles
- None detected.

## Communities (118 total, 19 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 3 - "Harness"
Cohesion: 0.07
Nodes (15): ShortcutCaptureRejection, RightAlt, TooFewModifiers, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate (+7 more)

### Community 4 - "S4-05 acceptance: save and load effect assignments"
Cohesion: 0.05
Nodes (44): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+36 more)

### Community 5 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 6 - "SaveHarness"
Cohesion: 0.08
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 7 - "JsonSettingsStoreTests"
Cohesion: 0.07
Nodes (20): ISettingsStore, Loaded, Missing, SettingsLoadResult, SettingsStoreException, Unreadable, ISettingsFailureReporter, SettingsStartup (+12 more)

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (17): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+9 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 12 - "MonitorBounds"
Cohesion: 0.12
Nodes (8): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests, EffectRenderLayoutTests

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 16 - ".Validate"
Cohesion: 0.18
Nodes (4): SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 19 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 22 - "FakePlayback"
Cohesion: 0.07
Nodes (24): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+16 more)

### Community 23 - "ScreenPoint"
Cohesion: 0.18
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "SettingsEditor"
Cohesion: 0.09
Nodes (12): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+4 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.06
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 26 - "TrayApplication"
Cohesion: 0.14
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.13
Nodes (8): SoundImportException, JsonSettingsStore, DefaultDirectory, SettingsPath, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 28 - "Window"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit, ChooseText (+46 more)

### Community 29 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 30 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 31 - "EffectChannelController"
Cohesion: 0.10
Nodes (17): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+9 more)

### Community 32 - "List"
Cohesion: 0.23
Nodes (5): FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - "ICursorPositionSource"
Cohesion: 0.14
Nodes (9): ICursorPositionSource, Win32CursorPositionSource, SettableCursor, Available, Position, Reads, MutableCursorPositionSource, Position (+1 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 36 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 37 - "DrawEM.App.Domain.Settings"
Cohesion: 0.11
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Domain.Settings

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 39 - "S4-03 acceptance: play built-in Lottie effects"
Cohesion: 0.15
Nodes (19): Built-in effect catalog (Domain/Effects), GlobalShortcutAdapter, Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations, EffectFailureLog / LoggingEffectFailureReporter (+11 more)

### Community 40 - "Harness"
Cohesion: 0.14
Nodes (12): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+4 more)

### Community 41 - "EffectSurfaceLayout"
Cohesion: 0.21
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.12
Nodes (7): Cancelled, Captured, Rejected, ShortcutCaptureResult, GlobalShortcutAdapter, IsCapturing, INeutralKeyEmitter

### Community 43 - "SoundChannelController"
Cohesion: 0.10
Nodes (16): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+8 more)

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
Cohesion: 0.21
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.12
Nodes (18): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+10 more)

### Community 50 - ".OnStartup"
Cohesion: 0.10
Nodes (7): Application, App, EffectStopRoutes, ISessionInterruptionSource, Win32DisplayChangeSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 51 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.15
Nodes (6): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots

### Community 53 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 54 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 55 - "drawEM v4 Roadmap: built-in Monitor and Cursor effects"
Cohesion: 0.14
Nodes (24): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001 Lottie built-in effects, Confetti effect (confetti), CONTEXT.md glossary (+16 more)

### Community 56 - "Action"
Cohesion: 0.09
Nodes (13): EffectSampleFailureRouter, EffectSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+5 more)

### Community 57 - ".Save"
Cohesion: 0.18
Nodes (8): IActiveShortcuts, Invalid, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult, Saved

### Community 58 - "LoggingSoundFailureReporter"
Cohesion: 0.08
Nodes (14): BackgroundLogWriter, EffectFailure, EffectFailureLog, DefaultPath, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, SoundFailure (+6 more)

### Community 59 - "AppFailureLog"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.19
Nodes (14): EffectMonitorGuard, Hook input ordering and monitor epoch, Configuration generation, S4-11 Acceptance criteria, LoggingEffectFailureReporter / EffectFailureLog, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects (+6 more)

### Community 62 - "EffectId"
Cohesion: 0.12
Nodes (10): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, BuiltInEffectAnimations, EffectCatalogTests (+2 more)

### Community 63 - "NativeMethods"
Cohesion: 0.22
Nodes (4): IMouseHookSource, NativeMethods, RECT, Win32MouseHookSource

### Community 64 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 65 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 68 - "EffectLifecycleTests"
Cohesion: 0.24
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 69 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 70 - "DllImport"
Cohesion: 0.20
Nodes (4): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT

### Community 71 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 72 - "SoundReference"
Cohesion: 0.13
Nodes (12): ActionSlot, Action, IsEmpty, Number, SlotAction, SoundAction, Sound, SoundReference (+4 more)

### Community 73 - "BuiltInEffectAnimations.cs"
Cohesion: 0.18
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 74 - "SettingsCommand"
Cohesion: 0.18
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 75 - "StartEffectCommand"
Cohesion: 0.15
Nodes (6): StartEffectCommand, EffectPlacement, Cursor, Monitor, EffectMonitorGuard, IEffectStartQueue

### Community 76 - "LottiePlayback"
Cohesion: 0.14
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 77 - "EffectSurfaceWindow"
Cohesion: 0.18
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 78 - ".Key"
Cohesion: 0.22
Nodes (3): INPUT, InputUnion, KEYBDINPUT

### Community 79 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 80 - "IDisposable"
Cohesion: 0.22
Nodes (5): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, TimedOutSound

### Community 81 - "IntPtr"
Cohesion: 0.26
Nodes (5): KBDLLHOOKSTRUCT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 83 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 85 - "App.xaml.cs"
Cohesion: 0.09
Nodes (4): DrawEM.Tests.Presentation.Drawing, RenderLagHarness, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 86 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (11): DrawEM.App.Application.Drawing, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects (+3 more)

### Community 87 - ".Digit"
Cohesion: 0.38
Nodes (3): ShortcutKey, Kind, Value

### Community 88 - "EffectChannelController"
Cohesion: 0.14
Nodes (17): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, SettingsSaveOperation, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect (+9 more)

### Community 89 - "drawEM README"
Cohesion: 0.28
Nodes (9): Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+1 more)

### Community 90 - ".TryGetBounds"
Cohesion: 0.29
Nodes (3): MONITORINFO, POINT, Win32MonitorBoundsSource

### Community 91 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 92 - "WidthBox"
Cohesion: 0.29
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 93 - "FIX-006: Single Screen Edge"
Cohesion: 0.38
Nodes (9): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+1 more)

### Community 94 - "S4-01 acceptance: prove the effect window"
Cohesion: 0.53
Nodes (5): S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window

### Community 95 - "S4-12 Acceptance criteria"
Cohesion: 0.27
Nodes (11): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+3 more)

### Community 96 - "EffectAction"
Cohesion: 0.33
Nodes (4): EffectAction, Effect, EffectSample, Effect

### Community 97 - "BlockingPlayerFactory"
Cohesion: 0.20
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 100 - "v4: simple implementation steps"
Cohesion: 0.21
Nodes (16): SettingsEditor, S4-05 task: save and load effect assignments, S4-06 task: launch effects through shortcuts, S4-07 task: stop effects when switching monitors, S4-08 task: choose effects in Settings, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler (+8 more)

### Community 103 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 104 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "NotifyIconTrayHost"
Cohesion: 0.22
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 109 - "SingleInstanceGuard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 110 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 111 - "Lottie Simple License (Design Barn Inc.)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 113 - "FakeSoundChannel"
Cohesion: 0.11
Nodes (6): ISaveSoundChannel, ISoundStartHold, StartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 115 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 116 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **488 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+483 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 666 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-05 acceptance: save and load effect assignments`, `WidthBox`?**
  _High betweenness centrality (0.113) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _488 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.1054421768707483 - nodes in this community are weakly interconnected._
- **Why does `drawEM v3 Roadmap: Drawing and Action Settings` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`, `Stage 1 Step 2 Review`, `drawEM architecture`, `Stage 1 sound implementation plan`, `FIX-006: Single Screen Edge`?**
  _High betweenness centrality (0.108) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.08225108225108226 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `SettingsWindow` to `.Window_BindsEveryFieldWithoutBindingErrors`, `FakePicker`, `SettingsViewModel`, `EffectSurfaceWindow`, `SettingsViewModel.cs`, `WidthBox`, `.OnStartup`, `Window`?**
  _High betweenness centrality (0.106) - this node is a cross-community bridge._