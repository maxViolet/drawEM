# Graph Report - worktree-r4-03  (2026-10-16)

## Corpus Check
- 337 files · ~152,399 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 13 file(s) not represented in the graph (top: (none) 10, .ico 1, .pubxml 1)

## Summary
- 3000 nodes · 8813 edges · 129 communities (106 shown, 23 thin omitted)
- Extraction: 83% EXTRACTED · 17% INFERRED · 0% AMBIGUOUS · INFERRED: 1515 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `44e2fac1`
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
- DrawEM.App.Domain.Settings
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
- DrawEM.App.Application.Sound
- ManagedSoundLibraryTests
- Fact
- ScreenPoint
- Fixture
- RecordingEffectSurface
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
- EffectLifecycleTests
- S4-12 Acceptance: Publish and Verify on Windows
- IEffectPlayback
- GlobalShortcutAdapter
- StartEffectCommand
- DrawEM.App.Infrastructure
- List
- Stage 1 sound implementation plan
- .Letter
- drawEM architecture
- S4-11 Acceptance criteria
- ManagedSoundLibrary
- LottiePlayback
- SaveHarness
- EffectId
- .Save
- SoundReference
- App
- Action
- ShortcutBindings
- App.xaml.cs
- EffectChannelController
- ReleaseScriptsTests
- EffectSurfaceLayout
- IntPtr
- S4-07 acceptance: stop effects when switching monitors
- JsonSettingsStore
- .Load
- SoundId
- R4-02 implementation plan
- EffectFailure
- STEPS.md
- v4 Simple Implementation Steps
- FakeSoundChannel
- BlockingPlayerFactory
- LayerDependencyTests.cs
- Settings Storage README
- .OnStartup
- SingleInstanceGuard
- .Deserialize
- drawEM rules (AGENTS.md)
- IDisposable
- Program.cs
- FEAT-005: Assisted E2E Runner
- DrawEM.Tests.csproj
- KeyDirection
- NotifyIconTrayHost
- SoundSlot
- .Connect
- Sound infrastructure README
- Stage 1 Step 2 Review
- FIX-006: Single Screen Edge
- MediaSoundPlayer
- drawEM v4 Parallel Work Packages
- v1 roadmap: drawing
- drawEM v4 out-of-scope task list
- .TryFind
- FakePlayback
- S4-06 acceptance: launch effects through shortcuts
- S4-08 acceptance: choose effects in Settings
- TimeSpan
- ISettingsSaveNotifications
- v4 / FEAT-009: implementation plan
- ADR 0001: Lottie built-in effects
- step-01-draft-release/task.md
- drawEM v4 Roadmap: Built-in Monitor and Cursor Effects
- Steps
- .Capture
- EffectSurfaceWindow
- WpfApplicationLifetime
- FakePlayer
- SettingsErrorCode
- Win32DisplayChangeSource
- S4-09 Acceptance criteria
- WidthBox
- WinGet manifest (zip + nested portable)
- .ShowMenu_WithoutTheNotifyIconsOwnMenuDisplay_ShowsTheMenuAtTheCursor
- SoundChannelControllerTests.cs
- Show visual effects (Loop, Max length)
- FakePicker
- EffectPlacement
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

