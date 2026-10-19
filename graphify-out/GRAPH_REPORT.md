# Graph Report - worktree-new-design  (2026-10-19)

## Corpus Check
- 377 files · ~167,339 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3577 nodes · 10264 edges · 146 communities (128 shown, 18 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1715 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `742b71cb`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- Effect Surface Docs (S4-01/02)
- Window
- v3 Step 8: Publish and verify on Windows (task)
- Harness
- PlaySoundCommand
- App.xaml.cs
- SettingsViewModel
- DrawEM.App.Domain.Drawing
- .Digit
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- ManualTimeProvider
- R4-07 Tray Updates Plan
- Action
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- ScreenPoint
- Fixture
- Effect Channel Tests
- DrawingSessionController
- DrawingModeInputGate
- Global Shortcut Adapter
- SettingsSnapshot
- List
- SettingsWindow
- Effect Placement Monitors
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- BuiltInEffectAnimations
- Lottie Simple License
- IEffectPlayback
- GlobalShortcutAdapter
- EffectSampler
- DrawEM.App.Infrastructure
- Harness
- Stage 1 sound implementation plan
- SlotViewModel
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- SettingsCommand
- LottiePlayback
- SaveHarness
- EffectId
- Shortcut
- SoundReference
- S4-03 Acceptance Criteria
- .OnStartup
- SoundFailure
- DrawEM.App.Presentation.Effects
- EffectChannelController
- EffectCatalog Group
- EffectSurfaceLayout
- DllImport
- S4-07 acceptance: stop effects when switching monitors
- Portable win-x64 ZIP
- EffectSurfaceWindow
- Fixture
- R4-02 implementation plan
- VelopackPackageIdentityTests
- STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- NativeMethods
- BlockingPlayerFactory
- system_runtime_interopservices
- Settings Storage README
- App
- SingleInstanceGuard
- StrokeWidth
- drawEM rules (AGENTS.md)
- IDisposable
- EffectMonitorGuard
- FEAT-005: Assisted E2E Runner
- DrawEM.App.Presentation.Effects Group
- IntPtr
- RenderLagHarness Group
- DrawEM.App.Application.Sound
- IEnumerable Group
- Sound Infrastructure README
- Step 2 Implementation Review
- FIX-006: Single Screen Edge
- v3 Step 6 plan
- drawEM v4 Parallel Work Packages
- TrayApplication
- S4-09 Preview Draft Effects (acceptance)
- Animation Group
- TrayMenuWindow
- GlobalShortcutAdapter
- S4-08 acceptance: choose effects in Settings
- .StopStartedBefore() Group
- .Attach
- FEAT-009-effect-loop-and-limit/task.md
- Lottie built-in effects via SkiaSharp.Skottie
- UiThemeState
- drawEM README
- SoundSlot.cs Group
- drawEM v3 Roadmap: Drawing and Action Settings
- drawEM v4: out of scope
- R4-06 Velopack Installer Plan
- FEAT-011-glass-ui/plan.md
- S4-10 Apply Settings Safely (acceptance) Group
- VelopackUpdateService
- Step 4 Plan: Publish and Verify
- FakeUpdateService
- S4-12 Acceptance: Publish and Verify on Windows
- UpdateController
- Infrastructure/Drawing/NativeMethods.cs Group
- LoggingUpdateFailureReporter
- ISoundFilePicker Group
- FakePlayback
- .TryFind
- Func
- RenderLagHarness tool
- graphify knowledge-graph usage rules
- .Create
- RenderLagHarness/Program.cs
- CLAUDE.md graphify rules
- drawEM smoke test
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- FakePlayer
- SoundReference
- Sound presentation placeholder README
- .Key
- NativeMethods
- Rect
- v1 roadmap: drawing
- TrayMenuCommand
- Proposed screen-action model (action, command, action slot)
- UpdateController
- FakeDisplayChangeSource
- RecordingReporter
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `SettingsViewModel` - 61 edges
7. `Window` - 61 edges
8. `MonitorBounds` - 60 edges
9. `EffectId` - 60 edges
10. `GlobalShortcutAdapter` - 59 edges

## Surprising Connections (you probably didn't know these)
- `Per stroke` --references--> `DrawingStyle`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/task.md → src/DrawEM.App/Domain/Drawing/DrawingStyle.cs
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-012: implementation plan` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs

## Import Cycles
- None detected.

## Communities (146 total, 18 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.07
Nodes (24): Out of scope, Per stroke, Previous behavior, Required behavior, Settings file, Settings window, Smoothing strategy, v4 / FEAT-012: smooth strokes with Bézier curves (+16 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.14
Nodes (11): R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), Consent-based update UX (startup + 24h check), Velopack installer and update feed, Velopack Per-User Installer, WinGet catalog distribution, SHA256SUMS.txt manifest format (sha256sum -c) (+3 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.11
Nodes (5): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter

### Community 4 - "Effect Surface Docs (S4-01/02)"
Cohesion: 0.09
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (56): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+48 more)

### Community 6 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.06
Nodes (51): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, Settings Schema/Version Policy, v3 Settings Defaults, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Right Alt reserved for AltGr (+43 more)

### Community 7 - "Harness"
Cohesion: 0.06
Nodes (19): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeNeutralKeyEmitter (+11 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.07
Nodes (6): DrawEM.Tests.Presentation.Drawing, DrawEM.Tests.Presentation.Theme, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (28): IShortcutCapture, BindableBase, ISoundFilePicker, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (11): DrawEM.App.Application.Drawing, DrawEM.Tests.Domain.Effects, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+3 more)

### Community 12 - ".Digit"
Cohesion: 0.08
Nodes (17): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+9 more)

### Community 13 - "Theory"
Cohesion: 0.09
Nodes (10): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, AppIconTests, EffectLifecycleTests, Events, Interruptions, BuiltInEffectAnimationsTests (+2 more)

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.07
Nodes (21): ISettingsSave, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops (+13 more)

### Community 15 - "Fixture"
Cohesion: 0.14
Nodes (13): DrawEM.Tests.Application.Sound, SoundId, Value, FakeRequest, IsCurrent, Fixture, Controller, Failures (+5 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.10
Nodes (16): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+8 more)

### Community 17 - "R4-07 Tray Updates Plan"
Cohesion: 0.12
Nodes (20): %LOCALAPPDATA%\drawEM settings directory, R4-06 Velopack Installer Acceptance, JsonSettingsStore.DefaultDirectory, %LOCALAPPDATA%\drawEM settings directory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, VelopackHookExitTests, VelopackPackageIdentityTests (+12 more)

### Community 18 - "Action"
Cohesion: 0.14
Nodes (9): EffectSample, Effect, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+1 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.12
Nodes (6): BackgroundLogWriter, LoggingEffectFailureReporter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.13
Nodes (7): DrawEM.App.Domain.Settings, DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Application.Settings, DrawEM.Tests.Domain.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.05
Nodes (32): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, SoundImportException, ISettingsFailureReporter, SettingsStartup (+24 more)

### Community 24 - "ScreenPoint"
Cohesion: 0.09
Nodes (14): StartEffectCommand, ScreenPoint, MonitorBounds, EffectPlacement, Cursor, Monitor, CursorSurfaceBounds, EffectCanvasPlacement (+6 more)

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "Effect Channel Tests"
Cohesion: 0.21
Nodes (5): Program, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "DrawingSessionController"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.19
Nodes (13): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, IMouseHookSource, Win32MonitorBoundsSource, CountingMonitors (+5 more)

### Community 29 - "Global Shortcut Adapter"
Cohesion: 0.09
Nodes (14): SettableCursor, Available, Position, Reads, EffectMonitorGuardTests, Fixture, Channel, Cursor (+6 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.08
Nodes (22): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+14 more)

### Community 31 - "List"
Cohesion: 0.11
Nodes (10): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+2 more)

### Community 32 - "SettingsWindow"
Cohesion: 0.10
Nodes (7): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, SettingsWindow, ViewModel, RepeatButton

### Community 33 - "Effect Placement Monitors"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.12
Nodes (13): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+5 more)

### Community 36 - "Harness"
Cohesion: 0.12
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.12
Nodes (8): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 38 - "Rig"
Cohesion: 0.13
Nodes (16): Release, Rig, Keyboard, New, Old, Players, Previous, Saved (+8 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.13
Nodes (7): EffectPlaybackException, BuiltInEffectAnimations, LottieEffectSurface, Playback, DisposeFailure, HasCompletionHandler, HasFailureHandler

### Community 40 - "Lottie Simple License"
Cohesion: 0.16
Nodes (24): Built-in Lottie Effect, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources, celebration-confetti.json (Lottie effect) (+16 more)

### Community 41 - "IEffectPlayback"
Cohesion: 0.13
Nodes (12): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+4 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.11
Nodes (7): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, IEffectStartQueue, INeutralKeyEmitter, FailingCursorPositionSource

### Community 43 - "EffectSampler"
Cohesion: 0.18
Nodes (6): EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, StaThread

### Community 45 - "Harness"
Cohesion: 0.09
Nodes (15): EffectStopRoutes, ISessionInterruptionSource, Harness, Controller, Displays, EffectChannel, Effects, Players (+7 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.22
Nodes (25): drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, v2 roadmap: screen actions, Ctrl+Alt+1..8 Sound Shortcut Slots, Stage 1 Sound Implementation Plan, Stage 1 sound implementation plan, Step 1 Acceptance: Command and Configuration, Step 1 Plan: Command and Configuration (+17 more)

### Community 47 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.13
Nodes (26): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+18 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.15
Nodes (22): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectFailureLog, EffectMonitorGuard.StopEffects(), EffectStopRoutes (+14 more)

### Community 50 - "SettingsCommand"
Cohesion: 0.19
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 51 - "LottiePlayback"
Cohesion: 0.13
Nodes (6): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing, LottiePlaybackTests

### Community 52 - "SaveHarness"
Cohesion: 0.07
Nodes (28): FakeSoundChannel, StopConfirmed, FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons (+20 more)

### Community 53 - "EffectId"
Cohesion: 0.12
Nodes (12): EffectId, Value, ActionSlot, Action, IsEmpty, Number, EffectAction, Effect (+4 more)

### Community 54 - "Shortcut"
Cohesion: 0.09
Nodes (14): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSaveNotifications, ISoundStartHold, NotSaved, Saved, SettingsSaveOperation (+6 more)

### Community 55 - "SoundReference"
Cohesion: 0.12
Nodes (17): SlotAction, SoundAction, Sound, SoundReference, DisplayName, LibraryFileName, EffectBinding, KeyChord (+9 more)

### Community 56 - "S4-03 Acceptance Criteria"
Cohesion: 0.12
Nodes (25): S4-03 acceptance: play built-in Lottie effects, S4-03 Acceptance Criteria, BuiltInEffectAnimations, Confetti Effect (Monitor), EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Flashing Check (max three flashes per second), Focus Ring Effect (Cursor) (+17 more)

### Community 57 - ".OnStartup"
Cohesion: 0.15
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 58 - "SoundFailure"
Cohesion: 0.16
Nodes (9): EffectFailure, EffectFailureLog, DefaultPath, SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests (+1 more)

### Community 60 - "EffectChannelController"
Cohesion: 0.11
Nodes (16): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+8 more)

### Community 61 - "EffectCatalog Group"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "DllImport"
Cohesion: 0.19
Nodes (3): MSLLHOOKSTRUCT, NativeMethods, POINT

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.19
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 65 - "Portable win-x64 ZIP"
Cohesion: 0.11
Nodes (9): Portable win-x64 ZIP, release.yml Release Workflow, WinGet Portable ZIP Listing, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation (+1 more)

### Community 66 - "EffectSurfaceWindow"
Cohesion: 0.18
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 67 - "Fixture"
Cohesion: 0.14
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 implementation plan"
Cohesion: 0.15
Nodes (16): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, SmartScreen Reputation Warnings, One-Byte Mutation Check, R4-02 acceptance criteria, actions/attest@v4 Build Provenance, gh attestation verify, R4-02 implementation plan (+8 more)

### Community 69 - "VelopackPackageIdentityTests"
Cohesion: 0.16
Nodes (3): DrawEM.Tests.Release, VelopackHookExitTests, VelopackPackageIdentityTests

### Community 70 - "STEPS.md"
Cohesion: 0.18
Nodes (20): R4 Release Publishing Roadmap, v4 Release Publishing Roadmap, Task, v4 / R4-01: create a versioned draft release, R4-02 Acceptance: Checksums and Provenance, R4-02 Plan: Checksums and Provenance, GitHub Build Provenance Attestation (actions/attest), R4-02 Checksums and Provenance Task (+12 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.08
Nodes (41): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+33 more)

### Community 72 - "NativeMethods"
Cohesion: 0.17
Nodes (4): NativeMethods, POINT, RECT, Win32MouseHookSource

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.19
Nodes (16): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, SettingsJson, S4-05 Implementation Plan, S4-05 task: save and load effect assignments (+8 more)

### Community 76 - "App"
Cohesion: 0.09
Nodes (5): Application, App, Win32DisplayChangeSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 78 - "StrokeWidth"
Cohesion: 0.17
Nodes (4): StrokeWidth, Pixels, SettingsFormatException, SettingsJson

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.18
Nodes (8): Dependabot config (nuget, github-actions, FIX prefix), Dependabot config, PR lint workflow, PR lint workflow (branch name and title check), Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM agent rules, Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.13
Nodes (19): Stage 1 Manual Desktop Gate, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog, tools/E2ERunner black-box runner, E2ERunner.Tests drift check, JsonSettingsStore, KeyboardHookEvents.NeutralKeyTag (+11 more)

### Community 83 - "DrawEM.App.Presentation.Effects Group"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "IntPtr"
Cohesion: 0.24
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 85 - "RenderLagHarness Group"
Cohesion: 0.13
Nodes (7): ITrayMenu, NotifyIconTrayHost, IconUri, FakeTrayMenu, Shown, Updates, NotifyIconTrayHostTests

### Community 86 - "DrawEM.App.Application.Sound"
Cohesion: 0.16
Nodes (5): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.Tests.Infrastructure.Settings

### Community 87 - "IEnumerable Group"
Cohesion: 0.16
Nodes (6): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.24
Nodes (19): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, Sound Application README, SoundChannelController (+11 more)

### Community 89 - "Step 2 Implementation Review"
Cohesion: 0.14
Nodes (21): Global sound channel, Ten-second playback cap, Confirmed test boundaries, Sound Command Business Test Scenarios, Sound business test scenarios, Ignore Stale Finish Scenario, Playback business scenarios (start, replace, restart, cap, stale finish), Shortcut and independent-channel scenarios (+13 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.21
Nodes (13): GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, First Key-Down Trigger (no auto-repeat), FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved) (+5 more)

### Community 91 - "v3 Step 6 plan"
Cohesion: 0.15
Nodes (19): v3 Step 5 acceptance criteria, Per-Monitor-V2 single overlay window transform, SettingsFailureDialog, v3 Step 5 plan, DrawingSessionController, StrokeRenderElement, v3 Step 5: Configurable drawing style (task), Stroke width in physical pixels (+11 more)

### Community 92 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.18
Nodes (15): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing, drawEM v4 Parallel Work Packages (+7 more)

### Community 93 - "TrayApplication"
Cohesion: 0.09
Nodes (22): build-test-publish job, CI workflow (build, test, coverage, publish, Velopack smoke), CI workflow, .NET SDK pinned by global.json, Velopack packaging smoke step, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip) (+14 more)

### Community 94 - "S4-09 Preview Draft Effects (acceptance)"
Cohesion: 0.10
Nodes (29): IEffectSurface Presentation Port, S4-08 Choose Effects in Settings (task), S4-09 Preview Draft Effects (acceptance), S4-09 Acceptance criteria, EffectChannelController, EffectMonitorGuard, EffectSampleFailureRouter, EffectSampler (+21 more)

### Community 95 - "Animation Group"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.16
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "GlobalShortcutAdapter"
Cohesion: 0.11
Nodes (24): S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement), S4-02 Implementation Plan, Ten-Second Effect Maximum, S4-02: Control One Running Effect (+16 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.20
Nodes (15): ManagedSoundLibrary, S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot) (+7 more)

### Community 100 - ".Attach"
Cohesion: 0.13
Nodes (10): GlassBackdrop, Supported, ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles, UiTheme (+2 more)

### Community 101 - "FEAT-009-effect-loop-and-limit/task.md"
Cohesion: 0.17
Nodes (11): v4 / FEAT-009: acceptance criteria, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior (+3 more)

### Community 102 - "Lottie built-in effects via SkiaSharp.Skottie"
Cohesion: 0.29
Nodes (8): Built-in effect, Effect, CONTEXT.md domain glossary, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, SkiaSharp.Skottie Lottie player

### Community 103 - "UiThemeState"
Cohesion: 0.20
Nodes (4): UiThemeState, Current, UiThemeStateTests, TrayMenuWindowTests

### Community 104 - "drawEM README"
Cohesion: 0.11
Nodes (25): Build provenance attestation, Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), SHA256SUMS.txt download manifest, Verify tag step, R4-04 signing step (blocked), ADR-0002: Releases may start unsigned; signing depends on license and SignPath (+17 more)

### Community 105 - "SoundSlot.cs Group"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.13
Nodes (12): SoundSlot, Eight Sound Slots Ctrl+Alt+1..8, v3 Additional Fixes, Eight Numbered Action Slots, drawEM v3 Roadmap: Drawing and Action Settings, Save Applies Validated Snapshot, Settings Persistence and Startup Fallback, Settings Window (Drawing and Actions tabs) (+4 more)

### Community 107 - "drawEM v4: out of scope"
Cohesion: 0.38
Nodes (4): Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, drawEM v4: out of scope

### Community 108 - "R4-06 Velopack Installer Plan"
Cohesion: 0.13
Nodes (15): GitHub release Environment, SIGNPATH_API_TOKEN, SIGNPATH_API_TOKEN in protected release Environment, SignPath Foundation, SignPath Foundation Code Signing, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip (+7 more)

### Community 109 - "FEAT-011-glass-ui/plan.md"
Cohesion: 0.23
Nodes (9): v4 / FEAT-011: acceptance criteria, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, Previous behavior, Required behavior (+1 more)

### Community 110 - "S4-10 Apply Settings Safely (acceptance) Group"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 112 - "Step 4 Plan: Publish and Verify"
Cohesion: 0.21
Nodes (15): Stage 1 Acceptance Results: Sound Command, sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Step 1 Implementation Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundAssignments.Slots, SoundConfiguration (Resolve / PathOf), SoundFailure record (+7 more)

### Community 113 - "FakeUpdateService"
Cohesion: 0.12
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 114 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.27
Nodes (11): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency (+3 more)

### Community 116 - "Infrastructure/Drawing/NativeMethods.cs Group"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 117 - "LoggingUpdateFailureReporter"
Cohesion: 0.28
Nodes (5): LoggingUpdateFailureReporter, UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 119 - "FakePlayback"
Cohesion: 0.29
Nodes (6): FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 120 - ".TryFind"
Cohesion: 0.28
Nodes (4): BuiltInEffect, EffectCatalog, BuiltIn, EffectCatalogTests

### Community 121 - "Func"
Cohesion: 0.27
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 122 - "RenderLagHarness tool"
Cohesion: 0.22
Nodes (13): F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, F03 synthetic harness results (before/after), RenderLagHarness tool (+5 more)

### Community 124 - ".Create"
Cohesion: 0.29
Nodes (3): RecordingReporter, Lines, UpdateFeedTests

### Community 125 - "RenderLagHarness/Program.cs"
Cohesion: 0.08
Nodes (5): RenderLagHarness, ShortcutKeyKind, Digit, Function, Letter

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "drawEM smoke test"
Cohesion: 0.19
Nodes (12): v1 acceptance pass (step 7), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, Step 7 acceptance pass plan, Bounded-exit recheck (+4 more)

### Community 128 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 131 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 132 - "SoundReference"
Cohesion: 0.67
Nodes (3): Sound domain README, Sound Domain README, SoundReference

### Community 136 - "NativeMethods"
Cohesion: 0.25
Nodes (4): MONITORINFO, NativeMethods, POINT, RECT

### Community 137 - "Rect"
Cohesion: 0.26
Nodes (3): v4 / FEAT-011: implementation plan, TrayMenuPlacement, TrayMenuPlacementTests

### Community 138 - "v1 roadmap: drawing"
Cohesion: 0.48
Nodes (6): v1 roadmap: drawing, GlobalMouseInputAdapter seam, GlobalShortcutAdapter seam, OverlayWindowAdapter seam, v1 proposed test seams, TrayApplication seam

### Community 139 - "TrayMenuCommand"
Cohesion: 0.33
Nodes (5): TrayMenuCommand, Exit, Settings, StopEffects, Update

### Community 140 - "Proposed screen-action model (action, command, action slot)"
Cohesion: 0.40
Nodes (5): Action slot, Per-monitor channels and global sound channel, Screen actions model (proposed evolution), Proposed screen-action model (action, command, action slot), SINGLE_SCREEN mode

### Community 141 - "UpdateController"
Cohesion: 0.40
Nodes (3): UpdateController, UpdateControllerTests, UpdateFailureLog / update.log

## Knowledge Gaps
- **555 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+550 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 799 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **18 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `Lottie built-in effects via SkiaSharp.Skottie`, `v3 Step 8: Publish and verify on Windows (task)`, `Lottie Simple License`, `drawEM README`, `drawEM v3 Roadmap: Drawing and Action Settings`, `drawEM v4: out of scope`, `Settings Storage README`, `S4-12 Acceptance: Publish and Verify on Windows`, `S4-03 Acceptance Criteria`, `drawEM v4 Parallel Work Packages`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.179) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _555 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.06827309236947791 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `drawEM v3 Roadmap: Drawing and Action Settings`, `FEAT-005: Assisted E2E Runner`, `v3 Step 6 plan`, `drawEM v4 Parallel Work Packages`?**
  _High betweenness centrality (0.117) - this node is a cross-community bridge._
- **Should `R4 release publishing roadmap` be split into smaller, more focused modules?**
  _Cohesion score 0.14285714285714285 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `ScreenPoint`, `LottiePlayback`, `EffectId`?**
  _High betweenness centrality (0.094) - this node is a cross-community bridge._