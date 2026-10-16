# Graph Report - worktree-r4-03  (2026-10-16)

## Corpus Check
- 23 files · ~151,650 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 3046 nodes · 8323 edges · 134 communities (115 shown, 19 thin omitted)
- Extraction: 85% EXTRACTED · 15% INFERRED · 0% AMBIGUOUS · INFERRED: 1274 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Settings Editor Tests
- Stroke Rendering & Overlay
- Release Publishing Roadmap
- JSON Settings Store Tests
- Global Shortcut Adapter Tests
- Settings Window Controls
- v3 Settings Roadmap Docs
- Shortcut Capture Tests
- Sound Sampler
- Domain & Infra Namespaces
- Settings ViewModel
- App Namespaces & Imports
- Shortcut Model
- Shortcut Bindings Tests
- Settings Window Tests
- Sound Channel Controller Tests
- Sound Channel Host
- R4 Signing & WinGet Steps
- Slot ViewModel
- Sound Player Abstractions
- Failure Logging & Time
- Settings & Sound Namespaces
- Managed Sound Library Tests
- Settings ViewModel Tests
- Effect Placement & Layout
- Effect Channel Controller Tests
- Effect Playback Surface
- Drawing Session Controller
- Drawing Input Gate
- Effect Monitor Guard Tests
- Settings Snapshot & Library
- Tray Application
- Settings Window Code-behind
- Effect Sampler Tests
- Sound Channel Controller
- Runtime Media Tests
- Effect Shortcut Tests
- Settings Editor
- Stuck Sound Save Tests
- Built-in Effect Catalog
- v4 Effects Roadmap Docs
- Effect Playback Fakes
- Global Shortcut Adapter
- Effect Monitor Guard
- Infrastructure Tray Files
- Effect Lifecycle Tests
- Stage 1 Sound Docs
- Global Shortcut Adapter (2)
- Architecture Overview Docs
- Effect Lifecycle Docs (S4-11)
- App Startup & Sound Library
- Lottie Playback
- Save Harness Fakes
- Effect Sampler
- Settings Save Interfaces
- Active Settings
- App Composition Root
- Render Lag Harness Program
- Runtime Settings Tests
- App Namespaces & Imports (2)
- Effect Channel Controller
- Release Scripts Tests
- Effect Surface Coordinates
- Win32 Monitor Interop
- Monitor Switch Docs (S4-07)
- JSON Settings Store
- JSON Settings Store Tests (2)
- Failure Records
- Settings Save Operation Tests
- Failure Records (2)
- E2E Runner & v3 Acceptance
- Lottie Effects Docs (S4-03/04)
- Play Sound Command
- Blocking Player Factory
- Architecture Tests
- Settings Storage & Schema 3
- App Failure Log
- Single Instance Guard
- Application Exit Policy
- Repo Rules (AGENTS.md)
- Composite Disposable
- Presentation Settings Files
- E2E Runner & v3 Acceptance (2)
- Projects & Build Config
- Keyboard Hook Events
- Notify Icon Tray Host
- Sound Slot Assignments
- Session Interruption Source
- Sound README Docs
- Stage 1 Sound Reviews
- Single Screen Edge Fix
- Win32 Keyboard Hook
- Win32 Mouse Hook
- v1 Drawing Roadmap Docs
- FEAT-009 Loop & Limit
- Stage 1 Sound Docs (2)
- E2E Runner & v3 Acceptance (3)
- Effect Shortcut Docs (S4-06)
- Effect Settings Docs (S4-08)
- Fake Sound Player
- Save Notifications
- Settings Commands
- Lottie Effect ADR
- Stage 1 Sound Reviews (2)
- README Overview
- Effect Lifecycle Tests (2)
- Shortcut Capture Port
- Effect Surface Window
- Tray Lifetime Interfaces
- Settings Startup
- Settings Error Codes
- SendInput Interop
- Effect Preview Docs (S4-09)
- Settings Failure Dialog
- Architecture Overview Docs (2)
- v1 Drawing Roadmap Docs (2)
- Built-in Effect Licenses
- Win32 Monitor Interop (2)
- Sound File Picker
- Architecture Overview Docs (3)
- Failure Records (3)
- F03 Render Lag Results
- Architecture Overview Docs (4)
- Runtime Media Tests (2)
- graphify Rules
- Tray Icon Design
- Neutral Key Emitter
- Sound Presentation Namespace
- Tray Update Item
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
8. `JsonSettingsStoreTests` - 56 edges
9. `SlotViewModel` - 56 edges
10. `Window` - 56 edges