## Hyperedges (group relationships)
- **Effect playback pipeline** — docs_v4_visual_effects_roadmap_v4_effectchannelcontroller, docs_v4_visual_effects_roadmap_v4_ieffectsurface, docs_v4_visual_effects_roadmap_v4_skiasharp_skottie_writeablebitmap_rendering, context_placement, docs_v4_visual_effects_roadmap_v4_built_in_lottie_effect [EXTRACTED 1.00]
- **Artifact Integrity and Provenance Verification** — docs_v4_release_publishing_step_02_checksums_provenance_plan_sha256sums_manifest, tools_release_write_sha256sums, docs_v4_release_publishing_step_02_checksums_provenance_plan_actions_attest, docs_v4_release_publishing_step_02_checksums_provenance_plan_gh_attestation_verify, docs_v4_release_publishing_step_02_checksums_provenance_acceptance_one_byte_mutation_check [EXTRACTED 1.00]
- **Code Signing Options Considered** — docs_adr_0002_release_signing_license_signpath_foundation, docs_adr_0002_release_signing_license_microsoft_artifact_signing, docs_adr_0002_release_signing_license_microsoft_store_msix, docs_adr_0002_release_signing_license_unsigned_first_release [EXTRACTED 1.00]
- **Sound shortcut play and Save interlock flow** — docs_architecture_globalshortcutadapter, docs_architecture_soundchannelhost, docs_architecture_soundchannelcontroller, docs_architecture_settingssaveoperation [EXTRACTED 1.00]
- **Tray update flow** — docs_v4_release_publishing_step_07_tray_updates_plan_updatecontroller, docs_v4_release_publishing_step_07_tray_updates_plan_velopackupdateservice, docs_v4_release_publishing_step_07_tray_updates_plan_itrayhost, docs_v4_release_publishing_step_07_tray_updates_plan_trayapplication, docs_v4_release_publishing_step_07_tray_updates_plan_wait_exit_then_apply [EXTRACTED 1.00]
- **Release artifact pipeline** — docs_v4_release_publishing_step_04_signpath_signing_plan_pe_file_signing, docs_v4_release_publishing_step_06_velopack_installer_plan_new_velopackpackage_ps1, docs_v4_release_publishing_step_06_velopack_installer_plan_write_sha256sums_ps1, docs_v4_release_publishing_step_05_winget_plan_winget_manifest [INFERRED 0.85]
- **Settings directory preservation** — docs_v4_release_publishing_step_06_velopack_installer_task_package_id, docs_v4_release_publishing_step_06_velopack_installer_plan_json_settings_store, docs_v4_release_publishing_step_05_winget_acceptance_localappdata_drawem, docs_v4_release_publishing_step_06_velopack_installer_plan_velopackpackageidentitytests [INFERRED 0.85]
- **Settings schema migration and rollback** — docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_jsonsettingsstore, docs_v4_visual_effects_step_05_effect_settings_schema_acceptance_settingsjson, docs_v4_visual_effects_roadmap_v4_schema1_migration_backup, docs_v4_visual_effects_roadmap_v4_settings_schema_3, src_drawem_app_infrastructure_settings_readme_v3_rollback_procedure [INFERRED 0.85]

## Communities (129 total, 23 thin omitted)

### Community 1 - "StrokeRenderElement"
Cohesion: 0.07
Nodes (20): DrawingState, Stroke, HexColor, IOverlayView, Window, OverlayWindow, PhysicalToLocalTransform, StrokeRenderElement (+12 more)

### Community 2 - "R4 release publishing roadmap"
Cohesion: 0.12
Nodes (18): Authenticode Signature, v3 Step 8 Windows Acceptance Gate, Portable win-x64 ZIP, release.yml Release Workflow, R4 acceptance strategy (automated, CI/provenance, real Windows, external), R4 release publishing roadmap, SmartScreen Reputation Warnings, Consent-based update UX (startup + 24h check) (+10 more)

### Community 3 - "JsonSettingsStoreTests"
Cohesion: 0.09
Nodes (8): SettingsStoreException, SettingsFailureDialog, JsonSettingsStoreTests, BackupPath, SettingsPath, NoReporter, RecordingReporter, Reasons

### Community 4 - "GlobalShortcutAdapterTests"
Cohesion: 0.09
Nodes (18): INeutralKeyEmitter, FakeCursorPositionSource, FakeEffectStartQueue, Dequeued, Enqueued, NextEpoch, Started, FakeKeyboardHookSource (+10 more)

### Community 5 - "Window"
Cohesion: 0.04
Nodes (54): CanDecreaseMaxLength, CanDecreaseWidth, CanEditMaxLength, CanIncreaseMaxLength, CanIncreaseWidth, CanSave, CanSetNoLimit, ChooseText (+46 more)

### Community 6 - "drawEM v3 Roadmap: Drawing and Action Settings"
Cohesion: 0.07
Nodes (58): drawEM v3 Roadmap: Drawing and Action Settings, Shortcut Rule: two modifiers plus one key, StrokeRenderElement, v3 Step 1 acceptance criteria, Defaults: #FF4500, 4 px, Ctrl+Alt+Z, Ctrl+Alt+X, v3 Step 1 plan, Default fallback with retained damaged file, Durable replacement settings storage (+50 more)

### Community 7 - "Harness"
Cohesion: 0.08
Nodes (15): Cancelled, Captured, ShortcutCaptureResult, FakeNeutralKeyEmitter, Harness, Adapter, Effects, Gate (+7 more)

### Community 8 - "PlaySoundCommand"
Cohesion: 0.16
Nodes (5): PlaySoundCommand, SampleFailureRouter, SoundSampler, RecordingReporter, SoundSamplerTests

### Community 9 - "DrawEM.App.Domain.Settings"
Cohesion: 0.09
Nodes (10): DrawEM.App.Domain.Settings, DrawEM.Tests.Presentation.Settings, DrawEM.Tests.Application.Settings, DrawEM.App.Presentation.Settings, DrawEM.App.Domain.Effects, DrawEM.Tests.Domain.Settings, SettingsCommandKind, Clear (+2 more)

