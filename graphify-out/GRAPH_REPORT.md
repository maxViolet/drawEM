# Graph Report - worktree-r4-03  (2026-10-16)

## Corpus Check
- 342 files · ~154,951 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3025 nodes · 8879 edges · 118 communities (100 shown, 18 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1516 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `92cba0fb`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- SettingsEditorTests
- StrokeRenderElement
- R4 release publishing roadmap
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- Window
- drawEM v3 Roadmap: Drawing and Action Settings
- Harness
- Action
- App.xaml.cs
- SettingsViewModel
- DrawEM.App.Domain.Drawing
- ShortcutModifiers
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- ManualTimeProvider
- VelopackUpdateService
- SlotViewModel
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- ScreenPoint
- Fixture
- List
- DrawingSessionController
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- IDisposable
- SettingsWindow
- Fixture
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- BuiltInEffectAnimations
- S4-12 Acceptance: Publish and Verify on Windows
- EffectInstance
- GlobalShortcutAdapter
- StartEffectCommand
- DrawEM.App.Infrastructure
- Harness
- Stage 1 sound implementation plan
- ShortcutKey
- drawEM architecture
- S4-11 Acceptance criteria
- Shortcut
- LottiePlayback
- SaveHarness
- EffectId
- .Save
- SoundReference
- Win32SessionInterruptionSource
- AppFailureLog
- ShortcutBindings
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- FakePlayer
- JsonSettingsStore
- drawEM v4: out of scope
- R4-02 implementation plan
- VelopackPackageIdentityTests
- STEPS.md
- v4 Simple Implementation Steps
- FakeSoundChannel
- BlockingPlayerFactory
- system_diagnostics
- Settings Storage README
- .OnStartup
- .ReleaseStartedResources
- .Deserialize
- drawEM rules (AGENTS.md)
- CompositeDisposable
- EffectMonitorGuard
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- FakeKeyboardHookSource
- NotifyIconTrayHost
- DrawEM.App.Application.Sound
- DrawEM.App/Program.cs
- Sound infrastructure README
- Stage 1 Step 2 Review
- FIX-006: Single Screen Edge
- S4-01 acceptance: prove the effect window
- drawEM v4 Parallel Work Packages
- v1 roadmap: drawing
- drawEM v4 out-of-scope task list
- BuiltInEffectAnimations.cs
- S4-06 acceptance: launch effects through shortcuts
- S4-08 acceptance: choose effects in Settings
- TimeSpan
- RecordingNotifications
- v4 / FEAT-009: implementation plan
- ADR 0001: Lottie built-in effects
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- Steps
- ShortcutCaptureRejection
- EffectSurfaceWindow
- SettingsErrorCode
- S4-09 Acceptance criteria
- RenderLagHarness/Program.cs
- Show visual effects (Loop, Max length)
- F03 harness results
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
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
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-009: implementation plan` --references--> `LottiePlayback`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Presentation/Effects/LottiePlayback.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs

## Import Cycles
- None detected.

## Communities (118 total, 18 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, Portable win-x64 ZIP, release.yml Release Workflow, R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SmartScreen Reputation Warnings, Consent-based update UX (startup + 24h check) (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.14
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+46 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (59): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+51 more)

### Community 7 - "Harness"
Cohesion: 0.07
Nodes (14): Cancelled, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log (+6 more)

### Community 8 - "Action"
Cohesion: 0.09
Nodes (12): PlaySoundCommand, SampleFailureRouter, SoundSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe (+4 more)

### Community 9 - "App.xaml.cs"
Cohesion: 0.08
Nodes (4): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing

### Community 10 - "SettingsViewModel"
Cohesion: 0.05
Nodes (28): IShortcutCapture, BindableBase, ISoundFilePicker, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (9): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+1 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.11
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.10
Nodes (6): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, EffectLifecycleTests, Events, Interruptions

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.07
Nodes (21): EffectSample, Effect, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples (+13 more)

### Community 15 - "Fixture"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.12
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 17 - "VelopackUpdateService"
Cohesion: 0.11
Nodes (16): GitHub release Environment, SIGNPATH_API_TOKEN, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, Write-Sha256Sums.ps1 / SHA256SUMS.txt (+8 more)

### Community 18 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.06
Nodes (16): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Path (+8 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.09
Nodes (13): BackgroundLogWriter, EffectFailure, EffectFailureLog, DefaultPath, SettingsSaveNotifications, LoggingSoundFailureReporter, SoundFailure, SoundFailureLog (+5 more)

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.13
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.21
Nodes (4): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory

### Community 24 - "ScreenPoint"
Cohesion: 0.12
Nodes (13): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, SettableCursor, Available, Position (+5 more)

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "List"
Cohesion: 0.24
Nodes (5): FakeDisposable, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "DrawingSessionController"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.14
Nodes (14): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, IMouseHookSource, CountingMonitors, Queries (+6 more)

### Community 29 - "Fixture"
Cohesion: 0.09
Nodes (13): EffectPlacement, Cursor, Monitor, EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays (+5 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (20): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+12 more)

### Community 31 - "IDisposable"
Cohesion: 0.14
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 32 - "SettingsWindow"
Cohesion: 0.12
Nodes (7): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, SettingsWindow, ViewModel, RepeatButton

### Community 33 - "Fixture"
Cohesion: 0.09
Nodes (14): IDisplayChangeSource, EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log (+6 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (12): ActiveSettings, Current, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig (+4 more)

### Community 36 - "Harness"
Cohesion: 0.12
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.09
Nodes (14): Evidence, IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots (+6 more)

### Community 38 - "Rig"
Cohesion: 0.13
Nodes (16): Release, Rig, Keyboard, New, Old, Players, Previous, Saved (+8 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.15
Nodes (5): DrawEM.Tests.Domain.Effects, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.19
Nodes (14): Built-in Lottie Effect, Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency, S4-12 Task: publish and verify on Windows (+6 more)

### Community 41 - "EffectInstance"
Cohesion: 0.06
Nodes (31): EffectInstance, EffectInstanceId, EffectPlaybackException, IEffectPlayback, IEffectSurface, LottieEffectSurface, FakePlayback, CompletedHandler (+23 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (7): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, IEffectStartQueue, FailingCursorPositionSource, RecoveringCursorPositionSource

### Community 43 - "StartEffectCommand"
Cohesion: 0.15
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.12
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.12
Nodes (13): Win32DisplayChangeSource, Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions (+5 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.15
Nodes (29): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+21 more)

### Community 47 - "ShortcutKey"
Cohesion: 0.14
Nodes (8): ShortcutKey, Kind, Value, ShortcutKeyKind, Digit, Function, Letter, ShortcutBindingsTests

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (37): SkiaSharp allowed only in Presentation, Action slot, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+29 more)

### Community 49 - "S4-11 Acceptance criteria"
Cohesion: 0.15
Nodes (18): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+10 more)

### Community 50 - "Shortcut"
Cohesion: 0.15
Nodes (12): Captured, SlotAction, SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw (+4 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.14
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 53 - "EffectId"
Cohesion: 0.11
Nodes (12): EffectId, Value, EffectAction, Effect, SoundAction, Sound, SettingsError, SettingsValidation (+4 more)

### Community 54 - ".Save"
Cohesion: 0.18
Nodes (6): Invalid, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveResult, Saved

### Community 55 - "SoundReference"
Cohesion: 0.13
Nodes (8): IActiveShortcuts, SettingsSaveOperation, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, MediaSoundPlayerFactoryTests

### Community 56 - "Win32SessionInterruptionSource"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 57 - "AppFailureLog"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 58 - "ShortcutBindings"
Cohesion: 0.17
Nodes (11): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, NoFailures (+3 more)

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.16
Nodes (10): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+2 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.18
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.06
Nodes (19): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource, INPUT (+11 more)

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 65 - "FakePlayer"
Cohesion: 0.18
Nodes (7): FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop, RecordingSoundFailures

### Community 66 - "JsonSettingsStore"
Cohesion: 0.08
Nodes (19): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+11 more)

### Community 67 - "drawEM v4: out of scope"
Cohesion: 0.23
Nodes (10): drawEM v4: out of scope, %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, Program.Main custom WPF entry, VelopackHookExitTests, VelopackPackageIdentityTests, Velopack, R4-07 Tray Updates Acceptance (+2 more)

### Community 68 - "R4-02 implementation plan"
Cohesion: 0.14
Nodes (16): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, Release publishing package (R4), SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), One-Byte Mutation Check, R4-02 acceptance criteria (+8 more)

### Community 70 - "STEPS.md"
Cohesion: 0.14
Nodes (19): Evidence, Implementation, R4-01: acceptance criteria, Validation, R4-01: implementation plan, Task, v4 / R4-01: create a versioned draft release, R4-04 SignPath Signing Acceptance (+11 more)

### Community 71 - "v4 Simple Implementation Steps"
Cohesion: 0.11
Nodes (27): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations (+19 more)

### Community 72 - "FakeSoundChannel"
Cohesion: 0.11
Nodes (6): ISaveSoundChannel, ISoundStartHold, StartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 74 - "system_diagnostics"
Cohesion: 0.15
Nodes (4): DrawEM.Tests.Release, DrawEM.Tests.Architecture, RetiredSoundAssignmentsTests, VelopackHookExitTests

### Community 75 - "Settings Storage README"
Cohesion: 0.19
Nodes (16): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+8 more)

### Community 76 - ".OnStartup"
Cohesion: 0.15
Nodes (7): Application, App, ApplicationExitPolicy, StartupFailure, WpfApplicationLifetime, StartupFailureTests, TimedOutSound

### Community 77 - ".ReleaseStartedResources"
Cohesion: 0.11
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.19
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 80 - "CompositeDisposable"
Cohesion: 0.33
Nodes (3): CompositeDisposable, CompositeDisposableTests, FakeDisposable

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (24): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog (+16 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "FakeKeyboardHookSource"
Cohesion: 0.10
Nodes (13): IKeyboardHookSource, KeyDirection, Down, Up, INeutralKeyEmitter, KeyboardHookEvents, FakeCursorPositionSource, FakeEffectStartQueue (+5 more)

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.17
Nodes (4): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri, NotifyIconTrayHostTests

### Community 86 - "DrawEM.App.Application.Sound"
Cohesion: 0.10
Nodes (15): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1 (+7 more)

### Community 88 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 89 - "Stage 1 Step 2 Review"
Cohesion: 0.10
Nodes (31): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+23 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 91 - "S4-01 acceptance: prove the effect window"
Cohesion: 0.53
Nodes (5): S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window

### Community 92 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.24
Nodes (14): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing, drawEM v4 Parallel Work Packages (+6 more)

### Community 93 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 97 - "S4-06 acceptance: launch effects through shortcuts"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.35
Nodes (8): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings

### Community 101 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 102 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.27
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 104 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.16
Nodes (16): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, Effect Playback Contract, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X) (+8 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "ShortcutCaptureRejection"
Cohesion: 0.50
Nodes (4): Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 112 - "S4-09 Acceptance criteria"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 122 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **512 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+507 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 714 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **18 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `drawEM v3 Roadmap: Drawing and Action Settings` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `drawEM architecture`, `Stage 1 Step 2 Review`, `FEAT-005: Assisted E2E Runner`, `Stage 1 sound implementation plan`?**
  _High betweenness centrality (0.109) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _512 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.1414141414141414 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.104) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07557964468533575 - nodes in this community are weakly interconnected._
- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-08 acceptance: choose effects in Settings`?**
  _High betweenness centrality (0.097) - this node is a cross-community bridge._