using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Application.Settings;

/// <summary>Why a captured chord was not recorded. The draft keeps its previous shortcut.</summary>
public enum ShortcutCaptureRejection
{
    /// <summary>Fewer than two of Ctrl, Alt, and Shift, for example <c>Ctrl+C</c>.</summary>
    TooFewModifiers,

    /// <summary>Right Alt was held. It is reserved for AltGr, so the key passed through.</summary>
    RightAlt,
}

/// <summary>One outcome of shortcut capture, reported to the focused capture field.</summary>
public abstract record ShortcutCaptureResult
{
    private ShortcutCaptureResult()
    {
    }

    /// <summary>A valid chord. Capture has ended.</summary>
    public sealed record Captured(Shortcut Shortcut) : ShortcutCaptureResult;

    /// <summary>An invalid chord. Capture stays on and rearms once all keys of the attempt are released.</summary>
    public sealed record Rejected(ShortcutCaptureRejection Reason) : ShortcutCaptureResult;

    /// <summary>Escape was pressed. Capture has ended.</summary>
    public sealed record Cancelled() : ShortcutCaptureResult;
}

/// <summary>
/// Records one shortcut through the global keyboard hook while a capture field has focus. While capture is
/// on, and until every key held when it ends is released, no shortcut draws, clears, or plays a sound.
/// </summary>
public interface IShortcutCapture
{
    /// <summary>
    /// Starts capture for a focused field, replacing any capture in progress. Keys already held must be
    /// released before a chord is evaluated.
    /// </summary>
    /// <param name="report">Receives each outcome through the UI dispatcher, never inside the hook callback.</param>
    void Begin(Action<ShortcutCaptureResult> report);

    /// <summary>Stops capture when the field loses focus. Does nothing if capture already ended.</summary>
    void End();
}