## Surprising Connections (you probably didn't know these)
- `SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests` --implements--> `Test-first development rule`  [INFERRED]
  docs/v4/visual-effects/step-08-effect-settings-ui/acceptance.md → AGENTS.md
- `Return to drawEM v3 (rollback)` --semantically_similar_to--> `Rollback by corrected higher version`  [INFERRED] [semantically similar]
  README.md → docs/v4/release-publishing/ROADMAP.md
- `Rig` --calls--> `SettingsFailureDialog`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Infrastructure/Settings/SettingsFailureDialog.cs
- `Rig` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `SoundTestHarness` --calls--> `ScreenPoint`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/GlobalShortcutAdapterTests.cs → src/DrawEM.App/Domain/Drawing/DrawingState.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Effect playback pipeline** — docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap_rendering, context_placement, docs_v4_visual_effects_roadmap_v4_built_in_lottie_effect [EXTRACTED 1.00]
- **Sound shortcut play and Save interlock flow** — docs_architecture_globalshortcutadapter, docs_architecture_soundchannelhost, docs_architecture_soundchannelcontroller, docs_architecture_settingssaveoperation [EXTRACTED 1.00]
- **Settings schema migration and rollback** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_jsonsettingsstore, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, docs_v4_visual_effects_roadmap_v4_schema1_migration_backup, docs_v4_visual_effects_roadmap_v4_settings_schema_3, src_drawem_app_infrastructure_settings_readme_v3_rollback_procedure [INFERRED 0.85]
- **Artifact Integrity and Provenance Verification** — docs_v4_release_publishing_step_02_checksums_provenance_plan_sha256sums_manifest, tools_release_write_sha256sums, docs_v4_release_publishing_step_02_checksums_provenance_plan_actions_attest, docs_v4_release_publishing_step_02_checksums_provenance_plan_gh_attestation_verify, docs_v4_release_publishing_step_02_checksums_provenance_acceptance_one_byte_mutation_check [EXTRACTED 1.00]
- **Code Signing Options Considered** — docs_adr_0002_release_signing_license_signpath_foundation, docs_adr_0002_release_signing_license_microsoft_artifact_signing, docs_adr_0002_release_signing_license_microsoft_store_msix, docs_adr_0002_release_signing_license_unsigned_first_release [EXTRACTED 1.00]
- **Release artifact pipeline** — docs_v4_release_publishing_step_04_signpath_signing_plan_pe_file_signing, docs_v4_release_publishing_step_06_velopack_installer_plan_new_velopackpackage_ps1, docs_v4_release_publishing_step_06_velopack_installer_plan_write_sha256sums_ps1, docs_v4_release_publishing_step_05_winget_plan_winget_manifest [INFERRED 0.85]
- **Tray update flow** — docs_v4_release_publishing_step_07_tray_updates_plan_updatecontroller, docs_v4_release_publishing_step_07_tray_updates_plan_velopackupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_itrayhost, docs_v4_release_publishing_step_07_tray_updates_plan_trayapplication, docs_v4_release_publishing_step_07_tray_updates_plan_wait_exit_then_apply [EXTRACTED 1.00]
- **Settings directory preservation** — docs_v4_release_publishing_step_06_velopack_installer_task_package_id, docs_v4_release_publishing_step_06_velopack_installer_plan_json_settings_store, docs_v4_release_publishing_step_05_winget_acceptance_localappdata_drawem, docs_v4_release_publishing_step_06_velopack_installer_plan_velopackpackageidentitytests [INFERRED 0.85]

## Communities (134 total, 19 thin omitted)

### Community 0 - "Settings Editor Tests"
Cohesion: 0.06
Nodes (17): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SlotAction, SoundAction (+9 more)

### Community 1 - "Stroke Rendering & Overlay"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "Release Publishing Roadmap"
Cohesion: 0.06
Nodes (57): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Authenticode Signature, Microsoft Artifact Signing (+49 more)

### Community 3 - "JSON Settings Store Tests"
Cohesion: 0.11
Nodes (6): JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "Global Shortcut Adapter Tests"
Cohesion: 0.10
Nodes (16): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, GlobalShortcutAdapterTests (+8 more)

