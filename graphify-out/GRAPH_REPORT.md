# Graph Report - worktree-new-design  (2026-10-19)

## Corpus Check
- 377 files · ~167,339 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3224 nodes · 9515 edges · 134 communities (120 shown, 14 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1590 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `39ec8191`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Fixture
- JsonSettingsStoreTests
- StrokeRenderElement
- GlobalShortcutAdapterTests
- EffectCatalog / BuiltInEffect
- Stage 1 Sound Implementation Plan
- drawEM README
- Window
- App.xaml.cs
- SettingsViewModel
- Harness
- ManagedSoundLibraryTests
- DrawEM.App.Domain.Settings
- TrayApplication
- .Window_BindsEveryFieldWithoutBindingErrors
- SaveHarness
- EffectId
- SettingsEditorTests
- ISoundPlayer
- LoggingSoundFailureReporter
- SlotViewModel
- Fact
- SettingsEditor
- MonitorBounds
- DrawEM.App.Domain.Drawing
- ManualTimeProvider
- Fixture
- EffectInstance
- ScreenPoint
- GlobalShortcutAdapter
- ShortcutBindings
- SettingsSnapshot
- SoundReference
- Fixture
- Fixture
- IntPtr
- EffectChannelController
- NotifyIconTrayHost
- SettingsWindow
- Theory
- .OnStartup
- Fixture
- .Save
- ReleaseScriptsTests
- FakeMonitorBoundsSource
- Rig
- SoundChannelController
- Harness
- DrawEM.App.Application.Sound
- Action
- DrawEM.App.Infrastructure
- R4-01..R4-07 Step Sequence
- .Digit
- EffectLifecycleTests
- v3 Step 8: Publish and Verify on Windows
- SingleInstanceGuard
- RuntimeSettingsTests
- DrawingModeInputGate
- TrayMenuWindow
- SamplingEditor
- DrawEM.App.Application.Updates
- Lottie Simple License (FL 9.13.21)
- .Attach
- FakeUpdateService
- DrawEM.Tests.csproj
- .Load
- ResourceDictionary
- drawEM v4 Roadmap: Monitor and Cursor Effects
- Sound Infrastructure README
- DateTimeOffset
- List
- v3 Step 6: Apply Saved Settings at Runtime
- drawEM architecture
- .Deserialize
- IDisposable
- Step 2 Implementation Review
- UiThemeState
- Step 1: Settings Snapshot and Storage
- R4 Release Publishing Steps
- EffectSurfaceLayout
- BlockingPlayerFactory
- LottiePlayback
- Harness
- DrawEM.App.Presentation.Effects
- FEAT-005: Assisted E2E Runner
- RenderLagHarness/Program.cs
- v3 Step 7: Settings Window
- S4-07 Acceptance Criteria
- JsonSettingsStore
- FakeSoundChannel
- App
- UpdateController
- R4-07 Plan: Tray Updates
- BuiltInEffectAnimations.cs
- S4-09 Preview Draft Effects (acceptance)
- BuiltInEffectAnimations
- LayerDependencyTests.cs
- S4-11 Stop Effects Through App Lifecycle (acceptance)
- FEAT-011 Implementation Plan
- StartEffectCommand
- VelopackUpdateService
- FEAT-009: Effect Loop and Length Limit
- LoggingEffectFailureReporter
- SettingsErrorCode
- SoundFailure
- drawEM v4: Out of Scope
- VelopackPackageIdentityTests
- drawEM v3 Roadmap: Drawing and Action Settings
- Func
- WidthBox
- FakePlayer
- IUpdateFailureReporter
- .Default_FollowsTheWindowsAppTheme
- S4-01 Acceptance: Effect Surface Probe
- S4-12 Publish and Verify on Windows (acceptance)
- R4-06 Plan: Velopack Installer
- S4-03 Acceptance Criteria
- TimeSpan
- drawEM v4: Parallel Work Packages
- drawEM domain glossary
- SettingsCommand
- DrawEM.App.Presentation.Sound
- Sound Domain README
- graphify knowledge-graph usage rules
- Sound Presentation Placeholder
- Step 5 Plan: External Microphone Mixing
- EffectChannelController
- S4-08 Acceptance Criteria
- GlassDark
- Step 3 Plan: Sound Shortcuts
- .Place
- .TryFind_ReturnsTheEntryForAKnownId
- TrayMenuCommand
- FakePlayback

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
- `v4 / FEAT-012: implementation plan` --references--> `SettingsEditor`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Application/Settings/SettingsEditor.cs
- `v4 / FEAT-012: implementation plan` --references--> `DrawingStyle`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Domain/Drawing/DrawingStyle.cs
- `Per stroke` --references--> `DrawingStyle`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/task.md → src/DrawEM.App/Domain/Drawing/DrawingStyle.cs
- `v4 / FEAT-012: implementation plan` --references--> `StrokeRenderElement`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/plan.md → src/DrawEM.App/Presentation/Drawing/StrokeRenderElement.cs
- `Previous behavior` --references--> `StrokeRenderElement`  [INFERRED]
  docs/v4/out-of-scope/FEAT-012-bezier-smoothing/task.md → src/DrawEM.App/Presentation/Drawing/StrokeRenderElement.cs

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Effect playback pipeline (channel to Lottie surface)** — docs_v4_visual_effects_roadmap_v4_effect_channel_controller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_effect_surface_window, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap, docs_v4_visual_effects_roadmap_v4_placement [EXTRACTED 1.00]
- **Settings Sample through the shortcut effect path** — docs_v4_visual_effects_step_09_effect_sample_acceptance_settingseditor, docs_v4_visual_effects_step_09_effect_sample_acceptance_ieffectsampler, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectsampler, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectmonitorguard, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectchannelcontroller, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectsamplefailurerouter [EXTRACTED 1.00]
- **Effect stop on lifecycle events via monitor epoch** — docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_effectstoproutes, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_effectmonitorguard_stopeffects, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32sessioninterruptionsource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32displaychangesource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_trayapplication, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_monitor_epoch [EXTRACTED 1.00]
- **F03 render-lag investigation and fix** — docs_v1_steps_f03_render_lag_investigation_plan_investigation_plan, docs_v1_steps_f03_render_lag_harness_results_harness_results, docs_v1_steps_f03_render_lag_harness_results_renderlagharness, docs_v1_steps_f03_render_lag_investigation_plan_hypothesis_full_redraw, docs_architecture_strokerenderelement, docs_v1_steps_f03_render_lag_harness_results_drawingstate_generation [EXTRACTED 1.00]
- **FIX-006 pointer-move monitor lookup path** — docs_v3_additional_fixes_fix_006_single_screen_edge_task_globalmouseinputadapter, docs_v3_additional_fixes_fix_006_single_screen_edge_task_drawingmodeinputgate, docs_v3_additional_fixes_fix_006_single_screen_edge_task_imonitorboundssource, docs_v3_step_5_drawing_style_plan_drawingsessioncontroller, docs_v3_additional_fixes_fix_006_single_screen_edge_task_clamp_to_start_monitor [EXTRACTED 1.00]
- **Glass Theme System** — docs_v4_out_of_scope_feat_011_glass_ui_plan_uithemestate, docs_v4_out_of_scope_feat_011_glass_ui_plan_glass_styles_dictionaries, docs_v4_out_of_scope_feat_011_glass_ui_plan_glassbackdrop, docs_v4_out_of_scope_feat_011_glass_ui_plan_traymenuwindow, docs_v4_out_of_scope_feat_011_glass_ui_task_glass_style [EXTRACTED 1.00]
- **Release Publishing Pipeline** — docs_v4_release_publishing_roadmap_draft_release_tag_versioning, docs_v4_release_publishing_roadmap_checksums_and_provenance, docs_v4_release_publishing_roadmap_conditional_signpath_signing, docs_v4_release_publishing_roadmap_velopack_installer_updates, docs_v4_release_publishing_roadmap_consent_based_update_ux, docs_v4_release_publishing_roadmap_winget_distribution [EXTRACTED 1.00]
- **Stage 1 shortcut-to-playback pipeline** — docs_v2_stage_1_step_3_sound_shortcuts_plan_globalshortcutadapter, docs_v2_stage_1_step_2_sound_channel_review_soundchannelhost, docs_v2_stage_1_step_2_sound_channel_review_soundchannelcontroller, docs_v2_stage_1_step_2_sound_channel_review_isoundplayer, docs_v2_stage_1_step_2_sound_channel_review_mediasoundplayer, docs_v2_stage_1_step_2_sound_channel_review_loggingsoundfailurereporter [EXTRACTED 1.00]
- **Consent-based tray update flow** — docs_v4_release_publishing_step_07_tray_updates_plan_update_controller, docs_v4_release_publishing_step_07_tray_updates_plan_iupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_velopackupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_itrayhost_update_requested, docs_v4_release_publishing_step_07_tray_updates_plan_consent_before_install [EXTRACTED 1.00]
- **v1 smoke test and acceptance flow** — docs_v1_smoke_test_smoke_test, docs_v1_steps_s07_acceptance_pass_plan_acceptance_pass_plan, docs_v1_s07_acceptance_results_acceptance_results, docs_v1_steps_s06_lifecycle_and_packaging_plan_lifecycle_packaging_plan [EXTRACTED 1.00]
- **v3 numbered settings steps 1-6** — docs_v3_step_1_settings_task, docs_v3_step_2_sound_library_task, docs_v3_step_3_action_routing_task, docs_v3_step_4_shortcut_capture_task, docs_v3_step_5_drawing_style_task, docs_v3_step_6_runtime_settings_task [EXTRACTED 1.00]
- **Effect Shortcut-to-Render Pipeline** — docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_globalshortcutadapter, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_ieffectstartqueue, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_effectmonitorguard, docs_v4_visual_effects_step_02_effect_channel_acceptance_effectchannelcontroller, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieeffectsurface, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieplayback [INFERRED 0.85]
- **Release integrity chain for unsigned builds** — _github_workflows_release_release_workflow, _github_workflows_release_sha256sums_manifest, _github_workflows_release_build_attestation, readme_verify_download, docs_adr_0002_release_signing_license_release_signing_license [INFERRED 0.85]
- **Release download verification chain (manifest, attestation, signing)** — tools_release_write_sha256sums, docs_v4_release_publishing_step_02_checksums_provenance_plan_sha256sums_manifest, docs_v4_release_publishing_step_02_checksums_provenance_plan_build_provenance_attestation, docs_v4_release_publishing_step_04_signpath_signing_task_signpath_foundation_signing [INFERRED 0.85]
- **S4-03.1 Performance Gate Decisions** — docs_v4_visual_effects_step_03_built_in_effects_acceptance_s4_03_1_speed_gate, docs_v4_visual_effects_step_03_built_in_effects_acceptance_monitor_half_resolution_fallback, docs_v4_visual_effects_step_03_built_in_effects_acceptance_target_revision_frame_interval_25ms, docs_v4_visual_effects_step_01_effect_surface_plan_owner_exception_wpf_effect_surface, docs_v4_visual_effects_step_03_built_in_effects_plan_skottie_writeablebitmap_render_path [INFERRED 0.85]
- **Settings Draft-to-Active Save Flow** — docs_v3_step_7_settings_window_task_draft_settings, docs_v3_step_6_runtime_settings_task_save_operation, docs_v3_step_6_runtime_settings_task_active_settings_snapshot, docs_v3_step_7_settings_window_plan_shortcut_conflict_validation [INFERRED 0.85]
- **Sound playback stack** — src_drawem_app_infrastructure_sound_readme_globalshortcutadapter, src_drawem_app_infrastructure_sound_readme_soundchannelhost, src_drawem_app_application_sound_readme_soundchannelcontroller, src_drawem_app_application_sound_readme_isoundplayerfactory, src_drawem_app_infrastructure_sound_readme_mediasoundplayerfactory, src_drawem_app_infrastructure_sound_readme_loggingsoundfailurereporter, src_drawem_app_infrastructure_sound_readme_soundfailurelog [INFERRED 0.85]
- **External call microphone routing setup** — docs_v2_stage_1_step_5_call_microphone_plan_voicemeeter_standard, docs_v2_stage_1_step_5_call_microphone_plan_bus_a_b_routing, docs_v2_stage_1_step_5_call_microphone_plan_google_meet, docs_v2_stage_1_step_5_call_microphone_task_meet_noise_cancellation_limitation, docs_v2_stage_1_step_5_call_microphone_plan_per_app_output_routing [INFERRED 0.85]
- **Generation/Epoch Guards Against Stale Effect Work** — docs_v4_visual_effects_step_02_effect_channel_plan_instance_bound_callbacks, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_configurationgeneration, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_monitor_epoch, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_stopstartedbefore [INFERRED 0.85]
- **v3 Save flow: persist, stop sound, exit drawing, clear, publish** — docs_v3_step_6_runtime_settings_plan_save_operation, docs_v3_step_6_runtime_settings_plan_active_settings_snapshot, docs_v3_step_6_runtime_settings_acceptance_sound_start_lock, docs_v3_step_6_runtime_settings_plan_media_retire_after_agree, docs_v3_step_3_action_routing_plan_soundchannelhost [INFERRED 0.85]
- **v3 shortcut input safety rules** — docs_v3_roadmap_v3_right_alt_reserved_for_altgr, docs_v3_roadmap_v3_neutral_key_injection, docs_v3_roadmap_v3_shortcut_capture_mode [INFERRED 0.85]

## Communities (134 total, 14 thin omitted)

### Community 0 - "Fixture"
Cohesion: 0.11
Nodes (19): DrawEM.Tests.Application.Sound, SoundId, Value, FakePlayer, Calls, DisposeFailure, IsDisposed, Sound (+11 more)

### Community 1 - "JsonSettingsStoreTests"
Cohesion: 0.10
Nodes (7): SettingsStoreException, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 2 - "StrokeRenderElement"
Cohesion: 0.07
Nodes (27): Out of scope, Per stroke, Previous behavior, Required behavior, Settings file, Settings window, Smoothing strategy, v4 / FEAT-012: smooth strokes with Bézier curves (+19 more)

### Community 3 - "GlobalShortcutAdapterTests"
Cohesion: 0.16
Nodes (9): GlobalShortcutAdapterTests, SoundTestHarness, Controller, Gate, Monitors, Played, Queued, SoundQueued (+1 more)

### Community 4 - "EffectCatalog / BuiltInEffect"
Cohesion: 0.21
Nodes (16): BuiltInEffectAnimations, Confetti Effect (Monitor), Focus Ring Effect (Cursor), Lottie Simple License, S4-04 Acceptance Criteria, EffectAction, EffectCatalog / BuiltInEffect, SettingsSnapshot.Validate (UnknownEffect, MissingEffect) (+8 more)

### Community 5 - "Stage 1 Sound Implementation Plan"
Cohesion: 0.24
Nodes (20): Step 7 acceptance results (conditional pass), drawEM manual smoke test, Step 7 acceptance pass plan, drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, Ctrl+Alt+1..8 Sound Shortcut Slots, Stage 1 Acceptance Results: Sound Command, Stage 1 Sound Implementation Plan (+12 more)

### Community 6 - "drawEM README"
Cohesion: 0.14
Nodes (20): CI workflow (build, test, coverage, publish, Velopack smoke), Velopack packaging smoke step, Build provenance attestation, Release workflow (tag -> draft GitHub release), SHA256SUMS.txt download manifest, Exit and shutdown order, TrayApplication lifecycle owner, Glass theme tokens (dark/light, OrangeRed accent) (+12 more)

### Community 7 - "Window"
Cohesion: 0.04
Nodes (56): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+48 more)