### Community 10 - "SettingsViewModel"
Cohesion: 0.06
Nodes (27): IShortcutCapture, BindableBase, SettingsViewModel, CanDecreaseWidth, CanIncreaseWidth, CanSave, Clear, Color (+19 more)

### Community 11 - "DrawEM.App.Domain.Drawing"
Cohesion: 0.12
Nodes (10): DrawEM.Tests.Presentation.Drawing, DrawEM.App.Application.Drawing, DrawEM.App.Infrastructure.Drawing, DrawEM.Tests.Infrastructure.Drawing, DrawEM.App.Domain.Drawing, DrawEM.Tests.Application.Effects, DrawEM.Tests.Application.Drawing, DrawEM.Tests.Infrastructure.Effects (+2 more)

### Community 12 - "ShortcutModifiers"
Cohesion: 0.09
Nodes (20): ShortcutError, IllegalKey, None, TooFewModifiers, Unrecognized, WindowsKey, ShortcutModifiers, Alt (+12 more)

### Community 13 - "Theory"
Cohesion: 0.12
Nodes (5): StrokeWidth, Pixels, LayerDependencyTests, AllowedLayerDependencies, NativeFunctionBridgeRule

### Community 14 - ".Window_BindsEveryFieldWithoutBindingErrors"
Cohesion: 0.08
Nodes (16): FakeCapture, Current, Ended, FakeEffectSampler, Forgets, Samples, Stops, FakeSampler (+8 more)

### Community 15 - "Fixture"
Cohesion: 0.19
Nodes (10): FakeRequest, IsCurrent, Fixture, Controller, Failures, Log, OnCreate, Players (+2 more)

### Community 16 - "ManualTimeProvider"
Cohesion: 0.11
Nodes (15): SoundChannelHost, SoundChannelShutdownTimeoutException, IgnoringReporter, SoundChannelHostTests, ThreadRecordingFactory, Created, OnCreate, OnPlay (+7 more)

### Community 17 - "VelopackUpdateService"
Cohesion: 0.10
Nodes (18): %LOCALAPPDATA%\drawEM settings directory, JsonSettingsStore.DefaultDirectory, New-VelopackPackage.ps1, Program.Main custom WPF entry, releases.win.json / full nupkg update feed, drawEM-<version>-win-x64-Setup.exe, VelopackHookExitTests, VelopackPackageIdentityTests (+10 more)

### Community 18 - "SlotViewModel"
Cohesion: 0.05
Nodes (33): EffectOption, All, ISoundFilePicker, SlotKind, Effect, Sound, SlotViewModel, Action (+25 more)

### Community 19 - "ISoundPlayer"
Cohesion: 0.07
Nodes (17): ISoundPlayer, ISoundPlayerFactory, FakePlayer, Calls, Path, FakePlayer, Calls, Sound (+9 more)

### Community 20 - "LoggingSoundFailureReporter"
Cohesion: 0.19
Nodes (4): BackgroundLogWriter, SettingsSaveNotifications, LoggingSoundFailureReporter, LoggingSoundFailureReporterTests

### Community 21 - "DrawEM.App.Application.Sound"
Cohesion: 0.14
Nodes (7): DrawEM.App.Infrastructure.Sound, DrawEM.Tests.Infrastructure.Sound, DrawEM.App.Application.Sound, DrawEM.App.Domain.Sound, DrawEM.App.Infrastructure.Settings, DrawEM.App.Application.Settings, DrawEM.Tests.Infrastructure.Settings

### Community 22 - "ManagedSoundLibraryTests"
Cohesion: 0.19
Nodes (5): SoundImportException, ManagedSoundLibraryTests, LibraryDirectory, SourceDirectory, NoReporter

### Community 24 - "ScreenPoint"
Cohesion: 0.16
Nodes (7): ScreenPoint, MonitorBounds, CursorSurfaceBounds, EffectCanvasPlacement, EffectRenderLayout, EffectCanvasPlacementTests, EffectRenderLayoutTests

### Community 25 - "Fixture"
Cohesion: 0.13
Nodes (8): EffectChannelControllerTests, Fixture, Controller, Failures, Log, OnShow, Playbacks, Time

### Community 26 - "RecordingEffectSurface"
Cohesion: 0.15
Nodes (9): Playback, DisposeFailure, HasCompletionHandler, HasFailureHandler, RecordingEffectSurface, Failures, OnRemove, Removed (+1 more)

