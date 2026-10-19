# Graph Report - worktree-new-design  (2026-10-19)

## Corpus Check
- 383 files · ~166,235 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3209 nodes · 9344 edges · 125 communities (108 shown, 17 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1583 edges (avg confidence: 0.85)
- Token cost: 675,325 input · 0 output

## Community Hubs (Navigation)
- Sound and Effect Value Types
- Settings Store Load Save
- Stroke Rendering Visuals
- Shortcut Test Fakes
- Effect Surface Docs (S4-01/02)
- v2 Sound Actions Roadmap
- CI and Release Workflows
- Settings ViewModel Commands
- Presentation Namespaces
- Shortcut Capture Interface
- Shortcut Capture Results
- JSON Settings Store
- Settings Namespaces
- Tray App Lifetime
- WPF Control Types
- Settings Test Fakes
- Effect Ids and Actions
- Settings Editor Sound Slots
- Sound Player
- Background Log Writer
- Effect Slot Options
- Settings ViewModel Tests
- Settings Editor
- Monitor Bounds
- Drawing Namespaces
- Sound Channel Host
- Effect Channel Tests
- Effect Surface Playback
- Drawing Session Controller
- Global Shortcut Adapter
- Sound Library Import
- Sound Library Save Cleanup
- Active Settings Publish
- Effect Placement Monitors
- Effect Sampler
- Win32 Monitor Interop
- Effect Channel Controller
- Tray Icon and Menu Port
- Settings Window
- Layer Dependency Rules
- App Failure Log
- Update Controller Tests
- Save Shortcut Interruption
- Release Script Tests
- Drawing Input Gate
- Stuck Save Recovery
- Sound Failure Reporting
- Effect Shortcut Tests
- Sound Infrastructure
- Sample Failure Routing
- Infrastructure Lifecycle
- v4 Work Packages Docs
- Shortcut Parsing Errors
- Effect Catalog
- v3 Action Shortcuts Docs
- App Startup and Exit
- Sound Player Fakes
- Win32 Input Hooks
- Glass Tray Menu Window
- Effect Sampling Editor
- DrawEM.Tests.Application.Updates Group
- EffectCatalog Group
- GlassBackdrop Group
- CancellationToken Group
- DrawEM.sln Group
- Action Group
- IsDropDownOpen Group
- drawEM v4 Roadmap: Monitor and Cursor Effects Group
- EffectFailureLog Group
- GeneratedRegex Group
- RunResult Group
- Step 5 Acceptance Group
- DpiScale Group
- JsonElement Group
- AggregateException Group
- ArgumentException Group
- Button Group
- Step 1 Acceptance Group
- R4-01 Task: Versioned Draft Release Group
- EffectSurfaceLayout Group
- BlockingPlayerFactory Group
- EventArgs Group
- Harness Group
- DrawEM.App.Presentation.Effects Group
- FEAT-005: Assisted E2E Runner Group
- RenderLagHarness Group
- v3 Step 6: Apply Saved Settings at Runtime Group
- IEnumerable Group
- PowerModeChangedEventArgs Group
- Queue Group
- .ReleaseStartedResources() Group
- CancellationTokenSource Group
- Custom WPF Program.Main with VelopackApp.Run Group
- skiasharp_resources Group
- S4-08 Choose Effects in Settings (task) Group
- Animation Group
- DrawEM.Tests.Architecture Group
- drawEM Prototype Tray Icon Group
- FEAT-011 Acceptance Criteria Group
- .StopStartedBefore() Group
- VelopackUpdateService Group
- FEAT-009 Acceptance Criteria Group
- R4-03 Acceptance: First Release Group
- SettingsErrorCode Group
- ShortcutKey Group
- SoundSlot.cs Group
- AssemblyMetadataAttribute Group
- R4-02 Acceptance: Checksums and Provenance Group
- SettingsFailureDialog Group
- HexText Group
- S4-10 Apply Settings Safely (acceptance) Group
- GithubSource Group
- DrawEM.Tests.Presentation.Theme Group
- EffectSurfaceWindow (nonactivating click-through) Group
- S4-12 Publish and Verify on Windows (acceptance) Group
- SignPath Foundation Code Signing Group
- Durable Settings Write Group
- ISoundFilePicker Group
- CanDecreaseWidth Group
- SettingsCommandKind Group
- DrawEM.App.Presentation.Sound Group
- Sound Domain README Group
- graphify knowledge-graph usage rules Group
- Sound Presentation Placeholder Group

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
- `SoundChannelController` --semantically_similar_to--> `EffectChannelController`  [INFERRED] [semantically similar]
  src/DrawEM.App/Application/Sound/README.md → docs/v4/visual-effects/step-09-effect-sample/acceptance.md
- `drawEM App Icon` --conceptually_related_to--> `TrayApplication`  [INFERRED]
  src/DrawEM.App/Assets/drawEM.svg → docs/v4/visual-effects/step-11-effect-lifecycle/acceptance.md
- `LoggingSoundFailureReporter` --semantically_similar_to--> `LoggingEffectFailureReporter`  [INFERRED] [semantically similar]
  src/DrawEM.App/Infrastructure/Sound/README.md → docs/v4/visual-effects/step-11-effect-lifecycle/acceptance.md
- `SoundFailureLog` --semantically_similar_to--> `EffectFailureLog`  [INFERRED] [semantically similar]
  src/DrawEM.App/Infrastructure/Sound/README.md → docs/v4/visual-effects/step-11-effect-lifecycle/acceptance.md
- `drawEM Prototype Tray Icon` --semantically_similar_to--> `drawEM App Icon`  [INFERRED] [semantically similar]
  docs/v3/step-7-settings-window/prototype-tray-icon.svg → src/DrawEM.App/Assets/drawEM.svg

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Release integrity chain for unsigned builds** — _github_workflows_release_release_workflow, _github_workflows_release_sha256sums_manifest, _github_workflows_release_build_attestation, readme_verify_download, docs_adr_0002_release_signing_license_release_signing_license [INFERRED 0.85]
- **F03 render-lag investigation and fix** — docs_v1_steps_f03_render_lag_investigation_plan_investigation_plan, docs_v1_steps_f03_render_lag_harness_results_harness_results, docs_v1_steps_f03_render_lag_harness_results_renderlagharness, docs_v1_steps_f03_render_lag_investigation_plan_hypothesis_full_redraw, docs_architecture_strokerenderelement, docs_v1_steps_f03_render_lag_harness_results_drawingstate_generation [EXTRACTED 1.00]
- **v1 smoke test and acceptance flow** — docs_v1_smoke_test_smoke_test, docs_v1_steps_s07_acceptance_pass_plan_acceptance_pass_plan, docs_v1_s07_acceptance_results_acceptance_results, docs_v1_steps_s06_lifecycle_and_packaging_plan_lifecycle_packaging_plan [EXTRACTED 1.00]
- **Stage 1 shortcut-to-playback pipeline** — docs_v2_stage_1_step_3_sound_shortcuts_plan_globalshortcutadapter, docs_v2_stage_1_step_2_sound_channel_review_soundchannelhost, docs_v2_stage_1_step_2_sound_channel_review_soundchannelcontroller, docs_v2_stage_1_step_2_sound_channel_review_isoundplayer, docs_v2_stage_1_step_2_sound_channel_review_mediasoundplayer, docs_v2_stage_1_step_2_sound_channel_review_loggingsoundfailurereporter [EXTRACTED 1.00]
- **External call microphone routing setup** — docs_v2_stage_1_step_5_call_microphone_plan_voicemeeter_standard, docs_v2_stage_1_step_5_call_microphone_plan_bus_a_b_routing, docs_v2_stage_1_step_5_call_microphone_plan_google_meet, docs_v2_stage_1_step_5_call_microphone_task_meet_noise_cancellation_limitation, docs_v2_stage_1_step_5_call_microphone_plan_per_app_output_routing [INFERRED 0.85]
- **v3 shortcut input safety rules** — docs_v3_roadmap_v3_right_alt_reserved_for_altgr, docs_v3_roadmap_v3_neutral_key_injection, docs_v3_roadmap_v3_shortcut_capture_mode [INFERRED 0.85]
- **v3 Save flow: persist, stop sound, exit drawing, clear, publish** — docs_v3_step_6_runtime_settings_plan_save_operation, docs_v3_step_6_runtime_settings_plan_active_settings_snapshot, docs_v3_step_6_runtime_settings_acceptance_sound_start_lock, docs_v3_step_6_runtime_settings_plan_media_retire_after_agree, docs_v3_step_3_action_routing_plan_soundchannelhost [INFERRED 0.85]
- **FIX-006 pointer-move monitor lookup path** — docs_v3_additional_fixes_fix_006_single_screen_edge_task_globalmouseinputadapter, docs_v3_additional_fixes_fix_006_single_screen_edge_task_drawingmodeinputgate, docs_v3_additional_fixes_fix_006_single_screen_edge_task_imonitorboundssource, docs_v3_step_5_drawing_style_plan_drawingsessioncontroller, docs_v3_additional_fixes_fix_006_single_screen_edge_task_clamp_to_start_monitor [EXTRACTED 1.00]
- **v3 numbered settings steps 1-6** — docs_v3_step_1_settings_task, docs_v3_step_2_sound_library_task, docs_v3_step_3_action_routing_task, docs_v3_step_4_shortcut_capture_task, docs_v3_step_5_drawing_style_task, docs_v3_step_6_runtime_settings_task [EXTRACTED 1.00]
- **Release Publishing Pipeline** — docs_v4_release_publishing_roadmap_draft_release_tag_versioning, docs_v4_release_publishing_roadmap_checksums_and_provenance, docs_v4_release_publishing_roadmap_conditional_signpath_signing, docs_v4_release_publishing_roadmap_velopack_installer_updates, docs_v4_release_publishing_roadmap_consent_based_update_ux, docs_v4_release_publishing_roadmap_winget_distribution [EXTRACTED 1.00]
- **Settings Draft-to-Active Save Flow** — docs_v3_step_7_settings_window_task_draft_settings, docs_v3_step_6_runtime_settings_task_save_operation, docs_v3_step_6_runtime_settings_task_active_settings_snapshot, docs_v3_step_7_settings_window_plan_shortcut_conflict_validation [INFERRED 0.85]
- **Glass Theme System** — docs_v4_out_of_scope_feat_011_glass_ui_plan_uithemestate, docs_v4_out_of_scope_feat_011_glass_ui_plan_glass_styles_dictionaries, docs_v4_out_of_scope_feat_011_glass_ui_plan_glassbackdrop, docs_v4_out_of_scope_feat_011_glass_ui_plan_traymenuwindow, docs_v4_out_of_scope_feat_011_glass_ui_task_glass_style [EXTRACTED 1.00]
- **Release download verification chain (manifest, attestation, signing)** — tools_release_write_sha256sums, docs_v4_release_publishing_step_02_checksums_provenance_plan_sha256sums_manifest, docs_v4_release_publishing_step_02_checksums_provenance_plan_build_provenance_attestation, docs_v4_release_publishing_step_04_signpath_signing_task_signpath_foundation_signing [INFERRED 0.85]
- **Consent-based tray update flow** — docs_v4_release_publishing_step_07_tray_updates_plan_update_controller, docs_v4_release_publishing_step_07_tray_updates_plan_iupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_velopackupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_itrayhost_update_requested, docs_v4_release_publishing_step_07_tray_updates_plan_consent_before_install [EXTRACTED 1.00]
- **Effect playback pipeline (channel to Lottie surface)** — docs_v4_visual_effects_roadmap_v4_effect_channel_controller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_effect_surface_window, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap, docs_v4_visual_effects_roadmap_v4_placement [EXTRACTED 1.00]
- **Effect Shortcut-to-Render Pipeline** — docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_globalshortcutadapter, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_ieffectstartqueue, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_effectmonitorguard, docs_v4_visual_effects_step_02_effect_channel_acceptance_effectchannelcontroller, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieeffectsurface, docs_v4_visual_effects_step_03_built_in_effects_acceptance_lottieplayback [INFERRED 0.85]
- **Generation/Epoch Guards Against Stale Effect Work** — docs_v4_visual_effects_step_02_effect_channel_plan_instance_bound_callbacks, docs_v4_visual_effects_step_06_effect_shortcuts_acceptance_configurationgeneration, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_monitor_epoch, docs_v4_visual_effects_step_07_effect_monitor_transitions_acceptance_stopstartedbefore [INFERRED 0.85]
- **S4-03.1 Performance Gate Decisions** — docs_v4_visual_effects_step_03_built_in_effects_acceptance_s4_03_1_speed_gate, docs_v4_visual_effects_step_03_built_in_effects_acceptance_monitor_half_resolution_fallback, docs_v4_visual_effects_step_03_built_in_effects_acceptance_target_revision_frame_interval_25ms, docs_v4_visual_effects_step_01_effect_surface_plan_owner_exception_wpf_effect_surface, docs_v4_visual_effects_step_03_built_in_effects_plan_skottie_writeablebitmap_render_path [INFERRED 0.85]
- **Effect stop on lifecycle events via monitor epoch** — docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_effectstoproutes, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_effectmonitorguard_stopeffects, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32sessioninterruptionsource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_win32displaychangesource, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_trayapplication, docs_v4_visual_effects_step_11_effect_lifecycle_acceptance_monitor_epoch [EXTRACTED 1.00]
- **Settings Sample through the shortcut effect path** — docs_v4_visual_effects_step_09_effect_sample_acceptance_settingseditor, docs_v4_visual_effects_step_09_effect_sample_acceptance_ieffectsampler, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectsampler, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectmonitorguard, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectchannelcontroller, docs_v4_visual_effects_step_09_effect_sample_acceptance_effectsamplefailurerouter [EXTRACTED 1.00]
- **Sound playback stack** — src_drawem_app_infrastructure_sound_readme_globalshortcutadapter, src_drawem_app_infrastructure_sound_readme_soundchannelhost, src_drawem_app_application_sound_readme_soundchannelcontroller, src_drawem_app_application_sound_readme_isoundplayerfactory, src_drawem_app_infrastructure_sound_readme_mediasoundplayerfactory, src_drawem_app_infrastructure_sound_readme_loggingsoundfailurereporter, src_drawem_app_infrastructure_sound_readme_soundfailurelog [INFERRED 0.85]

## Communities (125 total, 17 thin omitted)

### Community 0 - "Sound and Effect Value Types"
Cohesion: 0.05
Nodes (32): DrawEM.Tests.Application.Sound, SoundId, Value, EffectFailure, EffectFailureLog, DefaultPath, SoundFailure, SoundFailureLog (+24 more)

### Community 1 - "Settings Store Load Save"
Cohesion: 0.07
Nodes (18): ISettingsStore, Loaded, Missing, SettingsLoadResult, SettingsStoreException, Unreadable, ISettingsFailureReporter, FakeReporter (+10 more)

### Community 2 - "Stroke Rendering Visuals"
Cohesion: 0.08
Nodes (18): DrawingState, Stroke, DrawingStyle, HexColor, StrokeWidth, Pixels, IOverlayView, Window (+10 more)

### Community 3 - "Shortcut Test Fakes"
Cohesion: 0.09
Nodes (18): FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource, FailingCursorPositionSource (+10 more)

### Community 4 - "Effect Surface Docs (S4-01/02)"
Cohesion: 0.05
Nodes (57): EffectSurfaceProbe (DRAWEM_EFFECT_PROBE trigger), S4-01 Implementation Plan, Nonactivating Input-Transparent Effect Window, Renderer Decision, S4-01: Prove the Effect Window, S4-02 Acceptance Criteria, EffectChannelController, IEffectSurface / IEffectPlayback Port (+49 more)

### Community 5 - "v2 Sound Actions Roadmap"
Cohesion: 0.07
Nodes (48): drawEM v2 Roadmap: Screen Actions, Ctrl+Alt+Z Draw / Ctrl+Alt+X Clear Shortcuts, Ctrl+Alt+1..8 Sound Shortcut Slots, Sound Command Business Test Scenarios, Ignore Stale Finish Scenario, Stage 1 Acceptance Results: Sound Command, Stage 1 Sound Implementation Plan, Step 1 Acceptance: Command and Configuration (+40 more)

### Community 6 - "CI and Release Workflows"
Cohesion: 0.06
Nodes (53): Dependabot config (nuget, github-actions, FIX prefix), CI workflow (build, test, coverage, publish, Velopack smoke), Velopack packaging smoke step, PR lint workflow (branch name and title check), Build provenance attestation, Release workflow (tag -> draft GitHub release), SHA256SUMS.txt download manifest, drawEM agent rules (+45 more)

### Community 7 - "Settings ViewModel Commands"
Cohesion: 0.04
Nodes (56): CanDecreaseMaxLength, CanEditMaxLength, CanIncreaseMaxLength, CanSave, CanSetNoLimit, ChooseText, Clear, Color (+48 more)

### Community 8 - "Presentation Namespaces"
Cohesion: 0.06
Nodes (11): DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Presentation.Drawing, DrawEM.App.Presentation.Drawing, DrawEM.App.Presentation.Theme, DrawEM.App.Presentation.Tray, DrawEM.Tests.Presentation.Tray, ResourceDictionary, GlassDark (+3 more)

### Community 9 - "Shortcut Capture Interface"
Cohesion: 0.05
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 10 - "Shortcut Capture Results"
Cohesion: 0.07
Nodes (18): Cancelled, Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness (+10 more)

### Community 11 - "JSON Settings Store"
Cohesion: 0.13
Nodes (8): SoundImportException, JsonSettingsStore, DefaultDirectory, SettingsPath, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 12 - "Settings Namespaces"
Cohesion: 0.11
Nodes (9): DrawEM.App.Domain.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings (+1 more)

### Community 13 - "Tray App Lifetime"
Cohesion: 0.12
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 14 - "WPF Control Types"
Cohesion: 0.09
Nodes (17): EffectSample, Effect, FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples (+9 more)

### Community 15 - "Settings Test Fakes"
Cohesion: 0.10
Nodes (23): FakeStore, Failure, OnSave, Saved, SaveHarness, Channel, Controller, EffectChannel (+15 more)

### Community 16 - "Effect Ids and Actions"
Cohesion: 0.14
Nodes (8): EffectId, Value, EffectAction, Effect, SettingsError, SettingsValidation, IsValid, SettingsSnapshotTests

### Community 17 - "Settings Editor Sound Slots"
Cohesion: 0.15
Nodes (3): SoundAction, Sound, SettingsEditorTests

### Community 18 - "Sound Player"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory, FakePlayer, Calls, Sound (+9 more)

### Community 19 - "Background Log Writer"
Cohesion: 0.10
Nodes (9): BackgroundLogWriter, LoggingSoundFailureReporter, LoggingUpdateFailureReporter, LoggingSoundFailureReporterTests, ManualTimeProvider, ActiveTimerCount, TimestampFrequency, ManualTimer (+1 more)

### Community 20 - "Effect Slot Options"
Cohesion: 0.06
Nodes (32): EffectOption, All, SlotKind, Effect, Sound, SlotViewModel, Action, CanDecreaseMaxLength (+24 more)

### Community 22 - "Settings Editor"
Cohesion: 0.09
Nodes (20): Captured, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots, Width, ISettingsSave (+12 more)

### Community 23 - "Monitor Bounds"
Cohesion: 0.11
Nodes (10): MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, TrayMenuPlacement, CountingMonitorBoundsSource, Queries, EffectCanvasPlacementTests (+2 more)

### Community 24 - "Drawing Namespaces"
Cohesion: 0.13
Nodes (9): DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects, DrawEM.App.Infrastructure.Effects (+1 more)

### Community 25 - "Sound Channel Host"
Cohesion: 0.14
Nodes (12): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+4 more)