### Community 5 - "Settings Window Controls"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+46 more)

### Community 6 - "v3 Settings Roadmap Docs"
Cohesion: 0.08
Nodes (53): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Settings schema/version policy, v3 Step 1: Settings snapshot and storage (task), Domain/Application free of WPF, Win32, filesystem types (+45 more)

### Community 7 - "Shortcut Capture Tests"
Cohesion: 0.08
Nodes (13): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+5 more)

### Community 8 - "Sound Sampler"
Cohesion: 0.08
Nodes (14): PlaySoundCommand, SampleFailureRouter, SoundSampler, EffectSample, Effect, AsyncProbe, ClosureProbe, DelegateToPointerProbe (+6 more)

### Community 9 - "Domain & Infra Namespaces"
Cohesion: 0.12
Nodes (12): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.Tests.Infrastructure.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+4 more)

### Community 10 - "Settings ViewModel"
Cohesion: 0.06
Nodes (26): BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color, DraftColor (+18 more)

### Community 11 - "App Namespaces & Imports"
Cohesion: 0.08
Nodes (8): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, RenderLagHarness, DrawEM.App.Infrastructure.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Drawing, DrawEM.App

### Community 12 - "Shortcut Model"
Cohesion: 0.08
Nodes (23): Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized (+15 more)

### Community 13 - "Shortcut Bindings Tests"
Cohesion: 0.09
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 14 - "Settings Window Tests"
Cohesion: 0.08
Nodes (15): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+7 more)

### Community 15 - "Sound Channel Controller Tests"
Cohesion: 0.12
Nodes (17): DrawEM.Tests.Application.Sound, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure, FakeRequest (+9 more)

### Community 16 - "Sound Channel Host"
Cohesion: 0.12
Nodes (14): ISoundStartHold, SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created (+6 more)

### Community 17 - "R4 Signing & WinGet Steps"
Cohesion: 0.06
Nodes (38): R4-04 SignPath Signing Acceptance, R4-04 SignPath Signing Plan, ADR-0002 Release Signing License, GitHub release Environment, SIGNPATH_API_TOKEN, R4-04 SignPath Signing Task, SignPath Foundation, R4-05 WinGet Listing Acceptance (+30 more)

### Community 18 - "Slot ViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 19 - "Sound Player Abstractions"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "Failure Logging & Time"
Cohesion: 0.10
Nodes (9): BackgroundLogWriter, LoggingEffectFailureReporter, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer (+1 more)

### Community 21 - "Settings & Sound Namespaces"
Cohesion: 0.12
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings

### Community 22 - "Managed Sound Library Tests"
Cohesion: 0.18
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "Effect Placement & Layout"
Cohesion: 0.14
Nodes (9): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+1 more)

### Community 25 - "Effect Channel Controller Tests"
Cohesion: 0.14
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "Effect Playback Surface"
Cohesion: 0.08
Nodes (21): EffectInstance, EffectInstanceId, EffectPlaybackException, IEffectFailureReporter, IEffectPlayback, IEffectSurface, LottieEffectSurface, FailingSurface (+13 more)

### Community 27 - "Drawing Session Controller"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "Drawing Input Gate"
Cohesion: 0.14
Nodes (13): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, Win32MonitorBoundsSource, CountingMonitors, Queries (+5 more)

### Community 29 - "Effect Monitor Guard Tests"
Cohesion: 0.09
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 30 - "Settings Snapshot & Library"
Cohesion: 0.10
Nodes (16): ISoundLibrary, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots, Style (+8 more)

### Community 31 - "Tray Application"
Cohesion: 0.17
Nodes (7): TrayApplication, FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable, TrayApplicationTests

### Community 32 - "Settings Window Code-behind"
Cohesion: 0.12
Nodes (7): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, SettingsWindow, ViewModel, RepeatButton

### Community 33 - "Effect Sampler Tests"
Cohesion: 0.10
Nodes (11): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+3 more)

### Community 34 - "Sound Channel Controller"
Cohesion: 0.10
Nodes (16): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+8 more)

### Community 35 - "Runtime Media Tests"
Cohesion: 0.16
Nodes (10): NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library, Notifications (+2 more)

