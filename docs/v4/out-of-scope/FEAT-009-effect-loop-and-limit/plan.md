# v4 / FEAT-009: implementation plan

See [task](task.md) and [acceptance criteria](acceptance.md).

1. **Domain.** Add `Loop` and `MaxLength` (`TimeSpan?`, `null` for No limit)
   to `EffectAction`. Validation rejects a length outside 1–60 seconds.
   Default slots get `Loop = false`, `MaxLength = 10 s`.
2. **Settings file.** Bump `SettingsJson.CurrentSchemaVersion` to 3. Write
   `loop` and `maxSeconds`. Read schema 1 and 2 effect slots with the
   defaults. Update `Infrastructure/Settings/README.md`.
3. **Command.** Carry `Loop` and `MaxLength` on `StartEffectCommand`.
4. **Channel.** Replace the `EffectChannelController.MaxDuration` constant with
   the command's limit: no deadline timer when Loop is on or the limit is
   `null`. Add a toggle start: when the active instance comes from the same
   slot and that slot loops, stop it instead of restarting.
5. **Playback.** `LottiePlayback` plays from frame 0 again at the end of the
   file when the command loops, instead of raising `Completed`.
6. **Settings UI.** Add the second line to the slot template in
   `SettingsWindow.xaml`: Loop check box, Max length stepper reusing the
   line-width stepper pattern, No limit check box. Bind enable states.
7. **Sample.** For a looping slot, Sample toggles: the button reads `Stop`
   while that slot's Sample instance runs.
8. **Docs.** Update the playback rules in the
   [v4 roadmap](../../visual-effects/ROADMAP-v4.md).
9. Run `dotnet test DrawEM.sln` and `graphify update .`.
