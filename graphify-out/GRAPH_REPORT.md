# Graph Report - worktree-clear-field-6052  (2026-10-19)

## Corpus Check
- 377 files · ~167,149 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3316 nodes · 9574 edges · 142 communities (123 shown, 19 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1612 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `9bc6a8e2`
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
- PlaySoundCommand
- App.xaml.cs
- SettingsViewModel
- DrawEM.App.Domain.Drawing
- ShortcutModifiers
- Theory
- .Window_BindsEveryFieldWithoutBindingErrors
- Fixture
- ManualTimeProvider
- VelopackUpdateService
- Action
- ISoundPlayer
- LoggingSoundFailureReporter
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- MonitorBounds
- Fixture
- List
- DrawingSessionController
- DrawingModeInputGate
- Fixture
- SettingsSnapshot
- TrayApplication
- SettingsWindow
- Fixture
- SoundChannelController
- RuntimeMediaTests
- Harness
- SettingsEditor
- Rig
- BuiltInEffectAnimations
- S4-12 Acceptance: Publish and Verify on Windows
- IEffectPlayback
- GlobalShortcutAdapter
- StartEffectCommand
- DrawEM.App.Infrastructure
- Harness
- Stage 1 sound implementation plan
- .Digit
- drawEM architecture
- S4-11 Acceptance criteria
- Shortcut
- LottiePlayback
- SaveHarness
- EffectId
- .Save
- SoundReference
- .Load
- AppFailureLog
- RuntimeSettingsTests
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- DllImport
- S4-07 acceptance: stop effects when switching monitors
- ShortcutBindingsTests
- EffectSurfaceWindow
- Fixture
- R4-02 implementation plan
- VelopackPackageIdentityTests
- STEPS.md
- v4 Simple Implementation Steps
- NativeMethods
- BlockingPlayerFactory
- RenderLagHarness/Program.cs
- Settings Storage README
- .OnStartup
- SingleInstanceGuard
- DrawingStyle
- drawEM rules (AGENTS.md)
- IDisposable
- ScreenPoint
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- IntPtr
- NotifyIconTrayHost
- DrawEM.App.Application.Sound
- DrawEM.App.Application.Updates
- Sound infrastructure README
- Stage 1 Step 2 Review
- FIX-006: Single Screen Edge
- S4-01 acceptance: prove the effect window
- drawEM v4 Parallel Work Packages
- v1 roadmap: drawing
- drawEM v4 out-of-scope task list
- ResourceDictionary
- TrayMenuWindow
- S4-06 acceptance: launch effects through shortcuts
- S4-08 acceptance: choose effects in Settings
- TimeSpan
- .Attach
- v4 / FEAT-009: implementation plan
- ADR 0001: Lottie built-in effects
- UiThemeState
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- Steps
- ManagedSoundLibrary
- drawEM v4: out of scope
- JsonSettingsStore
- FEAT-011-glass-ui/task.md
- SettingsErrorCode
- VelopackUpdateService
- S4-09 Acceptance criteria
- FakeUpdateService
- IUpdateService
- UpdateController
- WidthBox
- EffectLifecycleTests
- Show visual effects (Loop, Max length)
- FakePlayback
- .TryFind_ReturnsTheEntryForAKnownId
- KeyDirection
- F03 harness results
- .Default_FollowsTheWindowsAppTheme
- .Create
- BuiltInEffectAnimations.cs
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- SoundSlot
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- Orange Brush Stroke Glyph
- Sound domain README
- Sound presentation placeholder README
- Required behavior
- .Key
- .TryGetBounds
- .Place
- SettingsSnapshot.cs
- DecreaseWidthButton
- DrawEM.App/Program.cs

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
- `Previous behavior` --references--> `StrokeRenderElement`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/task.md → src/DrawEM.App/Presentation/Drawing/StrokeRenderElement.cs
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs

## Import Cycles
- None detected.

## Communities (142 total, 19 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.09
Nodes (14): DrawingState, Stroke, IOverlayView, Window, OverlayWindow, PhysicalToLocalTransform, StrokeRenderElement, VisualChildrenCount (+6 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, Portable win-x64 ZIP, release.yml Release Workflow, R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SmartScreen Reputation Warnings, Consent-based update UX (startup + 24h check) (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (16): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+8 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (56): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+48 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (59): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+51 more)

### Community 7 - "Harness"
Cohesion: 0.07
Nodes (15): ShortcutCaptureRejection, RightAlt, TooFewModifiers, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate (+7 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "App.xaml.cs"
Cohesion: 0.07
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 10 - "SettingsViewModel"
Cohesion: 0.03
Nodes (61): IShortcutCapture, HexColor, BindableBase, EffectOption, All, ISoundFilePicker, SettingsViewModel, CanDecreaseWidth (+53 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.11
Nodes (10): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects (+2 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 13 - "Theory"
Cohesion: 0.11
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.07
Nodes (20): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakePicker (+12 more)

### Community 15 - "Fixture"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.10
Nodes (16): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+8 more)

### Community 17 - "VelopackUpdateService"
Cohesion: 0.09
Nodes (20): GitHub release Environment, SIGNPATH_API_TOKEN, %LOCALAPPDATA%\drawEM settings directory, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip, JsonSettingsStore.DefaultDirectory, releases.win.json / full nupkg update feed (+12 more)

### Community 18 - "Action"
Cohesion: 0.08
Nodes (15): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+7 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.09
Nodes (11): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment (+3 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.06
Nodes (19): BackgroundLogWriter, EffectFailure, EffectFailureLog, DefaultPath, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, SoundFailure (+11 more)

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.12
Nodes (10): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Application.Settings (+2 more)

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "MonitorBounds"
Cohesion: 0.13
Nodes (8): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests, EffectRenderLayoutTests

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "List"
Cohesion: 0.23
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "DrawingSessionController"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.25
Nodes (9): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource (+1 more)

### Community 29 - "Fixture"
Cohesion: 0.12
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (22): ISoundLibrary, SoundCleanupFailure, SettingsStartup, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects (+14 more)

### Community 31 - "TrayApplication"
Cohesion: 0.12
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.13
Nodes (13): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+5 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.15
Nodes (13): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+5 more)

### Community 36 - "Harness"
Cohesion: 0.12
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.10
Nodes (8): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 39 - "BuiltInEffectAnimations"
Cohesion: 0.21
Nodes (4): BuiltInEffectAnimations, LottieEffectSurface, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.19
Nodes (14): Built-in Lottie Effect, Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency, S4-12 Task: publish and verify on Windows (+6 more)

### Community 41 - "IEffectPlayback"
Cohesion: 0.10
Nodes (16): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+8 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.13
Nodes (7): Cancelled, Captured, Rejected, ShortcutCaptureResult, GlobalShortcutAdapter, IsCapturing, INeutralKeyEmitter

### Community 43 - "StartEffectCommand"
Cohesion: 0.13
Nodes (10): StartEffectCommand, EffectPlacement, Cursor, Monitor, EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures (+2 more)

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.12
Nodes (13): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+5 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.15
Nodes (29): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+21 more)

### Community 47 - ".Digit"
Cohesion: 0.33
Nodes (3): ShortcutKey, Kind, Value

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (37): SkiaSharp allowed only in Presentation, Action slot, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+29 more)

### Community 49 - "S4-11 Acceptance criteria"
Cohesion: 0.15
Nodes (18): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+10 more)

### Community 50 - "Shortcut"
Cohesion: 0.15
Nodes (6): SettingsCommand, Clear, Draw, Shortcut, Key, Modifiers

### Community 51 - "LottiePlayback"
Cohesion: 0.15
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.06
Nodes (31): ISaveSoundChannel, ISoundStartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore, Failure, OnSave (+23 more)

### Community 53 - "EffectId"
Cohesion: 0.10
Nodes (21): BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, ActionSlot, Action, IsEmpty (+13 more)

### Community 54 - ".Save"
Cohesion: 0.18
Nodes (8): IActiveShortcuts, Invalid, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult

### Community 55 - "SoundReference"
Cohesion: 0.15
Nodes (12): SoundReference, DisplayName, LibraryFileName, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw (+4 more)

### Community 56 - ".Load"
Cohesion: 0.16
Nodes (11): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, FakeReporter, Reports (+3 more)

### Community 57 - "AppFailureLog"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 58 - "RuntimeSettingsTests"
Cohesion: 0.14
Nodes (10): FakePlayer, Calls, Path, NoFailures, NoSettingsFailures, PlayerFactory, Created, RuntimeSettingsTests (+2 more)

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
Cohesion: 0.26
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "DllImport"
Cohesion: 0.16
Nodes (6): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, NativeMethods, POINT

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 66 - "EffectSurfaceWindow"
Cohesion: 0.15
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 67 - "Fixture"
Cohesion: 0.15
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 implementation plan"
Cohesion: 0.15
Nodes (15): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, Release publishing package (R4), SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), One-Byte Mutation Check, R4-02 acceptance criteria (+7 more)

