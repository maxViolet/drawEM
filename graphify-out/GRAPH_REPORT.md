# Graph Report - wprktree-s4-12  (2026-10-13)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2855 nodes · 7859 edges · 130 communities (102 shown, 28 thin omitted)
- Extraction: 84% EXTRACTED · 16% INFERRED · 0% AMBIGUOUS · INFERRED: 1244 edges (avg confidence: 0.85)
- Token cost: 89,133 input · 1,806 output

## Graph Freshness
- Built from commit: `5ec9a30b`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Sound and Effect Failure Logs
- Stroke Drawing Rendering
- Effect Channel Lifecycle
- Shortcut Capture Handling
- Repo Workflow Rules
- v3 Drawing Settings Roadmap
- Settings Save Test Fakes
- JSON Settings Store Tests
- Global Shortcut Adapter Tests
- Shortcut Keys and Layering
- Settings View Model Bindings
- Settings and Effects Namespaces
- Monitor Bounds Geometry
- Layered Architecture Overview
- Drawing Module Namespaces
- Settings Editor Tests
- Action Slot Model
- Background Failure Logging
- Settings View Model Tests
- Sound Channel Host Thread
- Sound Acceptance Testing
- Effect Monitor Switching
- Effect Instance Playback
- Drawing Session Controller
- Settings Editor Commands
- Sound Player Abstraction
- Tray Application Lifecycle
- Managed Sound Library Tests
- Slot View Properties
- Shortcut Value Type
- Sound Sampling Failures
- Global Shortcut Dispatch
- Built-in Effect Catalog
- Effect Monitor Guard Tests
- Save Interrupt Test Rigs
- Runtime Media Tests
- Infrastructure Utilities
- Settings and Effects Modules
- E2E Test Scenarios
- Effect Window Rendering Decisions
- Effect Interruption Lifecycle
- App Failure Handling
- Active Settings Snapshot
- Sound Channel Controller
- Sound Slot Design Reviews
- Effect Sampler Tests
- Drawing Input Gating
- Managed Sound Library
- Sound Library Test Fakes
- Settings Load Startup
- App Composition Root
- Effect Shortcut Tests
- Sound Module Namespaces
- Slot Kind Selection
- CI and Release Workflow
- Effect Domain Glossary
- Effect Sampling
- Settings Save Operation
- Runtime Settings Tests
- Shortcut Bindings
- Win32 Window Interop
- Safe Settings Apply
- Effect Surface Window
- Render Lag Benchmark
- Settings Window UI Tests
- Solution and Packages
- v4 Planning and Release
- JSON Settings Persistence
- Settings Validation and Save
- Settings Infrastructure Files
- Effect Surface Coordinates
- Sound Playback Fakes
- Effect Monitor Guard
- Keyboard Hook Interop
- Blocking Player Test Fake
- Settings JSON Parsing
- Tray Icon and Stop Routes
- Code-Owned Sound Assignments
- Single Instance Guard
- Application Lifetime Ports
- Composite Disposable
- Mouse Hook Interop
- Native Bridge Rule Tests
- Settings Window Buttons
- Architecture Rule Tests
- Effect Presentation Layout
- Sound Player Fakes
- Effect Start Queue
- Effect Channel Design
- README User Guide
- Session Interruption Source
- Sound Start Hold
- Settings Command Kinds
- Single Screen Edge Fix
- Input Injection Interop
- S4-12 Publish Verification
- Sound Sampler Fake
- Settings Error Codes
- Settings Failure Dialogs
- App Root Dependencies
- Draft Effect Preview Sampling
- Fake Effect Sampler
- Shortcut Capture Port
- Win32 Monitor Bounds
- Keyboard Hook Source
- Lottie Effects ADR
- Settings Store Port
- Sound File Picker
- Status and Error Text
- Color and Width Inputs
- Render Lag Harness
- Lottie Effect Licenses
- Fake Cursor Position
- Display Change Source
- Shortcut Slot List
- Graphify Knowledge Graph
- Prototype Icons and Settings
- Sound Presentation Namespace
- S4-12 Publish Plan
- App Icon Brush Glyph
- Sound Domain Reference
- S4-09 Preview Plan
- Sound Presentation README

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 106 edges
2. `Harness` - 66 edges
3. `DrawingSessionController` - 63 edges
4. `SaveHarness` - 60 edges
5. `GlobalShortcutAdapter` - 54 edges
6. `MonitorBounds` - 51 edges
7. `JsonSettingsStoreTests` - 51 edges
8. `SettingsEditorTests` - 49 edges
9. `GlobalShortcutAdapterTests` - 48 edges
10. `SettingsEditor` - 46 edges