### Community 8 - "App.xaml.cs"
Cohesion: 0.07
Nodes (7): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Settings, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray

### Community 9 - "SettingsViewModel"
Cohesion: 0.05
Nodes (28): IShortcutCapture, BindableBase, ISoundFilePicker, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear (+20 more)

### Community 10 - "Harness"
Cohesion: 0.08
Nodes (12): FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate, Log, NeutralKeys, Played (+4 more)

### Community 11 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 12 - "DrawEM.App.Domain.Settings"
Cohesion: 0.12
Nodes (8): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings, DrawEM.Tests.Domain.Settings

### Community 13 - "TrayApplication"
Cohesion: 0.12
Nodes (10): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+2 more)

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.06
Nodes (26): Invalid, NotSaved, Saved, SettingsSaveResult, EffectSample, Effect, FakeCapture, Current (+18 more)

### Community 15 - "SaveHarness"
Cohesion: 0.08
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 16 - "EffectId"
Cohesion: 0.11
Nodes (17): EffectId, Value, ActionSlot, Action, IsEmpty, Number, EffectAction, Effect (+9 more)

### Community 18 - "ISoundPlayer"
Cohesion: 0.08
Nodes (11): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, ThreadRecordingPlayer, IsDisposed, PlayApartment (+3 more)

