# Graph Report - drawEM  (2026-10-26)

## Corpus Check
- 401 files · ~174,846 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 12 file(s) not represented in the graph (top: (none) 9, .ico 1, .pubxml 1)

## Summary
- 3701 nodes · 10569 edges · 173 communities (150 shown, 23 thin omitted)
- Extraction: 84% EXTRACTED · 16% INFERRED · 0% AMBIGUOUS · INFERRED: 1743 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `72486aef`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Window
- v3 Step 1: Settings snapshot and storage (task)
- Harness
- PlaySoundCommand
- App.xaml.cs
- SlotViewModel
- DrawEM.App.Domain.Drawing
- ShortcutModifiers
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- SoundChannelHost
- New-VelopackPackage.ps1
- SettingsViewModel
- MediaSoundPlayer
- SettingsFailureDialog
- DrawEM.App.Application.Sound
- ManagedSoundLibraryTests
- Fact
- TrayApplication.cs
- Fixture
- .RunOnDispatcherThread
- ScreenPoint
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- List
- SettingsWindow
- Fixture
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- BuiltInEffectAnimations
- Lottie Simple License
- drawEM v4 Parallel Work Packages
- GlobalShortcutAdapter
- ShortcutBindingsTests
- DrawEM.App.Infrastructure
- Harness
- ISoundPlayer
- .Digit
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- JsonSettingsStore
- LottiePlayback
- SaveHarness
- .Validate
- .Save
- Step 2 Implementation Review
- .Deserialize
- AppFailureLog
- EffectId
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- MonitorBounds
- .TryGetBounds
- Fixture
- R4-02 Plan: Checksums and Provenance
- VelopackPackageIdentityTests
- release-publishing/STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- drawEM v3 Roadmap: Drawing and Action Settings
- BlockingPlayerFactory
- system_diagnostics
- Settings Storage README
- R4 Release Publishing Roadmap
- SingleInstanceGuard
- ManagedSoundLibrary
- drawEM rules (AGENTS.md)
- IDisposable
- Action
- Step 4 Acceptance: Publish and Verify
- DrawEM.Tests.csproj
- .OnStartup
- NotifyIconTrayHost
- RenderLagHarness/Program.cs
- DrawEM.App.Application.Updates
- Sound Infrastructure README
- Stage 1 sound implementation plan
- FIX-006: Single Screen Edge
- Win32SessionInterruptionSource
- UiThemeState
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
- GitHub release Environment
- drawEM README
- Steps
- DllImport
- v3 Step 6: Apply saved settings at runtime (task)
- FEAT-011-glass-ui/plan.md
- BuiltInEffectAnimations.cs
- SettingsErrorCode
- VelopackUpdateService
- v3 Step 8: Publish and verify on Windows (task)
- FakeUpdateService
- ActionSlot
- UpdateController
- S4-03 Acceptance Criteria
- SoundId
- Show visual effects (Loop, Max length)
- .CreatePieces
- v3 Step 5 acceptance criteria
- SoundReference
- StrokeRenderElement
- NativeMethods
- InstanceActivation
- FakePicker
- CLAUDE.md graphify rules
- v3 Step 8 Acceptance Results
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- v3 Step 7: Settings window (task)
- SoundReference
- Sound presentation placeholder README
- S4-12 Acceptance: Publish and Verify on Windows
- EffectFailure
- UpdateFailure
- EffectMonitorGuard
- Shortcut
- .Load
- release.yml Release Workflow
- RecordingReporter
- NativeMethods
- Step 1 Implementation Review
- .ReleaseStartedResources
- .Default_FollowsTheWindowsAppTheme
- v3 Step 2 acceptance criteria
- FakePlayback
- step-01-video-decoder-and-formats/task.md
- .Key
- step-02-video-action-and-library/task.md
- step-03-fullscreen-video-playback/task.md
- RecordingEffectSurface
- EffectSurfaceWindow
- step-04-video-controls-and-lifecycle/task.md
- step-05-video-publish/task.md
- WidthBox
- GlassDark
- graphify knowledge-graph usage rules
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder
- v1 roadmap: drawing
- README.md
- SettingsSaveResult
- Product contract
- .RejectedChord_ShowsWhyAndKeepsCapturing
- RecordingNotifications
- DecreaseWidthButton
- ROADMAP-v5.md
- FakeEffectSources.cs
- graphify

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
- `v5 / S5-02: implementation plan` --references--> `SlotAction`  [INFERRED]
  docs/v5/step-02-video-action-and-library/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `v4 / FEAT-012: implementation plan` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-011: implementation plan` --references--> `ITrayMenu`  [INFERRED]
  docs/v4/out-of-scope/FEAT-011-glass-ui/plan.md → src/DrawEM.App/Infrastructure/ITrayMenu.cs

## Import Cycles
- None detected.

## Communities (173 total, 23 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.06
Nodes (33): Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, Out of scope, Per stroke, Previous behavior, Required behavior, Settings file (+25 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SmartScreen Reputation Warnings, Consent-based update UX (startup + 24h check), Velopack installer and update feed, Velopack Per-User Installer (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.09
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (57): BezierSmoothing, CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear (+49 more)

### Community 6 - "v3 Step 1: Settings snapshot and storage (task)"
Cohesion: 0.12
Nodes (19): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, Settings Schema/Version Policy, v3 Settings Defaults, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Right Alt reserved for AltGr (+11 more)

### Community 7 - "Harness"
Cohesion: 0.07
Nodes (15): Cancelled, Captured, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate (+7 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.07
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 10 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.09
Nodes (14): DrawEM.App.Domain.Settings, DrawEM.Tests.Domain, DrawEM.App.Application.Drawing, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing (+6 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.08
Nodes (8): DrawEM.Tests.Domain.Effects, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, EffectCatalogTests, EffectLifecycleTests, Events, Interruptions

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 15 - "Fixture"
Cohesion: 0.12
Nodes (17): DrawEM.Tests.Application.Sound, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure, FakeRequest (+9 more)

### Community 16 - "SoundChannelHost"
Cohesion: 0.12
Nodes (13): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+5 more)

### Community 17 - "New-VelopackPackage.ps1"
Cohesion: 0.09
Nodes (18): %LOCALAPPDATA%\drawEM settings directory, CI Velopack packaging smoke, JsonSettingsStore.DefaultDirectory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, VelopackHookExitTests, VelopackPackageIdentityTests (+10 more)

### Community 18 - "SettingsViewModel"
Cohesion: 0.05
Nodes (28): IShortcutCapture, BindableBase, SettingsViewModel, BezierSmoothing, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 19 - "MediaSoundPlayer"
Cohesion: 0.17
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 21 - "DrawEM.App.Application.Sound"
Cohesion: 0.14
Nodes (8): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "TrayApplication.cs"
Cohesion: 0.20
Nodes (4): Application, IApplicationLifetime, IOverlayLifetime, WpfApplicationLifetime

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - ".RunOnDispatcherThread"
Cohesion: 0.22
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "ScreenPoint"
Cohesion: 0.18
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.17
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+4 more)

### Community 29 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.11
Nodes (17): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+9 more)

### Community 31 - "List"
Cohesion: 0.13
Nodes (8): ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable, TimedOutSound, TrayApplicationTests

### Community 33 - "Fixture"
Cohesion: 0.10
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.08
Nodes (17): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+9 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.16
Nodes (12): ActiveSettings, Current, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig (+4 more)

### Community 36 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.12
Nodes (10): Evidence, IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots (+2 more)

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.20
Nodes (4): BuiltInEffectAnimations, LottieEffectSurface, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "Lottie Simple License"
Cohesion: 0.16
Nodes (24): Built-in Lottie Effect, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources, celebration-confetti.json (Lottie effect) (+16 more)

### Community 41 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.20
Nodes (15): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing, drawEM v4 Parallel Work Packages (+7 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (10): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, IEffectStartQueue, INeutralKeyEmitter, SettableCursor, Available (+2 more)

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.09
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread, WpfCollection

### Community 45 - "Harness"
Cohesion: 0.11
Nodes (14): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+6 more)

### Community 46 - "ISoundPlayer"
Cohesion: 0.06
Nodes (19): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, PlayerFactory, Created, FakePlayer (+11 more)

### Community 47 - ".Digit"
Cohesion: 0.36
Nodes (3): ShortcutKey, Kind, Value

### Community 48 - "drawEM architecture"
Cohesion: 0.12
Nodes (28): Action slot, ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+20 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.15
Nodes (21): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectMonitorGuard.StopEffects(), EffectStopRoutes (+13 more)

### Community 50 - "JsonSettingsStore"
Cohesion: 0.11
Nodes (16): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+8 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.19
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.10
Nodes (23): FakeStore, Failure, OnSave, Saved, SaveHarness, Channel, Controller, EffectChannel (+15 more)

### Community 53 - ".Validate"
Cohesion: 0.19
Nodes (4): SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 54 - ".Save"
Cohesion: 0.09
Nodes (10): IActiveShortcuts, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, ISoundStartHold, Saved, SettingsSaveOperation, IdleSoundChannel (+2 more)

### Community 55 - "Step 2 Implementation Review"
Cohesion: 0.13
Nodes (23): SoundChannelController, Global sound channel, Ten-second playback cap, Confirmed test boundaries, Sound Command Business Test Scenarios, Sound business test scenarios, Ignore Stale Finish Scenario, Playback business scenarios (start, replace, restart, cap, stale finish) (+15 more)

### Community 57 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 58 - "EffectId"
Cohesion: 0.13
Nodes (9): BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, EffectAction, Effect, EffectSample (+1 more)

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.13
Nodes (12): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+4 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.26
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.25
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 65 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, TrayMenuPlacement, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+2 more)

### Community 67 - "Fixture"
Cohesion: 0.14
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 Plan: Checksums and Provenance"
Cohesion: 0.13
Nodes (17): SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), v4 Release Publishing Roadmap, Task, v4 / R4-01: create a versioned draft release, R4-02 Acceptance: Checksums and Provenance, One-Byte Mutation Check, R4-02 acceptance criteria, R4-02 Plan: Checksums and Provenance (+9 more)

### Community 70 - "release-publishing/STEPS.md"
Cohesion: 0.12
Nodes (26): drawEM v4: out of scope, R4-04 SignPath Signing Acceptance, R4-04 SignPath Signing Plan, ADR-0002 Release Signing License, R4-04 SignPath Signing Task, ADR-0002: Release signing and license, SignPath Foundation, SignPath Foundation Code Signing (+18 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.09
Nodes (40): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+32 more)

### Community 72 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.11
Nodes (21): FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog, tools/E2ERunner black-box runner, E2ERunner.Tests drift check, JsonSettingsStore, KeyboardHookEvents.NeutralKeyTag, ManagedSoundLibrary (+13 more)

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 74 - "system_diagnostics"
Cohesion: 0.15
Nodes (4): DrawEM.Tests.Release, DrawEM.Tests.Architecture, RetiredSoundAssignmentsTests, VelopackHookExitTests

### Community 75 - "Settings Storage README"
Cohesion: 0.19
Nodes (16): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, SettingsJson, S4-05 Implementation Plan, S4-05 task: save and load effect assignments (+8 more)

### Community 76 - "R4 Release Publishing Roadmap"
Cohesion: 0.24
Nodes (3): Portable win-x64 ZIP, R4 Release Publishing Roadmap, WinGet Portable ZIP Listing

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.24
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 78 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.18
Nodes (8): Dependabot config (nuget, github-actions, FIX prefix), Dependabot config, PR lint workflow, PR lint workflow (branch name and title check), Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM agent rules, Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 81 - "Action"
Cohesion: 0.10
Nodes (14): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe (+6 more)

### Community 82 - "Step 4 Acceptance: Publish and Verify"
Cohesion: 0.12
Nodes (25): v1 acceptance pass (step 7), drawEM v1 roadmap: drawing, Step 7 acceptance results (conditional pass), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template (+17 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - ".OnStartup"
Cohesion: 0.20
Nodes (4): App, ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.10
Nodes (12): ITrayMenu, TrayMenuCommand, Exit, Settings, StopEffects, Update, NotifyIconTrayHost, IconUri (+4 more)

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.14
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.21
Nodes (21): EffectFailureLog, LoggingEffectFailureReporter, Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand (+13 more)

### Community 89 - "Stage 1 sound implementation plan"
Cohesion: 0.22
Nodes (24): drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, v2 roadmap: screen actions, Ctrl+Alt+1..8 Sound Shortcut Slots, Stage 1 Sound Implementation Plan, Stage 1 sound implementation plan, Step 1 Acceptance: Command and Configuration, Step 1 Plan: Command and Configuration (+16 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.21
Nodes (13): GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, First Key-Down Trigger (no auto-repeat), FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved) (+5 more)

### Community 91 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 92 - "UiThemeState"
Cohesion: 0.22
Nodes (4): v4 / FEAT-011: implementation plan, UiThemeState, Current, TrayMenuWindowTests

### Community 93 - "Self-contained win-x64 publish profile"
Cohesion: 0.15
Nodes (11): build-test-publish job, CI workflow (build, test, coverage, publish, Velopack smoke), CI workflow, .NET SDK pinned by global.json, Velopack packaging smoke step, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip) (+3 more)

### Community 94 - "S4-09 Preview Draft Effects (acceptance)"
Cohesion: 0.12
Nodes (25): S4-09 Preview Draft Effects (acceptance), S4-09 Acceptance criteria, EffectChannelController, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, SettingsEditor (+17 more)

### Community 95 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.15
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "GlobalShortcutAdapter"
Cohesion: 0.09
Nodes (26): S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, Effect Invocation (instance ID, generation, placement), S4-02 Implementation Plan (+18 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.19
Nodes (16): ManagedSoundLibrary, S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot) (+8 more)

### Community 99 - "ManualTimeProvider"
Cohesion: 0.10
Nodes (9): BackgroundLogWriter, LoggingSoundFailureReporter, LoggingUpdateFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer (+1 more)

### Community 100 - ".Attach"
Cohesion: 0.22
Nodes (5): GlassBackdrop, Supported, UiTheme, Dark, Light

### Community 101 - "FEAT-009-effect-loop-and-limit/task.md"
Cohesion: 0.17
Nodes (11): v4 / FEAT-009: acceptance criteria, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior (+3 more)

### Community 102 - "Lottie built-in effects via SkiaSharp.Skottie"
Cohesion: 0.26
Nodes (9): Built-in effect, Effect, CONTEXT.md domain glossary, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, SkiaSharp allowed only in Presentation (+1 more)

### Community 103 - "GitHub release Environment"
Cohesion: 0.33
Nodes (6): GitHub release Environment, SIGNPATH_API_TOKEN, SIGNPATH_API_TOKEN in protected release Environment, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip

### Community 104 - "drawEM README"
Cohesion: 0.10
Nodes (28): Build provenance attestation, Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), SHA256SUMS.txt download manifest, Verify tag step, R4-04 signing step (blocked), ADR-0002: Releases may start unsigned; signing depends on license and SignPath (+20 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "DllImport"
Cohesion: 0.21
Nodes (3): MSLLHOOKSTRUCT, NativeMethods, POINT

### Community 107 - "v3 Step 6: Apply saved settings at runtime (task)"
Cohesion: 0.19
Nodes (12): v3 Step 6 acceptance criteria, Sound start lock shared by host and Save, omp orchestrator prompt for Step 6 (PR #39), Agent split: startup, save, media-tests, herdr pane/agent orchestration, omp Orchestrator in herdr, v3 Step 6 plan, Fresh press required after Save (+4 more)

### Community 108 - "FEAT-011-glass-ui/plan.md"
Cohesion: 0.23
Nodes (9): v4 / FEAT-011: acceptance criteria, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, Previous behavior, Required behavior (+1 more)

### Community 109 - "BuiltInEffectAnimations.cs"
Cohesion: 0.20
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 112 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.26
Nodes (13): v3 Step 3 acceptance criteria, v3 Step 3 plan, GlobalShortcutAdapter, SoundChannelHost, v3 Step 3: Configurable action shortcuts (task), Immutable in-memory binding snapshot, v3 Step 4 acceptance criteria, v3 Step 4 plan (+5 more)

### Community 113 - "FakeUpdateService"
Cohesion: 0.13
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 114 - "ActionSlot"
Cohesion: 0.22
Nodes (8): ActionSlot, Action, IsEmpty, Number, SlotAction, SoundAction, Sound, TestSettings

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.12
Nodes (26): IEffectSurface / IEffectPlayback Port, S4-03 acceptance: play built-in Lottie effects, S4-03 Acceptance Criteria, BuiltInEffectAnimations, Confetti Effect (Monitor), EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Flashing Check (max three flashes per second) (+18 more)

### Community 117 - "SoundId"
Cohesion: 0.22
Nodes (6): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 119 - ".CreatePieces"
Cohesion: 0.38
Nodes (5): Line, Quadratic, StrokePathPiece, StrokeSmoothingGeometry, StrokeSmoothingGeometryTests

### Community 120 - "v3 Step 5 acceptance criteria"
Cohesion: 0.33
Nodes (9): v3 Step 5 acceptance criteria, Per-Monitor-V2 single overlay window transform, SettingsFailureDialog, v3 Step 5 plan, DrawingSessionController, StrokeRenderElement, v3 Step 5: Configurable drawing style (task), Stroke width in physical pixels (+1 more)

### Community 121 - "SoundReference"
Cohesion: 0.12
Nodes (11): SoundReference, DisplayName, LibraryFileName, IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents (+3 more)

### Community 122 - "StrokeRenderElement"
Cohesion: 0.21
Nodes (15): OverlayWindow, StrokeRenderElement, F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results (+7 more)

### Community 123 - "NativeMethods"
Cohesion: 0.21
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 124 - "InstanceActivation"
Cohesion: 0.20
Nodes (3): InstanceActivation, DefaultName, InstanceActivationTests

### Community 125 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "v3 Step 8 Acceptance Results"
Cohesion: 0.21
Nodes (8): v3 Step 8 Acceptance Results, Automated Gate (restore/test/publish), Input-Language Hotkey Checks (group 3), v3 Release Blockers (unverified layout/DPI/multi-monitor checks), v3 Step 8 acceptance criteria, v3 Step 8 plan, Unintended input-layout switch is a release blocker, Separate manual desktop results document

### Community 128 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 131 - "v3 Step 7: Settings window (task)"
Cohesion: 0.26
Nodes (8): v3 Step 7 acceptance criteria, SettingsEditorTests / SettingsViewModelTests / SoundSamplerTests, v3 Step 7 plan, Ctrl+Alt+1..8 default slot shortcut proposal, drawEM Settings prototype (HTML), Prototype Drawing / Actions tabs with Save, Cancel, Restore defaults, Settings Prototype UI (Drawing: Stroke, Shortcuts), v3 Step 7: Settings window (task)

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
Cohesion: 0.33
Nodes (4): UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 137 - "EffectMonitorGuard"
Cohesion: 0.21
Nodes (4): EffectPlacement, Cursor, Monitor, EffectMonitorGuard

### Community 138 - "Shortcut"
Cohesion: 0.13
Nodes (10): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot, Shortcut (+2 more)

### Community 139 - ".Load"
Cohesion: 0.14
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 140 - "release.yml Release Workflow"
Cohesion: 0.19
Nodes (7): release.yml Release Workflow, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation, R4-01: implementation plan

### Community 141 - "RecordingReporter"
Cohesion: 0.32
Nodes (3): RecordingReporter, Lines, UpdateFeedTests

### Community 142 - "NativeMethods"
Cohesion: 0.25
Nodes (4): MONITORINFO, NativeMethods, POINT, RECT

### Community 143 - "Step 1 Implementation Review"
Cohesion: 0.27
Nodes (11): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Step 1 Implementation Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundAssignments.Slots, SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog (+3 more)

### Community 145 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 146 - "v3 Step 2 acceptance criteria"
Cohesion: 0.27
Nodes (9): v3 Step 2 acceptance criteria, SHA-256 content-addressed copy names, v3 Step 2 plan, Draft imports tracked separately from saved references, Startup Orphan Media Cleanup, v3 Step 2: Managed sound library (task), Managed sound library (%LOCALAPPDATA%\drawEM\sounds), Startup orphan cleanup (+1 more)

### Community 147 - "FakePlayback"
Cohesion: 0.20
Nodes (7): EffectPlaybackException, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 148 - "step-01-video-decoder-and-formats/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-01: acceptance criteria, v5 / S5-01: implementation plan, Out of scope, Required outcome, Task, v5 / S5-01: prove MP4 playback and choose supported profiles

### Community 150 - "step-02-video-action-and-library/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-02: acceptance criteria, v5 / S5-02: implementation plan, Out of scope, Required behavior, Task, v5 / S5-02: add video actions and managed imports

### Community 151 - "step-03-fullscreen-video-playback/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-03: acceptance criteria, v5 / S5-03: implementation plan, Out of scope, Required behavior, Task, v5 / S5-03: play video fullscreen on the active monitor

### Community 152 - "RecordingEffectSurface"
Cohesion: 0.08
Nodes (21): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+13 more)

### Community 153 - "EffectSurfaceWindow"
Cohesion: 0.19
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 154 - "step-04-video-controls-and-lifecycle/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-04: acceptance criteria, v5 / S5-04: implementation plan, Out of scope, Required behavior, Task, v5 / S5-04: connect controls and lifecycle

### Community 155 - "step-05-video-publish/task.md"
Cohesion: 0.22
Nodes (7): Evidence, v5 / S5-05: acceptance criteria, v5 / S5-05: implementation plan, Out of scope, Required outcome, Task, v5 / S5-05: verify the published application

### Community 156 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 157 - "GlassDark"
Cohesion: 0.25
Nodes (5): ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 161 - "v1 roadmap: drawing"
Cohesion: 0.48
Nodes (6): v1 roadmap: drawing, GlobalMouseInputAdapter seam, GlobalShortcutAdapter seam, OverlayWindowAdapter seam, v1 proposed test seams, TrayApplication seam

### Community 162 - "README.md"
Cohesion: 0.29
Nodes (5): drawEM v5: fullscreen video, Package structure, Steps, Terms, drawEM v5: steps

### Community 163 - "SettingsSaveResult"
Cohesion: 0.38
Nodes (4): Invalid, NotSaved, SettingsSaveResult, Saved

### Community 164 - "Product contract"
Cohesion: 0.33
Nodes (6): Action slots and media, Audio and preview, Playback and stop behavior, Playback surface and input, Product contract, Terms

### Community 165 - ".RejectedChord_ShowsWhyAndKeepsCapturing"
Cohesion: 0.40
Nodes (4): Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers

### Community 167 - "RecordingNotifications"
Cohesion: 0.33
Nodes (3): RecordingNotifications, NotSavedReasons, UnconfirmedStops

### Community 168 - "DecreaseWidthButton"
Cohesion: 0.40
Nodes (5): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, RepeatButton

### Community 169 - "ROADMAP-v5.md"
Cohesion: 0.40
Nodes (4): Completion, Delivery order, drawEM v5 roadmap: fullscreen MP4 video, Outcome

## Knowledge Gaps
- **560 isolated node(s):** `python`, `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback` (+555 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 844 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **23 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `release-publishing/STEPS.md`, `Lottie built-in effects via SkiaSharp.Skottie`, `drawEM v3 Roadmap: Drawing and Action Settings`, `drawEM v4 Parallel Work Packages`, `Lottie Simple License`, `Settings Storage README`, `drawEM README`, `S4-12 Acceptance: Publish and Verify on Windows`, `drawEM architecture`, `v3 Step 8: Publish and verify on Windows (task)`, `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `S4-03 Acceptance Criteria`, `Stage 1 sound implementation plan`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.160) - this node is a cross-community bridge._
- **Are the 89 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 89 INFERRED edges - model-reasoned connections that need verification._
- **What connects `python`, `CompletedStrokes`, `ActiveInstance` to the rest of the system?**
  _560 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.14343434343434344 - nodes in this community are weakly interconnected._
- **Why does `drawEM v4: out of scope` connect `release-publishing/STEPS.md` to `StrokeRenderElement`, `FEAT-009-effect-loop-and-limit/task.md`, `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `drawEM v4 Parallel Work Packages`, `R4 Release Publishing Roadmap`, `FEAT-011-glass-ui/plan.md`, `drawEM rules (AGENTS.md)`?**
  _High betweenness centrality (0.101) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.05657492354740061 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `Action`, `EffectId`, `LottiePlayback`?**
  _High betweenness centrality (0.097) - this node is a cross-community bridge._