### Community 70 - "STEPS.md"
Cohesion: 0.13
Nodes (20): Evidence, Implementation, R4-01: acceptance criteria, Validation, R4-01: implementation plan, Task, v4 / R4-01: create a versioned draft release, R4-02 Checksums and Provenance Task (+12 more)

### Community 71 - "v4 Simple Implementation Steps"
Cohesion: 0.11
Nodes (27): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations (+19 more)

### Community 72 - "NativeMethods"
Cohesion: 0.21
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 74 - "RenderLagHarness/Program.cs"
Cohesion: 0.10
Nodes (5): DrawEM.Tests.Release, DrawEM.Tests.Architecture, RenderLagHarness, RetiredSoundAssignmentsTests, VelopackHookExitTests

### Community 75 - "Settings Storage README"
Cohesion: 0.19
Nodes (16): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+8 more)

### Community 76 - ".OnStartup"
Cohesion: 0.08
Nodes (7): Application, App, EffectStopRoutes, ISessionInterruptionSource, Win32DisplayChangeSource, Win32SessionInterruptionSource, WpfApplicationLifetime

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.17
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 78 - "DrawingStyle"
Cohesion: 0.20
Nodes (6): v4 / FEAT-012: implementation plan, DrawingStyle, StrokeWidth, Pixels, SettingsFormatException, SettingsJson

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.19
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.22
Nodes (5): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, TimedOutSound