### Community 19 - "LoggingSoundFailureReporter"
Cohesion: 0.14
Nodes (5): BackgroundLogWriter, SettingsFailureDialog, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 20 - "SlotViewModel"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 22 - "SettingsEditor"
Cohesion: 0.10
Nodes (8): IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width

### Community 23 - "MonitorBounds"
Cohesion: 0.10
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, EffectSurfaceWindow, Layout, CountingMonitorBoundsSource, Queries (+2 more)

### Community 24 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (11): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+3 more)

### Community 25 - "ManualTimeProvider"
Cohesion: 0.12
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 26 - "Fixture"
Cohesion: 0.12
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 27 - "EffectInstance"
Cohesion: 0.07
Nodes (30): Run, Deadline, Instance, OnCompleted, OnFailed, Playback, EffectInstance, EffectPlaybackException (+22 more)

### Community 28 - "ScreenPoint"
Cohesion: 0.18
Nodes (9): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, RecoveringCursorPositionSource, FakeOverlayView, LastRenderedState (+1 more)

### Community 29 - "GlobalShortcutAdapter"
Cohesion: 0.10
Nodes (12): Cancelled, Captured, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, GlobalShortcutAdapter (+4 more)

### Community 30 - "ShortcutBindings"
Cohesion: 0.20
Nodes (9): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, SoundBinding (+1 more)

