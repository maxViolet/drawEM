# Graph Report - worktree-r4-02  (2026-10-15)

## Corpus Check
- 100 files · ~148,586 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 3025 nodes · 8292 edges · 136 communities (117 shown, 19 thin omitted)
- Extraction: 85% EXTRACTED · 15% INFERRED · 0% AMBIGUOUS · INFERRED: 1274 edges (avg confidence: 0.85)
- Token cost: 200,782 input · 0 output

## Community Hubs (Navigation)
- Stroke Rendering & Overlay
- JSON Settings Store Tests
- v3 Settings Roadmap Docs
- Settings Window Controls
- App Namespaces & Imports
- Global Shortcut Adapter Tests
- Shortcut Capture Tests
- Settings ViewModel
- Sound Channel Controller Tests
- Domain & Infra Namespaces
- Shortcut Bindings Tests
- Settings Window Tests
- Settings ViewModel Tests
- Sound Player Abstractions
- Architecture Overview Docs
- Failure Logging & Time
- Settings & Sound Namespaces
- Shortcut Model
- Sound Channel Host
- Settings Snapshot & Library
- Settings Editor Tests
- Managed Sound Library Tests
- Slot ViewModel
- Stage 1 Sound Docs
- Effect Channel Controller Tests
- Drawing Session Controller
- Failure Records
- Effect Playback Surface
- Settings Snapshot Validation
- Runtime Media Tests
- Effect Placement & Layout
- Effect Sampler Tests
- Effect Lifecycle Tests
- Tray Application
- Global Shortcut Adapter
- Settings Editor
- Drawing Input Gate
- Sound Channel Controller
- Effect Monitor Guard Tests
- Effect Shortcut Tests
- E2E Runner & v3 Acceptance
- Settings Save Operation Tests
- Stuck Sound Save Tests
- Stage 1 Sound Reviews
- Sound Sampler
- Settings Window Code-behind
- App Startup & Sound Library
- Win32 Monitor Interop
- Effect Monitor Guard
- Built-in Effect Catalog
- Infrastructure Tray Files
- v1 Drawing Roadmap Docs
- Neutral Key & Bridge Rule
- Runtime Settings Tests
- Lottie Effects Docs (S4-03/04)
- Save Harness Fakes
- Presentation Settings Files
- v4 Package Structure Docs
- Effect Channel Controller
- Effect Sampler
- Release Publishing Roadmap
- Effect Surface Coordinates
- Lottie Playback
- JSON Settings Store
- Effect Playback Fakes
- Play Sound Command
- Render Lag Harness Program
- Release Workflow (R4-01/02)
- Monitor Switch Docs (S4-07)
- Settings Commands
- Core System Imports
- Active Settings
- Blocking Player Factory
- Release Scripts Tests
- Architecture Tests
- Composite Disposable
- Settings Storage & Schema 3
- Effect Lifecycle Docs (S4-11)
- App Failure Log
- Win32 Keyboard Hook
- Settings JSON Parsing
- Single Instance Guard
- Action Slots
- Application Exit Policy
- Projects & Build Config
- App Composition Root
- Notify Icon Tray Host
- Sound Slot Assignments
- Win32 Mouse Hook
- Session Interruption Source
- Sound README Docs
- Repo Rules (AGENTS.md)
- v4 Effects Roadmap Docs
- SignPath & WinGet Docs
- Effect Shortcut Docs (S4-06)
- Effect Settings Docs (S4-08)
- Keyboard Hook Events
- Settings Save Interfaces
- Save Notifications
- Tray Lifetime Interfaces
- Lottie Effect ADR
- FEAT-009 Loop & Limit
- Tray Updates Docs (R4-04)
- README Overview
- Start Effect Command
- Shortcut Capture Port
- Effect Start Queue Fakes
- Single Screen Edge Fix
- Settings Startup
- Settings Error Codes
- Effect Surface Window
- SendInput Interop
- Effect Preview Docs (S4-09)
- Safe Settings Apply Docs
- Fake Sound Player
- Settings Save Operation
- Sound Start Hold
- Width & Hex Inputs
- First Release Docs (R4-05)
- Built-in Effect Licenses
- Sound File Picker
- Checksums & Installer Docs
- Effect Window Probe Docs
- Settings Failure Dialog
- F03 Render Lag Results
- Native Bridge Rule
- Shortcut Key Kinds
- Display Change Source
- graphify Rules
- Tray Icon Design
- Neutral Key Emitter
- Sound Presentation Namespace
- App Icon Glyph
- Sound Domain README
- Sound Presentation README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 103 edges
2. `SettingsSnapshot` - 69 edges
3. `DrawingSessionController` - 63 edges
4. `SaveHarness` - 61 edges
5. `SettingsViewModel` - 59 edges
6. `GlobalShortcutAdapter` - 58 edges
7. `Harness` - 58 edges
8. `SlotViewModel` - 56 edges
9. `Window` - 56 edges
10. `JsonSettingsStoreTests` - 56 edges