### Community 27 - "DrawingSessionController"
Cohesion: 0.17
Nodes (7): DrawingSessionController, CompletedStrokes, OverlayWindowAdapter, DrawingSessionControllerTests, FakeOverlayView, LastRenderedState, OverlayWindowAdapterTests

### Community 28 - "DrawingModeInputGate"
Cohesion: 0.18
Nodes (11): DrawingModeInputGate, IsActive, IsBlockedUntilReleased, GlobalMouseInputAdapter, CountingMonitors, Queries, FakeMonitorBoundsSource, FakeMouseHookSource (+3 more)

### Community 29 - "Fixture"
Cohesion: 0.10
Nodes (11): EffectMonitorGuardTests, Fixture, Channel, Cursor, Displays, Guard, Monitors, Mouse (+3 more)

### Community 30 - "SettingsSnapshot"
Cohesion: 0.10
Nodes (21): ISoundLibrary, SoundCleanupFailure, StartupSettings, SettingsSnapshot, ClearShortcut, Default, DefaultEffects, DrawShortcut (+13 more)

### Community 31 - "TrayApplication"
Cohesion: 0.14
Nodes (9): IApplicationLifetime, IOverlayLifetime, ITrayHost, TrayApplication, FakeApplicationLifetime, FakeOverlay, FakeTrayHost, ThrowingDisposable (+1 more)

### Community 33 - "Fixture"
Cohesion: 0.10
Nodes (12): EffectSamplerTests, Fixture, Channel, Cursor, Failures, Guard, Log, Monitors (+4 more)

### Community 34 - "SoundChannelController"
Cohesion: 0.13
Nodes (13): ISoundFailureReporter, AlwaysCurrent, IsCurrent, Attempt, Command, Deadline, OnCompleted, OnFailed (+5 more)

### Community 35 - "RuntimeMediaTests"
Cohesion: 0.12
Nodes (16): ActiveSettings, Current, IActiveShortcuts, SettingsSaveOperation, IdleShortcuts, NoSettingsFailures, RuntimeMediaTests, LibraryDirectory (+8 more)

### Community 36 - "Harness"
Cohesion: 0.10
Nodes (14): EffectShortcutTests, Harness, Adapter, Cursor, Effects, Monitors, Played, Queued (+6 more)

### Community 37 - "SettingsEditor"
Cohesion: 0.07
Nodes (21): Evidence, IEffectSampler, ISoundSampler, SettingsEditor, ClearShortcut, Color, DrawShortcut, Slots (+13 more)

### Community 38 - "Rig"
Cohesion: 0.12
Nodes (16): Release, NoSettingsFailures, Rig, Keyboard, New, Old, Players, Previous (+8 more)

### Community 39 - "EffectLifecycleTests"
Cohesion: 0.13
Nodes (6): BuiltInEffectAnimations, EffectLifecycleTests, Events, Interruptions, BuiltInEffectAnimationsTests, BuiltInIds

### Community 40 - "S4-12 Acceptance: Publish and Verify on Windows"
Cohesion: 0.33
Nodes (7): Published Build Identity (SHA-256), S4-12 Acceptance: Publish and Verify on Windows, S4-12 Published-App Checks, v4 Release Blockers, S4-12 plan: publish and verify on Windows, 100-invocation shortcut-to-present latency, S4-12 Task: publish and verify on Windows

### Community 41 - "IEffectPlayback"
Cohesion: 0.13
Nodes (12): IEffectPlayback, IEffectSurface, Playback, RecordingSurface, Disposed, Last, Shown, FailingSurface (+4 more)

### Community 42 - "GlobalShortcutAdapter"
Cohesion: 0.13
Nodes (5): GlobalShortcutAdapter, IsCapturing, ICursorPositionSource, Win32CursorPositionSource, RecoveringCursorPositionSource

### Community 43 - "StartEffectCommand"
Cohesion: 0.14
Nodes (8): StartEffectCommand, IMonitorBoundsSource, EffectMonitorGuard, EffectSampleFailureRouter, EffectSampler, IEffectStartQueue, CountingMonitorBoundsSource, Queries

### Community 44 - "DrawEM.App.Infrastructure"
Cohesion: 0.10
Nodes (7): DrawEM.App.Infrastructure, DrawEM.Tests.Infrastructure, DrawEM.Tests, CleanupSteps, VirtualKeys, AppIconTests, StaThread

### Community 45 - "List"
Cohesion: 0.10
Nodes (17): Harness, Controller, Displays, EffectChannel, Effects, Players, Sessions, Sound (+9 more)