### Community 26 - "Effect Channel Tests"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 27 - "Effect Surface Playback"
Cohesion: 0.07
Nodes (23): EffectPlaybackException, IEffectPlayback, IEffectSurface, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler (+15 more)

### Community 28 - "Drawing Session Controller"
Cohesion: 0.20
Nodes (8): DrawingSessionController, CompletedStrokes, ScreenPoint, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 29 - "Global Shortcut Adapter"
Cohesion: 0.09
Nodes (11): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, INeutralKeyEmitter, SettableCursor, Available, Position (+3 more)

### Community 30 - "Sound Library Import"
Cohesion: 0.10
Nodes (17): SoundReference, DisplayName, LibraryFileName, EffectBinding, KeyChord, ShortcutBindings, Clear, Draw (+9 more)

### Community 31 - "Sound Library Save Cleanup"
Cohesion: 0.11
Nodes (20): ISoundLibrary, SoundCleanupFailure, SettingsStartup, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects (+12 more)

### Community 32 - "Active Settings Publish"
Cohesion: 0.14
Nodes (14): ActiveSettings, Current, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory, SettingsDirectory, SourceDirectory (+6 more)

### Community 33 - "Effect Placement Monitors"
Cohesion: 0.09
Nodes (13): EffectPlacement, Cursor, Monitor, EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays (+5 more)