## Surprising Connections (you probably didn't know these)
- `SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests` --implements--> `Test-first development rule`  [INFERRED]
  docs/v4/visual-effects/step-08-effect-settings-ui/acceptance.md → AGENTS.md
- `Return to drawEM v3 (rollback)` --semantically_similar_to--> `Rollback by corrected higher version`  [INFERRED] [semantically similar]
  README.md → docs/v4/release-publishing/ROADMAP.md
- `Failed/Interrupted Update Recovery Path` --semantically_similar_to--> `Rolling Back to v3 drawEM`  [INFERRED] [semantically similar]
  docs/v4/release-publishing/step-04-tray-updates/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md
- `Rig` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `Rig` --calls--> `SoundChannelController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Application/Sound/SoundChannelController.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Tag-driven release pipeline: verify tag, package, checksum/attest, draft release** — _github_workflows_release_verify_tag_step, tools_release_get_releaseversion, tools_release_new_releasepackage, tools_release_write_sha256sums, _github_workflows_release_create_draft_release_step [INFERRED 0.85]
- **Code-signing options considered under zero-cost constraint** — docs_adr_0002_release_signing_license_signpath_foundation, docs_adr_0002_release_signing_license_microsoft_artifact_signing, docs_adr_0002_release_signing_license_microsoft_store_msix, docs_v4_release_publishing_roadmap_smartscreen [EXTRACTED 1.00]
- **Sound shortcut play and Save interlock flow** — docs_architecture_globalshortcutadapter, docs_architecture_soundchannelhost, docs_architecture_soundchannelcontroller, docs_architecture_settingssaveoperation [EXTRACTED 1.00]
- **Release secrets, signing and distribution chain** — docs_v4_release_publishing_step_06_signpath_signing_plan_release_environment, docs_v4_release_publishing_step_06_signpath_signing_acceptance_signpath_api_token, docs_v4_release_publishing_step_06_signpath_signing_task_signpath_foundation_signing, docs_v4_release_publishing_step_07_winget_task_winget_community_catalog, docs_v4_release_publishing_step_07_winget_task_velopack_setup_exe [INFERRED 0.85]
- **Settings schema migration and rollback** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_jsonsettingsstore, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, docs_v4_visual_effects_roadmap_v4_schema1_migration_backup, docs_v4_visual_effects_roadmap_v4_settings_schema_3, src_drawem_app_infrastructure_settings_readme_v3_rollback_procedure [INFERRED 0.85]
- **Effect playback pipeline** — docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap_rendering, context_placement, docs_v4_visual_effects_roadmap_v4_built_in_lottie_effect [EXTRACTED 1.00]

## Communities (136 total, 19 thin omitted)

### Community 0 - "Stroke Rendering & Overlay"
Cohesion: 0.07
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 1 - "JSON Settings Store Tests"
Cohesion: 0.08
Nodes (11): FakeReporter, Reports, FakeStore, Saves, SettingsStartupTests, JsonSettingsStoreTests, BackupPath, SettingsPath (+3 more)

### Community 2 - "v3 Settings Roadmap Docs"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 3 - "Settings Window Controls"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit, ChooseText (+46 more)

### Community 4 - "App Namespaces & Imports"
Cohesion: 0.07
Nodes (11): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, DrawEM.App.Presentation.Effects, DrawEM.App.Presentation.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Presentation.Effects, IMouseHookSource (+3 more)

### Community 5 - "Global Shortcut Adapter Tests"
Cohesion: 0.12
Nodes (11): FakeCursorPositionSource, FakeKeyboardHookSource, GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played (+3 more)

### Community 6 - "Shortcut Capture Tests"
Cohesion: 0.07
Nodes (11): Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played, Results (+3 more)

### Community 7 - "Settings ViewModel"
Cohesion: 0.06
Nodes (26): BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color, DraftColor (+18 more)

### Community 8 - "Sound Channel Controller Tests"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 9 - "Domain & Infra Namespaces"
Cohesion: 0.14
Nodes (10): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+2 more)

### Community 10 - "Shortcut Bindings Tests"
Cohesion: 0.09
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, ShortcutBindingsTests

### Community 11 - "Settings Window Tests"
Cohesion: 0.08
Nodes (16): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+8 more)

### Community 13 - "Sound Player Abstractions"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 14 - "Architecture Overview Docs"
Cohesion: 0.08
Nodes (34): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+26 more)

### Community 15 - "Failure Logging & Time"
Cohesion: 0.10
Nodes (9): BackgroundLogWriter, LoggingEffectFailureReporter, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer (+1 more)

### Community 16 - "Settings & Sound Namespaces"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings

### Community 17 - "Shortcut Model"
Cohesion: 0.10
Nodes (19): Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized (+11 more)

### Community 18 - "Sound Channel Host"
Cohesion: 0.13
Nodes (12): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+4 more)

### Community 19 - "Settings Snapshot & Library"
Cohesion: 0.09
Nodes (19): ISoundLibrary, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots, Style (+11 more)

### Community 21 - "Managed Sound Library Tests"
Cohesion: 0.18
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 22 - "Slot ViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 23 - "Stage 1 Sound Docs"
Cohesion: 0.12
Nodes (35): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Bounded-exit recheck, Stage 1 sound acceptance results, Stage 1 test media assignments (+27 more)

### Community 24 - "Effect Channel Controller Tests"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 25 - "Drawing Session Controller"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 26 - "Failure Records"
Cohesion: 0.11
Nodes (15): BuiltInEffect, EffectId, Value, EffectPlacement, Cursor, Monitor, EffectFailure, EffectFailureLog (+7 more)

### Community 27 - "Effect Playback Surface"
Cohesion: 0.08
Nodes (21): EffectInstance, EffectInstanceId, EffectPlaybackException, IEffectFailureReporter, IEffectPlayback, IEffectSurface, LottieEffectSurface, FailingSurface (+13 more)

### Community 28 - "Settings Snapshot Validation"
Cohesion: 0.19
Nodes (4): EffectAction, Effect, SettingsError, SettingsSnapshotTests

### Community 29 - "Runtime Media Tests"
Cohesion: 0.13
Nodes (11): IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library (+3 more)

### Community 30 - "Effect Placement & Layout"
Cohesion: 0.15
Nodes (9): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+1 more)

### Community 31 - "Effect Sampler Tests"
Cohesion: 0.09
Nodes (11): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+3 more)

### Community 32 - "Effect Lifecycle Tests"
Cohesion: 0.08
Nodes (15): EffectLifecycleTests, Events, Interruptions, Harness, Controller, Displays, EffectChannel, Effects (+7 more)

### Community 33 - "Tray Application"
Cohesion: 0.18
Nodes (6): TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable, TrayApplicationTests

### Community 34 - "Global Shortcut Adapter"
Cohesion: 0.13
Nodes (8): GlobalShortcutAdapter, IsCapturing, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds

### Community 35 - "Settings Editor"
Cohesion: 0.12
Nodes (7): IEffectSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 36 - "Drawing Input Gate"
Cohesion: 0.18
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 37 - "Sound Channel Controller"
Cohesion: 0.10
Nodes (16): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+8 more)

### Community 38 - "Effect Monitor Guard Tests"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 39 - "Effect Shortcut Tests"
Cohesion: 0.10
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 40 - "E2E Runner & v3 Acceptance"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 41 - "Settings Save Operation Tests"
Cohesion: 0.18
Nodes (5): FakeStore, Failure, OnSave, Saved, SettingsSaveOperationTests

### Community 42 - "Stuck Sound Save Tests"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 43 - "Stage 1 Sound Reviews"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 44 - "Sound Sampler"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 46 - "App Startup & Sound Library"
Cohesion: 0.16
Nodes (5): SoundCleanupFailure, SettingsSaveNotifications, ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 47 - "Win32 Monitor Interop"
Cohesion: 0.13
Nodes (7): MONITORINFO, MSLLHOOKSTRUCT, POINT, NativeMethods, POINT, RECT, NativeMethods

### Community 48 - "Effect Monitor Guard"
Cohesion: 0.11
Nodes (9): ICursorPositionSource, EffectMonitorGuard, IEffectStartQueue, SettableCursor, Available, Position, Reads, FailingCursorPositionSource (+1 more)

### Community 49 - "Built-in Effect Catalog"
Cohesion: 0.13
Nodes (7): DrawEM.Tests.Domain.Effects, EffectCatalog, BuiltIn, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 50 - "Infrastructure Tray Files"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 51 - "v1 Drawing Roadmap Docs"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 52 - "Neutral Key & Bridge Rule"
Cohesion: 0.11
Nodes (10): EffectSample, Effect, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+2 more)

### Community 53 - "Runtime Settings Tests"
Cohesion: 0.15
Nodes (7): NoFailures, NoSettingsFailures, PlayerFactory, Created, RuntimeSettingsTests, LibraryDirectory, SettingsDirectory

### Community 54 - "Lottie Effects Docs (S4-03/04)"
Cohesion: 0.11
Nodes (22): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations, EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Lottie Simple License (SOURCES.md, LICENSE.md) (+14 more)

### Community 55 - "Save Harness Fakes"
Cohesion: 0.09
Nodes (18): SaveHarness, Channel, Controller, EffectChannel, Effects, Events, Gate, Keyboard (+10 more)

### Community 56 - "Presentation Settings Files"
Cohesion: 0.11
Nodes (3): DrawEM.Tests.Presentation.Settings, DrawEM.App.Presentation.Settings, DrawEM.App

### Community 57 - "v4 Package Structure Docs"
Cohesion: 0.16
Nodes (19): drawEM v4: out of scope, drawEM v4 out-of-scope task list, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+11 more)

### Community 58 - "Effect Channel Controller"
Cohesion: 0.15
Nodes (9): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+1 more)

### Community 59 - "Effect Sampler"
Cohesion: 0.14
Nodes (6): EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 60 - "Release Publishing Roadmap"
Cohesion: 0.15
Nodes (19): ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing / OV certificate (rejected), Microsoft Store MSIX (deferred), Owner questions (license, SignPath), SignPath Foundation signing, v3 Step 8 Windows acceptance gate, Release publishing package (R4), R4 acceptance strategy (automated, CI/provenance, real Windows, external) (+11 more)

### Community 61 - "Effect Surface Coordinates"
Cohesion: 0.20
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 62 - "Lottie Playback"
Cohesion: 0.14
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 63 - "JSON Settings Store"
Cohesion: 0.19
Nodes (8): Loaded, Missing, SettingsLoadResult, SettingsStoreException, Unreadable, JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 64 - "Effect Playback Fakes"
Cohesion: 0.12
Nodes (11): FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance, Playback, RecordingSurface (+3 more)

### Community 65 - "Play Sound Command"
Cohesion: 0.13
Nodes (3): IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 66 - "Render Lag Harness Program"
Cohesion: 0.22
Nodes (5): FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 67 - "Release Workflow (R4-01/02)"
Cohesion: 0.15
Nodes (14): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, R4-01 acceptance criteria and tag-run evidence, ReleaseScriptsTests, R4-01 implementation plan, R4-01: create a versioned draft release (+6 more)

### Community 68 - "Monitor Switch Docs (S4-07)"
Cohesion: 0.21
Nodes (15): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+7 more)

### Community 69 - "Settings Commands"
Cohesion: 0.15
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 71 - "Active Settings"
Cohesion: 0.17
Nodes (8): ActiveSettings, Current, ISoundSampler, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, MediaSoundPlayerFactoryTests

### Community 72 - "Blocking Player Factory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 73 - "Release Scripts Tests"
Cohesion: 0.23
Nodes (4): DrawEM.Tests.Release, ReleaseScriptsTests, ScriptResult, Output

### Community 75 - "Composite Disposable"
Cohesion: 0.20
Nodes (5): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, TimedOutSound

### Community 76 - "Settings Storage & Schema 3"
Cohesion: 0.21
Nodes (14): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+6 more)

### Community 77 - "Effect Lifecycle Docs (S4-11)"
Cohesion: 0.19
Nodes (14): S4-11 Acceptance criteria, EffectStopRoutes, Monitor epoch, EffectMonitorGuard.StopEffects, TrayApplication (Stop effects / Exit), Win32DisplayChangeSource, Win32SessionInterruptionSource, S4-11 Windows desktop checks (unverified) (+6 more)

### Community 78 - "App Failure Log"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 79 - "Win32 Keyboard Hook"
Cohesion: 0.26
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 81 - "Single Instance Guard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 82 - "Action Slots"
Cohesion: 0.23
Nodes (8): ActionSlot, Action, IsEmpty, Number, SlotAction, SoundAction, Sound, SoundBinding

### Community 83 - "Application Exit Policy"
Cohesion: 0.27
Nodes (3): ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 84 - "Projects & Build Config"
Cohesion: 0.19
Nodes (7): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk

### Community 86 - "Notify Icon Tray Host"
Cohesion: 0.20
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 87 - "Sound Slot Assignments"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 88 - "Win32 Mouse Hook"
Cohesion: 0.22
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 89 - "Session Interruption Source"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 90 - "Sound README Docs"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 91 - "Repo Rules (AGENTS.md)"
Cohesion: 0.19
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 92 - "v4 Effects Roadmap Docs"
Cohesion: 0.27
Nodes (11): v3 Step 8 Release Gate, ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows (+3 more)

### Community 93 - "SignPath & WinGet Docs"
Cohesion: 0.26
Nodes (13): R4-06 Acceptance Criteria, SIGNPATH_API_TOKEN (release Environment Secret), ADR-0002 Release Signing and License, R4-06 Implementation Plan, Reviewer-Protected GitHub release Environment, R4-06 Task: Conditional SignPath Signing, SignPath Foundation Code Signing, R4-07 Acceptance Criteria (+5 more)

### Community 94 - "Effect Shortcut Docs (S4-06)"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 95 - "Effect Settings Docs (S4-08)"
Cohesion: 0.32
Nodes (9): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings (+1 more)

### Community 96 - "Keyboard Hook Events"
Cohesion: 0.27
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 97 - "Settings Save Interfaces"
Cohesion: 0.24
Nodes (5): IActiveShortcuts, Invalid, NotSaved, Saved, SettingsSaveResult

### Community 98 - "Save Notifications"
Cohesion: 0.17
Nodes (5): RecordingNotifications, CleanupFailures, RecordingNotifications, NotSavedReasons, UnconfirmedStops

### Community 99 - "Tray Lifetime Interfaces"
Cohesion: 0.17
Nodes (5): Application, IApplicationLifetime, IOverlayLifetime, ITrayHost, WpfApplicationLifetime

### Community 100 - "Lottie Effect ADR"
Cohesion: 0.27
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 101 - "FEAT-009 Loop & Limit"
Cohesion: 0.22
Nodes (10): Action slot, Screen actions model (proposed evolution), FEAT-009 acceptance criteria and evidence, EffectChannelController (per-command deadline, loop toggle), FEAT-009 implementation plan, LottiePlayback loop from frame 0, FEAT-009: effect Loop and Max length per slot, Settings schema version 3 (loop, maxSeconds) (+2 more)

### Community 102 - "Tray Updates Docs (R4-04)"
Cohesion: 0.25
Nodes (10): R4-04 Acceptance Criteria, Failed/Interrupted Update Recovery Path, R4-04 Implementation Plan, Fake Update Source and Clock Tests, %LOCALAPPDATA%\drawEM Settings and Sound Storage, Velopack Apply-and-Restart Flow, Consent-Based Tray Updates, R4-04 Task: Consent-Based Tray Updates (+2 more)

### Community 103 - "README Overview"
Cohesion: 0.22
Nodes (11): Effect Playback Contract, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+3 more)

### Community 105 - "Shortcut Capture Port"
Cohesion: 0.22
Nodes (8): Cancelled, Captured, IShortcutCapture, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult

### Community 106 - "Effect Start Queue Fakes"
Cohesion: 0.20
Nodes (7): FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, MutableCursorPositionSource, Position

### Community 107 - "Single Screen Edge Fix"
Cohesion: 0.38
Nodes (9): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+1 more)

### Community 108 - "Settings Startup"
Cohesion: 0.31
Nodes (4): ISettingsStore, ISettingsFailureReporter, SettingsStartup, StartupSettings

### Community 109 - "Settings Error Codes"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 112 - "Effect Preview Docs (S4-09)"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 113 - "Safe Settings Apply Docs"
Cohesion: 0.32
Nodes (7): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely

### Community 114 - "Fake Sound Player"
Cohesion: 0.29
Nodes (3): FakePlayer, Calls, Path

### Community 115 - "Settings Save Operation"
Cohesion: 0.29
Nodes (3): ISettingsSave, ISettingsSaveNotifications, SettingsSaveOperation

### Community 116 - "Sound Start Hold"
Cohesion: 0.25
Nodes (3): ISaveSoundChannel, ISoundStartHold, StartHold

### Community 117 - "Width & Hex Inputs"
Cohesion: 0.29
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 118 - "First Release Docs (R4-05)"
Cohesion: 0.33
Nodes (7): Defender and SmartScreen Outcomes, R4-05 Acceptance Criteria, Owner Manual Publication of Draft Release, R4-05 Implementation Plan, SHA-256 and Provenance Verification Before Install, R4-05 Task: First-Release Acceptance, Unsigned First GitHub Release

### Community 119 - "Built-in Effect Licenses"
Cohesion: 0.43
Nodes (7): Built-in Lottie Effect, Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), confetti.json, Built-in Effect Sources, focus-ring.json

### Community 120 - "Sound File Picker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 121 - "Checksums & Installer Docs"
Cohesion: 0.40
Nodes (5): Velopack installer and update feed, R4-02 acceptance criteria, R4-02: publish checksums and provenance, R4-03 acceptance criteria, R4-03: package a per-user installer

### Community 122 - "Effect Window Probe Docs"
Cohesion: 0.53
Nodes (5): S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window

### Community 124 - "F03 Render Lag Results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 127 - "Shortcut Key Kinds"
Cohesion: 0.50
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 129 - "graphify Rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 130 - "Tray Icon Design"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Ambiguous Edges - Review These
- `Settings schema version 3 (loop, maxSeconds)` → `R4-03 acceptance criteria`  [AMBIGUOUS]
  docs/v4/release-publishing/step-03-velopack-installer/acceptance.md · relation: conceptually_related_to

## Knowledge Gaps
- **498 isolated node(s):** `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)`, `Value`, `Calls` (+493 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 714 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Settings Roadmap Docs` to `E2E Runner & v3 Acceptance`?**
  _High betweenness centrality (0.127) - this node is a cross-community bridge._
- **Are the 61 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 61 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)` to the rest of the system?**
  _498 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Stroke Rendering & Overlay` be split into smaller, more focused modules?**
  _Cohesion score 0.07497741644083107 - nodes in this community are weakly interconnected._
- **Why does `Window` connect `Settings Window Controls` to `Settings Window Tests`, `Settings Window Code-behind`, `Width & Hex Inputs`, `Effect Settings Docs (S4-08)`?**
  _High betweenness centrality (0.094) - this node is a cross-community bridge._
- **Should `JSON Settings Store Tests` be split into smaller, more focused modules?**
  _Cohesion score 0.07848944835246205 - nodes in this community are weakly interconnected._

### Low-confidence Hints
_AMBIGUOUS edges — the extractor was unsure. Verify before acting on these._

- **What is the exact relationship between `Settings schema version 3 (loop, maxSeconds)` and `R4-03 acceptance criteria`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._