## Surprising Connections (you probably didn't know these)
- `One fixed placement per effect` --conceptually_related_to--> `Placement (Monitor / Cursor)`  [INFERRED]
  docs/adr/0001-lottie-built-in-effects.md → CONTEXT.md
- `EffectCatalog` --semantically_similar_to--> `Domain/Effects/EffectCatalog`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-05-effect-settings-schema/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md
- `Screen sharing checks` --semantically_similar_to--> `Screen share visibility of effects`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-12-effect-publish/acceptance.md → README.md
- `Harness` --calls--> `MonitorBounds`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/ShortcutCaptureTests.cs → src/DrawEM.App/Domain/Drawing/MonitorBounds.cs
- `SoundTestHarness` --calls--> `ScreenPoint`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/GlobalShortcutAdapterTests.cs → src/DrawEM.App/Domain/Drawing/DrawingState.cs

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
- **Lottie performance gate and fallbacks** — docs_v4_visual_effects_step_03_built_in_effects_plan_s4_03_1_speed_gate, docs_v4_visual_effects_roadmap_v4_release_targets, docs_v4_visual_effects_roadmap_v4_half_resolution_fallback, docs_v4_visual_effects_step_03_built_in_effects_acceptance_target_revision [EXTRACTED 1.00]
- **v4 release evidence gating** — docs_v4_visual_effects_step_12_effect_publish_acceptance_automated_gate, docs_v4_visual_effects_step_12_effect_publish_acceptance_published_app_checks, docs_v4_visual_effects_step_12_effect_publish_acceptance_measurements, docs_v4_visual_effects_step_12_effect_publish_acceptance_screen_sharing, docs_v4_visual_effects_step_12_effect_publish_acceptance_release_blockers, docs_v4_visual_effects_roadmap_v4_v3_step_8 [EXTRACTED 1.00]
- **Effect settings persistence flow (schema 2, store, catalog, backups)** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, src_drawem_app_infrastructure_settings_readme_jsonsettingsstore, src_drawem_app_infrastructure_settings_readme_effectcatalog, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_numbered_schema1_backups, src_drawem_app_infrastructure_settings_readme_schema_version_2 [INFERRED 0.85]
- **Effect start pipeline (shortcut/Sample -> guard -> channel -> surface -> player)** — docs_v4_visual_effects_roadmap_v4_globalshortcutadapter, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectsampler, docs_v4_visual_effects_roadmap_v4_effectmonitorguard, docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_lottieeffectsurface, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie [INFERRED 0.85]
- **Effect stop/interruption paths** — docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_stopeffects, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_effectstoproutes, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32sessioninterruptionsource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32displaychangesource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_trayapplication, docs_v4_visual_effects_roadmap_v4_settingssaveoperation [INFERRED 0.85]
- **Manual acceptance evidence documents** — docs_v1_smoke_test_file, docs_v1_s07_acceptance_results_file, docs_v1_steps_s07_acceptance_pass_plan_file, docs_v2_stage_1_acceptance_results_file [INFERRED 0.85]
- **Stage 1 sound pipeline: shortcut to channel to player to failure log** — docs_v2_stage_1_step_3_sound_shortcuts_plan_globalshortcutadapter, docs_v2_stage_1_step_2_sound_channel_review_soundchannelhost, docs_v2_stage_1_step_2_sound_channel_review_soundchannelcontroller, docs_v2_stage_1_step_2_sound_channel_review_mediasoundplayer, docs_v2_stage_1_step_2_sound_channel_review_loggingsoundfailurereporter, docs_v2_stage_1_step_1_sound_command_review_soundfailurelog [INFERRED 0.85]
- **Keyboard layout safety: Right Alt/AltGr and layout-switch prevention** — docs_v3_step_1_settings_plan_right_alt_reserved_altgr, docs_v3_step_3_action_routing_plan_right_alt_key_state_detection, docs_v3_step_4_shortcut_capture_plan_tagged_vk_0xe8_injection, docs_v3_step_8_publish_plan_layout_switch_release_blocker, docs_v3_step_8_publish_plan_test_layouts [INFERRED 0.85]
- **AltGr and input-layout protection in global keyboard hook** — docs_v3_roadmap_v3_right_alt_altgr_reserved, docs_v3_roadmap_v3_neutral_key_injection, docs_v3_roadmap_v3_shortcut_capture_mode, docs_v3_step_8_acceptance_results_input_language_hotkey_checks, docs_v3_additional_fixes_feat_005_e2e_runner_task_keyboardhookevents_neutralkeytag [INFERRED 0.85]
- **Save publication flow: persist, stop sound, clear, publish, media cleanup** — docs_v3_step_6_runtime_settings_plan_save_operation, docs_v3_step_6_runtime_settings_acceptance_start_lock, docs_v3_step_6_runtime_settings_plan_fresh_press_after_save, docs_v3_step_2_sound_library_task_orphan_cleanup, docs_v3_step_6_runtime_settings_task_active_settings_snapshot, docs_v3_step_7_settings_window_plan_settings_draft [INFERRED 0.85]
- **Shortcut-to-screen effect pipeline** — docs_v4_visual_effects_roadmap_v4_globalshortcutadapter, docs_v4_visual_effects_roadmap_v4_effectmonitorguard, docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieeffectsurface [INFERRED 0.85]

