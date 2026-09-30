using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Input;

/// <summary>Whether a keyboard event presses (including auto-repeat) or releases a key.</summary>
public enum KeyDirection
{
    Down,
    Up,
}

public enum ModifierKey
{
    LeftControl,
    RightControl,
    LeftAlt,
    RightAlt,
    LeftShift,
    RightShift,
    LeftWindows,
    RightWindows,
}

public enum InputKeyKind
{
    Modifier,

    /// <summary>A key that can end a shortcut: A–Z, 0–9, or F1–F12.</summary>
    Candidate,

    /// <summary>Any other key. Tracked only for suppression; its code is opaque.</summary>
    Other,
}

/// <summary>
/// A physical key, independent of the platform. The hook adapter translates platform key codes into
/// this type; shortcut decisions never see a platform code except as an opaque <see cref="Other"/> value.
/// </summary>
public readonly record struct InputKey
{
    private InputKey(InputKeyKind kind, ModifierKey modifier, ShortcutKey candidate, int otherCode)
    {
        Kind = kind;
        ModifierKey = modifier;
        CandidateKey = candidate;
        OtherCode = otherCode;
    }

    public InputKeyKind Kind { get; }

    public ModifierKey ModifierKey { get; }

    public ShortcutKey CandidateKey { get; }

    public int OtherCode { get; }

    public static InputKey Modifier(ModifierKey key) => new(InputKeyKind.Modifier, key, default, 0);

    /// <exception cref="ArgumentException"><paramref name="key"/> is not a legal shortcut key.</exception>
    public static InputKey Candidate(ShortcutKey key) =>
        ShortcutKey.IsValid(key)
            ? new(InputKeyKind.Candidate, default, key, 0)
            : throw new ArgumentException("Candidate must be A–Z, 0–9, or F1–F12.", nameof(key));

    /// <param name="code">An opaque platform code that identifies the key between its down and up events.</param>
    public static InputKey Other(int code) => new(InputKeyKind.Other, default, default, code);

    public override string ToString() => Kind switch
    {
        InputKeyKind.Modifier => ModifierKey.ToString(),
        InputKeyKind.Candidate => CandidateKey.ToString(),
        _ => $"Other({OtherCode})",
    };
}