### Community 46 - "Stage 1 sound implementation plan"
Cohesion: 0.13
Nodes (34): Step 7 acceptance results, drawEM smoke test, Manual smoke checks (tray, input blocking, multi-monitor), Acceptance results template, v2 roadmap: screen actions, Stage 1 sound acceptance results, Stage 1 test media assignments, Stage 1 sound implementation plan (+26 more)

### Community 48 - "drawEM architecture"
Cohesion: 0.08
Nodes (37): SkiaSharp allowed only in Presentation, Action slot, ActiveSettings, ApplicationExitPolicy, BackgroundLogWriter<T>, Per-monitor channels and global sound channel, App.xaml.cs composition root, DrawingModeInputGate (+29 more)

### Community 49 - "S4-11 Acceptance criteria"
Cohesion: 0.15
Nodes (18): Configuration generation, S4-10 Acceptance criteria, Managed sound copy removal on Sound->Effect change, S4-10 running-app check (unverified), Save interrupt sequence (stop sound, Interrupt, stop effect channel, publish), S4-10 plan: apply settings safely, S4-10 Task: apply settings safely, S4-11 Acceptance criteria (+10 more)

### Community 50 - "ManagedSoundLibrary"
Cohesion: 0.26
Nodes (3): ManagedSoundLibrary, DefaultDirectory, LibraryDirectory

### Community 51 - "LottiePlayback"
Cohesion: 0.17
Nodes (5): LottiePlayback, PlaybackState, CannotLoop, Completed, Playing

### Community 52 - "SaveHarness"
Cohesion: 0.09
Nodes (26): FakeStore, Failure, OnSave, Saved, RecordingNotifications, NotSavedReasons, UnconfirmedStops, SaveHarness (+18 more)

### Community 53 - "EffectId"
Cohesion: 0.15
Nodes (8): EffectId, Value, EffectAction, Effect, SettingsError, EffectSample, Effect, SettingsSnapshotTests

### Community 54 - ".Save"
Cohesion: 0.32
Nodes (5): Invalid, NotSaved, Saved, SettingsSaveResult, DrawingStyle

### Community 55 - "SoundReference"
Cohesion: 0.15
Nodes (8): SoundAction, Sound, SoundReference, DisplayName, LibraryFileName, SoundBinding, ActiveSettingsTests, TestSettings

### Community 57 - "Action"
Cohesion: 0.11
Nodes (9): Request, IsCurrent, AsyncProbe, ClosureProbe, DelegateToPointerProbe, IteratorProbe, ManagedCollectionProbe, NativeFunctionBridgeRuleTests (+1 more)

### Community 58 - "ShortcutBindings"
Cohesion: 0.15
Nodes (14): EffectBinding, KeyChord, ShortcutBindings, Clear, Draw, Effects, Sounds, NoFailures (+6 more)

### Community 59 - "App.xaml.cs"
Cohesion: 0.11
Nodes (5): DrawEM.App.Presentation.Effects, DrawEM.Tests.Presentation.Effects, DrawEM.App, LottieEffectSurface, LottiePlaybackTests

### Community 60 - "EffectChannelController"
Cohesion: 0.13
Nodes (12): EffectChannelController, ActiveInstance, Run, Deadline, Instance, OnCompleted, OnFailed, Playback (+4 more)

### Community 61 - "ReleaseScriptsTests"
Cohesion: 0.16
Nodes (4): DrawEM.Tests.Release, ReleaseScriptsTests, ScriptResult, Output

### Community 62 - "EffectSurfaceLayout"
Cohesion: 0.19
Nodes (6): EffectSurfaceLayout, DpiScale, LocalBounds, LocalSize, Monitor, EffectSurfaceCoordinateTests

### Community 63 - "IntPtr"
Cohesion: 0.06
Nodes (20): IMouseHookSource, MONITORINFO, MSLLHOOKSTRUCT, NativeMethods, POINT, RECT, Win32MonitorBoundsSource, Win32MouseHookSource (+12 more)

### Community 64 - "S4-07 acceptance: stop effects when switching monitors"
Cohesion: 0.21
Nodes (14): S4-07 acceptance: stop effects when switching monitors, EffectMonitorGuard.CheckCursor frame check, EffectMonitorGuard, EffectMonitorGuardTests, EffectSurfaceWindow, GlobalShortcutAdapter, IDisplayChangeSource, IEffectStartQueue (+6 more)

### Community 65 - "JsonSettingsStore"
Cohesion: 0.32
Nodes (3): JsonSettingsStore, DefaultDirectory, SettingsPath

### Community 66 - ".Load"
Cohesion: 0.15
Nodes (12): ISettingsStore, Loaded, Missing, SettingsLoadResult, Unreadable, ISettingsFailureReporter, SettingsStartup, FakeReporter (+4 more)

