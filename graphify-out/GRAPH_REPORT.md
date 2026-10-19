# Graph Report - worktree-clear-field-6052  (2026-10-19)

## Corpus Check
- 379 files · ~168,573 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3609 nodes · 10417 edges · 161 communities (139 shown, 22 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1734 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `6e4e0aa0`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Window
- v3 Step 6: Apply saved settings at runtime (task)
- Harness
- PlaySoundCommand
- App.xaml.cs
- SettingsViewModel
- DrawEM.App.Domain.Drawing
- ShortcutModifiers
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- SoundChannelHost
- R4-07 Tray Updates Plan
- Action
- MediaSoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- Rect
- Fixture
- .RunOnDispatcherThread
- DrawingSessionController
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- List
- DrawEM.App.Application.Sound
- Fixture
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- BuiltInEffectAnimations
- Lottie Simple License
- EffectInstance
- GlobalShortcutAdapter
- EffectSampler
- DrawEM.App.Infrastructure
- Harness
- Stage 1 sound implementation plan
- .Digit
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- ISoundPlayer
- LottiePlayback
- SaveHarness
- EffectId
- Shortcut
- SoundReference
- .Deserialize
- AppFailureLog
- JsonSettingsStore
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- ShortcutBindingsTests
- NativeMethods
- Fixture
- R4-02 Plan: Checksums and Provenance
- VelopackPackageIdentityTests
- STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- SoundId
- BlockingPlayerFactory
- LayerDependencyTests.cs
- Settings Storage README
- .OnStartup
- SingleInstanceGuardTests
- DrawingStyle
- drawEM v4 Parallel Work Packages
- IDisposable
- ScreenPoint
- drawEM v3 Roadmap: Drawing and Action Settings
- DrawEM.Tests.csproj
- BackgroundLogWriter
- NotifyIconTrayHost
- RenderLagHarness/Program.cs
- DrawEM.App.Application.Updates
- Sound Infrastructure README
- Step 2 Implementation Review
- FIX-006: Single Screen Edge
- Win32SessionInterruptionSource
- Stroke
- Self-contained win-x64 publish profile
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
- HexColor
- FailingSurface
- ITrayHost
- v3 Step 8: Publish and verify on Windows (task)
- SettingsErrorCode
- VelopackUpdateService
- v3 Step 7: Settings window (task)
- FakeUpdateService
- ShortcutCaptureResult
- UpdateController
- S4-03 Acceptance Criteria
- SoundFailure
- Show visual effects (Loop, Max length)
- .CreatePieces
- v3 Step 6 plan
- KeyDirection
- RenderLagHarness tool
- .Default_FollowsTheWindowsAppTheme
- RecordingReporter
- Step 5 Task: Route Sound into Call Microphone
- CLAUDE.md graphify rules
- v3 Step 8 Acceptance Results
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- v3 Step 2 acceptance criteria
- SoundReference
- Sound presentation placeholder README
- S4-12 Acceptance: Publish and Verify on Windows
- EffectFailure
- UpdateFailure
- v1 roadmap: drawing
- SettingsCommand
- .Load
- R4 Release Publishing Roadmap
- FakePlayer
- v3 Step 1: Settings snapshot and storage (task)
- v3 Step 5 acceptance criteria
- SoundChannelShutdownTimeoutException
- EffectLifecycleTests
- OverlayWindow
- EffectSurfaceWindow
- .Key
- WidthBox
- EffectSurfaceHost
- FakePlayback
- ErrorsText
- Branch and PR title work-id check
- %LOCALAPPDATA%\drawEM settings directory
- FakeDisplayChangeSource
- graphify knowledge-graph usage rules
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 143 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `SettingsViewModel` - 62 edges
7. `Window` - 62 edges
8. `MonitorBounds` - 60 edges
9. `EffectId` - 60 edges
10. `GlobalShortcutAdapter` - 59 edges

## Surprising Connections (you probably didn't know these)
- `Previous behavior` --references--> `StrokeRenderElement`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/task.md → src/DrawEM.App/Presentation/Drawing/StrokeRenderElement.cs
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

## Communities (161 total, 22 thin omitted)

### Community 0 - "SettingsEditorTests"
Cohesion: 0.15
Nodes (4): Evidence, SoundAction, Sound, SettingsEditorTests

### Community 1 - "StrokeRenderElement"
Cohesion: 0.21
Nodes (3): PhysicalToLocalTransform, StrokeRenderElement, VisualChildrenCount

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (17): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+9 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (59): BezierSmoothing, CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit (+51 more)

### Community 6 - "v3 Step 6: Apply saved settings at runtime (task)"
Cohesion: 0.16
Nodes (12): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, Settings Schema/Version Policy, v3 Settings Defaults, v3 Step 1 plan, Default fallback with retained damaged file, Right Alt reserved for AltGr, Settings schema/version policy (+4 more)

### Community 7 - "Harness"
Cohesion: 0.07
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.13
Nodes (9): ISoundFailureReporter, PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingSoundFailures, IgnoringReporter, ThrowingReporter, RecordingReporter (+1 more)

### Community 9 - "App.xaml.cs"
Cohesion: 0.06
Nodes (11): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray, ResourceDictionary, GlassDark (+3 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.03
Nodes (59): BindableBase, EffectOption, All, SettingsViewModel, BezierSmoothing, CanDecreaseWidth, CanIncreaseWidth, CanSave (+51 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (11): DrawEM.Tests.Domain, DrawEM.App.Application.Drawing, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+3 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.11
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.13
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (17): EffectSample, Effect, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples (+9 more)

### Community 15 - "Fixture"
Cohesion: 0.17
Nodes (11): DrawEM.Tests.Application.Sound, FakeRequest, IsCurrent, Fixture, Controller, Failures, Log, OnCreate (+3 more)

### Community 16 - "SoundChannelHost"
Cohesion: 0.17
Nodes (10): SoundChannelHost, StartHold, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay, Played (+2 more)

### Community 17 - "R4-07 Tray Updates Plan"
Cohesion: 0.11
Nodes (19): %LOCALAPPDATA%\drawEM settings directory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, VelopackHookExitTests, Velopack, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, DRAWEM_UPDATE_FEED local feed (+11 more)

### Community 18 - "Action"
Cohesion: 0.10
Nodes (12): ISoundFilePicker, FakePicker, Next, Picking, Picks, AsyncProbe, ClosureProbe, DelegateToPointerProbe (+4 more)

### Community 19 - "MediaSoundPlayer"
Cohesion: 0.20
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.28
Nodes (3): SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.13
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "Rect"
Cohesion: 0.14
Nodes (5): EffectCanvasPlacement, EffectRenderLayout, TrayMenuPlacement, EffectRenderLayoutTests, TrayMenuPlacementTests

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - ".RunOnDispatcherThread"
Cohesion: 0.22
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "DrawingSessionController"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.17
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+4 more)

### Community 29 - "Fixture"
Cohesion: 0.09
Nodes (14): SettableCursor, Available, Position, Reads, EffectMonitorGuardTests, Fixture, Channel, Cursor (+6 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.08
Nodes (23): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+15 more)

### Community 31 - "List"
Cohesion: 0.15
Nodes (8): TrayApplication, FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable, TimedOutSound, TrayApplicationTests

### Community 32 - "DrawEM.App.Application.Sound"
Cohesion: 0.12
Nodes (8): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Release, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, VelopackHookExitTests

### Community 33 - "Fixture"
Cohesion: 0.09
Nodes (16): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+8 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.12
Nodes (14): AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed, Player (+6 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.16
Nodes (10): NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library, Notifications (+2 more)

### Community 36 - "Harness"
Cohesion: 0.12
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.12
Nodes (9): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Smoothing (+1 more)

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.12
Nodes (7): DrawEM.Tests.Domain.Effects, EffectCatalog, BuiltIn, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "Lottie Simple License"
Cohesion: 0.16
Nodes (24): Built-in Lottie Effect, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources, celebration-confetti.json (Lottie effect) (+16 more)

### Community 41 - "EffectInstance"
Cohesion: 0.11
Nodes (14): EffectInstance, EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, Playback, DisposeFailure, HasCompletionHandler (+6 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.13
Nodes (5): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, INeutralKeyEmitter

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.12
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.11
Nodes (13): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+5 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.17
Nodes (32): Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, drawEM manual smoke test, drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts (+24 more)

### Community 47 - ".Digit"
Cohesion: 0.15
Nodes (11): ShortcutKey, Kind, Value, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw (+3 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.11
Nodes (31): ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController, EffectChannelController (+23 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.20
Nodes (18): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectMonitorGuard.StopEffects(), EffectStopRoutes (+10 more)

### Community 50 - "ISoundPlayer"
Cohesion: 0.08
Nodes (13): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, PlayerFactory, Created, ThreadRecordingPlayer (+5 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.15
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.06
Nodes (30): ISoundStartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore, Failure, OnSave, Saved (+22 more)

### Community 53 - "EffectId"
Cohesion: 0.10
Nodes (14): EffectId, Value, ActionSlot, Action, IsEmpty, Number, EffectAction, Effect (+6 more)

### Community 54 - "Shortcut"
Cohesion: 0.13
Nodes (13): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+5 more)

### Community 55 - "SoundReference"
Cohesion: 0.14
Nodes (8): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, MediaSoundPlayerFactoryTests

### Community 57 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 58 - "JsonSettingsStore"
Cohesion: 0.15
Nodes (8): JsonSettingsStore, DefaultDirectory, SettingsPath, NoFailures, NoSettingsFailures, RuntimeSettingsTests, LibraryDirectory, SettingsDirectory

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.22
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.16
Nodes (10): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+2 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.12
Nodes (11): MSLLHOOKSTRUCT, KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events, NativeMethods (+3 more)

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 66 - "NativeMethods"
Cohesion: 0.14
Nodes (6): MONITORINFO, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource

### Community 67 - "Fixture"
Cohesion: 0.15
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 Plan: Checksums and Provenance"
Cohesion: 0.18
Nodes (14): SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), R4-02 Acceptance: Checksums and Provenance, One-Byte Mutation Check, R4-02 acceptance criteria, R4-02 Plan: Checksums and Provenance, actions/attest@v4 Build Provenance, GitHub Build Provenance Attestation (actions/attest), gh attestation verify (+6 more)

### Community 70 - "STEPS.md"
Cohesion: 0.15
Nodes (24): v3 Step 8 Windows Acceptance Gate, Velopack Per-User Installer, v4 Release Publishing Roadmap, R4-03 Acceptance Criteria, First Release Gate Table, SmartScreen 'Windows protected your PC' warning, drawEM v4.0.0 Release, R4-03 Implementation Plan (+16 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.09
Nodes (40): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+32 more)

### Community 72 - "SoundId"
Cohesion: 0.14
Nodes (8): SoundId, Value, FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.19
Nodes (16): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, SettingsJson, S4-05 Implementation Plan, S4-05 task: save and load effect assignments (+8 more)

### Community 76 - ".OnStartup"
Cohesion: 0.19
Nodes (4): App, ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 78 - "DrawingStyle"
Cohesion: 0.10
Nodes (18): Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, Out of scope, Per stroke, Previous behavior, Required behavior, Settings file (+10 more)

### Community 79 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.15
Nodes (12): Dependabot config (nuget, github-actions, FIX prefix), PR lint workflow (branch name and title check), drawEM rules (AGENTS.md), drawEM agent rules, drawEM v4: out of scope, drawEM v4 Parallel Work Packages, Visual Effects Package (S4), Release Publishing README (+4 more)

### Community 80 - "IDisposable"
Cohesion: 0.18
Nodes (5): CompositeDisposable, SingleInstanceGuard, DefaultName, CompositeDisposableTests, FakeDisposable

### Community 81 - "ScreenPoint"
Cohesion: 0.10
Nodes (13): StartEffectCommand, ScreenPoint, MonitorBounds, EffectPlacement, Cursor, Monitor, EffectMonitorGuard, IEffectStartQueue (+5 more)

### Community 82 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.08
Nodes (33): Step 1 Implementation Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundAssignments.Slots, SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+25 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "BackgroundLogWriter"
Cohesion: 0.21
Nodes (3): BackgroundLogWriter, LoggingEffectFailureReporter, LoggingUpdateFailureReporter

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.05
Nodes (28): v4 / FEAT-011: acceptance criteria, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, v4 / FEAT-011: implementation plan, Previous behavior (+20 more)

### Community 86 - "RenderLagHarness/Program.cs"
Cohesion: 0.11
Nodes (5): RenderLagHarness, ShortcutKeyKind, Digit, Function, Letter

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.12
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.21
Nodes (21): EffectFailureLog, LoggingEffectFailureReporter, Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand (+13 more)

### Community 89 - "Step 2 Implementation Review"
Cohesion: 0.12
Nodes (24): Action slot, Per-monitor channels and global sound channel, Screen actions model (proposed evolution), Global sound channel, Ten-second playback cap, Confirmed test boundaries, Sound Command Business Test Scenarios, Sound business test scenarios (+16 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.21
Nodes (13): GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, First Key-Down Trigger (no auto-repeat), FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved) (+5 more)

### Community 91 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 92 - "Stroke"
Cohesion: 0.31
Nodes (4): DrawingState, Stroke, StrokeRenderElementTests, WidthsAndScales

### Community 93 - "Self-contained win-x64 publish profile"
Cohesion: 0.11
Nodes (16): build-test-publish job, CI workflow (build, test, coverage, publish, Velopack smoke), CI workflow, .NET SDK pinned by global.json, Velopack packaging smoke step, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip) (+8 more)

### Community 94 - "S4-09 Preview Draft Effects (acceptance)"
Cohesion: 0.10
Nodes (28): S4-09 Preview Draft Effects (acceptance), S4-09 Acceptance criteria, EffectChannelController, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, SettingsEditor (+20 more)

### Community 95 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.15
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "GlobalShortcutAdapter"
Cohesion: 0.09
Nodes (27): S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement) (+19 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.19
Nodes (16): ManagedSoundLibrary, S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot) (+8 more)

### Community 99 - "ManualTimeProvider"
Cohesion: 0.16
Nodes (5): ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 100 - ".Attach"
Cohesion: 0.17
Nodes (4): GlassBackdrop, Supported, NativeMethods, POINT

### Community 101 - "FEAT-009-effect-loop-and-limit/task.md"
Cohesion: 0.17
Nodes (11): v4 / FEAT-009: acceptance criteria, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior (+3 more)

### Community 102 - "Lottie built-in effects via SkiaSharp.Skottie"
Cohesion: 0.22
Nodes (11): Built-in effect, Effect, CONTEXT.md domain glossary, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, SkiaSharp allowed only in Presentation (+3 more)

### Community 103 - "R4-06 Velopack Installer Plan"
Cohesion: 0.14
Nodes (14): GitHub release Environment, SIGNPATH_API_TOKEN, SIGNPATH_API_TOKEN in protected release Environment, SignPath Foundation, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip, WinGet zip/nested-portable manifest (+6 more)

### Community 104 - "drawEM README"
Cohesion: 0.11
Nodes (26): Build provenance attestation, Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), SHA256SUMS.txt download manifest, Verify tag step, R4-04 signing step (blocked), ADR-0002: Releases may start unsigned; signing depends on license and SignPath (+18 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 107 - "FailingSurface"
Cohesion: 0.13
Nodes (10): Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface, Disposed, Last (+2 more)

### Community 108 - "ITrayHost"
Cohesion: 0.14
Nodes (5): Application, IApplicationLifetime, IOverlayLifetime, ITrayHost, WpfApplicationLifetime

### Community 109 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.29
Nodes (12): v3 Step 3 acceptance criteria, v3 Step 3 plan, GlobalShortcutAdapter, SoundChannelHost, v3 Step 3: Configurable action shortcuts (task), Immutable in-memory binding snapshot, v3 Step 4 acceptance criteria, v3 Step 4 plan (+4 more)

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.13
Nodes (8): IUpdateService, IsInstalled, PendingVersion, VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 112 - "v3 Step 7: Settings window (task)"
Cohesion: 0.23
Nodes (9): Hook-owned capture state, v3 Step 7 acceptance criteria, SettingsEditorTests / SettingsViewModelTests / SoundSamplerTests, v3 Step 7 plan, Ctrl+Alt+1..8 default slot shortcut proposal, drawEM Settings prototype (HTML), Prototype Drawing / Actions tabs with Save, Cancel, Restore defaults, Settings Prototype UI (Drawing: Stroke, Shortcuts) (+1 more)

### Community 113 - "FakeUpdateService"
Cohesion: 0.20
Nodes (7): FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled, LastToken, PendingVersion

### Community 114 - "ShortcutCaptureResult"
Cohesion: 0.19
Nodes (8): Cancelled, Captured, IShortcutCapture, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.12
Nodes (25): S4-03 acceptance: play built-in Lottie effects, S4-03 Acceptance Criteria, BuiltInEffectAnimations, Confetti Effect (Monitor), EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Flashing Check (max three flashes per second), Focus Ring Effect (Cursor) (+17 more)

### Community 117 - "SoundFailure"
Cohesion: 0.33
Nodes (4): SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 119 - ".CreatePieces"
Cohesion: 0.38
Nodes (5): Line, Quadratic, StrokePathPiece, StrokeSmoothingGeometry, StrokeSmoothingGeometryTests

### Community 120 - "v3 Step 6 plan"
Cohesion: 0.24
Nodes (10): v3 Step 6 acceptance criteria, Sound start lock shared by host and Save, omp orchestrator prompt for Step 6 (PR #39), Agent split: startup, save, media-tests, herdr pane/agent orchestration, omp Orchestrator in herdr, v3 Step 6 plan, Fresh press required after Save (+2 more)

### Community 121 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 122 - "RenderLagHarness tool"
Cohesion: 0.24
Nodes (12): F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, F03 synthetic harness results (before/after), RenderLagHarness tool (+4 more)

### Community 123 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 124 - "RecordingReporter"
Cohesion: 0.28
Nodes (3): RecordingReporter, Lines, UpdateFeedTests

### Community 125 - "Step 5 Task: Route Sound into Call Microphone"
Cohesion: 0.36
Nodes (8): Step 5 Acceptance: Call Microphone Routing, Voicemeeter A/B Bus Routing, Step 5 Plan: External Microphone Mixing, Google Meet, Windows Per-App Output Routing, Voicemeeter Standard external mixer, Step 5 Task: Route Sound into Call Microphone, Google Meet

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "v3 Step 8 Acceptance Results"
Cohesion: 0.23
Nodes (8): v3 Step 8 Acceptance Results, Automated Gate (restore/test/publish), Manual Desktop Checks, v3 Release Blockers (unverified layout/DPI/multi-monitor checks), v3 Step 8 acceptance criteria, v3 Step 8 plan, Unintended input-layout switch is a release blocker, Separate manual desktop results document

### Community 128 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 131 - "v3 Step 2 acceptance criteria"
Cohesion: 0.27
Nodes (9): v3 Step 2 acceptance criteria, SHA-256 content-addressed copy names, v3 Step 2 plan, Draft imports tracked separately from saved references, Startup Orphan Media Cleanup, v3 Step 2: Managed sound library (task), Managed sound library (%LOCALAPPDATA%\drawEM\sounds), Startup orphan cleanup (+1 more)

### Community 132 - "SoundReference"
Cohesion: 0.67
Nodes (3): Sound domain README, Sound Domain README, SoundReference

### Community 134 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.27
Nodes (11): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency (+3 more)

### Community 135 - "EffectFailure"
Cohesion: 0.33
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 136 - "UpdateFailure"
Cohesion: 0.38
Nodes (4): UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 137 - "v1 roadmap: drawing"
Cohesion: 0.29
Nodes (8): v1 acceptance pass (step 7), v1 roadmap: drawing, GlobalMouseInputAdapter seam, GlobalShortcutAdapter seam, OverlayWindowAdapter seam, v1 proposed test seams, TrayApplication seam, Step 7 acceptance pass plan

### Community 138 - "SettingsCommand"
Cohesion: 0.19
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 139 - ".Load"
Cohesion: 0.14
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 140 - "R4 Release Publishing Roadmap"
Cohesion: 0.11
Nodes (11): Portable win-x64 ZIP, release.yml Release Workflow, R4 Release Publishing Roadmap, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation (+3 more)

### Community 141 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 142 - "v3 Step 1: Settings snapshot and storage (task)"
Cohesion: 0.24
Nodes (9): Durable replacement settings storage, v3 Step 1: Settings snapshot and storage (task), Domain/Application free of WPF, Win32, filesystem types, Eight numbered action slots, Immutable validated settings snapshot, Active Settings Snapshot, Ten-second sound cap with stop-then-start replacement, Sample preview through global sound channel (+1 more)

### Community 143 - "v3 Step 5 acceptance criteria"
Cohesion: 0.33
Nodes (9): v3 Step 5 acceptance criteria, Per-Monitor-V2 single overlay window transform, SettingsFailureDialog, v3 Step 5 plan, DrawingSessionController, StrokeRenderElement, v3 Step 5: Configurable drawing style (task), Stroke width in physical pixels (+1 more)

### Community 146 - "EffectLifecycleTests"
Cohesion: 0.27
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 147 - "OverlayWindow"
Cohesion: 0.24
Nodes (3): IOverlayView, Window, OverlayWindow

### Community 150 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 151 - "EffectSurfaceHost"
Cohesion: 0.25
Nodes (3): EffectSurfaceHost, Visual, VisualChildrenCount

### Community 152 - "FakePlayback"
Cohesion: 0.29
Nodes (6): FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 154 - "ErrorsText"
Cohesion: 0.40
Nodes (5): Errors, Status, ErrorsText, StatusText, TextBlock

### Community 155 - "Branch and PR title work-id check"
Cohesion: 0.50
Nodes (3): Dependabot config, PR lint workflow, Branch and PR title work-id check

### Community 156 - "%LOCALAPPDATA%\drawEM settings directory"
Cohesion: 0.67
Nodes (3): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, VelopackPackageIdentityTests

## Knowledge Gaps
- **558 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+553 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 803 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **22 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `Lottie built-in effects via SkiaSharp.Skottie`, `S4-12 Acceptance: Publish and Verify on Windows`, `Lottie Simple License`, `drawEM README`, `Settings Storage README`, `v3 Step 8: Publish and verify on Windows (task)`, `Stage 1 sound implementation plan`, `drawEM v4 Parallel Work Packages`, `drawEM architecture`, `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `drawEM v3 Roadmap: Drawing and Action Settings`, `S4-03 Acceptance Criteria`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.174) - this node is a cross-community bridge._
- **Are the 88 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 88 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _558 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.14750290360046459 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `v3 Step 2 acceptance criteria`, `v3 Step 6: Apply saved settings at runtime (task)`, `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `v3 Step 1: Settings snapshot and storage (task)`, `v3 Step 5 acceptance criteria`, `v3 Step 7: Settings window (task)`, `drawEM v4 Parallel Work Packages`, `drawEM v3 Roadmap: Drawing and Action Settings`, `v3 Step 6 plan`, `v3 Step 8 Acceptance Results`?**
  _High betweenness centrality (0.099) - this node is a cross-community bridge._
- **Should `R4 release publishing roadmap` be split into smaller, more focused modules?**
  _Cohesion score 0.12380952380952381 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `ScreenPoint`, `LottiePlayback`, `EffectId`?**
  _High betweenness centrality (0.096) - this node is a cross-community bridge._