### Community 31 - "SettingsSnapshot"
Cohesion: 0.08
Nodes (22): ISoundLibrary, SoundCleanupFailure, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut, Slots (+14 more)

### Community 32 - "SoundReference"
Cohesion: 0.11
Nodes (18): ActiveSettings, Current, SoundReference, DisplayName, LibraryFileName, ActiveSettingsTests, IdleShortcuts, NoSettingsFailures (+10 more)

### Community 33 - "Fixture"
Cohesion: 0.09
Nodes (14): SettableCursor, Available, Position, Reads, EffectMonitorGuardTests, Fixture, Channel, Cursor (+6 more)

### Community 34 - "Fixture"
Cohesion: 0.09
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 35 - "IntPtr"
Cohesion: 0.05
Nodes (23): IMouseHookSource, MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32CursorPositionSource, Win32MonitorBoundsSource (+15 more)

### Community 36 - "EffectChannelController"
Cohesion: 0.16
Nodes (6): EffectChannelController, ActiveInstance, EffectInstanceId, IEffectFailureReporter, EffectSampleFailureRouter, EffectSampler

### Community 37 - "NotifyIconTrayHost"
Cohesion: 0.13
Nodes (7): ITrayMenu, NotifyIconTrayHost, IconUri, FakeTrayMenu, Shown, Updates, NotifyIconTrayHostTests