### Community 36 - "Effect Shortcut Tests"
Cohesion: 0.10
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 37 - "Settings Editor"
Cohesion: 0.13
Nodes (7): IEffectSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 38 - "Stuck Sound Save Tests"
Cohesion: 0.12
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 39 - "Built-in Effect Catalog"
Cohesion: 0.11
Nodes (7): DrawEM.Tests.Domain.Effects, EffectCatalog, BuiltIn, BuiltInEffectAnimations, EffectCatalogTests, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "v4 Effects Roadmap Docs"
Cohesion: 0.12
Nodes (24): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-01 acceptance: prove the effect window (+16 more)

### Community 41 - "Effect Playback Fakes"
Cohesion: 0.10
Nodes (12): FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance, Playback, RecordingSurface (+4 more)

### Community 43 - "Effect Monitor Guard"
Cohesion: 0.10
Nodes (10): ICursorPositionSource, Win32CursorPositionSource, EffectMonitorGuard, IEffectStartQueue, SettableCursor, Available, Position, Reads (+2 more)

### Community 44 - "Infrastructure Tray Files"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "Effect Lifecycle Tests"
Cohesion: 0.09
Nodes (12): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+4 more)

### Community 46 - "Stage 1 Sound Docs"
Cohesion: 0.21
Nodes (22): v2 roadmap: screen actions, Global sound channel, Ten-second playback cap, Playback business scenarios (start, replace, restart, cap, stale finish), Stage 1 sound implementation plan, Stage 1 Step 1 Acceptance, Stage 1 Step 1 Plan, Stage 1 Step 1 Task: Command and Code Configuration (+14 more)

### Community 47 - "Global Shortcut Adapter (2)"
Cohesion: 0.17
Nodes (9): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+1 more)

### Community 48 - "Architecture Overview Docs"
Cohesion: 0.15
Nodes (19): ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, EffectChannelController, drawEM architecture, GlobalShortcutAdapter, Per-Monitor V2 DPI and physical pixel coordinates (+11 more)

### Community 49 - "Effect Lifecycle Docs (S4-11)"
Cohesion: 0.12
Nodes (21): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+13 more)

### Community 50 - "App Startup & Sound Library"
Cohesion: 0.19
Nodes (4): SoundCleanupFailure, ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 51 - "Lottie Playback"
Cohesion: 0.12
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "Save Harness Fakes"
Cohesion: 0.10
Nodes (18): SaveHarness, Channel, Controller, EffectChannel, Effects, Events, Gate, Keyboard (+10 more)

### Community 53 - "Effect Sampler"
Cohesion: 0.14
Nodes (6): EffectSampleFailureRouter, EffectSampler, SamplingEditor, Failures, RunningStates, Sampler

### Community 54 - "Settings Save Interfaces"
Cohesion: 0.15
Nodes (9): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+1 more)

### Community 55 - "Active Settings"
Cohesion: 0.15
Nodes (9): ActiveSettings, Current, ISoundSampler, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, MediaSoundPlayerFactoryTests (+1 more)

### Community 57 - "Render Lag Harness Program"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 58 - "Runtime Settings Tests"
Cohesion: 0.19
Nodes (5): NoFailures, NoSettingsFailures, RuntimeSettingsTests, LibraryDirectory, SettingsDirectory

### Community 59 - "App Namespaces & Imports (2)"
Cohesion: 0.19
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 60 - "Effect Channel Controller"
Cohesion: 0.16
Nodes (9): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+1 more)

### Community 61 - "Release Scripts Tests"
Cohesion: 0.20
Nodes (4): DrawEM.Tests.Release, ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "Effect Surface Coordinates"
Cohesion: 0.21
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "Win32 Monitor Interop"
Cohesion: 0.17
Nodes (5): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, NativeMethods

### Community 64 - "Monitor Switch Docs (S4-07)"
Cohesion: 0.21
Nodes (15): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+7 more)

### Community 65 - "JSON Settings Store"
Cohesion: 0.21
Nodes (8): Loaded, Missing, SettingsLoadResult, SettingsStoreException, Unreadable, JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 66 - "JSON Settings Store Tests (2)"
Cohesion: 0.16
Nodes (9): FakeReporter, Reports, FakeStore, Saves, SettingsStartupTests, FakeStore, Failure, OnSave (+1 more)

### Community 67 - "Failure Records"
Cohesion: 0.22
Nodes (6): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 69 - "Failure Records (2)"
Cohesion: 0.23
Nodes (7): EffectId, Value, EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 70 - "E2E Runner & v3 Acceptance"
Cohesion: 0.15
Nodes (13): Global Single Sound Channel, Ten-Second Playback Cap, v3 Additional Fixes, drawEM v3 Roadmap: Drawing and Action Settings, Eight Numbered Action Slots, Managed Sound Library, Sample (draft playback), Settings Persistence and Startup Fallback (+5 more)

