# Graph Report - doc018  (2026-10-12)

## Corpus Check
- 16 files · ~108,178 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 2625 nodes · 7598 edges · 110 communities (91 shown, 19 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1288 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- Settings Store Loading
- v4 Package Structure Docs
- Drawing Rendering Primitives
- Branch and PR Rules
- JSON Settings Store Tests
- Global Shortcut Adapter Tests
- S4-07 Monitor Transitions
- Effect Failure Logging
- Shortcut Adapter Harness
- App Failure Log
- Settings Slot View State
- Sound Channel Thread Host
- Settings Editor Test Fakes
- Sound Library Cleanup
- Save Run Test Rig
- Settings Editor Draft
- App Composition Root
- Effect Id and Validation
- Drawing and Effects Namespaces
- Drawing Session Controller
- Settings Editor Tests
- Settings Namespaces
- Settings ViewModel Tests
- Effect Monitor Guard Tests
- Active Settings Runtime
- Effect Channel Controller Tests
- WPF Media Playback
- Sound Reference Bindings
- Settings ViewModel
- Slot Effect Editing
- Async Conversion Probes
- Background Log Writer
- Infrastructure Utilities
- Monitor Bounds Geometry
- Win32 Monitor Interop
- Sound Command and Sampler
- Settings WPF Controls
- Effect Placement Commands
- Architecture Overview
- Effect Shortcut Tests
- Sound and Settings Infrastructure
- Input Injection Structs
- Shortcut Validation Errors
- Drawing Mode Input Gate
- Shortcut Capture Results
- Sound Channel Controller
- Sound Playback Ports
- v3 Roadmap Concepts
- Settings Save Operation
- Layer Dependency Tests
- Save Sound Start Hold
- Tray Application Lifetime
- Settings Window
- Render Lag Harness
- Pointer Move Effect Queue
- Lottie Effect Surface
- E2E Scenario Runner
- Effect Channel Controller
- Effects Presentation Layout
- v3 Steps 2 and 6 Plans
- Fake Sound Player
- v2 Sound Business Rules
- E2E Runner and Failure Log
- Solution and Packages
- Shortcut Field ViewModel
- Lottie Effect Surface (2)
- Effect Surface Coordinates
- v3 Roadmap Concepts (2)
- Manual Timer
- Monitor Bounds Geometry (2)
- Tray Application Lifetime (2)
- Effect Surface Port
- Shortcut Capture Results (2)
- Effect Failure Logging (2)
- Low-Level Keyboard Hooks
- v2 Step 1 Sound Command
- v3 Settings and Capture Plans
- Sound Reference Bindings (2)
- Managed Sound Library
- App Startup and Exit
- CI and Release Workflows
- Composite Disposable
- Architecture Tests and Interop
- v1 Acceptance Pass
- v1 Drawing Roadmap
- Lottie Effects ADR
- Monitor Bounds Geometry (3)
- Shortcut Key Parsing
- Keyboard Hook Source
- F03 Render Lag Fix
- Single Instance Guard
- Sound Assignments
- FIX-006 Single Screen Edge
- WPF Media Playback (2)
- Settings Error Codes
- Call Microphone Routing
- Tray Notify Icon
- Shortcut Capture Results (3)
- Single Instance Guard (2)
- ISoundFilePicker
- ICustomRule
- ScreenPoint
- Confetti (Monitor effect)
- Settings Commands
- Monitor Bounds Geometry (4)
- Freehand Drawn Stroke Motif
- DrawEM.App.Presentation.Sound
- Orange Brush Stroke Glyph
- Sound domain README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 124 edges
2. `DrawingSessionController` - 70 edges
3. `Harness` - 66 edges
4. `SettingsSnapshot` - 62 edges
5. `GlobalShortcutAdapter` - 58 edges
6. `SaveHarness` - 55 edges
7. `SettingsViewModel` - 52 edges
8. `JsonSettingsStoreTests` - 51 edges
9. `DrawEM.App.Domain.Drawing` - 51 edges
10. `MonitorBounds` - 50 edges

## Surprising Connections (you probably didn't know these)
- `EffectCatalog` --semantically_similar_to--> `Domain/Effects/EffectCatalog`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-05-effect-settings-schema/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md
- `Harness` --calls--> `MonitorBounds`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/ShortcutCaptureTests.cs → src/DrawEM.App/Domain/Drawing/MonitorBounds.cs
- `Rig` --calls--> `MonitorBounds`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Domain/Drawing/MonitorBounds.cs
- `Rig` --calls--> `SettingsFailureDialog`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Infrastructure/Settings/SettingsFailureDialog.cs
- `Rig` --calls--> `SettingsSaveNotifications`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Infrastructure/Settings/SettingsSaveNotifications.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Draw shortcut input flow** — docs_architecture_win32keyboardhookadapter, docs_architecture_drawingmodeinputgate, docs_architecture_win32mousehookadapter, docs_architecture_drawingsessioncontroller, docs_architecture_overlaywindow [EXTRACTED 1.00]
- **Effect shortcut launch path** — docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_shortcutbindings, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_globalshortcutadapter, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_trystartonceperpress, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_configurationgeneration, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_starteffectcommand, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_effectchannelcontroller [EXTRACTED 1.00]
- **Pointer-move path deciding monitor change** — docs_v3_additional_fixes_fix_006_single_screen_edge_task_globalmouseinputadapter, docs_v3_additional_fixes_fix_006_single_screen_edge_task_drawingmodeinputgate, docs_v3_additional_fixes_fix_006_single_screen_edge_task_drawingsessioncontroller, docs_v3_additional_fixes_fix_006_single_screen_edge_task_imonitorboundssource, docs_v3_additional_fixes_fix_006_single_screen_edge_task_monitorbounds [EXTRACTED 1.00]
- **S4-07 monitor-transition stop flow** — docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_effectmonitorguard, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_monitor_epoch, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_stopstartedbefore, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_ieffectstartqueue, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_win32displaychangesource, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_checkcursor [EXTRACTED 1.00]
- **Settings save sequence** — docs_architecture_settingssaveoperation, docs_architecture_soundchannelhost, docs_architecture_globalshortcutadapter, docs_architecture_activesettings, docs_architecture_settingssnapshot [EXTRACTED 1.00]
- **Settings UI effect selection layers** — docs_v4_visual_effects_step_08_effect_settings_ui_acceptance_settingseditor, docs_v4_visual_effects_step_08_effect_settings_ui_acceptance_settingsviewmodel, docs_v4_visual_effects_step_08_effect_settings_ui_acceptance_effectoption, src_drawem_app_presentation_settings_settingswindow [EXTRACTED 1.00]
- **Global sound channel playback pipeline** — src_drawem_app_infrastructure_sound_readme_globalshortcutadapter, src_drawem_app_infrastructure_sound_readme_soundchannelhost, src_drawem_app_application_sound_readme_soundchannelcontroller, src_drawem_app_application_sound_readme_isoundplayerfactory, src_drawem_app_infrastructure_sound_readme_mediasoundplayerfactory, src_drawem_app_infrastructure_sound_readme_loggingsoundfailurereporter [EXTRACTED 1.00]
- **v3 delivery order: Steps 1-8 dependency chain** — docs_v3_step_1_settings_task, docs_v3_step_2_sound_library_task, docs_v3_step_3_action_routing_task, docs_v3_step_4_shortcut_capture_task, docs_v3_step_5_drawing_style_task, docs_v3_step_6_runtime_settings_task, docs_v3_step_7_settings_window_task, docs_v3_step_8_publish_task [EXTRACTED 1.00]
- **Lottie performance gate and fallbacks** — docs_v4_visual_effects_step_03_built_in_effects_plan_s4_03_1_speed_gate, docs_v4_visual_effects_roadmap_v4_release_targets, docs_v4_visual_effects_roadmap_v4_half_resolution_fallback, docs_v4_visual_effects_roadmap_v4_cursor_only_fallback, docs_v4_visual_effects_step_03_built_in_effects_acceptance_target_revision [EXTRACTED 1.00]
- **Effect settings persistence flow (schema 2, store, catalog, backups)** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, src_drawem_app_infrastructure_settings_readme_jsonsettingsstore, src_drawem_app_infrastructure_settings_readme_effectcatalog, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_numbered_schema1_backups, src_drawem_app_infrastructure_settings_readme_schema_version_2 [INFERRED 0.85]
- **Manual acceptance evidence documents** — docs_v1_smoke_test_file, docs_v1_s07_acceptance_results_file, docs_v1_steps_s07_acceptance_pass_plan_file, docs_v2_stage_1_acceptance_results_file [INFERRED 0.85]
- **Stage 1 sound pipeline: shortcut to channel to player to failure log** — docs_v2_stage_1_step_3_sound_shortcuts_plan_globalshortcutadapter, docs_v2_stage_1_step_2_sound_channel_review_soundchannelhost, docs_v2_stage_1_step_2_sound_channel_review_soundchannelcontroller, docs_v2_stage_1_step_2_sound_channel_review_mediasoundplayer, docs_v2_stage_1_step_2_sound_channel_review_loggingsoundfailurereporter, docs_v2_stage_1_step_1_sound_command_review_soundfailurelog [INFERRED 0.85]
- **Keyboard layout safety: Right Alt/AltGr and layout-switch prevention** — docs_v3_step_1_settings_plan_right_alt_reserved_altgr, docs_v3_step_3_action_routing_plan_right_alt_key_state_detection, docs_v3_step_4_shortcut_capture_plan_tagged_vk_0xe8_injection, docs_v3_step_8_publish_plan_layout_switch_release_blocker, docs_v3_step_8_publish_plan_test_layouts [INFERRED 0.85]
- **AltGr and input-layout protection in global keyboard hook** — docs_v3_roadmap_v3_right_alt_altgr_reserved, docs_v3_roadmap_v3_neutral_key_injection, docs_v3_roadmap_v3_shortcut_capture_mode, docs_v3_step_8_acceptance_results_input_language_hotkey_checks, docs_v3_additional_fixes_feat_005_e2e_runner_task_keyboardhookevents_neutralkeytag [INFERRED 0.85]
- **Save publication flow: persist, stop sound, clear, publish, media cleanup** — docs_v3_step_6_runtime_settings_plan_save_operation, docs_v3_step_6_runtime_settings_acceptance_start_lock, docs_v3_step_6_runtime_settings_plan_fresh_press_after_save, docs_v3_step_2_sound_library_task_orphan_cleanup, docs_v3_step_6_runtime_settings_task_active_settings_snapshot, docs_v3_step_7_settings_window_plan_settings_draft [INFERRED 0.85]
- **Shortcut-to-screen effect pipeline** — docs_v4_visual_effects_roadmap_v4_globalshortcutadapter, docs_v4_visual_effects_roadmap_v4_effectmonitorguard, docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieeffectsurface, docs_v4_visual_effects_roadmap_v4_effectsurfacewindow [INFERRED 0.85]

## Communities (110 total, 19 thin omitted)

### Community 0 - "Settings Store Loading"
Cohesion: 0.06
Nodes (23): Loaded, Missing, SettingsLoadResult, Unreadable, SoundImportException, ISettingsFailureReporter, SettingsStartup, StartupSettings (+15 more)

### Community 1 - "v4 Package Structure Docs"
Cohesion: 0.05
Nodes (71): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+63 more)