### Community 38 - "SettingsWindow"
Cohesion: 0.10
Nodes (7): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, SettingsWindow, ViewModel, RepeatButton

### Community 40 - ".OnStartup"
Cohesion: 0.15
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 41 - "Fixture"
Cohesion: 0.15
Nodes (8): Fixture, Consent, Controller, Log, Service, Time, WhileConfirming, UpdateControllerTests

### Community 42 - ".Save"
Cohesion: 0.11
Nodes (8): IActiveShortcuts, ISaveSoundChannel, ISettingsSave, ISettingsSaveNotifications, SettingsSaveOperation, IdleSoundChannel, RecordingNotifications, CleanupFailures

### Community 43 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 44 - "FakeMonitorBoundsSource"
Cohesion: 0.26
Nodes (7): GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource, GlobalMouseInputAdapterTests

### Community 45 - "Rig"
Cohesion: 0.11
Nodes (17): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+9 more)

### Community 46 - "SoundChannelController"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 47 - "Harness"
Cohesion: 0.10
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 48 - "DrawEM.App.Application.Sound"
Cohesion: 0.10
Nodes (16): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1 (+8 more)

### Community 49 - "Action"
Cohesion: 0.10
Nodes (12): PlaySoundCommand, SampleFailureRouter, SoundSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe (+4 more)

### Community 50 - "DrawEM.App.Infrastructure"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 51 - "R4-01..R4-07 Step Sequence"
Cohesion: 0.18
Nodes (3): release.yml Release Workflow, R4-01 Acceptance Criteria, ReleaseScriptsTests

### Community 52 - ".Digit"
Cohesion: 0.12
Nodes (16): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+8 more)

### Community 53 - "EffectLifecycleTests"
Cohesion: 0.36
Nodes (3): EffectLifecycleTests, Events, Interruptions

### Community 54 - "v3 Step 8: Publish and Verify on Windows"
Cohesion: 0.13
Nodes (18): Step 3 Acceptance, Step 3 Plan, GlobalShortcutAdapter, SoundChannelHost, Step 3: Configurable Action Shortcuts, In-Memory Binding Snapshot, Step 4 Acceptance, Step 4 Plan (+10 more)

### Community 55 - "SingleInstanceGuard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 56 - "RuntimeSettingsTests"
Cohesion: 0.13
Nodes (10): FakePlayer, Calls, Path, NoFailures, NoSettingsFailures, PlayerFactory, Created, RuntimeSettingsTests (+2 more)

### Community 57 - "DrawingModeInputGate"
Cohesion: 0.13
Nodes (12): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch (+4 more)

### Community 58 - "TrayMenuWindow"
Cohesion: 0.14
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 59 - "SamplingEditor"
Cohesion: 0.50
Nodes (4): SamplingEditor, Failures, RunningStates, Sampler

### Community 60 - "DrawEM.App.Application.Updates"
Cohesion: 0.15
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 61 - "Lottie Simple License (FL 9.13.21)"
Cohesion: 0.13
Nodes (27): EffectCatalog, ISettingsStore, JsonSettingsStore, Settings Storage README, Settings Schema Version 3, SettingsSnapshot, SettingsStoreException, Design Barn Inc. (+19 more)

### Community 62 - ".Attach"
Cohesion: 0.22
Nodes (5): GlassBackdrop, Supported, UiTheme, Dark, Light

### Community 63 - "FakeUpdateService"
Cohesion: 0.13
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 64 - "DrawEM.Tests.csproj"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 65 - ".Load"
Cohesion: 0.14
Nodes (13): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, StartupSettings (+5 more)

### Community 66 - "ResourceDictionary"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 67 - "drawEM v4 Roadmap: Monitor and Cursor Effects"
Cohesion: 0.15
Nodes (20): drawEM v4 Roadmap: Monitor and Cursor Effects, ADR 0001: Lottie built-in effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), EffectChannelController, EffectAction, EffectMonitorGuard (+12 more)

### Community 68 - "Sound Infrastructure README"
Cohesion: 0.19
Nodes (19): EffectFailureLog, LoggingEffectFailureReporter, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory, PlaySoundCommand, Sound Application README (+11 more)

### Community 69 - "DateTimeOffset"
Cohesion: 0.24
Nodes (5): LoggingUpdateFailureReporter, UpdateFailure, UpdateFailureLog, DefaultPath, UpdateFailureLogTests

### Community 70 - "List"
Cohesion: 0.25
Nodes (5): FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 71 - "v3 Step 6: Apply Saved Settings at Runtime"
Cohesion: 0.16
Nodes (15): Step 5 Acceptance, SettingsFailureDialog, Step 5 Plan, DrawingSessionController, StrokeRenderElement, Step 5: Configurable Drawing Style, Stroke Width in Physical Pixels, Step 6 Acceptance (+7 more)