### Community 34 - "Effect Sampler"
Cohesion: 0.09
Nodes (14): IDisplayChangeSource, EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log (+6 more)

### Community 35 - "Win32 Monitor Interop"
Cohesion: 0.10
Nodes (10): MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource, NativeMethods (+2 more)

### Community 36 - "Effect Channel Controller"
Cohesion: 0.10
Nodes (17): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+9 more)

### Community 37 - "Tray Icon and Menu Port"
Cohesion: 0.10
Nodes (12): ITrayMenu, TrayMenuCommand, Exit, Settings, StopEffects, Update, NotifyIconTrayHost, IconUri (+4 more)

### Community 39 - "Layer Dependency Rules"
Cohesion: 0.12
Nodes (3): LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 40 - "App Failure Log"
Cohesion: 0.16
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 41 - "Update Controller Tests"
Cohesion: 0.14
Nodes (9): FakeFailureReporter, Fixture, Consent, Controller, Log, Service, Time, WhileConfirming (+1 more)

### Community 42 - "Save Shortcut Interruption"
Cohesion: 0.09
Nodes (10): IActiveShortcuts, ISaveSoundChannel, ISettingsSaveNotifications, SettingsSaveOperation, IdleSoundChannel, RecordingNotifications, CleanupFailures, RecordingNotifications (+2 more)