## Communities (130 total, 28 thin omitted)

### Community 0 - "Sound and Effect Failure Logs"
Cohesion: 0.06
Nodes (28): DrawEM.Tests.Application.Sound, SoundId, Value, EffectFailure, EffectFailureLog, DefaultPath, SoundFailure, SoundFailureLog (+20 more)

### Community 1 - "Stroke Drawing Rendering"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 2 - "Effect Channel Lifecycle"
Cohesion: 0.05
Nodes (32): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+24 more)

### Community 3 - "Shortcut Capture Handling"
Cohesion: 0.06
Nodes (20): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, KeyboardHookEvents (+12 more)

### Community 4 - "Repo Workflow Rules"
Cohesion: 0.05
Nodes (45): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+37 more)

### Community 5 - "v3 Drawing Settings Roadmap"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 6 - "Settings Save Test Fakes"
Cohesion: 0.08
Nodes (22): FakeSoundChannel, StopConfirmed, Saved, SaveHarness, Channel, Controller, EffectChannel, Effects (+14 more)

### Community 7 - "JSON Settings Store Tests"
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 8 - "Global Shortcut Adapter Tests"
Cohesion: 0.11
Nodes (12): FakeCursorPositionSource, FakeKeyboardHookSource, FailingCursorPositionSource, GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors (+4 more)

### Community 9 - "Shortcut Keys and Layering"
Cohesion: 0.09
Nodes (4): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule, ShortcutTests

### Community 10 - "Settings View Model Bindings"
Cohesion: 0.06
Nodes (26): IShortcutCapture, BindableBase, ISoundFilePicker, SettingsViewModel, CanSave, Clear, Color, DraftColor (+18 more)

### Community 11 - "Settings and Effects Namespaces"
Cohesion: 0.14
Nodes (6): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Application.Settings, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Application.Settings, DrawEM.App.Infrastructure.Effects, DrawEM.Tests.Infrastructure.Settings

### Community 12 - "Monitor Bounds Geometry"
Cohesion: 0.12
Nodes (11): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, CountingMonitorBoundsSource, Queries, MutableCursorPositionSource (+3 more)

### Community 13 - "Layered Architecture Overview"
Cohesion: 0.08
Nodes (35): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+27 more)

### Community 14 - "Drawing Module Namespaces"
Cohesion: 0.09
Nodes (7): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, RenderLagHarness, DrawEM.App.Infrastructure.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Drawing

### Community 16 - "Action Slot Model"
Cohesion: 0.15
Nodes (13): ActionSlot, Action, IsEmpty, Number, EffectAction, Effect, SlotAction, SoundAction (+5 more)

### Community 17 - "Background Failure Logging"
Cohesion: 0.10
Nodes (8): BackgroundLogWriter, LoggingEffectFailureReporter, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, ManualTimer, DueAt