### Community 71 - "Lottie Effects Docs (S4-03/04)"
Cohesion: 0.15
Nodes (16): S4-03 acceptance: play built-in Lottie effects, BuiltInEffectAnimations, EffectFailureLog / LoggingEffectFailureReporter, EffectRenderLayout, Lottie Simple License (SOURCES.md, LICENSE.md), S4-03 plan: play built-in Lottie effects, LayerDependencyTests, S4-03 task: play built-in Lottie effects (+8 more)

### Community 72 - "Play Sound Command"
Cohesion: 0.14
Nodes (3): IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 73 - "Blocking Player Factory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage & Schema 3"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+7 more)

### Community 76 - "App Failure Log"
Cohesion: 0.27
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 77 - "Single Instance Guard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 78 - "Application Exit Policy"
Cohesion: 0.27
Nodes (3): ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 79 - "Repo Rules (AGENTS.md)"
Cohesion: 0.18
Nodes (6): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), drawEM v4: out of scope, Running-app check (unverified)

### Community 80 - "Composite Disposable"
Cohesion: 0.22
Nodes (5): CompositeDisposable, ITrayHost, CompositeDisposableTests, FakeDisposable, TimedOutSound

### Community 82 - "E2E Runner & v3 Acceptance (2)"
Cohesion: 0.25
Nodes (13): Stage 1 Step 4 Plan, App.xaml.cs composition root, AppFailureLog, Self-contained Windows x64 Publish (PublishSingleFile=false), FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override, docs/E2E-SCENARIOS.md scenario catalog, tools/E2ERunner black-box runner (+5 more)

### Community 83 - "Projects & Build Config"
Cohesion: 0.19
Nodes (7): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk

### Community 84 - "Keyboard Hook Events"
Cohesion: 0.22
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 85 - "Notify Icon Tray Host"
Cohesion: 0.20
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 86 - "Sound Slot Assignments"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 87 - "Session Interruption Source"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 88 - "Sound README Docs"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 89 - "Stage 1 Sound Reviews"
Cohesion: 0.26
Nodes (10): Stage 1 Step 2 Review, ISoundFailureReporter, ISoundPlayer (playback port), ISoundPlayerFactory, MediaSoundPlayer (WPF MediaPlayer), MediaSoundPlayerFactory, SoundChannelController, SoundChannelHost (dedicated STA sound thread) (+2 more)

### Community 90 - "Single Screen Edge Fix"
Cohesion: 0.27
Nodes (12): GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, First Key-Down Trigger (no auto-repeat), FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved) (+4 more)

### Community 91 - "Win32 Keyboard Hook"
Cohesion: 0.29
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 92 - "Win32 Mouse Hook"
Cohesion: 0.23
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 93 - "v1 Drawing Roadmap Docs"
Cohesion: 0.17
Nodes (10): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, Step 6 tray lifecycle and packaging plan (+2 more)

### Community 94 - "FEAT-009 Loop & Limit"
Cohesion: 0.20
Nodes (11): Action slot, Screen actions model (proposed evolution), FEAT-009 acceptance criteria and evidence, EffectChannelController (per-command deadline, loop toggle), FEAT-009 implementation plan, LottiePlayback loop from frame 0, FEAT-009: effect Loop and Max length per slot, Settings schema version 3 (loop, maxSeconds) (+3 more)

### Community 95 - "Stage 1 Sound Docs (2)"
Cohesion: 0.21
Nodes (11): v1 acceptance pass (step 7), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, Step 7 acceptance pass plan, Stage 1 sound acceptance results (+3 more)

### Community 96 - "E2E Runner & v3 Acceptance (3)"
Cohesion: 0.20
Nodes (12): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, Right Alt reserved for AltGr, Right Alt detection by actual key state (not LLKHF_INJECTED), v3 Step 8 Acceptance Results, v3 Release Blockers (unverified layout/DPI/multi-monitor checks) (+4 more)

### Community 97 - "Effect Shortcut Docs (S4-06)"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 98 - "Effect Settings Docs (S4-08)"
Cohesion: 0.32
Nodes (9): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings (+1 more)