### Community 2 - "Drawing Rendering Primitives"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 3 - "Branch and PR Rules"
Cohesion: 0.05
Nodes (43): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+35 more)

### Community 4 - "JSON Settings Store Tests"
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 5 - "Global Shortcut Adapter Tests"
Cohesion: 0.12
Nodes (11): FakeCursorPositionSource, FakeKeyboardHookSource, GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played (+3 more)

### Community 6 - "S4-07 Monitor Transitions"
Cohesion: 0.06
Nodes (46): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+38 more)

### Community 7 - "Effect Failure Logging"
Cohesion: 0.10
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 8 - "Shortcut Adapter Harness"
Cohesion: 0.07
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 9 - "App Failure Log"
Cohesion: 0.08
Nodes (11): AppFailureLog, DefaultPath, ApplicationExitPolicy, EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, StartupFailure (+3 more)

### Community 10 - "Settings Slot View State"
Cohesion: 0.05
Nodes (46): CanSave, ChooseText, Clear, Color, Visible, Draw, Effect, Error (+38 more)

### Community 11 - "Sound Channel Thread Host"
Cohesion: 0.12
Nodes (14): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+6 more)

### Community 12 - "Settings Editor Test Fakes"
Cohesion: 0.08
Nodes (24): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+16 more)

### Community 13 - "Sound Library Cleanup"
Cohesion: 0.09
Nodes (18): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DrawShortcut, Slots, Style (+10 more)

