# Graph Report - wokrtree-s04-11  (2026-10-13)

## Corpus Check
- 288 files · ~114,564 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 2738 nodes · 8216 edges · 118 communities (94 shown, 24 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1400 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ec944aea`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- ManagedSoundLibraryTests
- drawEM v4 roadmap: built-in Monitor and Cursor effects
- StrokeRenderElement
- S4-05 acceptance: save and load effect assignments
- JsonSettingsStoreTests
- GlobalShortcutAdapterTests
- S4-07 acceptance: stop effects when switching monitors
- Fixture
- Harness
- AppFailureLog
- Window
- ManualTimeProvider
- SaveHarness
- SettingsSnapshot
- Rig
- SettingsEditor
- App.xaml.cs
- .Validate
- DrawEM.App.Domain.Drawing
- DrawingSessionController
- SettingsEditorTests
- DrawEM.App.Application.Sound
- Fact
- Fixture
- RuntimeMediaTests
- Fixture
- ScreenPoint
- SoundReference
- .Window_BindsEveryFieldWithoutBindingErrors
- SlotViewModel
- DrawEM.App.Domain.Effects
- LoggingSoundFailureReporter
- DrawEM.App.Infrastructure
- MonitorBounds
- NativeMethods
- Action
- DrawEM.App.Domain.Settings
- EffectId
- drawEM architecture
- Harness
- drawEM v4: parallel work packages
- IntPtr
- Theory
- DrawingModeInputGate
- GlobalShortcutAdapter
- SoundChannelController
- Stage 1 Step 2 Review
- v3 Step 8: Publish and verify on Windows (task)
- .Save
- ISoundPlayer
- EffectLifecycleTests
- TrayApplication
- EffectSurfaceLayout
- Shortcut.cs
- .Deserialize
- LottiePlayback
- LoggingEffectFailureReporter
- EffectChannelController
- DrawEM.App.Presentation.Effects
- JsonSettingsStore
- ShortcutBindings
- ManualTimer
- drawEM v3 Roadmap: Drawing and Action Settings
- DrawEM.Tests.csproj
- Sound infrastructure README
- List
- SettingsLoadResult
- Win32SessionInterruptionSource
- StartEffectCommand
- S4-03 acceptance: play built-in Lottie effects
- ShortcutFieldViewModel
- FailingSurface
- SettingsViewModel
- SoundId
- DllImport
- .OnStartup
- SettingsWindow
- Program.cs
- ManagedSoundLibrary
- SingleInstanceGuardTests
- IDisposable
- FakeSoundChannel
- system_runtime_interopservices
- S4-11 task: stop effects through app lifecycle
- drawEM README
- ADR 0001: Lottie built-in effects
- .ReleaseStartedResources
- ShortcutModifiers
- Harness
- F03 harness results
- NativeMethods
- SoundSlot
- FIX-006: Single Screen Edge
- Fixture
- SettingsErrorCode
- Stage 1 sound implementation plan
- ShortcutBindingsTests
- NativeFunctionBridgeRule
- SettingsViewModel.cs
- FakePicker
- S4-09 task: preview draft effects
- .Key
- .TryGetBounds
- Lottie Simple License (Design Barn Inc.)
- .Load
- S4-03 plan: play built-in Lottie effects
- drawEM Prototype Tray Icon
- DrawEM.App.Presentation.Sound
- Orange Brush Stroke Glyph
- Sound domain README
- CLAUDE.md
- .Digit
- EffectChannelController
- KeyDirection
- v4: simple implementation steps
- NotifyIconTrayHost
- IApplicationLifetime
- RecordingNotifications

## God Nodes (most connected - your core abstractions)
1. `ScreenPoint` - 132 edges
2. `DrawingSessionController` - 71 edges
3. `SettingsSnapshot` - 67 edges
4. `Harness` - 66 edges
5. `SaveHarness` - 64 edges
6. `MonitorBounds` - 60 edges
7. `EffectId` - 59 edges
8. `GlobalShortcutAdapter` - 59 edges
9. `DrawEM.App.Domain.Drawing` - 56 edges
10. `SettingsViewModel` - 52 edges

## Surprising Connections (you probably didn't know these)
- `EffectCatalog` --semantically_similar_to--> `Domain/Effects/EffectCatalog`  [INFERRED] [semantically similar]
  docs/v4/visual-effects/step-05-effect-settings-schema/acceptance.md → src/DrawEM.App/Infrastructure/Settings/README.md
- `Harness` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/EffectShortcutTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `Harness` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Drawing/ShortcutCaptureTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `Rig` --calls--> `DrawingSessionController`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Settings/SettingsSaveStuckSoundTests.cs → src/DrawEM.App/Application/Drawing/DrawingSessionController.cs
- `Harness` --calls--> `ActiveSettings`  [INFERRED]
  tests/DrawEM.Tests/Infrastructure/Effects/EffectLifecycleTests.cs → src/DrawEM.App/Application/Settings/ActiveSettings.cs

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

## Communities (118 total, 24 thin omitted)

### Community 0 - "ManagedSoundLibraryTests"
Cohesion: 0.18
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 1 - "drawEM v4 roadmap: built-in Monitor and Cursor effects"
Cohesion: 0.15
Nodes (15): drawEM v4 roadmap: built-in Monitor and Cursor effects, ADR 0001: Lottie built-in effects, EffectMonitorGuard, GlobalShortcutAdapter, Monitor identity, topology generation and epoch, Playback contract, Settings schema 2 with effect payload, SettingsEditor (+7 more)

### Community 2 - "StrokeRenderElement"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 3 - "S4-05 acceptance: save and load effect assignments"
Cohesion: 0.05
Nodes (45): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), EffectCatalog, JsonSettingsStore.Save migration backup, JsonSettingsStoreTests, ManagedSoundLibrary (counts only SoundAction slots) (+37 more)

### Community 4 - "JsonSettingsStoreTests"
Cohesion: 0.11
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 5 - "GlobalShortcutAdapterTests"
Cohesion: 0.10
Nodes (17): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+9 more)

### Community 6 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.23
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 7 - "Fixture"
Cohesion: 0.12
Nodes (17): DrawEM.Tests.Application.Sound, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure, FakeRequest (+9 more)

### Community 8 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 9 - "AppFailureLog"
Cohesion: 0.24
Nodes (3): AppFailureLog, DefaultPath, AppFailureLogTests

### Community 10 - "Window"
Cohesion: 0.05
Nodes (45): CanSave, ChooseText, Clear, Color, Visible, Draw, Effect, Error (+37 more)

### Community 11 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (15): SoundChannelHost, StartHold, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate (+7 more)

### Community 12 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 13 - "SettingsSnapshot"
Cohesion: 0.13
Nodes (17): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DrawShortcut, Slots, Style (+9 more)

### Community 14 - "Rig"
Cohesion: 0.08
Nodes (23): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Release (+15 more)

### Community 15 - "SettingsEditor"
Cohesion: 0.07
Nodes (18): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width (+10 more)

### Community 16 - "App.xaml.cs"
Cohesion: 0.14
Nodes (3): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App

### Community 17 - ".Validate"
Cohesion: 0.17
Nodes (8): EffectAction, Effect, SoundAction, Sound, SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 18 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.17
Nodes (6): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Settings

### Community 19 - "DrawingSessionController"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 21 - "DrawEM.App.Application.Sound"
Cohesion: 0.15
Nodes (5): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Infrastructure.Settings, RetiredSoundAssignmentsTests

### Community 23 - "Fixture"
Cohesion: 0.11
Nodes (10): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+2 more)

### Community 24 - "RuntimeMediaTests"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 25 - "Fixture"
Cohesion: 0.15
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "ScreenPoint"
Cohesion: 0.12
Nodes (10): ScreenPoint, IMonitorBoundsSource, EffectMonitorGuard, SettableCursor, Available, Position, Reads, MutableCursorPositionSource (+2 more)

### Community 27 - "SoundReference"
Cohesion: 0.13
Nodes (11): ActionSlot, Action, IsEmpty, Number, SlotAction, SoundReference, DisplayName, LibraryFileName (+3 more)

### Community 28 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.08
Nodes (13): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+5 more)

### Community 29 - "SlotViewModel"
Cohesion: 0.09
Nodes (21): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, ChooseText (+13 more)

### Community 30 - "DrawEM.App.Domain.Effects"
Cohesion: 0.15
Nodes (7): DrawEM.App.Domain.Effects, DrawEM.Tests.Application.Effects, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects, DrawEM.App.Application.Effects, IDisplayChangeSource, FakeDisplayChangeSource

### Community 31 - "LoggingSoundFailureReporter"
Cohesion: 0.14
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 32 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (6): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, VirtualKeys, AppIconTests, StaThread

### Community 33 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, EffectSurfaceWindow, Layout, CountingMonitorBoundsSource, Queries (+2 more)

### Community 34 - "NativeMethods"
Cohesion: 0.22
Nodes (3): NativeMethods, RECT, Win32MouseHookSource

### Community 35 - "Action"
Cohesion: 0.09
Nodes (12): PlaySoundCommand, SampleFailureRouter, SoundSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe (+4 more)

### Community 36 - "DrawEM.App.Domain.Settings"
Cohesion: 0.17
Nodes (5): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Sound, DrawEM.App.Application.Settings, DrawEM.Tests.Domain.Settings

### Community 37 - "EffectId"
Cohesion: 0.09
Nodes (13): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectId, Value, EffectPlacement, Cursor (+5 more)

### Community 38 - "drawEM architecture"
Cohesion: 0.08
Nodes (33): SkiaSharp allowed only in Presentation, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor drawing/video/effect channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController (+25 more)

### Community 39 - "Harness"
Cohesion: 0.13
Nodes (10): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+2 more)

### Community 40 - "drawEM v4: parallel work packages"
Cohesion: 0.18
Nodes (16): drawEM v4: out of scope, drawEM v4: parallel work packages, AGENTS.md branch and PR rules, Release publishing package (R4-xx), Shared files (App.xaml.cs, DrawEM.App.csproj, README.md, ci.yml), v3 Step 8 Windows acceptance gate, Visual effects package (S4-01..S4-12), drawEM v4: release publishing (+8 more)

### Community 41 - "IntPtr"
Cohesion: 0.26
Nodes (6): KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 43 - "DrawingModeInputGate"
Cohesion: 0.25
Nodes (9): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource (+1 more)

### Community 44 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (12): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+4 more)

### Community 45 - "SoundChannelController"
Cohesion: 0.09
Nodes (17): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+9 more)

### Community 46 - "Stage 1 Step 2 Review"
Cohesion: 0.16
Nodes (20): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+12 more)

### Community 47 - "v3 Step 8: Publish and verify on Windows (task)"
Cohesion: 0.07
Nodes (56): v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage, Right Alt reserved for AltGr, Settings schema/version policy, v3 Step 1: Settings snapshot and storage (task) (+48 more)

### Community 48 - ".Save"
Cohesion: 0.14
Nodes (12): IActiveShortcuts, Invalid, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, NotSaved, Saved, SettingsSaveOperation (+4 more)

### Community 49 - "ISoundPlayer"
Cohesion: 0.05
Nodes (22): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Path (+14 more)

### Community 50 - "EffectLifecycleTests"
Cohesion: 0.23
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 51 - "TrayApplication"
Cohesion: 0.16
Nodes (9): IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeDisposable, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 52 - "EffectSurfaceLayout"
Cohesion: 0.22
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 53 - "Shortcut.cs"
Cohesion: 0.22
Nodes (4): ShortcutKeyKind, Digit, Function, Letter

### Community 55 - "LottiePlayback"
Cohesion: 0.08
Nodes (14): EffectPlaybackException, IEffectPlayback, LottieEffectSurface, LottiePlayback, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure (+6 more)

### Community 56 - "LoggingEffectFailureReporter"
Cohesion: 0.22
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, EffectFailureLogTests

### Community 57 - "EffectChannelController"
Cohesion: 0.10
Nodes (17): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+9 more)

### Community 59 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 60 - "ShortcutBindings"
Cohesion: 0.20
Nodes (10): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, RuntimeSettingsTests (+2 more)

### Community 62 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.08
Nodes (35): Ten-second playback cap, Confirmed test boundaries, Sound business test scenarios, Playback business scenarios (start, replace, restart, cap, stale finish), Shortcut and independent-channel scenarios, SoundSlot, Eight Sound Slots Ctrl+Alt+1..8, Global Single Sound Channel (+27 more)

### Community 63 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 64 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 65 - "List"
Cohesion: 0.19
Nodes (7): IMouseHookSource, TimedOutSound, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 66 - "SettingsLoadResult"
Cohesion: 0.18
Nodes (10): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, FakeReporter, Reports, FakeStore (+2 more)

### Community 68 - "StartEffectCommand"
Cohesion: 0.20
Nodes (7): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, SamplingEditor, Failures, Sampler

### Community 69 - "S4-03 acceptance: play built-in Lottie effects"
Cohesion: 0.17
Nodes (14): Confetti effect (confetti, Monitor), Cursor-only fallback (not needed), Cursor placement (canvas-sized, clipped), Focus ring effect (focus-ring, Cursor), Built-in Lottie effects, Monitor placement (cover scaling), Placement (Monitor / Cursor), Release performance targets (100 ms appearance, 4 ms render, 25 ms frame interval) (+6 more)

### Community 70 - "ShortcutFieldViewModel"
Cohesion: 0.14
Nodes (10): BindableBase, ShortcutFieldViewModel, Command, Error, IsCapturing, IsEnabled, Message, Shortcut (+2 more)

### Community 71 - "FailingSurface"
Cohesion: 0.18
Nodes (9): RecordingSurface, Disposed, Last, Shown, FailingSurface, Disposed, Last, ShowFailure (+1 more)

### Community 72 - "SettingsViewModel"
Cohesion: 0.08
Nodes (15): IShortcutCapture, SettingsViewModel, CanSave, Clear, Color, DraftColor, Draw, Errors (+7 more)

### Community 73 - "SoundId"
Cohesion: 0.19
Nodes (7): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, SoundFailureLogTests

### Community 75 - ".OnStartup"
Cohesion: 0.22
Nodes (4): App, ApplicationExitPolicy, StartupFailure, StartupFailureTests

### Community 78 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 80 - "IDisposable"
Cohesion: 0.18
Nodes (5): CompositeDisposable, SingleInstanceGuard, DefaultName, CompositeDisposableTests, FakeDisposable

### Community 81 - "FakeSoundChannel"
Cohesion: 0.14
Nodes (4): ISoundStartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 83 - "S4-11 task: stop effects through app lifecycle"
Cohesion: 0.22
Nodes (9): Two-monitor manual check, S4-11 acceptance: stop effects through app lifecycle, S4-11 plan: stop effects through app lifecycle, Tray Stop effects, S4-11 task: stop effects through app lifecycle, S4-12 acceptance: publish and verify on Windows, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency (+1 more)

### Community 84 - "drawEM README"
Cohesion: 0.08
Nodes (24): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+16 more)

### Community 85 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.29
Nodes (6): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects

### Community 87 - "ShortcutModifiers"
Cohesion: 0.14
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 88 - "Harness"
Cohesion: 0.15
Nodes (9): Harness, Controller, EffectChannel, Effects, Players, Sound, SoundFailures, States (+1 more)

### Community 89 - "F03 harness results"
Cohesion: 0.60
Nodes (5): F03 harness raw output (after fix), F03 harness raw output (before fix), DrawingState.Generation reset signal, F03 harness results, RenderLagHarness tool

### Community 90 - "NativeMethods"
Cohesion: 0.25
Nodes (4): MONITORINFO, NativeMethods, POINT, RECT

### Community 91 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 92 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 93 - "Fixture"
Cohesion: 0.11
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 94 - "SettingsErrorCode"
Cohesion: 0.22
Nodes (9): SettingsErrorCode, DuplicateShortcut, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound, SlotLayout (+1 more)

### Community 95 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 99 - "FakePicker"
Cohesion: 0.29
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 100 - "S4-09 task: preview draft effects"
Cohesion: 0.33
Nodes (7): S4-09 acceptance: preview draft effects, S4-09 plan: preview draft effects, S4-09 task: preview draft effects, S4-10 acceptance: apply settings safely, S4-10 plan: apply settings safely, S4-10 task: apply settings safely, Sound presentation placeholder README

### Community 103 - "Lottie Simple License (Design Barn Inc.)"
Cohesion: 0.60
Nodes (5): Lottie Simple License, Lottie Simple License (Design Barn Inc.), Built-in effect sources, confetti.json (Rohith R Krishnan), focus-ring.json (Gowtham Anbu)

### Community 104 - ".Load"
Cohesion: 0.26
Nodes (5): ISettingsFailureReporter, SettingsStartup, StartupSettings, NoSettingsFailures, NoSettingsFailures

### Community 105 - "S4-03 plan: play built-in Lottie effects"
Cohesion: 0.27
Nodes (11): EffectCatalog (Domain/Effects), EffectAction, VideoAction / imported effects (later phase), S4-03 plan: play built-in Lottie effects, LayerDependencyTests, S4-03 task: play built-in Lottie effects, S4-04 acceptance: add effects to action slots, SettingsSnapshot.Validate (+3 more)

### Community 106 - "drawEM Prototype Tray Icon"
Cohesion: 0.67
Nodes (3): Freehand Drawn Stroke Motif, drawEM Prototype Tray Icon, v3 Step 7 Settings Window

### Community 111 - ".Digit"
Cohesion: 0.38
Nodes (3): ShortcutKey, Kind, Value

### Community 112 - "EffectChannelController"
Cohesion: 0.36
Nodes (7): Effect instance, EffectChannelController, IEffectSurface presentation port, S4-02 acceptance: control one running effect, S4-02 plan: control one running effect, S4-02 task: control one running effect, LottieEffectSurface / LottiePlayback

### Community 113 - "KeyDirection"
Cohesion: 0.25
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 114 - "v4: simple implementation steps"
Cohesion: 0.33
Nodes (8): CONTEXT.md glossary, EffectSurfaceWindow, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window, v4: simple implementation steps

### Community 116 - "IApplicationLifetime"
Cohesion: 0.29
Nodes (3): Application, IApplicationLifetime, WpfApplicationLifetime

## Ambiguous Edges - Review These
- `Sound presentation placeholder README` → `Sample ownership per editor and instance`  [AMBIGUOUS]
  src/DrawEM.App/Presentation/Sound/README.md · relation: conceptually_related_to

## Knowledge Gaps
- **448 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+443 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 622 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `v3 Step 8: Publish and verify on Windows (task)` to `drawEM v4 roadmap: built-in Monitor and Cursor effects`, `drawEM v3 Roadmap: Drawing and Action Settings`?**
  _High betweenness centrality (0.147) - this node is a cross-community bridge._
- **Are the 83 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 83 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _448 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.07618187292984041 - nodes in this community are weakly interconnected._
- **Why does `drawEM v4 roadmap: built-in Monitor and Cursor effects` connect `drawEM v4 roadmap: built-in Monitor and Cursor effects` to `S4-05 acceptance: save and load effect assignments`, `S4-09 task: preview draft effects`, `S4-03 acceptance: play built-in Lottie effects`, `drawEM architecture`, `S4-07 acceptance: stop effects when switching monitors`, `drawEM v4: parallel work packages`, `S4-03 plan: play built-in Lottie effects`, `v3 Step 8: Publish and verify on Windows (task)`, `EffectChannelController`, `v4: simple implementation steps`, `S4-11 task: stop effects through app lifecycle`, `ADR 0001: Lottie built-in effects`, `drawEM v3 Roadmap: Drawing and Action Settings`, `Stage 1 sound implementation plan`?**
  _High betweenness centrality (0.104) - this node is a cross-community bridge._
- **Are the 40 inferred relationships involving `DrawingSessionController` (e.g. with `.ClearAndExitDrawMode_RemovesCompletedAndActiveStrokes()` and `.ClearMonitor_PreservesStrokesOnOtherMonitor()`) actually correct?**
  _`DrawingSessionController` has 40 INFERRED edges - model-reasoned connections that need verification._

### Low-confidence Hints
_AMBIGUOUS edges — the extractor was unsure. Verify before acting on these._

- **What is the exact relationship between `Sound presentation placeholder README` and `Sample ownership per editor and instance`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._