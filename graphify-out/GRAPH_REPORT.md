# Graph Report - worktree-silver-river-9b47  (2026-10-14)

## Corpus Check
- 309 files · ~139,816 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2885 nodes · 8593 edges · 116 communities (102 shown, 14 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1495 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `4df83ce1`
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
- ScreenPoint
- drawEM architecture
- SettingsViewModel.cs
- SettingsEditorTests
- .Validate
- ManualTimeProvider
- Fact
- SoundChannelHost
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- FakePlayback
- .Move
- SettingsEditor
- ISoundPlayer
- List
- ManagedSoundLibraryTests
- Window
- ShortcutModifiers
- PlaySoundCommand
- EffectChannelController
- .RunOnDispatcherThread
- Fixture
- .OnStartup
- RuntimeMediaTests
- DrawEM.Tests.Infrastructure
- DrawEM.App.Domain.Settings
- FEAT-005: Assisted E2E Runner
- S4-03 acceptance: play built-in Lottie effects
- Harness
- EffectFailure
- GlobalShortcutAdapter
- SoundChannelController
- Stage 1 Step 2 Review
- Fixture
- DrawingSessionController
- ManagedSoundLibrary
- SettingsSnapshot
- .Load
- Win32SessionInterruptionSource
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
- FakePlayer
- DrawEM.App.Presentation.Effects
- DllImport
- ShortcutBindings
- SoundReference
- JsonSettingsStore
- Shortcut
- StartEffectCommand
- LottiePlayback
- EffectSurfaceWindow
- .Key
- SoundSlot
- IDisposable
- IntPtr
- system_runtime_interopservices
- Sound infrastructure README
- LayerDependencyTests.cs
- App.xaml.cs
- DrawEM.App.Domain.Drawing
- FakePlayer
- EffectChannelController
- drawEM README
- .TryGetBounds
- EffectSurfaceHost
- RecordingEffectSurface
- FIX-006: Single Screen Edge
- S4-01 acceptance: prove the effect window
- S4-12 Acceptance criteria
- .ShowMenu_WithoutTheNotifyIconsOwnMenuDisplay_ShowsTheMenuAtTheCursor
- BlockingPlayerFactory
- SoundFailure
- .ReleaseStartedResources
- v4: simple implementation steps
- FakePicker
- Func
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
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-009: implementation plan` --references--> `LottiePlayback`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Presentation/Effects/LottiePlayback.cs
- `One fixed placement per effect` --conceptually_related_to--> `Placement (Monitor / Cursor)`  [INFERRED]
  docs/adr/0001-lottie-built-in-effects.md → CONTEXT.md

## Import Cycles
- None detected.

## Communities (116 total, 14 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.14
Nodes (13): DrawEM.Tests.Application.Sound, SoundId, Value, FakeRequest, IsCurrent, Fixture, Controller, Failures (+5 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.10
Nodes (15): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+7 more)

### Community 2 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 3 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

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
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.09
Nodes (17): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+9 more)

### Community 9 - "Theory"
Cohesion: 0.07
Nodes (9): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule (+1 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 12 - "ScreenPoint"
Cohesion: 0.10
Nodes (16): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, SettableCursor, Available, Position (+8 more)

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 15 - "SettingsEditorTests"
Cohesion: 0.14
Nodes (3): SoundAction, Sound, SettingsEditorTests

### Community 16 - ".Validate"
Cohesion: 0.12
Nodes (13): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SlotAction, SettingsError (+5 more)

### Community 17 - "ManualTimeProvider"
Cohesion: 0.18
Nodes (5): ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 19 - "SoundChannelHost"
Cohesion: 0.15
Nodes (10): SoundChannelHost, SoundChannelShutdownTimeoutException, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay, Played (+2 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 22 - "FakePlayback"
Cohesion: 0.07
Nodes (24): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+16 more)

### Community 23 - ".Move"
Cohesion: 0.21
Nodes (4): OverlayWindowAdapter, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "SettingsEditor"
Cohesion: 0.13
Nodes (9): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+1 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.09
Nodes (10): ISoundPlayer, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment, PlayThreadId (+2 more)

### Community 26 - "List"
Cohesion: 0.14
Nodes (10): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+2 more)

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 28 - "Window"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+46 more)

### Community 29 - "ShortcutModifiers"
Cohesion: 0.09
Nodes (20): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+12 more)

### Community 30 - "PlaySoundCommand"
Cohesion: 0.15
Nodes (8): ISoundFailureReporter, PlaySoundCommand, SampleFailureRouter, SoundSampler, IgnoringReporter, ThrowingReporter, RecordingReporter, SoundSamplerTests

### Community 31 - "EffectChannelController"
Cohesion: 0.13
Nodes (12): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+4 more)

### Community 32 - ".RunOnDispatcherThread"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 33 - "Fixture"
Cohesion: 0.09
Nodes (13): EffectPlacement, Cursor, Monitor, EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays (+5 more)

### Community 34 - ".OnStartup"
Cohesion: 0.14
Nodes (7): Application, App, ApplicationExitPolicy, Win32CursorPositionSource, StartupFailure, WpfApplicationLifetime, StartupFailureTests

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.11
Nodes (15): ActiveSettings, Current, IdleShortcuts, IdleSoundChannel, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory (+7 more)

### Community 36 - "DrawEM.Tests.Infrastructure"
Cohesion: 0.11
Nodes (4): DrawEM.Tests.Infrastructure, DrawEM.Tests, AppIconTests, StaThread

### Community 37 - "DrawEM.App.Domain.Settings"
Cohesion: 0.12
Nodes (14): DrawEM.App.Infrastructure, DrawEM.App.Domain.Settings, DrawEM.App.Application.Drawing, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Infrastructure.Settings (+6 more)

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 39 - "S4-03 acceptance: play built-in Lottie effects"
Cohesion: 0.15
Nodes (19): Built-in effect catalog (Domain/Effects), GlobalShortcutAdapter, Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations, EffectFailureLog / LoggingEffectFailureReporter (+11 more)

### Community 40 - "Harness"
Cohesion: 0.11
Nodes (14): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+6 more)

### Community 41 - "EffectFailure"
Cohesion: 0.27
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.11
Nodes (11): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+3 more)

### Community 43 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundPlayerFactory, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 44 - "Stage 1 Step 2 Review"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 45 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 46 - "DrawingSessionController"
Cohesion: 0.15
Nodes (13): DrawingSessionController, CompletedStrokes, DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, DrawingSessionControllerTests (+5 more)

### Community 47 - "ManagedSoundLibrary"
Cohesion: 0.20
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (21): ISoundLibrary, SoundCleanupFailure, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut (+13 more)

### Community 49 - ".Load"
Cohesion: 0.15
Nodes (12): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, FakeReporter (+4 more)

### Community 50 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 51 - "Harness"
Cohesion: 0.12
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots

### Community 53 - "SlotViewModel"
Cohesion: 0.05
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
Cohesion: 0.16
Nodes (9): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult (+1 more)

### Community 58 - "LoggingSoundFailureReporter"
Cohesion: 0.19
Nodes (4): BackgroundLogWriter, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 59 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.19
Nodes (14): EffectMonitorGuard, Hook input ordering and monitor epoch, Configuration generation, S4-11 Acceptance criteria, LoggingEffectFailureReporter / EffectFailureLog, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects (+6 more)

### Community 62 - "EffectId"
Cohesion: 0.10
Nodes (13): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, BuiltInEffectAnimations, EffectCatalogTests (+5 more)

### Community 63 - "NativeMethods"
Cohesion: 0.23
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

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
Cohesion: 0.18
Nodes (9): Evidence, v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file (+1 more)

### Community 68 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 69 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.17
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 71 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 72 - "SoundReference"
Cohesion: 0.18
Nodes (6): SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, ShortcutBindingsTests, TestSettings

### Community 73 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 74 - "Shortcut"
Cohesion: 0.07
Nodes (20): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot, SettingsErrorCode (+12 more)

### Community 75 - "StartEffectCommand"
Cohesion: 0.21
Nodes (3): StartEffectCommand, EffectMonitorGuard, IEffectStartQueue

### Community 78 - ".Key"
Cohesion: 0.22
Nodes (3): INPUT, InputUnion, KEYBDINPUT

### Community 79 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 80 - "IDisposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 81 - "IntPtr"
Cohesion: 0.23
Nodes (5): KBDLLHOOKSTRUCT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 82 - "system_runtime_interopservices"
Cohesion: 0.20
Nodes (3): NativeMethods, POINT, RECT

### Community 83 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 85 - "App.xaml.cs"
Cohesion: 0.10
Nodes (4): DrawEM.Tests.Presentation.Drawing, RenderLagHarness, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 86 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (7): DrawEM.App.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects, IDisplayChangeSource, FakeDisplayChangeSource

### Community 87 - "FakePlayer"
Cohesion: 0.22
Nodes (6): FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop

### Community 88 - "EffectChannelController"
Cohesion: 0.14
Nodes (17): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, SettingsSaveOperation, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect (+9 more)

### Community 89 - "drawEM README"
Cohesion: 0.28
Nodes (9): Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+1 more)

### Community 90 - ".TryGetBounds"
Cohesion: 0.25
Nodes (3): MONITORINFO, POINT, Win32MonitorBoundsSource

### Community 91 - "EffectSurfaceHost"
Cohesion: 0.25
Nodes (3): EffectSurfaceHost, Visual, VisualChildrenCount

### Community 92 - "RecordingEffectSurface"
Cohesion: 0.29
Nodes (5): RecordingEffectSurface, Failures, OnRemove, Removed, Shown

### Community 93 - "FIX-006: Single Screen Edge"
Cohesion: 0.38
Nodes (9): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+1 more)

### Community 94 - "S4-01 acceptance: prove the effect window"
Cohesion: 0.53
Nodes (5): S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window

### Community 95 - "S4-12 Acceptance criteria"
Cohesion: 0.27
Nodes (11): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+3 more)

### Community 97 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 98 - "SoundFailure"
Cohesion: 0.33
Nodes (4): SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 100 - "v4: simple implementation steps"
Cohesion: 0.21
Nodes (16): SettingsEditor, S4-05 task: save and load effect assignments, S4-06 task: launch effects through shortcuts, S4-07 task: stop effects when switching monitors, S4-08 task: choose effects in Settings, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler (+8 more)

### Community 103 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 104 - "Func"
Cohesion: 0.27
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "NotifyIconTrayHost"
Cohesion: 0.22
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 107 - "SettingsWindow"
Cohesion: 0.11
Nodes (7): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, SettingsWindow, ViewModel, RepeatButton

### Community 109 - "SingleInstanceGuard"
Cohesion: 0.21
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
- **485 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+480 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 662 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **14 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v3 Roadmap: Drawing and Action Settings` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`, `Stage 1 Step 2 Review`, `drawEM architecture`, `Stage 1 sound implementation plan`, `FIX-006: Single Screen Edge`?**
  _High betweenness centrality (0.126) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _485 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.13900134952766532 - nodes in this community are weakly interconnected._
- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-05 acceptance: save and load effect assignments`?**
  _High betweenness centrality (0.111) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.10059676044330776 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `SettingsWindow` to `.Window_BindsEveryFieldWithoutBindingErrors`, `.OnStartup`, `FakePicker`, `SettingsViewModel`, `SettingsViewModel.cs`, `Window`?**
  _High betweenness centrality (0.107) - this node is a cross-community bridge._