### Community 43 - "Release Script Tests"
Cohesion: 0.16
Nodes (3): ReleaseScriptsTests, ScriptResult, Output

### Community 44 - "Drawing Input Gate"
Cohesion: 0.20
Nodes (10): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, IMonitorBoundsSource, CountingMonitors, Queries, FakeMonitorBoundsSource (+2 more)

### Community 45 - "Stuck Save Recovery"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 46 - "Sound Failure Reporting"
Cohesion: 0.11
Nodes (15): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+7 more)

### Community 47 - "Effect Shortcut Tests"
Cohesion: 0.11
Nodes (11): BuiltInEffect, EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played (+3 more)

### Community 48 - "Sound Infrastructure"
Cohesion: 0.13
Nodes (8): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Release, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, SoundAssignment, SoundAssignments, Slots, VelopackHookExitTests

### Community 49 - "Sample Failure Routing"
Cohesion: 0.17
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 50 - "Infrastructure Lifecycle"
Cohesion: 0.11
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 51 - "v4 Work Packages Docs"
Cohesion: 0.18
Nodes (10): drawEM v4: Parallel Work Packages, drawEM v4: Release Publishing, ADR-0002 Release Signing License, release.yml Release Workflow, R4 Release Publishing Roadmap, R4-01 Acceptance Criteria, ReleaseScriptsTests, R4-01 Implementation Plan (+2 more)

