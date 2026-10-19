# Graph Report - worktree-instal-bug  (2026-10-19)

## Corpus Check
- 381 files · ~169,115 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3627 nodes · 10473 edges · 153 communities (129 shown, 24 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1742 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `f9c9c021`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Window
- v3 Step 8: Publish and verify on Windows (task)
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
- New-VelopackPackage.ps1
- FakeKeyboardHookSource
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- .OnStartup
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
- drawEM v4 Parallel Work Packages
- GlobalShortcutAdapter
- ShortcutBindingsTests
- DrawEM.App.Infrastructure
- Harness
- SoundId
- .Digit
- drawEM architecture
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- ShortcutBindings
- LottiePlayback
- SaveHarness
- EffectId
- .Save
- Step 2 Implementation Review
- .Deserialize
- AppFailureLog
- JsonSettingsStore
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- ScreenPoint
- .TryGetBounds
- Fixture
- R4-02 Plan: Checksums and Provenance
- VelopackPackageIdentityTests
- STEPS.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- drawEM v3 Roadmap: Drawing and Action Settings
- BlockingPlayerFactory
- LayerDependencyTests.cs
- Settings Storage README
- R4 Release Publishing Roadmap
- SingleInstanceGuardTests
- ManagedSoundLibrary
- drawEM rules (AGENTS.md)
- IDisposable
- Action
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- BackgroundLogWriter
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
- RecordingSurface
- FEAT-011-glass-ui/plan.md
- BuiltInEffectAnimations.cs
- SettingsErrorCode
- VelopackUpdateService
- Rect
- FakeUpdateService
- EffectLifecycleTests
- UpdateController
- S4-03 Acceptance Criteria
- SoundFailure
- Show visual effects (Loop, Max length)
- .CreatePieces
- v3 Step 5 acceptance criteria
- KeyDirection
- RenderLagHarness tool
- NativeMethods
- InstanceActivationTests
- FakePicker
- CLAUDE.md graphify rules
- TrayMenuCommand
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- FakePlayer
- SoundReference
- Sound presentation placeholder README
- S4-12 Acceptance: Publish and Verify on Windows
- EffectFailure
- UpdateFailure
- EffectMonitorGuard
- SettingsCommandKind
- SettingsLoadResult
- release.yml Release Workflow
- RecordingReporter
- NativeMethods
- .Dispose
- App
- .Default_FollowsTheWindowsAppTheme
- SamplingEditor
- .Key
- FakePlayback
- EffectSurfaceWindow
- graphify knowledge-graph usage rules
- drawEM v4 out-of-scope task list
- Sound Presentation Placeholder

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
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-012: implementation plan` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs

## Import Cycles
- None detected.

## Communities (153 total, 24 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.06
Nodes (33): Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, Out of scope, Per stroke, Previous behavior, Required behavior, Settings file (+25 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SmartScreen Reputation Warnings, Consent-based update UX (startup + 24h check), Velopack installer and update feed, Velopack Per-User Installer (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.16
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 5 - "Window"
Cohesion: 0.03
Nodes (69): BezierSmoothing, CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit (+61 more)

### Community 6 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.05
Nodes (61): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, Settings Schema/Version Policy, v3 Settings Defaults, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Right Alt reserved for AltGr (+53 more)

### Community 7 - "Harness"
Cohesion: 0.07
Nodes (15): ShortcutCaptureRejection, RightAlt, TooFewModifiers, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate (+7 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.20
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.08
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 10 - "SettingsViewModel"
Cohesion: 0.03
Nodes (60): IShortcutCapture, BindableBase, EffectOption, All, SettingsViewModel, BezierSmoothing, CanDecreaseWidth, CanIncreaseWidth (+52 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (11): DrawEM.Tests.Domain, DrawEM.App.Application.Drawing, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing (+3 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.12
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

### Community 17 - "New-VelopackPackage.ps1"
Cohesion: 0.09
Nodes (18): %LOCALAPPDATA%\drawEM settings directory, CI Velopack packaging smoke, JsonSettingsStore.DefaultDirectory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, VelopackHookExitTests, VelopackPackageIdentityTests (+10 more)

### Community 18 - "FakeKeyboardHookSource"
Cohesion: 0.16
Nodes (7): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource

### Community 19 - "ISoundPlayer"
Cohesion: 0.10
Nodes (10): ISoundPlayer, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment, PlayThreadId (+2 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.28
Nodes (3): SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.12
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.22
Nodes (4): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory

### Community 24 - ".OnStartup"
Cohesion: 0.12
Nodes (6): EffectStopRoutes, IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, WpfApplicationLifetime

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - ".RunOnDispatcherThread"
Cohesion: 0.22
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "DrawingSessionController"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.17
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+4 more)

### Community 29 - "Fixture"
Cohesion: 0.09
Nodes (13): EffectPlacement, Cursor, Monitor, EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays (+5 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.09
Nodes (22): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+14 more)

### Community 31 - "List"
Cohesion: 0.16
Nodes (7): FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable, TimedOutSound, TrayApplicationTests

### Community 32 - "DrawEM.App.Application.Sound"
Cohesion: 0.12
Nodes (8): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Release, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, VelopackHookExitTests

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.08
Nodes (19): ISoundFailureReporter, ISoundPlayerFactory, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted (+11 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 36 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.08
Nodes (15): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Smoothing (+7 more)

### Community 38 - "Rig"
Cohesion: 0.13
Nodes (15): Release, Rig, Keyboard, New, Old, Players, Previous, Saved (+7 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.11
Nodes (8): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "Lottie Simple License"
Cohesion: 0.15
Nodes (25): Built-in Lottie Effect, EffectCatalog, Design Barn Inc., Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), big-bada-boom.json (Lottie effect), Built-in Effect Sources (+17 more)

### Community 41 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.20
Nodes (15): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing, drawEM v4 Parallel Work Packages (+7 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (10): Cancelled, Captured, Rejected, ShortcutCaptureResult, GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, INeutralKeyEmitter (+2 more)

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.12
Nodes (13): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+5 more)

### Community 46 - "SoundId"
Cohesion: 0.14
Nodes (8): SoundId, Value, FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop

### Community 47 - ".Digit"
Cohesion: 0.36
Nodes (3): ShortcutKey, Kind, Value

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (41): Action slot, ActiveSettings, ApplicationExitPolicy, drawEM architecture, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+33 more)

### Community 49 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.14
Nodes (23): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), S4-11 Acceptance criteria, EffectFailureLog, EffectMonitorGuard.StopEffects() (+15 more)

### Community 50 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.14
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.05
Nodes (33): ISaveSoundChannel, ISoundStartHold, IDisplayChangeSource, FakeDisplayChangeSource, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore (+25 more)

### Community 53 - "EffectId"
Cohesion: 0.10
Nodes (18): EffectId, Value, ActionSlot, Action, IsEmpty, Number, EffectAction, Effect (+10 more)

### Community 54 - ".Save"
Cohesion: 0.15
Nodes (10): ISettingsStore, IActiveShortcuts, Invalid, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+2 more)

### Community 55 - "Step 2 Implementation Review"
Cohesion: 0.26
Nodes (13): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), SoundFailure record, SoundFailureLog, Step 2 Implementation Review, ISoundFailureReporter, ISoundPlayer (playback port), ISoundPlayerFactory, LoggingSoundFailureReporter (+5 more)

### Community 57 - "AppFailureLog"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 58 - "JsonSettingsStore"
Cohesion: 0.15
Nodes (8): ISettingsFailureReporter, SettingsStartup, StartupSettings, JsonSettingsStore, DefaultDirectory, SettingsPath, NoSettingsFailures, NoReporter

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.10
Nodes (17): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+9 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.27
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.20
Nodes (18): S4-07 acceptance: stop effects when switching monitors, S4-07 Acceptance Criteria, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource (+10 more)

### Community 65 - "ScreenPoint"
Cohesion: 0.11
Nodes (13): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, SettableCursor, Available, Position (+5 more)

### Community 67 - "Fixture"
Cohesion: 0.14
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 Plan: Checksums and Provenance"
Cohesion: 0.13
Nodes (17): SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), v4 Release Publishing Roadmap, Task, v4 / R4-01: create a versioned draft release, R4-02 Acceptance: Checksums and Provenance, One-Byte Mutation Check, R4-02 acceptance criteria, R4-02 Plan: Checksums and Provenance (+9 more)

### Community 70 - "STEPS.md"
Cohesion: 0.12
Nodes (26): drawEM v4: out of scope, R4-04 SignPath Signing Acceptance, R4-04 SignPath Signing Plan, ADR-0002 Release Signing License, R4-04 SignPath Signing Task, ADR-0002: Release signing and license, SignPath Foundation, SignPath Foundation Code Signing (+18 more)

### Community 71 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.09
Nodes (40): ADR 0001 Lottie Built-in Effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectSurfaceWindow (nonactivating click-through), EffectAction (+32 more)

### Community 72 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.11
Nodes (19): Step 1 Implementation Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundAssignments.Slots, SoundConfiguration (Resolve / PathOf), SoundId, SoundSlot, Eight Sound Slots Ctrl+Alt+1..8 (+11 more)

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, SettingsJson, S4-05 Implementation Plan, S4-05 task: save and load effect assignments (+7 more)

### Community 76 - "R4 Release Publishing Roadmap"
Cohesion: 0.24
Nodes (3): Portable win-x64 ZIP, R4 Release Publishing Roadmap, WinGet Portable ZIP Listing

### Community 78 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.18
Nodes (8): Dependabot config (nuget, github-actions, FIX prefix), Dependabot config, PR lint workflow, PR lint workflow (branch name and title check), Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM agent rules, Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.31
Nodes (3): CompositeDisposable, CompositeDisposableTests, FakeDisposable

### Community 81 - "Action"
Cohesion: 0.10
Nodes (11): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe (+3 more)

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.07
Nodes (42): v1 acceptance pass (step 7), drawEM v1 roadmap: drawing, Step 7 acceptance results (conditional pass), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template (+34 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.12
Nodes (9): v4 / FEAT-011: implementation plan, ITrayMenu, NotifyIconTrayHost, IconUri, TrayMenuPlacement, FakeTrayMenu, Shown, Updates (+1 more)

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.14
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 88 - "Sound Infrastructure README"
Cohesion: 0.24
Nodes (19): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, Sound Application README, SoundChannelController (+11 more)

### Community 89 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (36): drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, v2 roadmap: screen actions, Global sound channel, Ctrl+Alt+1..8 Sound Shortcut Slots, Ten-second playback cap, Confirmed test boundaries, Sound Command Business Test Scenarios (+28 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.21
Nodes (13): GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, First Key-Down Trigger (no auto-repeat), FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved) (+5 more)

### Community 92 - "UiThemeState"
Cohesion: 0.24
Nodes (3): UiThemeState, Current, TrayMenuWindowTests

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
Cohesion: 0.18
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (26): S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement) (+18 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.19
Nodes (16): ManagedSoundLibrary, S4-08 Acceptance Criteria, CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot) (+8 more)

### Community 99 - "ManualTimeProvider"
Cohesion: 0.16
Nodes (5): ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer, DueAt

### Community 100 - ".Attach"
Cohesion: 0.13
Nodes (10): GlassBackdrop, Supported, ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles, UiTheme (+2 more)

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
Cohesion: 0.17
Nodes (3): Win32CursorPositionSource, NativeMethods, POINT

### Community 107 - "RecordingSurface"
Cohesion: 0.22
Nodes (5): Playback, RecordingSurface, Disposed, Last, Shown

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

### Community 113 - "FakeUpdateService"
Cohesion: 0.13
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 114 - "EffectLifecycleTests"
Cohesion: 0.27
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.11
Nodes (26): S4-03 acceptance: play built-in Lottie effects, S4-03 Acceptance Criteria, BuiltInEffectAnimations, Confetti Effect (Monitor), EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Flashing Check (max three flashes per second), Focus Ring Effect (Cursor) (+18 more)

### Community 117 - "SoundFailure"
Cohesion: 0.33
Nodes (4): SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 119 - ".CreatePieces"
Cohesion: 0.38
Nodes (5): Line, Quadratic, StrokePathPiece, StrokeSmoothingGeometry, StrokeSmoothingGeometryTests

### Community 120 - "v3 Step 5 acceptance criteria"
Cohesion: 0.33
Nodes (9): v3 Step 5 acceptance criteria, Per-Monitor-V2 single overlay window transform, SettingsFailureDialog, v3 Step 5 plan, DrawingSessionController, StrokeRenderElement, v3 Step 5: Configurable drawing style (task), Stroke width in physical pixels (+1 more)

### Community 121 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 122 - "RenderLagHarness tool"
Cohesion: 0.36
Nodes (10): F03 harness raw output (after fix), F03 harness raw output (after fix), F03 harness raw output (before fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, F03 synthetic harness results (before/after), RenderLagHarness tool (+2 more)

### Community 123 - "NativeMethods"
Cohesion: 0.19
Nodes (4): MSLLHOOKSTRUCT, NativeMethods, RECT, Win32MouseHookSource

### Community 125 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "TrayMenuCommand"
Cohesion: 0.33
Nodes (5): TrayMenuCommand, Exit, Settings, StopEffects, Update

### Community 128 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 131 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

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

### Community 138 - "SettingsCommandKind"
Cohesion: 0.50
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

### Community 139 - "SettingsLoadResult"
Cohesion: 0.20
Nodes (9): Loaded, Missing, SettingsLoadResult, Unreadable, FakeReporter, Reports, FakeStore, Saves (+1 more)

### Community 140 - "release.yml Release Workflow"
Cohesion: 0.19
Nodes (7): release.yml Release Workflow, Evidence, Implementation, R4-01: acceptance criteria, ReleaseScriptsTests, Validation, R4-01: implementation plan

### Community 141 - "RecordingReporter"
Cohesion: 0.32
Nodes (3): RecordingReporter, Lines, UpdateFeedTests

### Community 142 - "NativeMethods"
Cohesion: 0.25
Nodes (4): MONITORINFO, NativeMethods, POINT, RECT

### Community 144 - "App"
Cohesion: 0.09
Nodes (7): Application, App, CleanupSteps, InstanceActivation, DefaultName, SingleInstanceGuard, DefaultName

### Community 145 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 146 - "SamplingEditor"
Cohesion: 0.50
Nodes (4): SamplingEditor, Failures, RunningStates, Sampler

### Community 152 - "FakePlayback"
Cohesion: 0.09
Nodes (19): EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+11 more)

### Community 153 - "EffectSurfaceWindow"
Cohesion: 0.12
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

## Knowledge Gaps
- **559 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+554 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 805 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects` to `S4-07 acceptance: stop effects when switching monitors`, `S4-08 acceptance: choose effects in Settings`, `FEAT-009-effect-loop-and-limit/task.md`, `STEPS.md`, `Lottie built-in effects via SkiaSharp.Skottie`, `drawEM v3 Roadmap: Drawing and Action Settings`, `drawEM v4 Parallel Work Packages`, `v3 Step 8: Publish and verify on Windows (task)`, `Lottie Simple License`, `Settings Storage README`, `drawEM README`, `S4-12 Acceptance: Publish and Verify on Windows`, `drawEM architecture`, `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `S4-03 Acceptance Criteria`, `Stage 1 sound implementation plan`, `S4-09 Preview Draft Effects (acceptance)`?**
  _High betweenness centrality (0.194) - this node is a cross-community bridge._
- **Are the 89 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 89 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _559 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.1391304347826087 - nodes in this community are weakly interconnected._
- **Why does `v4 / FEAT-009: implementation plan` connect `FEAT-009-effect-loop-and-limit/task.md` to `Action`, `LottiePlayback`, `EffectId`?**
  _High betweenness centrality (0.101) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.05572545022086307 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `drawEM v4 Roadmap: Built-in Monitor and Cursor Effects`, `drawEM v3 Roadmap: Drawing and Action Settings`, `R4 Release Publishing Roadmap`, `FEAT-005: Assisted E2E Runner`, `v3 Step 5 acceptance criteria`?**
  _High betweenness centrality (0.088) - this node is a cross-community bridge._