### Community 14 - "Save Run Test Rig"
Cohesion: 0.08
Nodes (24): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Release (+16 more)

### Community 15 - "Settings Editor Draft"
Cohesion: 0.08
Nodes (18): ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width, ActionSlot (+10 more)

### Community 16 - "App Composition Root"
Cohesion: 0.07
Nodes (5): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 17 - "Effect Id and Validation"
Cohesion: 0.15
Nodes (8): EffectAction, Effect, SoundAction, Sound, SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 18 - "Drawing and Effects Namespaces"
Cohesion: 0.13
Nodes (10): DrawEM.App.Application.Drawing, DrawEM.App.Domain.Effects, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects (+2 more)

### Community 19 - "Drawing Session Controller"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 21 - "Settings Namespaces"
Cohesion: 0.14
Nodes (7): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 23 - "Effect Monitor Guard Tests"
Cohesion: 0.10
Nodes (11): EffectMonitorGuardTests, FakeDisplays, Fixture, Channel, Cursor, Displays, Guard, Monitors (+3 more)

### Community 24 - "Active Settings Runtime"
Cohesion: 0.13
Nodes (11): NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library, Notifications (+3 more)

### Community 25 - "Effect Channel Controller Tests"
Cohesion: 0.15
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "WPF Media Playback"
Cohesion: 0.09
Nodes (10): ISoundPlayer, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment, PlayThreadId (+2 more)