### Community 52 - "Shortcut Parsing Errors"
Cohesion: 0.12
Nodes (13): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+5 more)

### Community 53 - "Effect Catalog"
Cohesion: 0.10
Nodes (7): DrawEM.Tests.Domain.Effects, EffectCatalogTests, EffectLifecycleTests, Events, Interruptions, BuiltInEffectAnimationsTests, BuiltInIds

### Community 54 - "v3 Action Shortcuts Docs"
Cohesion: 0.13
Nodes (18): Step 3 Acceptance, Step 3 Plan, GlobalShortcutAdapter, SoundChannelHost, Step 3: Configurable Action Shortcuts, In-Memory Binding Snapshot, Step 4 Acceptance, Step 4 Plan (+10 more)

### Community 55 - "App Startup and Exit"
Cohesion: 0.11
Nodes (6): Application, App, SingleInstanceGuard, DefaultName, WpfApplicationLifetime, SingleInstanceGuardTests

### Community 56 - "Sound Player Fakes"
Cohesion: 0.13
Nodes (10): FakePlayer, Calls, Path, NoFailures, NoSettingsFailures, PlayerFactory, Created, RuntimeSettingsTests (+2 more)

### Community 57 - "Win32 Input Hooks"
Cohesion: 0.14
Nodes (8): INPUT, InputUnion, KBDLLHOOKSTRUCT, KEYBDINPUT, MOUSEINPUT, NativeMethods, Win32KeyboardHookSource, Events