### Community 67 - "SoundId"
Cohesion: 0.22
Nodes (6): SoundId, Value, SoundFailure, SoundFailureLog, DefaultPath, SoundFailureLogTests

### Community 68 - "R4-02 implementation plan"
Cohesion: 0.14
Nodes (16): Create draft release step (gh release create --draft --verify-tag), Package step, Release workflow (release.yml), Verify tag step, Release publishing package (R4), SHA256SUMS.txt and GitHub build-provenance attestations (actions/attest@v4), One-Byte Mutation Check, R4-02 acceptance criteria (+8 more)

### Community 69 - "EffectFailure"
Cohesion: 0.27
Nodes (5): EffectFailure, EffectFailureLog, DefaultPath, TextFileLogSink, EffectFailureLogTests

### Community 70 - "STEPS.md"
Cohesion: 0.22
Nodes (16): drawEM v4: out of scope, R4-04 SignPath Signing Acceptance, R4-04 SignPath Signing Plan, ADR-0002 Release Signing License, R4-04 SignPath Signing Task, SignPath Foundation, R4-05 WinGet Listing Acceptance, R4-05 WinGet Listing Plan (+8 more)

### Community 71 - "v4 Simple Implementation Steps"
Cohesion: 0.10
Nodes (32): EffectChannelController, EffectMonitorGuard, IEffectSurface Presentation Port, S4-01 acceptance: prove the effect window, S4-01 plan: prove the effect window, EffectSurfaceProbe (DRAWEM_EFFECT_PROBE=1), Owner exception closing S4-01 with gates unverified, S4-01 task: prove the effect window (+24 more)

### Community 72 - "FakeSoundChannel"
Cohesion: 0.11
Nodes (6): ISaveSoundChannel, ISoundStartHold, StartHold, IdleSoundChannel, FakeSoundChannel, StopConfirmed

### Community 73 - "BlockingPlayerFactory"
Cohesion: 0.18
Nodes (8): BlockingPlayerFactory, Blocked, BlockedPath, BlockInPlay, Events, NewPlayStarted, OldPlayStarted, Player

### Community 75 - "Settings Storage README"
Cohesion: 0.20
Nodes (15): Numbered Schema 1 Migration Backup, Settings Schema 3 (loop, maxSeconds), S4-05 Acceptance: Save and Load Effect Assignments, EffectCatalog, JsonSettingsStore, ManagedSoundLibrary, SettingsJson, S4-05 Implementation Plan (+7 more)

### Community 76 - ".OnStartup"
Cohesion: 0.15
Nodes (6): AppFailureLog, DefaultPath, ApplicationExitPolicy, StartupFailure, AppFailureLogTests, StartupFailureTests

### Community 77 - "SingleInstanceGuard"
Cohesion: 0.20
Nodes (3): SingleInstanceGuard, DefaultName, SingleInstanceGuardTests

### Community 79 - "drawEM rules (AGENTS.md)"
Cohesion: 0.19
Nodes (5): Dependabot config, PR lint workflow, Branch and PR title work-id check, drawEM rules (AGENTS.md), Running-app check (unverified)

### Community 80 - "IDisposable"
Cohesion: 0.22
Nodes (5): CompositeDisposable, CompositeDisposableTests, FakeDisposable, FakeDisposable, TimedOutSound

### Community 82 - "FEAT-005: Assisted E2E Runner"
Cohesion: 0.11
Nodes (26): Confirmed test boundaries, Sound business test scenarios, Shortcut and independent-channel scenarios, Stage 1 Manual Desktop Gate, App.xaml.cs composition root, AppFailureLog, FEAT-005: Assisted E2E Runner, DRAWEM_DATA_DIR override (+18 more)

### Community 83 - "DrawEM.Tests.csproj"
Cohesion: 0.12
Nodes (12): coverlet.collector (10.1.0), Microsoft.NET.Test.Sdk (18.10.1), NetArchTest.Rules (1.3.2), SkiaSharp.Skottie (4.153.1), xunit (2.9.3), xunit.runner.visualstudio (4.0.0), net8.0-windows, Microsoft.NET.Sdk (+4 more)

### Community 84 - "KeyDirection"
Cohesion: 0.17
Nodes (5): IKeyboardHookSource, KeyDirection, Down, Up, KeyboardHookEvents

### Community 85 - "NotifyIconTrayHost"
Cohesion: 0.22
Nodes (3): NotifyIconTrayHost, CanShowMenuOnLeftClick, IconUri