### Community 27 - "Sound Reference Bindings"
Cohesion: 0.11
Nodes (16): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, KeyChord, ShortcutBindings, Clear (+8 more)

### Community 28 - "Settings ViewModel"
Cohesion: 0.10
Nodes (14): SettingsViewModel, CanSave, Clear, Color, DraftColor, Draw, Errors, Fields (+6 more)

### Community 29 - "Slot Effect Editing"
Cohesion: 0.09
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 30 - "Async Conversion Probes"
Cohesion: 0.08
Nodes (15): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+7 more)

### Community 31 - "Background Log Writer"
Cohesion: 0.13
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 32 - "Infrastructure Utilities"
Cohesion: 0.09
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread

### Community 33 - "Monitor Bounds Geometry"
Cohesion: 0.18
Nodes (8): ScreenPoint, MonitorBounds, CursorSurfaceBounds, CountingMonitorBoundsSource, Queries, MutableCursorPositionSource, Position, EffectCanvasPlacementTests

### Community 34 - "Win32 Monitor Interop"
Cohesion: 0.12
Nodes (6): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource

### Community 35 - "Sound Command and Sampler"
Cohesion: 0.18
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 36 - "Settings WPF Controls"
Cohesion: 0.14
Nodes (9): FakeSampler, Forgets, Samples, StopConfirmed, Stops, FakeSettingsSave, Result, Saved (+1 more)

