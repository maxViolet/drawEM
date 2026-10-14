# Graph Report - worktree-silver-river-9b47  (2026-10-14)

## Corpus Check
- 305 files · ~136,491 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2812 nodes · 8385 edges · 109 communities (96 shown, 13 thin omitted)
- Extraction: 82% EXTRACTED · 18% INFERRED · 0% AMBIGUOUS · INFERRED: 1476 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d21957ea`
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
- SettingsEditorTests
- .Validate
- ManualTimeProvider
- Fact
- SoundChannelHost
- Stage 1 sound implementation plan
- S4-07 acceptance: stop effects when switching monitors
- RecordingEffectSurface
- DrawingSessionController
- SettingsEditor
- ISoundPlayer
- List
- ManagedSoundLibraryTests
- Window
- Shortcut
- PlaySoundCommand
- Program.cs
- Program
- Fixture
- .RemappedDraw_BoundaryCrossing_RequiresKeyReleaseBeforeDrawingOnNextMonitor
- RuntimeMediaTests
- DrawEM.App.Infrastructure
- DrawEM.App.Domain.Drawing
- FEAT-005: Assisted E2E Runner
- v4: simple implementation steps
- Harness
- .OnStartup
- GlobalShortcutAdapter
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
- .Digit
- Action
- EffectId
- S4-11 Acceptance criteria
- BuiltInEffectAnimations
- DrawEM.App.Domain.Settings
- .Window_BindsEveryFieldWithoutBindingErrors
- DrawEM.Tests.csproj
- drawEM v4: parallel work packages
- ShortcutFieldViewModel
- MediaSoundPlayer
- EffectCatalog
- EffectSurfaceWindow
- RuntimeSettingsTests
- SoundReference
- BlockingPlayerFactory
- ThreadRecordingPlayer
- EffectMonitorGuard
- FakePlayback
- SettingsFailureDialog
- WidthBox
- RecordingNotifications
- CompositeDisposable
- IntPtr
- ErrorsText
- Sound infrastructure README
- LayerDependencyTests.cs
- App.xaml.cs
- FakeDisplayChangeSource
- .RejectedChord_ShowsWhyAndKeepsCapturing
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
- WpfApplicationLifetime
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

## Communities (109 total, 13 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.06
Nodes (28): DrawEM.Tests.Application.Sound, SoundId, Value, EffectFailure, EffectFailureLog, DefaultPath, SoundFailure, SoundFailureLog (+20 more)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

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
Cohesion: 0.10
Nodes (23): FakeStore, Failure, OnSave, Saved, SaveHarness, Channel, Controller, EffectChannel (+15 more)

### Community 7 - "JsonSettingsStoreTests"
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "GlobalShortcutAdapterTests"
Cohesion: 0.16
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 9 - "Theory"
Cohesion: 0.07
Nodes (10): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule (+2 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.07
Nodes (23): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot, SettingsViewModel (+15 more)

### Community 11 - "Rig"
Cohesion: 0.13
Nodes (15): Release, Rig, Keyboard, New, Old, Players, Previous, Saved (+7 more)

### Community 12 - "ScreenPoint"
Cohesion: 0.13
Nodes (10): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, RecoveringCursorPositionSource (+2 more)

### Community 13 - "drawEM architecture"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 15 - "SettingsEditorTests"
Cohesion: 0.16
Nodes (3): SoundAction, Sound, SettingsEditorTests

### Community 16 - ".Validate"
Cohesion: 0.14
Nodes (10): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SettingsError, SettingsValidation (+2 more)

### Community 17 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (8): BackgroundLogWriter, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 19 - "SoundChannelHost"
Cohesion: 0.14
Nodes (12): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+4 more)

### Community 20 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.22
Nodes (13): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, IDisplayChangeSource, LottieEffectSurface / LottiePlayback FrameStarting, StartEffectCommand.MonitorEpoch (+5 more)

### Community 22 - "RecordingEffectSurface"
Cohesion: 0.08
Nodes (21): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+13 more)

### Community 23 - "DrawingSessionController"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "SettingsEditor"
Cohesion: 0.13
Nodes (9): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+1 more)

### Community 25 - "ISoundPlayer"
Cohesion: 0.08
Nodes (13): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, PlayerFactory, Created, FakePlayer (+5 more)

### Community 26 - "List"
Cohesion: 0.12
Nodes (11): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost (+3 more)

### Community 27 - "ManagedSoundLibraryTests"
Cohesion: 0.06
Nodes (24): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, SoundImportException, ISettingsFailureReporter, SettingsStartup (+16 more)

### Community 28 - "Window"
Cohesion: 0.05
Nodes (42): CanDecreaseWidth, CanIncreaseWidth, CanSave, ChooseText, Clear, Color, Visible, Draw (+34 more)

### Community 29 - "Shortcut"
Cohesion: 0.09
Nodes (16): SlotAction, Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers (+8 more)

### Community 30 - "PlaySoundCommand"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 32 - "Program"
Cohesion: 0.26
Nodes (4): Program, RunResult, Sample, ScenarioSeed

### Community 33 - "Fixture"
Cohesion: 0.12
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - ".RemappedDraw_BoundaryCrossing_RequiresKeyReleaseBeforeDrawingOnNextMonitor"
Cohesion: 0.14
Nodes (9): ICursorPositionSource, Win32CursorPositionSource, SettableCursor, Available, Position, Reads, FailingCursorPositionSource, MutableCursorPositionSource (+1 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.16
Nodes (12): IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library (+4 more)

### Community 36 - "DrawEM.App.Infrastructure"
Cohesion: 0.10
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, NotifyIconTrayHostTests, StaThread

### Community 37 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (11): DrawEM.App.Application.Drawing, DrawEM.Tests.Domain.Effects, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+3 more)

### Community 38 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 39 - "v4: simple implementation steps"
Cohesion: 0.12
Nodes (28): Built-in effect catalog (Domain/Effects), Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified (+20 more)

### Community 40 - "Harness"
Cohesion: 0.09
Nodes (15): EffectStopRoutes, ISessionInterruptionSource, Harness, Controller, Displays, EffectChannel, Effects, Players (+7 more)

### Community 41 - ".OnStartup"
Cohesion: 0.14
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.09
Nodes (14): Cancelled, Captured, Rejected, ShortcutCaptureResult, GlobalShortcutAdapter, IsCapturing, INeutralKeyEmitter, FakeCursorPositionSource (+6 more)

### Community 43 - "SoundChannelController"
Cohesion: 0.12
Nodes (14): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+6 more)

### Community 44 - "Stage 1 Step 2 Review"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 45 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 46 - "DrawingModeInputGate"
Cohesion: 0.14
Nodes (13): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, IMouseHookSource, Win32MonitorBoundsSource, CountingMonitors (+5 more)

### Community 47 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "SettingsSnapshot"
Cohesion: 0.11
Nodes (18): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+10 more)

### Community 49 - "EffectChannelController"
Cohesion: 0.13
Nodes (12): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+4 more)

### Community 50 - "App"
Cohesion: 0.13
Nodes (3): App, Win32DisplayChangeSource, Win32SessionInterruptionSource

### Community 51 - "Harness"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 52 - "DrawEM.App.Application.Sound"
Cohesion: 0.10
Nodes (15): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1 (+7 more)

### Community 53 - "SlotViewModel"
Cohesion: 0.08
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 54 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 55 - "drawEM v4 Roadmap: built-in Monitor and Cursor effects"
Cohesion: 0.15
Nodes (23): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001 Lottie built-in effects, Confetti effect (confetti), CONTEXT.md glossary (+15 more)

### Community 56 - "StartEffectCommand"
Cohesion: 0.18
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, SamplingEditor, Failures, Sampler

### Community 57 - ".Save"
Cohesion: 0.09
Nodes (12): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult (+4 more)

### Community 58 - ".Digit"
Cohesion: 0.12
Nodes (11): ShortcutKey, Kind, Value, KeyChord, ShortcutBindings, Clear, Draw, Effects (+3 more)

### Community 59 - "Action"
Cohesion: 0.11
Nodes (9): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+1 more)

### Community 60 - "EffectId"
Cohesion: 0.13
Nodes (10): EffectId, Value, EffectPlacement, Cursor, Monitor, EffectBinding, EffectCatalogTests, EffectLifecycleTests (+2 more)

### Community 61 - "S4-11 Acceptance criteria"
Cohesion: 0.12
Nodes (24): EffectMonitorGuard, GlobalShortcutAdapter, Hook input ordering and monitor epoch, SettingsSaveOperation, Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified) (+16 more)

### Community 62 - "BuiltInEffectAnimations"
Cohesion: 0.22
Nodes (4): BuiltInEffectAnimations, LottieEffectSurface, BuiltInEffectAnimationsTests, BuiltInIds

### Community 63 - "DrawEM.App.Domain.Settings"
Cohesion: 0.10
Nodes (9): DrawEM.App.Domain.Settings, DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings (+1 more)

### Community 64 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 65 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "ShortcutFieldViewModel"
Cohesion: 0.12
Nodes (11): IShortcutCapture, BindableBase, ShortcutFieldViewModel, Command, Error, IsCapturing, IsEnabled, Message (+3 more)

### Community 68 - "MediaSoundPlayer"
Cohesion: 0.17
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 69 - "EffectCatalog"
Cohesion: 0.20
Nodes (6): EffectCatalog, BuiltIn, ShortcutKeyKind, Digit, Function, Letter

### Community 70 - "EffectSurfaceWindow"
Cohesion: 0.14
Nodes (3): EffectSurfaceWindow, Layout, LottiePlayback

### Community 71 - "RuntimeSettingsTests"
Cohesion: 0.29
Nodes (4): NoSettingsFailures, RuntimeSettingsTests, LibraryDirectory, SettingsDirectory

### Community 72 - "SoundReference"
Cohesion: 0.18
Nodes (7): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, MediaSoundPlayerFactoryTests

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.20
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 74 - "ThreadRecordingPlayer"
Cohesion: 0.20
Nodes (6): ThreadRecordingPlayer, IsDisposed, PlayApartment, PlayThreadId, Stopped, StopThreadId

### Community 76 - "FakePlayback"
Cohesion: 0.20
Nodes (7): EffectPlaybackException, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 78 - "WidthBox"
Cohesion: 0.29
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 79 - "RecordingNotifications"
Cohesion: 0.33
Nodes (3): RecordingNotifications, NotSavedReasons, UnconfirmedStops

### Community 80 - "CompositeDisposable"
Cohesion: 0.26
Nodes (4): CleanupSteps, CompositeDisposable, CompositeDisposableTests, FakeDisposable

### Community 81 - "IntPtr"
Cohesion: 0.06
Nodes (18): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MouseHookSource, INPUT, InputUnion (+10 more)

### Community 82 - "ErrorsText"
Cohesion: 0.40
Nodes (5): Errors, Status, ErrorsText, StatusText, TextBlock

### Community 83 - "Sound infrastructure README"
Cohesion: 0.19
Nodes (16): GlobalShortcutAdapter, IEffectStartQueue, Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController (+8 more)

### Community 85 - "App.xaml.cs"
Cohesion: 0.09
Nodes (5): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Effects, DrawEM.App.Presentation.Drawing, DrawEM.Tests.Presentation.Effects, DrawEM.App

### Community 87 - ".RejectedChord_ShowsWhyAndKeepsCapturing"
Cohesion: 0.50
Nodes (3): ShortcutCaptureRejection, RightAlt, TooFewModifiers

### Community 88 - "EffectChannelController"
Cohesion: 0.31
Nodes (9): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback (+1 more)

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
Cohesion: 0.27
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 106 - "NotifyIconTrayHost"
Cohesion: 0.22
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 109 - "SingleInstanceGuard"
Cohesion: 0.18
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
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 636 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **13 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Window` to `ErrorsText`, `S4-05 acceptance: save and load effect assignments`, `WidthBox`?**
  _High betweenness centrality (0.114) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _462 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.06421052631578947 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `Window` to `.Window_BindsEveryFieldWithoutBindingErrors`, `EffectSurfaceWindow`, `FakePicker`, `.OnStartup`, `SettingsViewModel`, `SettingsWindow.xaml.cs`, `WidthBox`, `App`?**
  _High betweenness centrality (0.111) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07777777777777778 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.110) - this node is a cross-community bridge._