### Community 58 - "Glass Tray Menu Window"
Cohesion: 0.14
Nodes (12): Body, ExitItem, SettingsItem, SettingsText, StopEffectsItem, UpdateItem, UpdateText, Window (+4 more)

### Community 59 - "Effect Sampling Editor"
Cohesion: 0.16
Nodes (8): StartEffectCommand, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, SamplingEditor, Failures, RunningStates, Sampler

### Community 60 - "DrawEM.Tests.Application.Updates Group"
Cohesion: 0.14
Nodes (7): DrawEM.Tests.Application.Updates, DrawEM.Tests.Infrastructure.Updates, DrawEM.App.Infrastructure.Updates, DrawEM.App.Application.Updates, DrawEM.App, UpdateFeed, Program

### Community 61 - "EffectCatalog Group"
Cohesion: 0.18
Nodes (21): EffectCatalog, Design Barn Inc., Lottie Simple License (FL 9.13.21), big-bada-boom.json (Lottie effect), Built-in Effect Sources, celebration-confetti.json (Lottie effect), confetti-02.json (Lottie effect), confetti-effects.json (Lottie effect) (+13 more)

### Community 62 - "GlassBackdrop Group"
Cohesion: 0.14
Nodes (7): GlassBackdrop, Supported, NativeMethods, POINT, UiTheme, Dark, Light

### Community 63 - "CancellationToken Group"
Cohesion: 0.13
Nodes (10): IUpdateService, IsInstalled, PendingVersion, FakeUpdateService, ApplyFailure, CheckCount, CheckFailure, IsInstalled (+2 more)

### Community 64 - "DrawEM.sln Group"
Cohesion: 0.11
Nodes (13): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), Velopack (1.2.161), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows (+5 more)

### Community 65 - "Action Group"
Cohesion: 0.14
Nodes (8): IEffectSampler, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests, PointerToDelegateProbe

### Community 66 - "IsDropDownOpen Group"
Cohesion: 0.13
Nodes (18): IsDropDownOpen, Body, Box, Mark, Moon, PART_ContentHost, PART_Popup, PART_SelectedContentHost (+10 more)

### Community 67 - "drawEM v4 Roadmap: Monitor and Cursor Effects Group"
Cohesion: 0.19
Nodes (17): drawEM v4 Roadmap: Monitor and Cursor Effects, ADR 0001: Lottie built-in effects, Built-in Lottie Effect, Built-in effect catalog (Domain/Effects), Confetti (Monitor effect), EffectChannelController, EffectAction, EffectMonitorGuard (+9 more)

### Community 68 - "EffectFailureLog Group"
Cohesion: 0.19
Nodes (19): EffectFailureLog, LoggingEffectFailureReporter, ActiveSettings, ISoundFailureReporter, ISoundPlayer, ISoundPlayerFactory, PlaySoundCommand, Sound Application README (+11 more)

### Community 69 - "GeneratedRegex Group"
Cohesion: 0.20
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 70 - "RunResult Group"
Cohesion: 0.22
Nodes (6): IMouseHookSource, FakeMouseHookSource, Program, RunResult, Sample, ScenarioSeed

### Community 71 - "Step 5 Acceptance Group"
Cohesion: 0.17
Nodes (15): Step 5 Acceptance, SettingsFailureDialog, Step 5 Plan, DrawingSessionController, StrokeRenderElement, Step 5: Configurable Drawing Style, Stroke Width in Physical Pixels, Step 6 Acceptance (+7 more)

### Community 72 - "DpiScale Group"
Cohesion: 0.16
Nodes (3): EffectSurfaceWindow, Layout, NativeMethods

### Community 74 - "AggregateException Group"
Cohesion: 0.18
Nodes (6): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, RecordingSoundFailures, TimedOutSound

### Community 76 - "Button Group"
Cohesion: 0.24
Nodes (3): UiThemeState, Current, TrayMenuWindowTests

### Community 77 - "Step 1 Acceptance Group"
Cohesion: 0.19
Nodes (13): Step 1 Acceptance, Settings Schema/Version Policy, v3 Settings Defaults, Step 1 Plan, Step 1: Settings Snapshot and Storage, Eight Numbered Action Slots, Immutable Settings Snapshot, Step 2 Acceptance (+5 more)

### Community 78 - "R4-01 Task: Versioned Draft Release Group"
Cohesion: 0.23
Nodes (15): R4-01 Task: Versioned Draft Release, v4 Release Publishing Roadmap, R4-02 Task: Checksums and Provenance, R4-03 Task: First Release Acceptance, R4-04 Acceptance: SignPath Signing, R4-04 Plan: SignPath Signing, GitHub release Environment (required reviewers), SIGNPATH_API_TOKEN in protected release Environment (+7 more)

