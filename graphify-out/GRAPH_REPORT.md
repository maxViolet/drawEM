# Graph Report - drawEM  (2026-11-02)

## Corpus Check
- 413 files · ~178,381 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 12 file(s) not represented in the graph (top: (none) 9, .ico 1, .pubxml 1)

## Summary
- 3882 nodes · 11221 edges · 162 communities (139 shown, 23 thin omitted)
- Extraction: 84% EXTRACTED · 16% INFERRED · 0% AMBIGUOUS · INFERRED: 1799 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d450b543`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElementTests
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
- ManualTimeProvider
- R4-07 Tray Updates Plan
- StrokeRenderElement
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- SlotViewModel
- Fixture
- List
- ScreenPoint
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- TrayApplication
- DrawEM.App.Application.Sound
- Fixture
- SoundChannelController
- SoundReference
- Harness
- SettingsEditor
- Rig
- EffectId
- Lottie Simple License
- SettingsWindow
- GlobalShortcutAdapter
- .FromSnapshot_BindsSoundAndEffectSlotsFromOneSnapshot_WithPlacementFromTheCatalog
- DrawEM.App.Infrastructure
- Harness
- v3 Step 8: Publish and verify on Windows (task)
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- ShortcutBindings
- LottiePlayback
- SaveHarness
- ActionSlot
- .Save
- Step 2 Implementation Review
- SettingsJson
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
- release-publishing/STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- drawEM v3 Roadmap: Drawing and Action Settings
- BlockingPlayerFactory
- LayerDependencyTests.cs
- Settings Storage README
- drawEM v4 Parallel Work Packages
- SingleInstanceGuardTests
- .Run
- drawEM v4: out of scope
- IDisposable
- Action
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- StrokeSmoothing
- NotifyIconTrayHost
- RenderLagHarness/Program.cs
- DrawEM.App.Application.Updates
- Sound Infrastructure README
- Stage 1 sound implementation plan
- FIX-006: Single Screen Edge
- .OnStartup
- UiThemeState
- EffectChannelController
- S4-09 Preview Draft Effects (acceptance)
- ResourceDictionary
- TrayMenuWindow
- GlobalShortcutAdapter
- S4-08 acceptance: choose effects in Settings
- TimeSpan
- .Attach
- FEAT-009-effect-loop-and-limit/task.md
- Lottie built-in effects via SkiaSharp.Skottie
- R4-06 Velopack Installer Plan
- drawEM README
- Steps
- DllImport
- ICursorPositionSource
- FEAT-011-glass-ui/plan.md
- ShortcutKey
- SettingsErrorCode
- VelopackUpdateService
- .Place
- FakeUpdateService
- EffectLifecycleTests
- UpdateController
- S4-03 Acceptance Criteria
- SoundFailure
- Show visual effects (Loop, Max length)
- .CreatePieces
- WidthBox
- Func
- RenderLagHarness tool
- MouseHookEventsTests
- InstanceActivationTests
- FakePicker
- CLAUDE.md graphify rules
- step-01-video-decoder-and-formats/task.md
- step-03-fullscreen-video-playback/task.md
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- step-04-video-controls-and-lifecycle/task.md
- SoundReference
- Sound presentation placeholder README
- S4-12 Acceptance: Publish and Verify on Windows
- DateTimeOffset
- LoggingUpdateFailureReporter
- EffectMonitorGuard
- Shortcut
- .Load
- step-01-draft-release/task.md
- IUpdateFailureReporter
- step-05-video-publish/task.md
- step-02-video-action-and-library/task.md
- InstanceActivation
- GlassBackdropTests.cs
- EffectSurfaceHost
- DrawEM.Tests
- README.md
- .Key
- Product contract
- OverlayWindowRenderTests
- FakePlayback
- .WindowProcedure
- DecreaseWidthButton
- ROADMAP-v5.md
- %LOCALAPPDATA%\drawEM settings directory
- Win32MonitorBoundsSource
- graphify knowledge-graph usage rules
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder
- graphify

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 153 edges
2. `SettingsSnapshot` - 74 edges
3. `GlobalShortcutAdapter` - 73 edges
4. `DrawingSessionController` - 71 edges
5. `SettingsViewModel` - 67 edges
6. `Harness` - 66 edges
7. `EffectId` - 65 edges
8. `MonitorBounds` - 64 edges
9. `SaveHarness` - 64 edges
10. `DrawEM.App.Domain.Drawing` - 63 edges

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

## Communities (162 total, 23 thin omitted)

### Community 0 - "SettingsEditorTests"
Cohesion: 0.14
Nodes (4): Evidence, SoundAction, Sound, SettingsEditorTests

### Community 1 - "StrokeRenderElementTests"
Cohesion: 0.18
Nodes (8): v4 / FEAT-012: implementation plan, DrawingStyle, HexColor, StrokeWidth, Pixels, DeviceSurface, StrokeRenderElementTests, WidthsAndScales

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.11
Nodes (19): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing (+11 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.09
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (56): BezierSmoothing, CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear (+48 more)

### Community 6 - "v3 Step 6: Apply saved settings at runtime (task)"
Cohesion: 0.06
Nodes (50): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, Settings Schema/Version Policy, v3 Settings Defaults, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Settings schema/version policy (+42 more)

### Community 7 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.07
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 10 - "SettingsViewModel"
Cohesion: 0.05
Nodes (28): IShortcutCapture, BindableBase, SettingsViewModel, BezierSmoothing, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (11): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+3 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.13
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.09
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.08
Nodes (17): EffectSample, Effect, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples (+9 more)

### Community 15 - "Fixture"
Cohesion: 0.10
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.10
Nodes (16): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+8 more)

### Community 17 - "R4-07 Tray Updates Plan"
Cohesion: 0.11
Nodes (19): %LOCALAPPDATA%\drawEM settings directory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, VelopackHookExitTests, Velopack, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, DRAWEM_UPDATE_FEED local feed (+11 more)

### Community 18 - "StrokeRenderElement"
Cohesion: 0.16
Nodes (4): Stroke, PhysicalToLocalTransform, StrokeRenderElement, VisualChildrenCount

### Community 19 - "ISoundPlayer"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.15
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.13
Nodes (7): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.18
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "List"
Cohesion: 0.21
Nodes (5): FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "ScreenPoint"
Cohesion: 0.20
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.19
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 29 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.06
Nodes (22): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+14 more)

### Community 31 - "TrayApplication"
Cohesion: 0.12
Nodes (10): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+2 more)

### Community 32 - "DrawEM.App.Application.Sound"
Cohesion: 0.08
Nodes (17): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Release, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, SoundSlot (+9 more)

### Community 33 - "Fixture"
Cohesion: 0.10
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 35 - "SoundReference"
Cohesion: 0.11
Nodes (19): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, NoSettingsFailures (+11 more)

### Community 36 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.09
Nodes (10): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Smoothing (+2 more)

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 39 - "EffectId"
Cohesion: 0.09
Nodes (15): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, EffectPlacement, Cursor (+7 more)

### Community 40 - "Lottie Simple License"
Cohesion: 0.15
Nodes (25): Built-in Lottie Effect, EffectCatalog, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources (+17 more)

### Community 41 - "SettingsWindow"
Cohesion: 0.11
Nodes (4): PickColorButton, SettingsWindow, PickColor, ViewModel

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.08
Nodes (10): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+2 more)

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.12
Nodes (4): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, VirtualKeys, AppIconTests

### Community 45 - "Harness"
Cohesion: 0.12
Nodes (13): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+5 more)

### Community 46 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.14
Nodes (21): Right Alt reserved for AltGr, v3 Step 3 acceptance criteria, v3 Step 3 plan, GlobalShortcutAdapter, Right Alt detection by actual key state (not LLKHF_INJECTED), SoundChannelHost, v3 Step 3: Configurable action shortcuts (task), v3 Step 4 acceptance criteria (+13 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (42): Action slot, ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+34 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.20
Nodes (18): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectMonitorGuard.StopEffects(), EffectStopRoutes (+10 more)

### Community 50 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.15
Nodes (6): LottieFrameRenderer, LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.05
Nodes (32): ISoundStartHold, IDisplayChangeSource, FakeDisplayChangeSource, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore, Failure (+24 more)

### Community 53 - "ActionSlot"
Cohesion: 0.11
Nodes (11): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, MaxLength, SettingsError (+3 more)

### Community 54 - ".Save"
Cohesion: 0.16
Nodes (9): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult (+1 more)

### Community 55 - "Step 2 Implementation Review"
Cohesion: 0.17
Nodes (17): Ignore Stale Finish Scenario, Step 2 Implementation Review, ISoundFailureReporter, ISoundPlayer (playback port), ISoundPlayerFactory, LoggingSoundFailureReporter, MediaSoundPlayer (WPF MediaPlayer), MediaSoundPlayerFactory (+9 more)

### Community 57 - "AppFailureLog"
Cohesion: 0.14
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 58 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.24
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.10
Nodes (16): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+8 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.24
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.17
Nodes (9): IMouseHookSource, Win32MouseHookSource, Events, KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource (+1 more)

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 65 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, EffectSurfaceWindow, Layout, CountingMonitorBoundsSource, Queries (+2 more)

### Community 66 - "NativeMethods"
Cohesion: 0.18
Nodes (5): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT

### Community 67 - "Fixture"
Cohesion: 0.16
Nodes (8): Fixture, Consent, Controller, Log, Service, Time, WhileConfirming, UpdateControllerTests

### Community 68 - "R4-02 Plan: Checksums and Provenance"
Cohesion: 0.18
Nodes (14): SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), R4-02 Acceptance: Checksums and Provenance, One-Byte Mutation Check, R4-02 acceptance criteria, R4-02 Plan: Checksums and Provenance, actions/attest@v4 Build Provenance, GitHub Build Provenance Attestation (actions/attest), gh attestation verify (+6 more)

### Community 70 - "release-publishing/STEPS.md"
Cohesion: 0.16
Nodes (24): v3 Step 8 Windows Acceptance Gate, v4 Release Publishing Roadmap, R4-03 Acceptance Criteria, First Release Gate Table, SmartScreen 'Windows protected your PC' warning, drawEM v4.0.0 Release, R4-03 Implementation Plan, R4-03 First-Release Acceptance Task (+16 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.09
Nodes (41): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+33 more)

### Community 72 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.09
Nodes (24): Step 1 Implementation Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundAssignments.Slots, SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+16 more)

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, SettingsJson, S4-05 Implementation Plan, S4-05 task: save and load effect assignments (+7 more)

### Community 76 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.16
Nodes (10): drawEM v4 Parallel Work Packages, Visual Effects Package (S4), Release Publishing README, ADR-0002 Release Signing License, Portable win-x64 ZIP, R4 Release Publishing Roadmap, WinGet Portable ZIP Listing, Standard-User Windows Portable ZIP Test (+2 more)

### Community 78 - ".Run"
Cohesion: 0.17
Nodes (4): DrawingState, IOverlayView, Window, OverlayWindow

### Community 79 - "drawEM v4: out of scope"
Cohesion: 0.15
Nodes (9): Dependabot config (nuget, github-actions, FIX prefix), Dependabot config, PR lint workflow, PR lint workflow (branch name and title check), Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM agent rules, drawEM v4: out of scope (+1 more)

### Community 80 - "IDisposable"
Cohesion: 0.14
Nodes (6): CleanupSteps, CompositeDisposable, CleanupStepsTests, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 81 - "Action"
Cohesion: 0.09
Nodes (15): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe (+7 more)

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.16
Nodes (18): Global sound channel, Confirmed test boundaries, Sound Command Business Test Scenarios, Sound business test scenarios, Playback business scenarios (start, replace, restart, cap, stale finish), Shortcut and independent-channel scenarios, App.xaml.cs composition root, AppFailureLog (+10 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "StrokeSmoothing"
Cohesion: 0.14
Nodes (14): Evidence, v4 / FEAT-012: acceptance criteria, Out of scope, Per stroke, Previous behavior, Required behavior, Settings file, Settings window (+6 more)

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.10
Nodes (12): ITrayMenu, TrayMenuCommand, Exit, Settings, StopEffects, Update, NotifyIconTrayHost, IconUri (+4 more)

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.15
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.21
Nodes (21): EffectFailureLog, LoggingEffectFailureReporter, Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand (+13 more)

### Community 89 - "Stage 1 sound implementation plan"
Cohesion: 0.12
Nodes (41): v1 acceptance pass (step 7), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, drawEM manual smoke test, Step 7 acceptance pass plan (+33 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.19
Nodes (16): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+8 more)

### Community 91 - ".OnStartup"
Cohesion: 0.08
Nodes (7): Application, App, EffectStopRoutes, ISessionInterruptionSource, Win32DisplayChangeSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 92 - "UiThemeState"
Cohesion: 0.22
Nodes (4): v4 / FEAT-011: implementation plan, UiThemeState, Current, TrayMenuWindowTests

### Community 93 - "EffectChannelController"
Cohesion: 0.19
Nodes (11): S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement) (+3 more)

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
Cohesion: 0.17
Nodes (15): S4-06 Acceptance Criteria, EffectBinding, EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance (+7 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.19
Nodes (16): ManagedSoundLibrary, S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot) (+8 more)

### Community 99 - "TimeSpan"
Cohesion: 0.12
Nodes (4): LottieFrameLoop, ManualTimer, DueAt, LottieFrameLoopTests

### Community 100 - ".Attach"
Cohesion: 0.12
Nodes (10): GlassBackdrop, Supported, ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles, UiTheme (+2 more)

### Community 101 - "FEAT-009-effect-loop-and-limit/task.md"
Cohesion: 0.17
Nodes (11): v4 / FEAT-009: acceptance criteria, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior (+3 more)

### Community 102 - "Lottie built-in effects via SkiaSharp.Skottie"
Cohesion: 0.26
Nodes (9): Built-in effect, Effect, CONTEXT.md domain glossary, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, SkiaSharp allowed only in Presentation (+1 more)

### Community 103 - "R4-06 Velopack Installer Plan"
Cohesion: 0.18
Nodes (11): SignPath Foundation, SignPath Foundation Code Signing, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip, R4-06 Velopack Installer Plan, CI Velopack packaging smoke, Custom WPF Program.Main with VelopackApp.Run (+3 more)

### Community 104 - "drawEM README"
Cohesion: 0.06
Nodes (44): build-test-publish job, CI workflow (build, test, coverage, publish, Velopack smoke), CI workflow, .NET SDK pinned by global.json, Velopack packaging smoke step, Warnings as errors build, Build provenance attestation, Create draft release step (gh release create --draft --verify-tag) (+36 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "DllImport"
Cohesion: 0.13
Nodes (6): NativeMethods, POINT, RECT, NativeMethods, NativeMethods, POINT

### Community 107 - "ICursorPositionSource"
Cohesion: 0.18
Nodes (8): ICursorPositionSource, Win32CursorPositionSource, SettableCursor, Available, Position, Reads, MutableCursorPositionSource, Position

### Community 108 - "FEAT-011-glass-ui/plan.md"
Cohesion: 0.23
Nodes (9): v4 / FEAT-011: acceptance criteria, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, Previous behavior, Required behavior (+1 more)

### Community 109 - "ShortcutKey"
Cohesion: 0.21
Nodes (7): ShortcutKey, Kind, Value, ShortcutKeyKind, Digit, Function, Letter

### Community 110 - "SettingsErrorCode"
Cohesion: 0.33
Nodes (6): SettingsErrorCode, DuplicateShortcut, InvalidWidth, MissingShortcut, SlotLayout, UnknownEffect

### Community 111 - "VelopackUpdateService"
Cohesion: 0.23
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 113 - "FakeUpdateService"
Cohesion: 0.18
Nodes (7): FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled, LastToken, PendingVersion

### Community 114 - "EffectLifecycleTests"
Cohesion: 0.36
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 115 - "UpdateController"
Cohesion: 0.19
Nodes (4): IUpdateService, IsInstalled, PendingVersion, UpdateController

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.12
Nodes (25): S4-03 acceptance: play built-in Lottie effects, S4-03 Acceptance Criteria, BuiltInEffectAnimations, Confetti Effect (Monitor), EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Flashing Check (max three flashes per second), Focus Ring Effect (Cursor) (+17 more)

### Community 117 - "SoundFailure"
Cohesion: 0.28
Nodes (5): SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, SoundFailureLogTests

### Community 119 - ".CreatePieces"
Cohesion: 0.29
Nodes (6): DrawEM.Tests.Domain, Line, Quadratic, StrokePathPiece, StrokeSmoothingGeometry, StrokeSmoothingGeometryTests

### Community 120 - "WidthBox"
Cohesion: 0.20
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 121 - "Func"
Cohesion: 0.21
Nodes (6): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents, KeyboardHookEventsTests

### Community 122 - "RenderLagHarness tool"
Cohesion: 0.36
Nodes (10): F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, F03 synthetic harness results (before/after), RenderLagHarness tool (+2 more)

### Community 123 - "MouseHookEventsTests"
Cohesion: 0.22
Nodes (4): MouseHookEvents, MouseHookEventsTests, ButtonMessages, WheelMessages

### Community 125 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "step-01-video-decoder-and-formats/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-01: acceptance criteria, v5 / S5-01: implementation plan, Out of scope, Required outcome, Task, v5 / S5-01: prove MP4 playback and choose supported profiles

### Community 128 - "step-03-fullscreen-video-playback/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-03: acceptance criteria, v5 / S5-03: implementation plan, Out of scope, Required behavior, Task, v5 / S5-03: play video fullscreen on the active monitor

### Community 131 - "step-04-video-controls-and-lifecycle/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-04: acceptance criteria, v5 / S5-04: implementation plan, Out of scope, Required behavior, Task, v5 / S5-04: connect controls and lifecycle

### Community 132 - "SoundReference"
Cohesion: 0.67
Nodes (3): Sound domain README, Sound Domain README, SoundReference

### Community 134 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.27
Nodes (11): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency (+3 more)

### Community 135 - "DateTimeOffset"
Cohesion: 0.24
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, EffectFailureLogTests

### Community 136 - "LoggingUpdateFailureReporter"
Cohesion: 0.28
Nodes (5): LoggingUpdateFailureReporter, UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 138 - "Shortcut"
Cohesion: 0.12
Nodes (14): v5 / S5-02: implementation plan, SlotAction, SettingsCommand, Clear, Draw, Kind, Slot, SettingsCommandKind (+6 more)

### Community 139 - ".Load"
Cohesion: 0.14
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 140 - "step-01-draft-release/task.md"
Cohesion: 0.14
Nodes (9): release.yml Release Workflow, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation, R4-01: implementation plan, Task (+1 more)

### Community 141 - "IUpdateFailureReporter"
Cohesion: 0.20
Nodes (5): IUpdateFailureReporter, FakeFailureReporter, RecordingReporter, Lines, UpdateFeedTests

### Community 142 - "step-05-video-publish/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-05: acceptance criteria, v5 / S5-05: implementation plan, Out of scope, Required outcome, Task, v5 / S5-05: verify the published application

### Community 143 - "step-02-video-action-and-library/task.md"
Cohesion: 0.25
Nodes (6): Evidence, v5 / S5-02: acceptance criteria, Out of scope, Required behavior, Task, v5 / S5-02: add video actions and managed imports

### Community 144 - "InstanceActivation"
Cohesion: 0.13
Nodes (4): InstanceActivation, DefaultName, SingleInstanceGuard, DefaultName

### Community 145 - "GlassBackdropTests.cs"
Cohesion: 0.40
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 146 - "EffectSurfaceHost"
Cohesion: 0.25
Nodes (3): EffectSurfaceHost, Visual, VisualChildrenCount

### Community 147 - "DrawEM.Tests"
Cohesion: 0.29
Nodes (3): DrawEM.Tests, StaThread, WpfCollection

### Community 148 - "README.md"
Cohesion: 0.29
Nodes (5): drawEM v5: fullscreen video, Package structure, Steps, Terms, drawEM v5: steps

### Community 150 - "Product contract"
Cohesion: 0.33
Nodes (6): Action slots and media, Audio and preview, Playback and stop behavior, Playback surface and input, Product contract, Terms

### Community 152 - "FakePlayback"
Cohesion: 0.07
Nodes (24): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+16 more)

### Community 154 - "DecreaseWidthButton"
Cohesion: 0.40
Nodes (5): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, RepeatButton

### Community 155 - "ROADMAP-v5.md"
Cohesion: 0.40
Nodes (4): Completion, Delivery order, drawEM v5 roadmap: fullscreen MP4 video, Outcome

### Community 156 - "%LOCALAPPDATA%\drawEM settings directory"
Cohesion: 0.67
Nodes (3): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, VelopackPackageIdentityTests

## Knowledge Gaps
- **562 isolated node(s):** `python`, `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback` (+557 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 847 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **23 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `Lottie built-in effects via SkiaSharp.Skottie`, `S4-12 Acceptance: Publish and Verify on Windows`, `drawEM v3 Roadmap: Drawing and Action Settings`, `Lottie Simple License`, `drawEM README`, `Settings Storage README`, `drawEM v4 Parallel Work Packages`, `v3 Step 8: Publish and verify on Windows (task)`, `drawEM v4: out of scope`, `drawEM architecture`, `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `S4-03 Acceptance Criteria`, `Stage 1 sound implementation plan`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.175) - this node is a cross-community bridge._
- **Are the 93 inferred relationships involving `ScreenPoint` (e.g. with `.Move()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 93 INFERRED edges - model-reasoned connections that need verification._
- **What connects `python`, `CompletedStrokes`, `ActiveInstance` to the rest of the system?**
  _562 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.14174972314507198 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `v3 Step 6: Apply saved settings at runtime (task)`, `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `drawEM v3 Roadmap: Drawing and Action Settings`, `drawEM v4 Parallel Work Packages`, `FIX-006: Single Screen Edge`?**
  _High betweenness centrality (0.111) - this node is a cross-community bridge._
- **Should `R4 release publishing roadmap` be split into smaller, more focused modules?**
  _Cohesion score 0.11462450592885376 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `Action`, `LottiePlayback`, `ActionSlot`?**
  _High betweenness centrality (0.095) - this node is a cross-community bridge._