### Community 86 - "SoundSlot"
Cohesion: 0.14
Nodes (12): SoundAssignment, SoundAssignments, Slots, SoundSlot, Slot1, Slot2, Slot3, Slot4 (+4 more)

### Community 88 - "Sound infrastructure README"
Cohesion: 0.23
Nodes (14): Sound application README, ActiveSettings, ISoundFailureReporter, ISoundPlayerFactory / ISoundPlayer, PlaySoundCommand, SoundChannelController, Sound infrastructure README, BackgroundLogWriter<T> / TextFileLogSink (+6 more)

### Community 89 - "Stage 1 Step 2 Review"
Cohesion: 0.12
Nodes (25): sound.log failure log (%LOCALAPPDATA%\drawEM\logs), Stage 1 Step 1 Review, PlaySoundCommand, SoundAssignments (code-owned Slots), SoundConfiguration (Resolve / PathOf), SoundFailure record, SoundFailureLog, SoundId (+17 more)

### Community 90 - "FIX-006: Single Screen Edge"
Cohesion: 0.44
Nodes (8): FIX-006: Single Screen Edge, DrawingModeInputGate (StopAtBoundary), DrawingSessionController, GlobalMouseInputAdapter (OnPointerMoved), IMonitorBoundsSource (TryGetBounds), MonitorBounds.Contains, SINGLE_SCREEN screen mode, Win32MonitorBoundsSource (MONITOR_DEFAULTTONULL)

### Community 91 - "MediaSoundPlayer"
Cohesion: 0.17
Nodes (3): SoundPlaybackException, MediaSoundPlayer, MediaSoundPlayerFactory

### Community 92 - "drawEM v4 Parallel Work Packages"
Cohesion: 0.24
Nodes (14): ADR-0002 Release Signing and License, ADR-0002: unsigned GitHub releases, conditional SignPath signing, Microsoft Artifact Signing, Microsoft Store MSIX, OSI-Approved License Owner Decision, Owner questions (license, SignPath), SignPath Foundation Signing, drawEM v4 Parallel Work Packages (+6 more)

### Community 93 - "v1 roadmap: drawing"
Cohesion: 0.10
Nodes (19): build-test-publish job, CI workflow, .NET SDK pinned by global.json, Warnings as errors build, Release workflow, Tag-triggered GitHub Release (drawEM-<version>-win-x64.zip), TrayApplication, v1 acceptance pass (step 7) (+11 more)

### Community 95 - ".TryFind"
Cohesion: 0.19
Nodes (5): DrawEM.Tests.Domain.Effects, BuiltInEffect, EffectCatalog, BuiltIn, EffectCatalogTests

### Community 96 - "FakePlayback"
Cohesion: 0.18
Nodes (7): EffectPlaybackException, FakePlayback, CompletedHandler, DisposeCount, DisposeFailure, FailedHandler, Instance

### Community 97 - "S4-06 acceptance: launch effects through shortcuts"
Cohesion: 0.21
Nodes (10): EffectChannelController, EffectShortcutTests, GlobalShortcutAdapter, IEffectSurface, S4-01 effect probe (EffectSurfaceProbe, EffectProbeHotkeys, EffectProbeInputLoad, DRAWEM_EFFECT_PROBE), S4-03 built-in effects acceptance, S4-06 acceptance: launch effects through shortcuts, SettingsSaveOperation / IActiveShortcuts.Interrupt (+2 more)

### Community 98 - "S4-08 acceptance: choose effects in Settings"
Cohesion: 0.32
Nodes (9): CollectionViewSource effect grouping, EffectOption.All (grouped by placement), Placement groups (Monitor: Confetti, Cursor: Focus ring), S4-08 acceptance: choose effects in Settings, SettingsEditorTests / SettingsViewModelTests / SettingsWindowTests, SettingsEditor (SelectEffect, ClearSlot), SettingsViewModel / SlotViewModel, S4-08 plan: choose effects in Settings (+1 more)

### Community 100 - "ISettingsSaveNotifications"
Cohesion: 0.18
Nodes (3): ISettingsSaveNotifications, RecordingNotifications, CleanupFailures

### Community 101 - "v4 / FEAT-009: implementation plan"
Cohesion: 0.20
Nodes (8): v4 / FEAT-009: acceptance criteria, v4 / FEAT-009: implementation plan, Documentation, Playback, Previous behavior, Required behavior, Settings file, v4 / FEAT-009: loop an effect and set its length limit per slot