### Community 72 - "drawEM architecture"
Cohesion: 0.23
Nodes (15): drawEM architecture, App.xaml.cs composition root, DrawingModeInputGate, DrawingSessionController, GlobalShortcutAdapter, SettingsSaveOperation save flow, SoundChannelHost and sound thread, StrokeRenderElement incremental rendering (+7 more)

### Community 74 - "IDisposable"
Cohesion: 0.24
Nodes (4): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable

### Community 75 - "Step 2 Implementation Review"
Cohesion: 0.19
Nodes (10): Sound Command Business Test Scenarios, Ignore Stale Finish Scenario, %LOCALAPPDATA%\drawEM\logs\sound.log, SoundFailureLog, Step 2 Implementation Review, ISoundPlayer Playback Port, LoggingSoundFailureReporter, MediaSoundPlayer (+2 more)

### Community 76 - "UiThemeState"
Cohesion: 0.24
Nodes (3): UiThemeState, Current, TrayMenuWindowTests

### Community 77 - "Step 1: Settings Snapshot and Storage"
Cohesion: 0.18
Nodes (14): Step 1 Acceptance, Settings Schema/Version Policy, v3 Settings Defaults, Step 1 Plan, Step 1: Settings Snapshot and Storage, Eight Numbered Action Slots, Immutable Settings Snapshot, Step 2 Acceptance (+6 more)

### Community 78 - "R4 Release Publishing Steps"
Cohesion: 0.15
Nodes (27): R4 Release Publishing Roadmap, R4-01 Implementation Plan, R4-01 Task: Versioned Draft Release, v4 Release Publishing Roadmap, R4-02 Acceptance: Checksums and Provenance, R4-02 Plan: Checksums and Provenance, GitHub Build Provenance Attestation (actions/attest), SHA256SUMS.txt Manifest (+19 more)

### Community 79 - "EffectSurfaceLayout"
Cohesion: 0.24
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 80 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 81 - "LottiePlayback"
Cohesion: 0.17
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 82 - "Harness"
Cohesion: 0.07
Nodes (16): EffectStopRoutes, ISessionInterruptionSource, Win32DisplayChangeSource, Win32SessionInterruptionSource, Harness, Controller, Displays, EffectChannel (+8 more)

### Community 83 - "DrawEM.App.Presentation.Effects"
Cohesion: 0.24
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 84 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.21
Nodes (13): FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR Override, tools/E2ERunner, E2ERunner.Tests Catalog Drift Check, SendInput Scenarios with Pixel Sampling Verdicts, FIX-006: Keep Drawing Active at Single Screen Edge, DrawingModeInputGate.StopAtBoundary, GlobalMouseInputAdapter.OnPointerMoved (+5 more)

### Community 86 - "v3 Step 7: Settings Window"
Cohesion: 0.33
Nodes (6): v3 Step 7 Acceptance Criteria, SettingsEditorTests / SettingsViewModelTests / SoundSamplerTests, v3 Step 7 Implementation Plan, drawEM Settings Prototype (HTML), Settings Prototype UI (Drawing: Stroke, Shortcuts), v3 Step 7: Settings Window

### Community 87 - "S4-07 Acceptance Criteria"
Cohesion: 0.24
Nodes (13): S4-06 Acceptance Criteria, GlobalShortcutAdapter, ShortcutBindings.FromSnapshot, S4-06 Implementation Plan, S4-06: Launch Effects Through Shortcuts, S4-07 Acceptance Criteria, EffectMonitorGuard, IEffectStartQueue (+5 more)

### Community 88 - "JsonSettingsStore"
Cohesion: 0.23
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 89 - "FakeSoundChannel"
Cohesion: 0.15
Nodes (4): ISoundStartHold, StartHold, FakeSoundChannel, StopConfirmed

### Community 90 - "App"
Cohesion: 0.10
Nodes (3): Application, App, WpfApplicationLifetime

### Community 92 - "R4-07 Plan: Tray Updates"
Cohesion: 0.30
Nodes (9): R4-07 Acceptance: Tray Updates, R4-07 Plan: Tray Updates, DRAWEM_UPDATE_FEED local feed override, ITrayHost UpdateRequested / ShowUpdateReady, IUpdateService port, LayerDependencyTests, UpdateController, update.log (UpdateFailureLog) (+1 more)

### Community 93 - "BuiltInEffectAnimations.cs"
Cohesion: 0.15
Nodes (6): EffectCatalog, BuiltIn, ShortcutKeyKind, Digit, Function, Letter

### Community 94 - "S4-09 Preview Draft Effects (acceptance)"
Cohesion: 0.22
Nodes (13): S4-09 Preview Draft Effects (acceptance), EffectChannelController, EffectSampleFailureRouter, EffectSampler, IEffectSampler, SettingsEditor, S4-09 Preview Draft Effects (plan), S4-09 Preview Draft Effects (task) (+5 more)