### Community 19 - "Sound Channel Host Thread"
Cohesion: 0.14
Nodes (12): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+4 more)

### Community 20 - "Sound Acceptance Testing"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 21 - "Effect Monitor Switching"
Cohesion: 0.09
Nodes (32): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+24 more)

### Community 22 - "Effect Instance Playback"
Cohesion: 0.08
Nodes (20): Playback, RecordingSurface, Disposed, Last, Shown, RecordingLog, FailingSurface, Disposed (+12 more)

### Community 23 - "Drawing Session Controller"
Cohesion: 0.18
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 24 - "Settings Editor Commands"
Cohesion: 0.09
Nodes (7): IEffectSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 25 - "Sound Player Abstraction"
Cohesion: 0.08
Nodes (11): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment (+3 more)

### Community 26 - "Tray Application Lifecycle"
Cohesion: 0.20
Nodes (7): TrayApplication, FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable, TrayApplicationTests

### Community 27 - "Managed Sound Library Tests"
Cohesion: 0.22
Nodes (4): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory

### Community 28 - "Slot View Properties"
Cohesion: 0.08
Nodes (25): ChooseText, Clear, Color, Visible, Draw, Effect, Error, FileName (+17 more)

### Community 29 - "Shortcut Value Type"
Cohesion: 0.09
Nodes (22): Shortcut, Key, Modifiers, ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized (+14 more)

### Community 30 - "Sound Sampling Failures"
Cohesion: 0.15
Nodes (6): ISoundSampler, PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 31 - "Global Shortcut Dispatch"
Cohesion: 0.12
Nodes (6): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, INeutralKeyEmitter, RecoveringCursorPositionSource

### Community 32 - "Built-in Effect Catalog"
Cohesion: 0.12
Nodes (10): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, BuiltInEffectAnimations, EffectCatalogTests (+2 more)

### Community 33 - "Effect Monitor Guard Tests"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 34 - "Save Interrupt Test Rigs"
Cohesion: 0.11
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 35 - "Runtime Media Tests"
Cohesion: 0.17
Nodes (10): NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory, SaveRig, Library, Notifications (+2 more)

### Community 36 - "Infrastructure Utilities"
Cohesion: 0.09
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 37 - "Settings and Effects Modules"
Cohesion: 0.11
Nodes (7): DrawEM.App.Domain.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.Tests.Infrastructure.Drawing, DrawEM.Tests.Application.Effects, DrawEM.App.Application.Effects, DrawEM.Tests.Domain.Settings

### Community 38 - "E2E Test Scenarios"
Cohesion: 0.11
Nodes (25): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+17 more)

### Community 39 - "Effect Window Rendering Decisions"
Cohesion: 0.13
Nodes (26): Built-in effect catalog (Domain/Effects), Half-resolution Monitor rendering fallback, v4 release performance targets, S4-03.1 speed gate, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified (+18 more)

### Community 40 - "Effect Interruption Lifecycle"
Cohesion: 0.16
Nodes (15): EffectLifecycleTests, Events, Interruptions, Harness, Controller, Displays, EffectChannel, Effects (+7 more)

### Community 41 - "App Failure Handling"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 42 - "Active Settings Snapshot"
Cohesion: 0.12
Nodes (14): ActiveSettings, Current, SettingsSnapshot, ClearShortcut, Default, DrawShortcut, Slots, Style (+6 more)

### Community 43 - "Sound Channel Controller"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 44 - "Sound Slot Design Reviews"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 45 - "Effect Sampler Tests"
Cohesion: 0.11
Nodes (11): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+3 more)

### Community 46 - "Drawing Input Gating"
Cohesion: 0.21
Nodes (11): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, Win32MonitorBoundsSource, CountingMonitors, Queries (+3 more)

### Community 47 - "Managed Sound Library"
Cohesion: 0.15
Nodes (5): ISoundLibrary, SoundCleanupFailure, ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 48 - "Sound Library Test Fakes"
Cohesion: 0.14
Nodes (11): FakeLibrary, Discarded, Failure, Imported, Imports, RecordingNotifications, CleanupFailures, FakeLibrary (+3 more)

### Community 49 - "Settings Load Startup"
Cohesion: 0.15
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 51 - "Effect Shortcut Tests"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 52 - "Sound Module Namespaces"
Cohesion: 0.17
Nodes (4): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Domain.Sound