### Community 79 - "EffectSurfaceLayout Group"
Cohesion: 0.23
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 80 - "BlockingPlayerFactory Group"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 81 - "EventArgs Group"
Cohesion: 0.16
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 82 - "Harness Group"
Cohesion: 0.14
Nodes (12): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+4 more)

### Community 83 - "DrawEM.App.Presentation.Effects Group"
Cohesion: 0.27
Nodes (3): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, LottiePlaybackTests

### Community 84 - "FEAT-005: Assisted E2E Runner Group"
Cohesion: 0.21
Nodes (13): FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR Override, tools/E2ERunner, E2ERunner.Tests Catalog Drift Check, SendInput Scenarios with Pixel Sampling Verdicts, FIX-006: Keep Drawing Active at Single Screen Edge, DrawingModeInputGate.StopAtBoundary, GlobalMouseInputAdapter.OnPointerMoved (+5 more)

### Community 86 - "v3 Step 6: Apply Saved Settings at Runtime Group"
Cohesion: 0.24
Nodes (7): v3 Step 6: Apply Saved Settings at Runtime, v3 Step 7 Acceptance Criteria, SettingsEditorTests / SettingsViewModelTests / SoundSamplerTests, v3 Step 7 Implementation Plan, drawEM Settings Prototype (HTML), Settings Prototype UI (Drawing: Stroke, Shortcuts), v3 Step 7: Settings Window

### Community 87 - "IEnumerable Group"
Cohesion: 0.18
Nodes (6): ISoundSampler, Invalid, NotSaved, Saved, SettingsSaveResult, Saved

### Community 88 - "PowerModeChangedEventArgs Group"
Cohesion: 0.18
Nodes (3): EffectStopRoutes, ISessionInterruptionSource, Win32SessionInterruptionSource

### Community 89 - "Queue Group"
Cohesion: 0.15
Nodes (4): ISoundStartHold, StartHold, FakeSoundChannel, StopConfirmed

### Community 92 - "Custom WPF Program.Main with VelopackApp.Run Group"
Cohesion: 0.27
Nodes (10): Custom WPF Program.Main with VelopackApp.Run, R4-07 Acceptance: Tray Updates, R4-07 Plan: Tray Updates, DRAWEM_UPDATE_FEED local feed override, ITrayHost UpdateRequested / ShowUpdateReady, IUpdateService port, LayerDependencyTests, UpdateController (+2 more)

### Community 93 - "skiasharp_resources Group"
Cohesion: 0.15
Nodes (6): EffectCatalog, BuiltIn, ShortcutKeyKind, Digit, Function, Letter

### Community 94 - "S4-08 Choose Effects in Settings (task) Group"
Cohesion: 0.30
Nodes (10): S4-08 Choose Effects in Settings (task), S4-09 Preview Draft Effects (acceptance), EffectChannelController, EffectMonitorGuard, EffectSampleFailureRouter, EffectSampler, IEffectSampler, SettingsEditor (+2 more)

### Community 97 - "drawEM Prototype Tray Icon Group"
Cohesion: 0.27
Nodes (10): drawEM Prototype Tray Icon, S4-11 Stop Effects Through App Lifecycle (acceptance), EffectMonitorGuard.StopEffects(), EffectStopRoutes, TrayApplication, Win32DisplayChangeSource, Win32SessionInterruptionSource, S4-11 Stop Effects Through App Lifecycle (plan) (+2 more)

### Community 98 - "FEAT-011 Acceptance Criteria Group"
Cohesion: 0.29
Nodes (9): FEAT-011 Acceptance Criteria, FEAT-011 Implementation Plan, GlassDark/GlassLight/GlassStyles XAML Dictionaries, GlassBackdrop (Acrylic, build 22621+), TrayMenuPlacement, TrayMenuWindow / ITrayMenu, UiThemeState, FEAT-011: Glass Design for Settings and Tray Menu (+1 more)

### Community 100 - "VelopackUpdateService Group"
Cohesion: 0.24
Nodes (5): VelopackUpdateService, IsInstalled, PendingVersion, Reporter, VelopackUpdateServiceTests

### Community 101 - "FEAT-009 Acceptance Criteria Group"
Cohesion: 0.40
Nodes (6): FEAT-009 Acceptance Criteria, FEAT-009 Implementation Plan, EffectAction (Loop, MaxLength), EffectChannelController, LottiePlayback, FEAT-009: Effect Loop and Length Limit

### Community 102 - "R4-03 Acceptance: First Release Group"
Cohesion: 0.20
Nodes (8): R4-03 Acceptance: First Release, First Release Gate Table, SmartScreen 'Windows protected your PC' warning, drawEM v4.0.0 Release, R4-03 Plan: First Release, R4-06 Acceptance: Velopack Installer, %LOCALAPPDATA%\drawEM settings directory, S4-12 Publish and verify

### Community 103 - "SettingsErrorCode Group"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 104 - "ShortcutKey Group"
Cohesion: 0.42
Nodes (3): ShortcutKey, Kind, Value