### Community 37 - "Effect Placement Commands"
Cohesion: 0.12
Nodes (12): DrawEM.Tests.Domain.Effects, StartEffectCommand, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, EffectPlacement (+4 more)

### Community 38 - "Architecture Overview"
Cohesion: 0.15
Nodes (20): SkiaSharp allowed only in Presentation, ActiveSettings, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController, EffectChannelController, drawEM architecture, Domain / Application / Infrastructure / Presentation layers (+12 more)

### Community 39 - "Effect Shortcut Tests"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 40 - "Sound and Settings Infrastructure"
Cohesion: 0.14
Nodes (8): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, v3 Step 3 plan, SoundAssignment, SoundAssignments, Slots, RetiredSoundAssignmentsTests

### Community 41 - "Input Injection Structs"
Cohesion: 0.14
Nodes (8): INPUT, InputUnion, KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 42 - "Shortcut Validation Errors"
Cohesion: 0.14
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 43 - "Drawing Mode Input Gate"
Cohesion: 0.24
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 44 - "Shortcut Capture Results"
Cohesion: 0.17
Nodes (3): GlobalShortcutAdapter, IsCapturing, INeutralKeyEmitter

### Community 45 - "Sound Channel Controller"
Cohesion: 0.14
Nodes (9): ISoundFailureReporter, ISoundPlayerFactory, AlwaysCurrent, IsCurrent, IPlayRequest, IsCurrent, SoundChannelController, ActiveSound (+1 more)

### Community 46 - "Sound Playback Ports"
Cohesion: 0.16
Nodes (21): Stage 1 Step 2 Plan, Stage 1 Step 2 Review, ISoundFailureReporter, ISoundPlayer (playback port), ISoundPlayerFactory, MediaSoundPlayer (WPF MediaPlayer), MediaSoundPlayerFactory, SoundChannelController (+13 more)

### Community 47 - "v3 Roadmap Concepts"
Cohesion: 0.16
Nodes (23): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1: Settings snapshot and storage (task), Domain/Application free of WPF, Win32, filesystem types, Eight numbered action slots, Immutable validated settings snapshot, v3 Step 2 acceptance criteria, SHA-256 content-addressed copy names (+15 more)

### Community 48 - "Settings Save Operation"
Cohesion: 0.15
Nodes (9): ISettingsStore, IActiveShortcuts, Invalid, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+1 more)

### Community 50 - "Save Sound Start Hold"
Cohesion: 0.11
Nodes (6): ISaveSoundChannel, ISoundStartHold, StartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 51 - "Tray Application Lifetime"
Cohesion: 0.26
Nodes (6): FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable, TrayApplicationTests

### Community 53 - "Render Lag Harness"
Cohesion: 0.11
Nodes (5): RenderLagHarness, ShortcutKeyKind, Digit, Function, Letter

### Community 54 - "Pointer Move Effect Queue"
Cohesion: 0.10
Nodes (13): ICursorPositionSource, Win32CursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, SettableCursor (+5 more)

### Community 55 - "Lottie Effect Surface"
Cohesion: 0.13
Nodes (8): EffectPlaybackException, LottiePlayback, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 56 - "E2E Scenario Runner"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 57 - "Effect Channel Controller"
Cohesion: 0.17
Nodes (11): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+3 more)

### Community 59 - "v3 Steps 2 and 6 Plans"
Cohesion: 0.13
Nodes (20): v3 Step 2 plan, Draft imports tracked separately from saved references, v3 Step 5 acceptance criteria, Per-Monitor-V2 single overlay window transform, v3 Step 5 plan, v3 Step 5: Configurable drawing style (task), Stroke width in physical pixels, v3 Step 6 acceptance criteria (+12 more)