### Community 53 - "Slot Kind Selection"
Cohesion: 0.10
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 54 - "CI and Release Workflow"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 55 - "Effect Domain Glossary"
Cohesion: 0.16
Nodes (22): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001 Lottie built-in effects, Confetti effect (confetti), CONTEXT.md glossary (+14 more)

### Community 56 - "Effect Sampling"
Cohesion: 0.15
Nodes (6): EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, SamplingEditor, Failures, Sampler

### Community 57 - "Settings Save Operation"
Cohesion: 0.13
Nodes (8): IActiveShortcuts, ISaveSoundChannel, ISettingsSaveNotifications, SettingsSaveOperation, IdleShortcuts, RecordingNotifications, NotSavedReasons, UnconfirmedStops

### Community 58 - "Runtime Settings Tests"
Cohesion: 0.17
Nodes (5): NoFailures, NoSettingsFailures, RuntimeSettingsTests, LibraryDirectory, SettingsDirectory

### Community 59 - "Shortcut Bindings"
Cohesion: 0.19
Nodes (9): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+1 more)

### Community 60 - "Win32 Window Interop"
Cohesion: 0.15
Nodes (5): MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, NativeMethods

### Community 61 - "Safe Settings Apply"
Cohesion: 0.15
Nodes (20): EffectMonitorGuard, GlobalShortcutAdapter, Hook input ordering and monitor epoch, SettingsSaveOperation, Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified) (+12 more)

### Community 62 - "Effect Surface Window"
Cohesion: 0.13
Nodes (3): EffectSurfaceWindow, Layout, LottiePlayback

### Community 63 - "Render Lag Benchmark"
Cohesion: 0.21
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 65 - "Solution and Packages"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 66 - "v4 Planning and Release"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 67 - "JSON Settings Persistence"
Cohesion: 0.22
Nodes (4): JsonSettingsStore, DefaultDirectory, SettingsPath, NoReporter

### Community 68 - "Settings Validation and Save"
Cohesion: 0.18
Nodes (8): Invalid, ISettingsSave, NotSaved, Saved, SettingsSaveResult, FakeSettingsSave, Result, Saved

### Community 70 - "Effect Surface Coordinates"
Cohesion: 0.22
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 71 - "Sound Playback Fakes"
Cohesion: 0.14
Nodes (7): FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop, RecordingSoundFailures

### Community 73 - "Keyboard Hook Interop"
Cohesion: 0.23
Nodes (5): KBDLLHOOKSTRUCT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 74 - "Blocking Player Test Fake"
Cohesion: 0.20
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 76 - "Tray Icon and Stop Routes"
Cohesion: 0.15
Nodes (3): EffectStopRoutes, NotifyIconTrayHost, IconUri

### Community 77 - "Code-Owned Sound Assignments"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 78 - "Single Instance Guard"
Cohesion: 0.22
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 79 - "Application Lifetime Ports"
Cohesion: 0.14
Nodes (5): Application, IApplicationLifetime, IOverlayLifetime, ITrayHost, WpfApplicationLifetime

### Community 80 - "Composite Disposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, TimedOutSound

### Community 81 - "Mouse Hook Interop"
Cohesion: 0.23
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 82 - "Native Bridge Rule Tests"
Cohesion: 0.15
Nodes (7): AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests, PointerToDelegateProbe

### Community 83 - "Settings Window Buttons"
Cohesion: 0.17
Nodes (8): CanSave, CancelButton, ClearShortcutButton, DrawShortcutButton, PickColorButton, RestoreDefaultsButton, SaveButton, Button

### Community 86 - "Sound Player Fakes"
Cohesion: 0.18
Nodes (5): FakePlayer, Calls, Path, PlayerFactory, Created

### Community 87 - "Effect Start Queue"
Cohesion: 0.21
Nodes (6): StartEffectCommand, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started

### Community 88 - "Effect Channel Design"
Cohesion: 0.24
Nodes (11): Effect channel, Effect instance, EffectChannelController, IEffectSurface port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback (+3 more)

### Community 89 - "README User Guide"
Cohesion: 0.22
Nodes (11): EffectSurfaceWindow (nonactivating click-through WPF window), LottieEffectSurface, Screen sharing checks, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X), Current limits and plans, drawEM README, Draw shortcut (Ctrl+Alt+Z) (+3 more)