### Community 95 - "BuiltInEffectAnimations"
Cohesion: 0.29
Nodes (3): BuiltInEffectAnimations, BuiltInEffectAnimationsTests, BuiltInIds

### Community 96 - "LayerDependencyTests.cs"
Cohesion: 0.12
Nodes (4): DrawEM.Tests.Release, DrawEM.Tests.Architecture, NativeFunctionBridgeRule, VelopackHookExitTests

### Community 97 - "S4-11 Stop Effects Through App Lifecycle (acceptance)"
Cohesion: 0.26
Nodes (11): drawEM Prototype Tray Icon, EffectMonitorGuard, S4-11 Stop Effects Through App Lifecycle (acceptance), EffectMonitorGuard.StopEffects(), EffectStopRoutes, TrayApplication, Win32DisplayChangeSource, Win32SessionInterruptionSource (+3 more)

### Community 98 - "FEAT-011 Implementation Plan"
Cohesion: 0.39
Nodes (8): FEAT-011 Acceptance Criteria, FEAT-011 Implementation Plan, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, FEAT-011: Glass Design for Settings and Tray Menu

### Community 99 - "StartEffectCommand"
Cohesion: 0.14
Nodes (6): StartEffectCommand, EffectPlacement, Cursor, Monitor, EffectMonitorGuard, IEffectStartQueue

### Community 100 - "VelopackUpdateService"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 101 - "FEAT-009: Effect Loop and Length Limit"
Cohesion: 0.40
Nodes (6): FEAT-009 Acceptance Criteria, FEAT-009 Implementation Plan, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, FEAT-009: Effect Loop and Length Limit

### Community 102 - "LoggingEffectFailureReporter"
Cohesion: 0.26
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, LoggingEffectFailureReporter, EffectFailureLogTests

### Community 103 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 104 - "SoundFailure"
Cohesion: 0.28
Nodes (5): SoundFailure, SoundFailureLog, DefaultPath, TextFileLogSink, SoundFailureLogTests

### Community 105 - "drawEM v4: Out of Scope"
Cohesion: 0.20
Nodes (8): Dependabot config (nuget, github-actions, FIX prefix), PR lint workflow (branch name and title check), drawEM agent rules, v4 task folder structure (task.md, plan.md, acceptance.md), Evidence, v4 / FEAT-012: acceptance criteria, v4 / FEAT-012: implementation plan, drawEM v4: Out of Scope

### Community 107 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.19
Nodes (9): Step 1 Implementation Review, PlaySoundCommand, SoundAssignments.Slots, SoundConfiguration (Resolve/PathOf), SoundId, drawEM v3 Roadmap: Drawing and Action Settings, Save Applies Validated Snapshot, Settings Window (Drawing/Actions tabs) (+1 more)

### Community 108 - "Func"
Cohesion: 0.24
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 109 - "WidthBox"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 110 - "FakePlayer"
Cohesion: 0.18
Nodes (7): FakePlayer, Calls, Sound, FakePlayerFactory, Created, OnStop, RecordingSoundFailures

### Community 111 - "IUpdateFailureReporter"
Cohesion: 0.21
Nodes (5): IUpdateFailureReporter, FakeFailureReporter, RecordingReporter, Lines, UpdateFeedTests

### Community 112 - ".Default_FollowsTheWindowsAppTheme"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 113 - "S4-01 Acceptance: Effect Surface Probe"
Cohesion: 0.24
Nodes (8): EffectSurfaceWindow (nonactivating click-through), S4-01 Acceptance: Effect Surface Probe, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), EffectSurfaceProbe (DRAWEM_EFFECT_PROBE trigger), S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window

### Community 114 - "S4-12 Publish and Verify on Windows (acceptance)"
Cohesion: 0.38
Nodes (6): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Build Identity (DrawEM.App.exe 1.0.0+335de88), Published-app Checks, S4-12 Publish and Verify on Windows (plan), S4-12 Publish and Verify on Windows (task)

### Community 115 - "R4-06 Plan: Velopack Installer"
Cohesion: 0.20
Nodes (9): SmartScreen 'Windows protected your PC' warning, SignPath Foundation Code Signing, WinGet zip/nested-portable manifest, R4-06 Acceptance: Velopack Installer, R4-06 Plan: Velopack Installer, CI Velopack packaging smoke, Custom WPF Program.Main with VelopackApp.Run, %LOCALAPPDATA%\drawEM settings directory (+1 more)

### Community 116 - "S4-03 Acceptance Criteria"
Cohesion: 0.26
Nodes (9): S4-03 Acceptance Criteria, EffectFailureLog (effect.log), EffectRenderLayout, Flashing Check (max three flashes per second), LottieEffectSurface, LottiePlayback, S4-03.1 Speed Gate (Lottie benchmark), S4-03 Implementation Plan (+1 more)