### Community 60 - "Fake Sound Player"
Cohesion: 0.13
Nodes (9): FakePlayer, Calls, Path, NoFailures, PlayerFactory, Created, RuntimeSettingsTests, LibraryDirectory (+1 more)

### Community 61 - "v2 Sound Business Rules"
Cohesion: 0.13
Nodes (16): Per-monitor drawing/video/effect channels and global sound channel, Proposed screen-action model (action, command, action slot), SINGLE_SCREEN mode, SoundChannelController, v2 roadmap: screen actions, Global sound channel, Ten-second playback cap, Confirmed test boundaries (+8 more)

### Community 62 - "E2E Runner and Failure Log"
Cohesion: 0.18
Nodes (17): Stage 1 Step 2 Acceptance, Stage 1 Step 4 Acceptance, Stage 1 Manual Desktop Gate, Stage 1 Step 4 Plan, App.xaml.cs composition root, AppFailureLog, Self-contained Windows x64 Publish (PublishSingleFile=false), Stage 1 Step 4 Task: Publish and Verify (+9 more)

### Community 63 - "Solution and Packages"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 64 - "Shortcut Field ViewModel"
Cohesion: 0.12
Nodes (11): IShortcutCapture, BindableBase, ShortcutFieldViewModel, Command, Error, IsCapturing, IsEnabled, Message (+3 more)

### Community 65 - "Lottie Effect Surface (2)"
Cohesion: 0.16
Nodes (4): BuiltInEffectAnimations, LottieEffectSurface, BuiltInEffectAnimationsTests, BuiltInIds

### Community 66 - "Effect Surface Coordinates"
Cohesion: 0.20
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 67 - "v3 Roadmap Concepts (2)"
Cohesion: 0.15
Nodes (13): v3 Additional Fixes, drawEM v3 Roadmap: Drawing and Action Settings, Eight Numbered Action Slots, Managed Sound Library, Settings Persistence and Startup Fallback, Settings Window (Drawing and Actions tabs), Hook-Owned Shortcut Capture Mode, Shortcut Rule: two modifiers plus one key (+5 more)

### Community 68 - "Manual Timer"
Cohesion: 0.15
Nodes (8): Attempt, Command, Deadline, OnCompleted, OnFailed, Player, ManualTimer, DueAt

### Community 69 - "Monitor Bounds Geometry (2)"
Cohesion: 0.25
Nodes (3): MONITORINFO, EffectRenderLayout, EffectRenderLayoutTests

### Community 70 - "Tray Application Lifetime (2)"
Cohesion: 0.19
Nodes (4): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication

### Community 71 - "Effect Surface Port"
Cohesion: 0.17
Nodes (7): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown

### Community 72 - "Shortcut Capture Results (2)"
Cohesion: 0.17
Nodes (10): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeCapture (+2 more)

### Community 73 - "Effect Failure Logging (2)"
Cohesion: 0.28
Nodes (5): SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, SoundFailureLogTests

### Community 74 - "Low-Level Keyboard Hooks"
Cohesion: 0.21
Nodes (4): NativeMethods, POINT, RECT, NativeMethods

### Community 75 - "v2 Step 1 Sound Command"
Cohesion: 0.22
Nodes (14): Stage 1 Step 1 Acceptance, Stage 1 Step 1 Plan, sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record (+6 more)

### Community 76 - "v3 Settings and Capture Plans"
Cohesion: 0.14
Nodes (14): v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Right Alt reserved for AltGr, Settings schema/version policy, Startup orphan cleanup, Right Alt detection by actual key state (not LLKHF_INJECTED), v3 Step 4 plan (+6 more)

### Community 78 - "Managed Sound Library"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 79 - "App Startup and Exit"
Cohesion: 0.18
Nodes (3): Application, App, WpfApplicationLifetime

### Community 80 - "CI and Release Workflows"
Cohesion: 0.17
Nodes (10): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, Step 6 tray lifecycle and packaging plan (+2 more)