### Community 102 - "ADR 0001: Lottie built-in effects"
Cohesion: 0.16
Nodes (13): Built-in effect, Effect, CONTEXT.md domain glossary, Imported effect, Placement (Monitor / Cursor), ADR 0001: Lottie built-in effects, Built-in Lottie Effect, Lottie Simple License (FL 9.13.21) (+5 more)

### Community 103 - "step-01-draft-release/task.md"
Cohesion: 0.22
Nodes (7): Evidence, Implementation, R4-01: acceptance criteria, Validation, R4-01: implementation plan, Task, v4 / R4-01: create a versioned draft release

### Community 104 - "drawEM v4 Roadmap: Built-in Monitor and Cursor Effects"
Cohesion: 0.16
Nodes (16): ADR 0001 Lottie Built-in Effects, drawEM v4 Roadmap: Built-in Monitor and Cursor Effects, Effect Playback Contract, v4 Release Performance Targets, VideoAction and Imported Effects (Later Phase), S4-03.1 Lottie speed gate, Continuous integration and release workflow, Clear shortcut (Ctrl+Alt+X) (+8 more)

### Community 105 - "Steps"
Cohesion: 0.20
Nodes (10): First action, R4-01: create a versioned draft release, R4-02: publish checksums and provenance, R4-03: pass first-release acceptance, R4-04: add conditional SignPath signing, R4-05: list stable releases in WinGet, R4-06: package a per-user installer, R4-07: add consent-based tray updates (+2 more)

### Community 106 - ".Capture"
Cohesion: 0.25
Nodes (4): Rejected, ShortcutCaptureRejection, RightAlt, TooFewModifiers

### Community 109 - "FakePlayer"
Cohesion: 0.29
Nodes (6): FakePlayer, Calls, DisposeFailure, IsDisposed, Sound, StopFailure

### Community 110 - "SettingsErrorCode"
Cohesion: 0.20
Nodes (10): SettingsErrorCode, DuplicateShortcut, InvalidEffectLength, InvalidShortcut, InvalidWidth, MissingEffect, MissingShortcut, MissingSound (+2 more)

### Community 112 - "S4-09 Acceptance criteria"
Cohesion: 0.39
Nodes (8): S4-09 Acceptance criteria, EffectSampleFailureRouter, EffectSampler, IEffectSampler port, Sample draft effect preview, S4-09 Windows check (unverified), S4-09 plan: preview draft effects, S4-09 Task: preview draft effects

### Community 114 - "WidthBox"
Cohesion: 0.29
Nodes (5): HexText, WidthText, HexBox, WidthBox, TextBox

### Community 115 - "WinGet manifest (zip + nested portable)"
Cohesion: 0.40
Nodes (5): GitHub release Environment, SIGNPATH_API_TOKEN, InstallerSha256, WinGet manifest (zip + nested portable), Portable ZIP drawEM-<version>-win-x64.zip

### Community 119 - "FakePicker"
Cohesion: 0.40
Nodes (4): FakePicker, Next, Picking, Picks

### Community 121 - "EffectPlacement"
Cohesion: 0.50
Nodes (3): EffectPlacement, Cursor, Monitor

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
- **511 isolated node(s):** `CompletedStrokes`, `ActiveInstance`, `Instance`, `Playback`, `Deadline` (+506 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 714 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **23 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `v3 Step 8: Publish and verify on Windows (task)` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `FEAT-005: Assisted E2E Runner`?**
  _High betweenness centrality (0.108) - this node is a cross-community bridge._
- **Are the 84 inferred relationships involving `ScreenPoint` (e.g. with `.HookCallback()` and `.PushMonitorClip()`) actually correct?**
  _`ScreenPoint` has 84 INFERRED edges - model-reasoned connections that need verification._
- **What connects `CompletedStrokes`, `ActiveInstance`, `Instance` to the rest of the system?**
  _511 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `SettingsEditorTests` be split into smaller, more focused modules?**
  _Cohesion score 0.14396135265700483 - nodes in this community are weakly interconnected._
- **Why does `drawEM v3 Roadmap: Drawing and Action Settings` connect `drawEM v3 Roadmap: Drawing and Action Settings` to `drawEM architecture`, `Stage 1 Step 2 Review`, `FEAT-005: Assisted E2E Runner`, `Stage 1 sound implementation plan`?**
  _High betweenness centrality (0.093) - this node is a cross-community bridge._
- **Should `StrokeRenderElement` be split into smaller, more focused modules?**
  _Cohesion score 0.06593406593406594 - nodes in this community are weakly interconnected._
- **Why does `Window` connect `Window` to `SettingsWindow`, `S4-08 acceptance: choose effects in Settings`, `WidthBox`?**
  _High betweenness centrality (0.088) - this node is a cross-community bridge._