### Community 81 - "ScreenPoint"
Cohesion: 0.09
Nodes (14): ScreenPoint, ICursorPositionSource, IMonitorBoundsSource, Win32CursorPositionSource, EffectMonitorGuard, IEffectStartQueue, SettableCursor, Available (+6 more)

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (24): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog (+16 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "IntPtr"
Cohesion: 0.27
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.10
Nodes (12): ITrayMenu, TrayMenuCommand, Exit, Settings, StopEffects, Update, NotifyIconTrayHost, IconUri (+4 more)

### Community 86 - "DrawEM.App.Application.Sound"
Cohesion: 0.12
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, MediaSoundPlayerFactoryTests

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.18
Nodes (5): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, UpdateFeedTests

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

### Community 95 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 96 - "TrayMenuWindow"
Cohesion: 0.15
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 97 - "S4-06 acceptance: launch effects through shortcuts"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.35
Nodes (8): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings

### Community 100 - ".Attach"
Cohesion: 0.16
Nodes (7): GlassBackdrop, Supported, ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 101 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 102 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.27
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 103 - "UiThemeState"
Cohesion: 0.17
Nodes (7): v4 / FEAT-011: implementation plan, UiTheme, Dark, Light, UiThemeState, Current, TrayMenuWindowTests

### Community 104 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.16
Nodes (16): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, Effect Playback Contract, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X) (+8 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "ManagedSoundLibrary"
Cohesion: 0.20
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 107 - "drawEM v4: out of scope"
Cohesion: 0.24
Nodes (8): Evidence, v4 / FEAT-012: acceptance criteria, drawEM v4: out of scope, Program.Main custom WPF entry, VelopackHookExitTests, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, R4-07 Tray Updates Task

### Community 108 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 109 - "FEAT-011-glass-ui/task.md"
Cohesion: 0.33
Nodes (4): v4 / FEAT-011: acceptance criteria, Previous behavior, Required behavior, v4 / FEAT-011: glass design for Settings and the tray menu

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 112 - "S4-09 Acceptance criteria"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 113 - "FakeUpdateService"
Cohesion: 0.20
Nodes (7): FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled, LastToken, PendingVersion

### Community 114 - "IUpdateService"
Cohesion: 0.22
Nodes (3): IUpdateService, IsInstalled, PendingVersion

### Community 116 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 117 - "EffectLifecycleTests"
Cohesion: 0.24
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 119 - "FakePlayback"
Cohesion: 0.20
Nodes (7): EffectPlaybackException, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 121 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 122 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 123 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 124 - ".Create"
Cohesion: 0.22
Nodes (3): UpdateFeed, RecordingReporter, Lines

### Community 125 - "BuiltInEffectAnimations.cs"
Cohesion: 0.20
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

### Community 128 - "SoundSlot"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 134 - "Required behavior"
Cohesion: 0.22
Nodes (9): Out of scope, Per stroke, Previous behavior, Required behavior, Settings file, Settings window, Smoothing strategy, v4 / FEAT-012: smooth strokes with Bézier curves (+1 more)

### Community 136 - ".TryGetBounds"
Cohesion: 0.25
Nodes (3): MONITORINFO, POINT, Win32MonitorBoundsSource

### Community 138 - "SettingsSnapshot.cs"
Cohesion: 0.33
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

### Community 139 - "DecreaseWidthButton"
Cohesion: 0.40
Nodes (5): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, RepeatButton

## Knowledge Gaps
- **547 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+542 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 785 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SettingsWindow` connect `SettingsWindow` to `.Attach`, `Window`, `UiThemeState`, `App.xaml.cs`, `SettingsViewModel`, `.OnStartup`, `.Window_BindsEveryFieldWithoutBindingErrors`, `WidthBox`?**
  _High betweenness centrality (0.087) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _547 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.1391304347826087 - nodes in this community are weakly interconnected._
- **Why does `drawEM v4: out of scope` connect `drawEM v4: out of scope` to `v4 / FEAT-009: implementation plan`, `STEPS.md`, `FEAT-011-glass-ui/task.md`, `drawEM rules (AGENTS.md)`, `drawEM v4 Parallel Work Packages`?**
  _High betweenness centrality (0.087) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.0881287726358149 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.086) - this node is a cross-community bridge._