### Community 91 - "Sound Start Hold"
Cohesion: 0.20
Nodes (3): ISoundStartHold, StartHold, IdleSoundChannel

### Community 92 - "Settings Command Kinds"
Cohesion: 0.20
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 93 - "Single Screen Edge Fix"
Cohesion: 0.38
Nodes (9): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL) (+1 more)

### Community 94 - "Input Injection Interop"
Cohesion: 0.22
Nodes (3): INPUT, InputUnion, KEYBDINPUT

### Community 95 - "S4-12 Publish Verification"
Cohesion: 0.33
Nodes (9): v3 Step 8 Windows acceptance (release prerequisite), Automated gate (701 tests, Release build, win-x64 publish), Build identity (commit 335de887, SHA-256 hashes), Build machine record, S4-12 Acceptance criteria, Performance measurements (unverified), Published-app checks (10, all unverified), Release blockers (+1 more)

### Community 96 - "Sound Sampler Fake"
Cohesion: 0.22
Nodes (5): FakeSampler, Forgets, Samples, StopConfirmed, Stops

### Community 97 - "Settings Error Codes"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 100 - "Draft Effect Preview Sampling"
Cohesion: 0.36
Nodes (8): SettingsEditor, S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 Task: preview draft effects

### Community 101 - "Fake Effect Sampler"
Cohesion: 0.29
Nodes (4): FakeEffectSampler, Forgets, Samples, Stops

### Community 102 - "Shortcut Capture Port"
Cohesion: 0.33
Nodes (3): FakeCapture, Current, Ended

### Community 104 - "Keyboard Hook Source"
Cohesion: 0.33
Nodes (4): IKeyboardHookSource, KeyDirection, Down, Up

### Community 106 - "Settings Store Port"
Cohesion: 0.33
Nodes (3): FakeStore, Failure, OnSave

### Community 107 - "Sound File Picker"
Cohesion: 0.33
Nodes (4): FakePicker, Next, Picking, Picks

### Community 108 - "Status and Error Text"
Cohesion: 0.40
Nodes (5): Errors, Status, ErrorsText, StatusText, TextBlock

### Community 109 - "Color and Width Inputs"
Cohesion: 0.40
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 110 - "Render Lag Harness"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 111 - "Lottie Effect Licenses"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 112 - "Fake Cursor Position"
Cohesion: 0.40
Nodes (4): SettableCursor, Available, Position, Reads

### Community 114 - "Shortcut Slot List"
Cohesion: 0.67
Nodes (3): Slots, SlotList, ItemsControl

### Community 115 - "Graphify Knowledge Graph"
Cohesion: 0.67
Nodes (3): CLAUDE.md graphify rules, graphify, graphify knowledge graph (graphify-out)

### Community 116 - "Prototype Icons and Settings"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

## Knowledge Gaps
- **457 isolated node(s):** `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)`, `DefaultPath`, `Calls` (+452 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 663 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **28 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Window` connect `Slot View Properties` to `Settings Window UI Tests`, `Repo Workflow Rules`, `Status and Error Text`, `Color and Width Inputs`, `Shortcut Slot List`, `Settings Window Buttons`?**
  _High betweenness centrality (0.120) - this node is a cross-community bridge._
- **Are the 67 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 67 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Freehand Drawn Stroke Motif`, `Orange Brush Stroke Glyph`, `drawEM App Icon (SVG)` to the rest of the system?**
  _457 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Sound and Effect Failure Logs` be split into smaller, more focused modules?**
  _Cohesion score 0.06060606060606061 - nodes in this community are weakly interconnected._
- **Why does `SettingsWindow` connect `Slot View Properties` to `Settings Window UI Tests`, `App Composition Root`, `Settings Window Buttons`, `Tray Icon and Stop Routes`?**
  _High betweenness centrality (0.099) - this node is a cross-community bridge._
- **Should `Stroke Drawing Rendering` be split into smaller, more focused modules?**
  _Cohesion score 0.08339029391660971 - nodes in this community are weakly interconnected._
- **Should `Effect Channel Lifecycle` be split into smaller, more focused modules?**
  _Cohesion score 0.05369369369369369 - nodes in this community are weakly interconnected._