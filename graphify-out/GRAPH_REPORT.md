# Graph Report - worktree-new-design  (2026-10-19)

## Corpus Check
- 374 files · ~166,235 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3301 nodes · 9550 edges · 138 communities (118 shown, 20 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1606 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `3a3469c0`
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
- SoundChannelHost
- VelopackUpdateService
- SlotViewModel
- ISoundPlayer
- ManualTimeProvider
- DrawEM.App.Domain.Settings
- ManagedSoundLibraryTests
- Fact
- MonitorBounds
- Fixture
- Program
- ScreenPoint
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
- EffectId
- S4-12 Acceptance: Publish and Verify on Windows
- IEffectPlayback
- GlobalShortcutAdapter
- StartEffectCommand
- DrawEM.App.Infrastructure
- Harness
- Stage 1 sound implementation plan
- ActionSlot
- drawEM architecture
- S4-11 Acceptance criteria
- Shortcut
- LottiePlayback
- SaveHarness
- .Validate
- .Save
- SoundReference
- Win32DisplayChangeSource
- .OnStartup
- ShortcutBindings
- DrawEM.App.Presentation.Effects
- EffectChannelController
- ReleaseScriptsTests
- DllImport
- NativeMethods
- S4-07 acceptance: stop effects when switching monitors
- FakePlayback
- SoundId
- Fixture
- R4-02 implementation plan
- VelopackPackageIdentityTests
- STEPS.md
- v4 Simple Implementation Steps
- UpdateController
- BlockingPlayerFactory
- SoundChannelHost.cs
- Settings Storage README
- .RunAll
- SingleInstanceGuard
- .Deserialize
- drawEM rules (AGENTS.md)
- List
- EffectMonitorGuard
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- IntPtr
- NotifyIconTrayHost
- RenderLagHarness/Program.cs
- DrawEM.App.Application.Updates
- Sound infrastructure README
- Stage 1 Step 2 Review
- FIX-006: Single Screen Edge
- Func
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
- SoundSlot
- ITrayHost
- FEAT-011-glass-ui/task.md
- SettingsErrorCode
- VelopackUpdateService
- S4-09 Acceptance criteria
- EffectFailure
- FakeUpdateService
- EffectSurfaceWindow
- WidthBox
- FakePicker
- Show visual effects (Loop, Max length)
- IUpdateFailureReporter
- TrayMenuCommand
- system_runtime_interopservices
- F03 harness results
- BuiltInEffectAnimations.cs
- .Key
- GlassDark
- CLAUDE.md graphify rules
- drawEM Prototype Tray Icon
- R4-03 First-Release Acceptance Task
- DrawEM.App.Presentation.Sound
- ITrayHost / NotifyIconTrayHost update item
- Orange Brush Stroke Glyph
- Sound domain README
- Sound presentation placeholder README
- Portable win-x64 ZIP
- SettingsFailureDialog
- WinGet manifest (zip + nested portable)
- .Connect

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 133 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 69 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `Window` - 61 edges
7. `MonitorBounds` - 60 edges
8. `EffectId` - 60 edges
9. `SettingsViewModel` - 60 edges
10. `GlobalShortcutAdapter` - 59 edges

## Surprising Connections (you probably didn't know these)
- `v4 / FEAT-009: implementation plan` --references--> `StartEffectCommand`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Application/Effects/StartEffectCommand.cs
- `Evidence` --references--> `IEffectSampler`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/IEffectSampler.cs
- `Evidence` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/acceptance.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-009: implementation plan` --references--> `EffectAction`  [INFERRED]
  docs/v4/out-of-scope/FEAT-009-effect-loop-and-limit/plan.md → src/DrawEM.App/Domain/Settings/ActionSlot.cs
- `v4 / FEAT-011: implementation plan` --references--> `ITrayMenu`  [INFERRED]
  docs/v4/out-of-scope/FEAT-011-glass-ui/plan.md → src/DrawEM.App/Infrastructure/ITrayMenu.cs

## Import Cycles
- None detected.

## Communities (138 total, 20 thin omitted)

### Community 0 - "SettingsEditorTests"
Cohesion: 0.11
Nodes (5): Evidence, ShortcutKey, Kind, Value, SettingsEditorTests

### Community 1 - "StrokeRenderElement"
Cohesion: 0.07
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.13
Nodes (18): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.07
Nodes (18): ISettingsStore, Loaded, Missing, SettingsLoadResult, SettingsStoreException, Unreadable, ISettingsFailureReporter, FakeReporter (+10 more)

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.09
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (61): CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit, ChooseText (+53 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 7 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 8 - "Action"
Cohesion: 0.11
Nodes (12): PlaySoundCommand, SampleFailureRouter, SoundSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe (+4 more)

### Community 9 - "App.xaml.cs"
Cohesion: 0.06
Nodes (10): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.Tests.Presentation.Theme, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray (+2 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.09
Nodes (11): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+3 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.12
Nodes (14): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+6 more)

### Community 13 - "Theory"
Cohesion: 0.06
Nodes (9): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule (+1 more)

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.09
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 15 - "Fixture"
Cohesion: 0.11
Nodes (17): DrawEM.Tests.Application.Sound, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure, FakeRequest (+9 more)

### Community 16 - "SoundChannelHost"
Cohesion: 0.12
Nodes (13): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+5 more)

### Community 17 - "VelopackUpdateService"
Cohesion: 0.10
Nodes (17): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, VelopackHookExitTests, VelopackPackageIdentityTests, Write-Sha256Sums.ps1 / SHA256SUMS.txt (+9 more)

### Community 18 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.06
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "ManualTimeProvider"
Cohesion: 0.09
Nodes (9): BackgroundLogWriter, LoggingEffectFailureReporter, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingUpdateFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount (+1 more)

### Community 21 - "DrawEM.App.Domain.Settings"
Cohesion: 0.10
Nodes (11): DrawEM.App.Infrastructure.Sound, DrawEM.App.Domain.Settings, DrawEM.Tests.Infrastructure.Sound, DrawEM.Tests.Application.Settings, DrawEM.App.Application.Sound, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings (+3 more)

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.13
Nodes (8): SoundImportException, JsonSettingsStore, DefaultDirectory, SettingsPath, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "MonitorBounds"
Cohesion: 0.12
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, TrayMenuPlacement, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+2 more)

### Community 25 - "Fixture"
Cohesion: 0.14
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "Program"
Cohesion: 0.21
Nodes (5): Program, Program, RunResult, Sample, ScenarioSeed

### Community 27 - "ScreenPoint"
Cohesion: 0.14
Nodes (11): DrawingSessionController, CompletedStrokes, ScreenPoint, IMouseHookSource, OverlayWindowAdapter, DrawingSessionControllerTests, RecoveringCursorPositionSource, FakeOverlayView (+3 more)

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.22
Nodes (9): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource (+1 more)

### Community 29 - "Fixture"
Cohesion: 0.09
Nodes (14): SettableCursor, Available, Position, Reads, EffectMonitorGuardTests, Fixture, Channel, Cursor (+6 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.09
Nodes (21): ISoundLibrary, SoundCleanupFailure, SettingsStartup, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects (+13 more)

### Community 31 - "TrayApplication"
Cohesion: 0.17
Nodes (6): TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable, TrayApplicationTests

### Community 33 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.10
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.20
Nodes (10): NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library, Notifications (+2 more)

### Community 36 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.11
Nodes (8): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 39 - "EffectId"
Cohesion: 0.12
Nodes (10): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, BuiltInEffectAnimations, EffectCatalogTests (+2 more)

### Community 40 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.19
Nodes (14): Built-in Lottie Effect, Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency, S4-12 Task: publish and verify on Windows (+6 more)

### Community 41 - "IEffectPlayback"
Cohesion: 0.10
Nodes (16): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+8 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.08
Nodes (16): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+8 more)

### Community 43 - "StartEffectCommand"
Cohesion: 0.16
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Harness"
Cohesion: 0.12
Nodes (15): EffectLifecycleTests, Events, Interruptions, Harness, Controller, Displays, EffectChannel, Effects (+7 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 47 - "ActionSlot"
Cohesion: 0.16
Nodes (10): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SlotAction, EffectSample (+2 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (37): SkiaSharp allowed only in Presentation, Action slot, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+29 more)

### Community 49 - "S4-11 Acceptance criteria"
Cohesion: 0.16
Nodes (18): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+10 more)

### Community 50 - "Shortcut"
Cohesion: 0.13
Nodes (10): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot, Shortcut (+2 more)

### Community 51 - "LottiePlayback"
Cohesion: 0.15
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.06
Nodes (31): ISaveSoundChannel, ISoundStartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed, FakeStore, Failure, OnSave (+23 more)

### Community 53 - ".Validate"
Cohesion: 0.19
Nodes (6): SoundAction, Sound, SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 54 - ".Save"
Cohesion: 0.18
Nodes (8): Invalid, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation, SettingsSaveResult, Saved

### Community 55 - "SoundReference"
Cohesion: 0.14
Nodes (9): ActiveSettings, Current, IActiveShortcuts, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts (+1 more)

### Community 57 - ".OnStartup"
Cohesion: 0.10
Nodes (9): Application, App, AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, WpfApplicationLifetime, AppFailureLogTests (+1 more)

### Community 58 - "ShortcutBindings"
Cohesion: 0.10
Nodes (18): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+10 more)

### Community 59 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.30
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.11
Nodes (16): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+8 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "DllImport"
Cohesion: 0.15
Nodes (6): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, NativeMethods, POINT

### Community 63 - "NativeMethods"
Cohesion: 0.14
Nodes (5): MONITORINFO, NativeMethods, POINT, RECT, Win32MouseHookSource

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.19
Nodes (15): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+7 more)

### Community 65 - "FakePlayback"
Cohesion: 0.14
Nodes (8): EffectPlaybackException, LottieEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 66 - "SoundId"
Cohesion: 0.22
Nodes (6): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 67 - "Fixture"
Cohesion: 0.15
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 68 - "R4-02 implementation plan"
Cohesion: 0.17
Nodes (14): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), One-Byte Mutation Check, R4-02 acceptance criteria, actions/attest@v4 Build Provenance (+6 more)

### Community 70 - "STEPS.md"
Cohesion: 0.13
Nodes (20): Evidence, Implementation, R4-01: acceptance criteria, Validation, R4-01: implementation plan, Task, v4 / R4-01: create a versioned draft release, R4-02 Checksums and Provenance Task (+12 more)

### Community 71 - "v4 Simple Implementation Steps"
Cohesion: 0.10
Nodes (31): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window (+23 more)

### Community 72 - "UpdateController"
Cohesion: 0.24
Nodes (4): IUpdateService, IsInstalled, PendingVersion, UpdateController

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.20
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 74 - "SoundChannelHost.cs"
Cohesion: 0.14
Nodes (4): DrawEM.Tests.Release, DrawEM.Tests.Architecture, RetiredSoundAssignmentsTests, VelopackHookExitTests

### Community 75 - "Settings Storage README"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+7 more)

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.22
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.21
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 80 - "List"
Cohesion: 0.18
Nodes (6): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, RecordingSoundFailures, TimedOutSound

### Community 81 - "EffectMonitorGuard"
Cohesion: 0.18
Nodes (4): EffectPlacement, Cursor, Monitor, EffectMonitorGuard

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (26): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+18 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 84 - "IntPtr"
Cohesion: 0.26
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.13
Nodes (7): ITrayMenu, NotifyIconTrayHost, IconUri, FakeTrayMenu, Shown, Updates, NotifyIconTrayHostTests

### Community 87 - "DrawEM.App.Application.Updates"
Cohesion: 0.16
Nodes (6): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed

### Community 88 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 89 - "Stage 1 Step 2 Review"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 91 - "Func"
Cohesion: 0.23
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 92 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.44
Nodes (9): drawEM v4: out of scope, drawEM v4 Parallel Work Packages, Visual Effects Package (S4), Release Publishing README, R4-07 Tray Updates Acceptance, R4-07 Tray Updates Plan, R4-07 Tray Updates Task, TODO: groom the release publishing package (+1 more)

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
Cohesion: 0.28
Nodes (9): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings (+1 more)

### Community 100 - ".Attach"
Cohesion: 0.22
Nodes (5): GlassBackdrop, Supported, UiTheme, Dark, Light

### Community 101 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 102 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.27
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 103 - "UiThemeState"
Cohesion: 0.22
Nodes (4): v4 / FEAT-011: implementation plan, UiThemeState, Current, TrayMenuWindowTests

### Community 104 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.16
Nodes (16): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, Effect Playback Contract, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X) (+8 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 107 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 108 - "ITrayHost"
Cohesion: 0.22
Nodes (3): IApplicationLifetime, IOverlayLifetime, ITrayHost

### Community 109 - "FEAT-011-glass-ui/task.md"
Cohesion: 0.33
Nodes (4): v4 / FEAT-011: acceptance criteria, Previous behavior, Required behavior, v4 / FEAT-011: glass design for Settings and the tray menu

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 111 - "VelopackUpdateService"
Cohesion: 0.17
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 112 - "S4-09 Acceptance criteria"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 113 - "EffectFailure"
Cohesion: 0.18
Nodes (9): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, UpdateFailure, UpdateFailureLog, DefaultPath, EffectFailureLogTests (+1 more)

### Community 114 - "FakeUpdateService"
Cohesion: 0.20
Nodes (7): FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled, LastToken, PendingVersion

### Community 116 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 117 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 119 - "IUpdateFailureReporter"
Cohesion: 0.25
Nodes (4): IUpdateFailureReporter, RecordingReporter, Lines, UpdateFeedTests

### Community 120 - "TrayMenuCommand"
Cohesion: 0.33
Nodes (5): TrayMenuCommand, Exit, Settings, StopEffects, Update

### Community 122 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 123 - "BuiltInEffectAnimations.cs"
Cohesion: 0.20
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 125 - "GlassDark"
Cohesion: 0.25
Nodes (5): ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 126 - "CLAUDE.md graphify rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

### Community 128 - "R4-03 First-Release Acceptance Task"
Cohesion: 0.48
Nodes (6): v3 Step 8 Windows Acceptance Gate, Velopack Per-User Installer, R4-03 Acceptance Criteria, R4-03 Implementation Plan, Manual ZIP Update Procedure, R4-03 First-Release Acceptance Task

### Community 134 - "Portable win-x64 ZIP"
Cohesion: 0.33
Nodes (4): Portable win-x64 ZIP, release.yml Release Workflow, WinGet Portable ZIP Listing, Standard-User Windows Portable ZIP Test

### Community 136 - "WinGet manifest (zip + nested portable)"
Cohesion: 0.40
Nodes (5): GitHub release Environment, SIGNPATH_API_TOKEN, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip

## Knowledge Gaps
- **547 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+542 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 779 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **20 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.114) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _547 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.11493506493506493 - nodes in this community are weakly interconnected._
- **Why does `drawEM v3 Roadmap: Drawing and Action Settings` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `drawEM architecture`, `Stage 1 Step 2 Review`, `FEAT-005: Assisted E2E Runner`, `Stage 1 sound implementation plan`?**
  _High betweenness centrality (0.097) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07372346528973035 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `SettingsWindow` to `.Attach`, `Window`, `UiThemeState`, `App.xaml.cs`, `SettingsViewModel`, `.Window_BindsEveryFieldWithoutBindingErrors`, `WidthBox`, `FakePicker`, `.OnStartup`?**
  _High betweenness centrality (0.085) - this node is a cross-community bridge._