### Community 99 - "Fake Sound Player"
Cohesion: 0.18
Nodes (5): FakePlayer, Calls, Path, PlayerFactory, Created

### Community 100 - "Save Notifications"
Cohesion: 0.17
Nodes (5): RecordingNotifications, CleanupFailures, RecordingNotifications, NotSavedReasons, UnconfirmedStops

### Community 101 - "Settings Commands"
Cohesion: 0.21
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 102 - "Lottie Effect ADR"
Cohesion: 0.27
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 103 - "Stage 1 Sound Reviews (2)"
Cohesion: 0.27
Nodes (11): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+3 more)

### Community 104 - "README Overview"
Cohesion: 0.22
Nodes (11): Effect Playback Contract, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z), drawEM tray app, Play sounds (Actions slots) (+3 more)

### Community 105 - "Effect Lifecycle Tests (2)"
Cohesion: 0.24
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 106 - "Shortcut Capture Port"
Cohesion: 0.22
Nodes (8): Cancelled, Captured, IShortcutCapture, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult

### Community 108 - "Tray Lifetime Interfaces"
Cohesion: 0.20
Nodes (4): Application, IApplicationLifetime, IOverlayLifetime, WpfApplicationLifetime

### Community 109 - "Settings Startup"
Cohesion: 0.31
Nodes (4): ISettingsStore, ISettingsFailureReporter, SettingsStartup, StartupSettings

### Community 110 - "Settings Error Codes"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 112 - "Effect Preview Docs (S4-09)"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 115 - "Architecture Overview Docs (2)"
Cohesion: 0.33
Nodes (4): SkiaSharp allowed only in Presentation, App.xaml.cs composition root, Domain / Application / Infrastructure / Presentation layers, IOverlayLifetime exception

### Community 116 - "v1 Drawing Roadmap Docs (2)"
Cohesion: 0.48
Nodes (6): v1 roadmap: drawing, GlobalMouseInputAdapter seam, GlobalShortcutAdapter seam, OverlayWindowAdapter seam, v1 proposed test seams, TrayApplication seam

### Community 117 - "Built-in Effect Licenses"
Cohesion: 0.43
Nodes (7): Built-in Lottie Effect, Lottie Simple License (FL 9.13.21), Lottie Simple License, LottieFiles (Design Barn Inc.), confetti.json, Built-in Effect Sources, focus-ring.json

### Community 119 - "Sound File Picker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 120 - "Architecture Overview Docs (3)"
Cohesion: 0.40
Nodes (5): OverlayWindow, StrokeRenderElement, F03 render lag investigation plan, Hypothesis 2: whole drawing rewritten on every point, Render lag hypotheses (queue overload, full redraw, GC, hook delay, composition)

### Community 121 - "Failure Records (3)"
Cohesion: 0.33
Nodes (4): BuiltInEffect, EffectPlacement, Cursor, Monitor

### Community 122 - "F03 Render Lag Results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 123 - "Architecture Overview Docs (4)"
Cohesion: 0.83
Nodes (4): DrawingModeInputGate, DrawingSessionController, Win32KeyboardHookAdapter, Win32MouseHookAdapter

### Community 126 - "graphify Rules"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 127 - "Tray Icon Design"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **503 isolated node(s):** `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)`, `Pixels`, `VisualChildrenCount` (+498 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 722 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Settings Window Controls` to `Settings Window Code-behind`, `Effect Settings Docs (S4-08)`, `Settings Window Tests`?**
  _High betweenness centrality (0.148) - this node is a cross-community bridge._
- **Are the 61 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 61 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)` to the rest of the system?**
  _503 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Settings Editor Tests` be split into smaller, more focused modules?**
  _Cohesion score 0.058075933075933076 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `Settings Window Code-behind` to `Settings Window Controls`, `Settings ViewModel`, `Settings Window Tests`, `Presentation Settings Files`, `Sound File Picker`, `App Composition Root`?**
  _High betweenness centrality (0.117) - this node is a cross-community bridge._
- **Should `Stroke Rendering & Overlay` be split into smaller, more focused modules?**
  _Cohesion score 0.07654320987654321 - nodes in this community are weakly interconnected._
- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Settings Roadmap Docs` to `E2E Runner & v3 Acceptance (3)`, `E2E Runner & v3 Acceptance`?**
  _High betweenness centrality (0.115) - this node is a cross-community bridge._