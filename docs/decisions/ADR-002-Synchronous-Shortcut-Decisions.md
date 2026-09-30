# ADR-002: Synchronous, platform-neutral shortcut decisions

## Status

Accepted

## Date

2026-09-30

## Context

drawEM decides every keyboard event inside a Windows low-level keyboard hook
(`WH_KEYBOARD_LL`). The hook callback must return whether to hide the event
before Windows passes it on, and it must return quickly: a slow callback is
skipped, and repeated timeouts can remove the hook
([LowLevelKeyboardProc](https://learn.microsoft.com/en-us/windows/win32/winmsg/lowlevelkeyboardproc)).

Until v3, `GlobalShortcutAdapter` combined three concerns: Win32 key codes,
pressed-key state and chord resolution, and side effects on the drawing gate,
controller, and sound channel. v3 Steps 3–4 add configurable bindings, Right
Alt reservation, capture mode, and neutral-key injection. Save (Step 6) must
require a fresh press for keys held across it. Testing that logic through
Win32 codes and side-effecting adapters would couple every test to the hook.

The hook source also exposed three events (`KeyDown`, `KeySuppressionRequested`,
`KeyUp`) with an ordering contract, so one key event had no single answer.

## Decision

Put shortcut decisions in `ShortcutDecisionEngine`
(`DrawEM.App.Application.Input`). For each key event it returns, synchronously,
one `ShortcutDecision`: whether to suppress the event, and an ordered list of
`ShortcutCommand`s (`BeginDraw`, `EndDraw`, `ReleaseDrawBlock`,
`ClearMonitor`, `PlaySlot`). It works on platform-neutral `InputKey` values
(modifier, shortcut candidate, or opaque other key) and does no I/O.

The drawing gate can also be closed by the mouse hook, so the engine does not
own it. The adapter passes the gate's state (`DrawGateStatus`) with each event.

`IKeyboardHookSource` raises one event per key event,
`KeyEvent(vkCode, direction) → bool`, whose return value is the suppression
answer.

Interfaces and where they live:

| Type | Layer | Role |
|---|---|---|
| `InputKey`, `KeyDirection`, `ShortcutDecisionEngine`, `ShortcutDecision`, `ShortcutCommand` | Application | Key state, chord resolution, suppression, fresh-press rule |
| `IKeyboardHookSource`, `Win32KeyboardHookSource` | Infrastructure | One callback per hook event |
| `VirtualKeys.ToInputKey`, `GlobalShortcutAdapter` | Infrastructure | Win32 translation and command execution |

## Adapter guarantees

The hook adapter (`GlobalShortcutAdapter`) must:

- Call the engine inside the hook callback, once per event, and return the
  decision's `Suppress` value before the callback returns.
- Translate side-specific modifiers exactly (Left/Right Ctrl, Alt, Shift, Win),
  so the engine can tell Right Alt (AltGr) from Left Alt. Keep side-neutral and
  unsupported codes opaque.
- Perform commands in order and without blocking: update the input gate
  directly, queue UI work on the dispatcher, and queue sound work on the sound
  thread. No file access, decoding, or player creation in the callback.
- Pass the gate's current state with every event.
- Own platform-only input behavior: neutral-key (`VK 0xE8`) injection for
  suppressed `Alt+Shift`/`Ctrl+Shift` candidates (Step 4), and tagging injected
  events so they never reach the engine.
- Share its engine instance with `SettingsSaver`, and run Save on the hook's
  thread, so `RequireFreshPress` and publication of the new snapshot happen
  between two key events.

## Rationale

- **Keep everything in the adapter:** rejected. Steps 3–4 would add
  configurable bindings, capture, and AltGr rules to a class that also owns
  Win32 codes and side effects, testable only through fakes of the hook.
- **Queue events and decide asynchronously:** rejected. The hook needs the
  suppression answer before it returns; a queued decision cannot hide the
  event it is about.
- **Engine calls side-effecting ports (for example `TryBeginDraw`):**
  rejected for now. Returning commands keeps the engine free of callbacks and
  easy to test. The cost is described below.
- **Keep the three-event hook contract:** rejected. Splitting one event into a
  state update and a later suppression query allowed answers for events that
  never happened and made the ordering part of every caller's contract.

## Consequences

**Easier:**
- Steps 3–4 change bindings, Right Alt handling, and capture in one
  synchronous class with direct tests on `InputKey` sequences.
- Save's fresh-press rule is one engine method, tested through
  `ISettingsSaver`.

**Harder:**
- The engine predicts the gate after `BeginDraw`. If the cursor or monitor
  cannot be found, drawing does not start, but a non-chord key pressed in that
  same event is still suppressed. The pre-v3 adapter let that key through.
  Making this exact would require the adapter to report command results back.
- Every new side effect needs a new `ShortcutCommand` and adapter handling.