### Community 81 - "Composite Disposable"
Cohesion: 0.27
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, TimedOutSound

### Community 83 - "v1 Acceptance Pass"
Cohesion: 0.21
Nodes (11): ApplicationExitPolicy, BackgroundLogWriter<T>, v1 acceptance pass (step 7), Step 7 conditional pass, Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template (+3 more)

### Community 84 - "v1 Drawing Roadmap"
Cohesion: 0.23
Nodes (11): v1 roadmap: drawing, GlobalMouseInputAdapter seam, GlobalShortcutAdapter seam, OverlayWindowAdapter seam, v1 proposed test seams, TrayApplication seam, Ctrl+Alt+X clear shortcut, Current limits (non-elevated, fixed shortcuts) (+3 more)

### Community 85 - "Lottie Effects ADR"
Cohesion: 0.29
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 87 - "Shortcut Key Parsing"
Cohesion: 0.38
Nodes (3): ShortcutKey, Kind, Value

### Community 88 - "Keyboard Hook Source"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 89 - "F03 Render Lag Fix"
Cohesion: 0.27
Nodes (9): StrokeRenderElement, F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool, F03 render lag investigation plan, Hypothesis 2: whole drawing rewritten on every point (+1 more)

### Community 90 - "Single Instance Guard"
Cohesion: 0.20
Nodes (3): CleanupSteps, SingleInstanceGuard, DefaultName

### Community 91 - "Sound Assignments"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 92 - "FIX-006 Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 94 - "Settings Error Codes"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 95 - "Call Microphone Routing"
Cohesion: 0.36
Nodes (7): Step 5 Acceptance: Call Microphone Routing, Step 5 Plan: External Microphone Mixing, Voicemeeter A/B Bus Routing, Windows Per-App Output Routing, Voicemeeter Standard external mixer, Step 5 Task: Route Sound into Call Microphone, Google Meet

### Community 99 - "ISoundFilePicker"
Cohesion: 0.33
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 103 - "Confetti (Monitor effect)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 104 - "Settings Commands"
Cohesion: 0.50
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

### Community 106 - "Freehand Drawn Stroke Motif"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Ambiguous Edges - Review These
- `Sound presentation placeholder README` → `Sample ownership per editor and instance`  [AMBIGUOUS]
  src/DrawEM.App/Presentation/Sound/README.md · relation: conceptually_related_to

## Knowledge Gaps
- **414 isolated node(s):** `Pixels`, `VisualChildrenCount`, `Visual`, `VisualChildrenCount`, `WidthsAndScales` (+409 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 598 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **19 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Settings Slot View State` to `Branch and PR Rules`, `Settings Window`?**
  _High betweenness centrality (0.106) - this node is a cross-community bridge._
- **Are the 78 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 78 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Pixels`, `VisualChildrenCount`, `Visual` to the rest of the system?**
  _414 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Settings Store Loading` be split into smaller, more focused modules?**
  _Cohesion score 0.0638148667601683 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `Settings Window` to `ISoundFilePicker`, `Settings WPF Controls`, `Settings Slot View State`, `App Startup and Exit`, `App Composition Root`, `Monitor Bounds Geometry (3)`, `Settings ViewModel`?**
  _High betweenness centrality (0.104) - this node is a cross-community bridge._
- **Are the 40 inferred relationships involving `DrawingSessionController` (e.g. with `.ClearAndExitDrawMode_RemovesCompletedAndActiveStrokes()` and `.ClearMonitor_PreservesStrokesOnOtherMonitor()`) actually correct?**
  _`DrawingSessionController` has 40 INFERRED edges - model-reasoned connections that need verification._

### Low-confidence Hints
_AMBIGUOUS edges — the extractor was unsure. Verify before acting on these._

- **What is the exact relationship between `Sound presentation placeholder README` and `Sample ownership per editor and instance`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._