### Community 118 - "drawEM v4: Parallel Work Packages"
Cohesion: 0.36
Nodes (7): R4-04 signing step (blocked), ADR-0002: Releases may start unsigned; signing depends on license and SignPath, SignPath Foundation signing, drawEM v4: Parallel Work Packages, drawEM v4: Release Publishing, ADR-0002 Release Signing License, TODO: Release-Publishing Grooming

### Community 119 - "drawEM domain glossary"
Cohesion: 0.43
Nodes (8): Built-in effect, Effect, drawEM domain glossary, Imported effect, Placement (Monitor / Cursor), ADR-0001: Built-in effects are Lottie files with fixed placement, SkiaSharp.Skottie Lottie player, Proposed screen actions model (actions, commands, slots, channels)

### Community 120 - "SettingsCommand"
Cohesion: 0.21
Nodes (7): SettingsCommand, Clear, Draw, SettingsCommandKind, Clear, Draw, Slot

### Community 125 - "Step 5 Plan: External Microphone Mixing"
Cohesion: 0.46
Nodes (6): Step 5 Acceptance: Call Microphone Routing, Bus A/B Routing (mic to B, drawEM to A+B), Step 5 Plan: External Microphone Mixing, Google Meet, Voicemeeter Standard External Mixer, Step 5 Task: Route Sound into Call Microphone

### Community 126 - "EffectChannelController"
Cohesion: 0.39
Nodes (7): S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port, Effect Invocation (instance ID, generation, placement), S4-02 Implementation Plan, Ten-Second Effect Maximum, S4-02: Control One Running Effect

### Community 127 - "S4-08 Acceptance Criteria"
Cohesion: 0.39
Nodes (8): ManagedSoundLibrary Reference Accounting, S4-08 Acceptance Criteria, Default Shortcut Proposal Ctrl+Alt+N, SettingsEditor.SelectEffect / ClearSlot, SettingsViewModel EffectOption and SlotViewModel, SettingsWindow Sound/Effect Type Selector, S4-08 Implementation Plan, S4-08 Choose Effects in Settings (task)

### Community 128 - "GlassDark"
Cohesion: 0.25
Nodes (5): ResourceDictionary, GlassDark, ResourceDictionary, GlassLight, GlassStyles

### Community 129 - "Step 3 Plan: Sound Shortcuts"
Cohesion: 0.43
Nodes (6): SoundChannelHost, Step 3 Plan: Sound Shortcuts, GlobalShortcutAdapter, IKeyboardHookSource, VirtualKeys, App.xaml.cs Composition Root

### Community 132 - "TrayMenuCommand"
Cohesion: 0.33
Nodes (5): TrayMenuCommand, Exit, Settings, StopEffects, Update

### Community 133 - "FakePlayback"
Cohesion: 0.33
Nodes (6): FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

## Knowledge Gaps
- **496 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+491 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 714 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **14 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `v4 / FEAT-012: implementation plan` connect `drawEM v4: Out of Scope` to `SettingsViewModel`, `StrokeRenderElement`, `SettingsEditor`?**
  _High betweenness centrality (0.257) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _496 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Fixture` be split into smaller, more focused modules?**
  _Cohesion score 0.1054421768707483 - nodes in this community are weakly interconnected._
- **Why does `drawEM v4: Out of Scope` connect `drawEM v4: Out of Scope` to `FEAT-011 Implementation Plan`, `drawEM v4 Roadmap: Monitor and Cursor Effects`, `FEAT-009: Effect Loop and Length Limit`, `R4-01..R4-07 Step Sequence`, `R4-06 Plan: Velopack Installer`, `drawEM v4: Parallel Work Packages`, `R4-07 Plan: Tray Updates`?**
  _High betweenness centrality (0.243) - this node is a cross-community bridge._
- **Should `JsonSettingsStoreTests` be split into smaller, more focused modules?**
  _Cohesion score 0.10437158469945355 - nodes in this community are weakly interconnected._
- **Why does `drawEM v4 Roadmap: Monitor and Cursor Effects` connect `drawEM v4 Roadmap: Monitor and Cursor Effects` to `S4-11 Stop Effects Through App Lifecycle (acceptance)`, `EffectCatalog / BuiltInEffect`, `FEAT-009: Effect Loop and Length Limit`, `Stage 1 Sound Implementation Plan`, `drawEM README`, `drawEM architecture`, `drawEM v4: Out of Scope`, `drawEM v3 Roadmap: Drawing and Action Settings`, `S4-01 Acceptance: Effect Surface Probe`, `S4-12 Publish and Verify on Windows (acceptance)`, `S4-03 Acceptance Criteria`, `S4-09 Preview Draft Effects (acceptance)`, `drawEM v4: Parallel Work Packages`, `drawEM domain glossary`, `v3 Step 8: Publish and Verify on Windows`, `S4-07 Acceptance Criteria`, `EffectChannelController`, `S4-08 Acceptance Criteria`?**
  _High betweenness centrality (0.143) - this node is a cross-community bridge._