### Community 105 - "SoundSlot.cs Group"
Cohesion: 0.20
Nodes (9): SoundSlot, Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7 (+1 more)

### Community 107 - "R4-02 Acceptance: Checksums and Provenance Group"
Cohesion: 0.31
Nodes (5): R4-02 Acceptance: Checksums and Provenance, R4-02 Plan: Checksums and Provenance, GitHub Build Provenance Attestation (actions/attest), SHA256SUMS.txt Manifest, Velopack per-user Setup.exe

### Community 109 - "HexText Group"
Cohesion: 0.25
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 110 - "S4-10 Apply Settings Safely (acceptance) Group"
Cohesion: 0.39
Nodes (6): S4-10 Apply Settings Safely (acceptance), RecordingEffectSurface, SettingsSaveOperation, S4-10 Apply Settings Safely (plan), S4-10 Apply Settings Safely (task), SettingsStoreException

### Community 111 - "GithubSource Group"
Cohesion: 0.32
Nodes (3): RecordingReporter, Lines, UpdateFeedTests

### Community 112 - "DrawEM.Tests.Presentation.Theme Group"
Cohesion: 0.29
Nodes (3): DrawEM.Tests.Presentation.Theme, GlassBackdropTests, UiThemeStateTests

### Community 113 - "EffectSurfaceWindow (nonactivating click-through) Group"
Cohesion: 0.38
Nodes (5): EffectSurfaceWindow (nonactivating click-through), Release performance targets (100ms/4ms/25ms), S4-01 Acceptance: Effect Surface Probe, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), S4-03.1 Lottie speed gate

### Community 114 - "S4-12 Publish and Verify on Windows (acceptance) Group"
Cohesion: 0.38
Nodes (6): S4-12 Publish and Verify on Windows (acceptance), Automated Gate (701 tests, win-x64 publish), Build Identity (DrawEM.App.exe 1.0.0+335de88), Published-app Checks, S4-12 Publish and Verify on Windows (plan), S4-12 Publish and Verify on Windows (task)

### Community 115 - "SignPath Foundation Code Signing Group"
Cohesion: 0.40
Nodes (5): SignPath Foundation Code Signing, WinGet zip/nested-portable manifest, R4-06 Plan: Velopack Installer, CI Velopack packaging smoke, vpk pack CLI

### Community 117 - "Durable Settings Write Group"
Cohesion: 0.60
Nodes (5): ISettingsStore, JsonSettingsStore, Settings Storage README, Settings Schema Version 3, SettingsSnapshot

### Community 118 - "ISoundFilePicker Group"
Cohesion: 0.33
Nodes (5): ISoundFilePicker, FakePicker, Next, Picking, Picks

### Community 119 - "CanDecreaseWidth Group"
Cohesion: 0.40
Nodes (5): CanDecreaseWidth, CanIncreaseWidth, DecreaseWidthButton, IncreaseWidthButton, RepeatButton

### Community 120 - "SettingsCommandKind Group"
Cohesion: 0.50
Nodes (4): SettingsCommandKind, Clear, Draw, Slot

## Knowledge Gaps
- **499 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+494 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 711 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **17 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SettingsWindow` connect `Settings Window` to `Settings ViewModel Commands`, `Presentation Namespaces`, `Shortcut Capture Interface`, `Button Group`, `HexText Group`, `WPF Control Types`, `ISoundFilePicker Group`, `App Startup and Exit`, `GlassBackdrop Group`, `Animation Group`?**
  _High betweenness centrality (0.053) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _499 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Sound and Effect Value Types` be split into smaller, more focused modules?**
  _Cohesion score 0.05362614913176711 - nodes in this community are weakly interconnected._
- **Why does `SettingsViewModel` connect `Shortcut Capture Interface` to `Stroke Rendering Visuals`, `Shortcut Test Fakes`, `Settings Window`, `Settings Namespaces`, `WPF Control Types`, `Effect Slot Options`, `Settings ViewModel Tests`, `Settings Editor`, `IEnumerable Group`, `ISoundFilePicker Group`, `Animation Group`, `Sound Library Save Cleanup`?**
  _High betweenness centrality (0.045) - this node is a cross-community bridge._
- **Should `Settings Store Load Save` be split into smaller, more focused modules?**
  _Cohesion score 0.06913367756741251 - nodes in this community are weakly interconnected._
- **Why does `ScreenPoint` connect `Drawing Session Controller` to `Settings Store Load Save`, `Stroke Rendering Visuals`, `Shortcut Test Fakes`, `Shortcut Capture Results`, `Settings Test Fakes`, `Monitor Bounds`, `Effect Channel Tests`, `Global Shortcut Adapter`, `Effect Placement Monitors`, `Effect Sampler`, `Win32 Monitor Interop`, `Effect Channel Controller`, `Drawing Input Gate`, `Stuck Save Recovery`, `Effect Shortcut Tests`, `Effect Catalog`, `Effect Sampling Editor`, `RunResult Group`, `EffectSurfaceLayout Group`, `Harness Group`, `.StopStartedBefore() Group`?**
  _High betweenness centrality (0.035) - this node is a cross-community bridge._