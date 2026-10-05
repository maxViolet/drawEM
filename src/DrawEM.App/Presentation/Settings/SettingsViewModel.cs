using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;

namespace DrawEM.App.Presentation.Settings;

/// <summary>Asks the user for a WAV or MP3 file.</summary>
public interface ISoundFilePicker
{
    /// <returns>The selected file, or <c>null</c> when the user cancelled.</returns>
    string? Pick();
}

/// <summary>Base for the Settings window's bindable objects.</summary>
public abstract class BindableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Raises <see cref="PropertyChanged"/> for every property.</summary>
    public void RaiseAllChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));

    protected void Raise([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

/// <summary>
/// The Settings window's state over one <see cref="SettingsEditor"/> draft. Capture fields record shortcuts
/// through the global keyboard hook. Call every member on the UI thread.
/// </summary>
public sealed class SettingsViewModel : BindableBase
{
    public const string HexFormatError = "Use # followed by six hexadecimal digits, for example #FF4500.";
    public const string TooFewModifiersMessage =
        "Hold at least two of Ctrl, Left Alt, and Shift, then press a letter, digit, or F1–F12.";
    public const string RightAltMessage = "Right Alt is reserved for AltGr. Use Left Alt.";
    public static readonly string WidthFormatError = $"Use a whole number from {StrokeWidth.Min} to {StrokeWidth.Max}.";

    private readonly SettingsEditor editor;
    private readonly IShortcutCapture capture;
    private readonly ISoundFilePicker picker;
    private string hexText = "";
    private string widthText = "";
    private ShortcutFieldViewModel? capturing;

    /// <summary>Advanced whenever capture starts or stops, so a result queued for an earlier capture is dropped.</summary>
    private int captureSession;

    public SettingsViewModel(SettingsEditor editor, IShortcutCapture capture, ISoundFilePicker picker)
    {
        ArgumentNullException.ThrowIfNull(editor);
        ArgumentNullException.ThrowIfNull(capture);
        ArgumentNullException.ThrowIfNull(picker);
        this.editor = editor;
        this.capture = capture;
        this.picker = picker;
        Draw = new ShortcutFieldViewModel(this, SettingsCommand.Draw);
        Clear = new ShortcutFieldViewModel(this, SettingsCommand.Clear);
        Slots = Enumerable.Range(1, ActionSlot.Count).Select(number => new SlotViewModel(this, number)).ToArray();
        LoadTexts();
        Refresh();
    }

    public ShortcutFieldViewModel Draw { get; }

    public ShortcutFieldViewModel Clear { get; }

    public IReadOnlyList<SlotViewModel> Slots { get; }

    /// <summary>The HEX color text. A valid value updates the draft color at once.</summary>
    public string HexText
    {
        get => hexText;
        set
        {
            hexText = value ?? "";
            if (HexColor.TryParse(hexText.Trim(), out var color))
            {
                editor.SetColor(color.Value);
            }

            Refresh();
        }
    }

    public string? HexError { get; private set; }

    /// <summary>The draft color: the last valid HEX value.</summary>
    public HexColor DraftColor => editor.Color;

    /// <summary>The draft color as <c>#RRGGBB</c>, for binding.</summary>
    public string Color => editor.Color.ToString();

    public string WidthText
    {
        get => widthText;
        set
        {
            widthText = value ?? "";
            editor.SetWidth(int.TryParse(widthText.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var pixels)
                ? pixels
                : 0);
            Refresh();
        }
    }

    public string? WidthError { get; private set; }

    /// <summary>Every reason the draft cannot be saved, one per line; empty when it can.</summary>
    public string Errors { get; private set; } = "";

    public bool CanSave => Errors.Length == 0;

    /// <summary>Why the last Save failed, or <c>null</c>.</summary>
    public string? Status { get; private set; }

    private IEnumerable<ShortcutFieldViewModel> Fields => [Draw, Clear, .. Slots.Select(slot => slot.Shortcut)];

    /// <summary>Sets the draft color, for example from a color picker.</summary>
    public void SetColor(HexColor color)
    {
        editor.SetColor(color);
        hexText = color.ToString();
        Refresh();
    }

    public void RestoreDefaults()
    {
        EndCapture();
        editor.RestoreDefaults();
        LoadTexts();
        foreach (var slot in Slots)
        {
            slot.Message = null;
        }

        foreach (var field in Fields)
        {
            field.Message = null;
        }

        Status = null;
        Refresh();
    }

    /// <summary>Saves the draft.</summary>
    /// <returns><c>true</c> when the settings were saved and the window should close.</returns>
    public bool Save()
    {
        EndCapture();
        if (!CanSave)
        {
            return false;
        }

        var result = editor.Save();
        Status = (result as SettingsSaveResult.NotSaved)?.Reason;
        Refresh();
        return result is SettingsSaveResult.Saved;
    }

    /// <summary>Discards the draft and its imports. Call when the window closes; does nothing after Save.</summary>
    public void Close()
    {
        EndCapture();
        editor.Cancel();
    }

    /// <summary>Starts capture for a focused field, replacing capture for any other field.</summary>
    public void BeginCapture(ShortcutFieldViewModel field)
    {
        ArgumentNullException.ThrowIfNull(field);
        if (!field.IsEnabled)
        {
            return;
        }

        var previous = capturing;
        capturing = field;
        var session = ++captureSession;
        field.Message = null;
        previous?.RaiseAllChanged();
        field.RaiseAllChanged();
        capture.Begin(result => OnCaptureResult(session, field, result));
    }

    /// <summary>Stops capture if it is on, for example when the capturing field loses focus.</summary>
    public void EndCapture()
    {
        if (capturing is not { } field)
        {
            return;
        }

        StopCapturing();
        capture.End();
        field.RaiseAllChanged();
    }

    internal bool IsCapturing(ShortcutFieldViewModel field) => ReferenceEquals(capturing, field);

    internal Shortcut? ShortcutOf(SettingsCommand command) => editor.ShortcutOf(command);

    internal bool AcceptsShortcut(SettingsCommand command) => editor.AcceptsShortcut(command);

    internal ActionSlot SlotOf(int number) => editor.Slots[number - 1];

    internal void ChooseFile(SlotViewModel slot)
    {
        EndCapture();
        if (picker.Pick() is not { } file)
        {
            return;
        }

        try
        {
            editor.SelectSound(slot.Number, file);
            slot.Message = null;
        }
        catch (SoundImportException exception)
        {
            slot.Message = exception.Message;
        }

        Refresh();
    }

    internal void RemoveSound(SlotViewModel slot)
    {
        slot.Shortcut.EndCapture();
        editor.RemoveSound(slot.Number);
        slot.Message = null;
        slot.Shortcut.Message = null;
        Refresh();
    }

    internal void Sample(SlotViewModel slot)
    {
        slot.Message = null;
        editor.Sample(slot.Number, reason => slot.Message = $"Sample failed: {reason}");
    }

    private void OnCaptureResult(int session, ShortcutFieldViewModel field, ShortcutCaptureResult result)
    {
        if (session != captureSession)
        {
            return;
        }

        switch (result)
        {
            case ShortcutCaptureResult.Captured captured:
                StopCapturing();
                field.Message = null;
                editor.SetShortcut(field.Command, captured.Shortcut);
                break;
            case ShortcutCaptureResult.Rejected rejected:
                field.Message = rejected.Reason == ShortcutCaptureRejection.RightAlt
                    ? RightAltMessage
                    : TooFewModifiersMessage;
                break;
            default:
                StopCapturing();
                field.Message = null;
                break;
        }

        Refresh();
    }

    private void StopCapturing()
    {
        capturing = null;
        captureSession++;
    }

    private void LoadTexts()
    {
        hexText = editor.Color.ToString();
        widthText = editor.Width.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>Validates the draft and shows each error beside its field.</summary>
    private void Refresh()
    {
        var errors = editor.Validate();
        var fieldErrors = new Dictionary<SettingsCommand, string>();
        foreach (var error in errors)
        {
            if (error.Command is not { } command)
            {
                continue;
            }

            switch (error.Code)
            {
                case SettingsErrorCode.DuplicateShortcut:
                    fieldErrors.TryAdd(command, $"Same shortcut as {Name(error.Other!.Value)}.");
                    fieldErrors.TryAdd(error.Other!.Value, $"Same shortcut as {Name(command)}.");
                    break;
                case SettingsErrorCode.MissingShortcut:
                    fieldErrors.TryAdd(command, "A shortcut is required.");
                    break;
                case SettingsErrorCode.InvalidShortcut:
                    fieldErrors.TryAdd(command, "This shortcut is not valid.");
                    break;
            }
        }

        HexError = HexColor.TryParse(hexText.Trim(), out _) ? null : HexFormatError;
        WidthError = errors.Any(error => error.Code == SettingsErrorCode.InvalidWidth) ? WidthFormatError : null;
        var lines = new List<string>();
        if (HexError is not null)
        {
            lines.Add($"Color: {HexFormatError}");
        }

        lines.AddRange(errors.Select(error => error.Code == SettingsErrorCode.InvalidWidth
            ? $"Line width: {WidthFormatError}"
            : error.Describe(Name)));
        Errors = string.Join(Environment.NewLine, lines);

        foreach (var field in Fields)
        {
            field.ValidationError = fieldErrors.GetValueOrDefault(field.Command);
            field.RaiseAllChanged();
        }

        foreach (var slot in Slots)
        {
            slot.RaiseAllChanged();
        }

        RaiseAllChanged();
    }

    /// <summary>The command as the window labels it.</summary>
    public static string Name(SettingsCommand command) =>
        command == SettingsCommand.Draw ? "Hold to draw" : command.ToString();
}

/// <summary>A shortcut capture field: draw, clear, or one slot.</summary>
public sealed class ShortcutFieldViewModel : BindableBase
{
    public const string CapturePrompt = "Press a shortcut…";

    private readonly SettingsViewModel owner;
    private string? message;

    internal ShortcutFieldViewModel(SettingsViewModel owner, SettingsCommand command)
    {
        this.owner = owner;
        Command = command;
    }

    public SettingsCommand Command { get; }

    /// <summary>An empty slot has no shortcut to capture.</summary>
    public bool IsEnabled => owner.AcceptsShortcut(Command);

    public bool IsCapturing => owner.IsCapturing(this);

    public Shortcut? Shortcut => owner.ShortcutOf(Command);

    public string Text => IsCapturing ? CapturePrompt : Shortcut?.ToString() ?? "No shortcut";

    /// <summary>The capture rejection or validation error to show beside the field, or <c>null</c>.</summary>
    public string? Error => Message ?? ValidationError;

    /// <summary>Why the last captured chord was rejected.</summary>
    internal string? Message
    {
        get => message;
        set
        {
            message = value;
            Raise(nameof(Error));
        }
    }

    internal string? ValidationError { get; set; }

    public void BeginCapture() => owner.BeginCapture(this);

    /// <summary>Stops capture if this field is capturing.</summary>
    public void EndCapture()
    {
        if (IsCapturing)
        {
            owner.EndCapture();
        }
    }
}

/// <summary>One numbered action slot.</summary>
public sealed class SlotViewModel : BindableBase
{
    private readonly SettingsViewModel owner;
    private string? message;

    internal SlotViewModel(SettingsViewModel owner, int number)
    {
        this.owner = owner;
        Number = number;
        Shortcut = new ShortcutFieldViewModel(owner, SettingsCommand.ForSlot(number));
    }

    public int Number { get; }

    public string Label => Number.ToString("00", CultureInfo.InvariantCulture);

    public ShortcutFieldViewModel Shortcut { get; }

    public bool HasSound => !owner.SlotOf(Number).IsEmpty;

    public string FileName => (owner.SlotOf(Number).Action as SoundAction)?.Sound.DisplayName ?? "No sound selected";

    public string ChooseText => HasSound ? "Change file…" : "Choose WAV / MP3…";

    /// <summary>A copy or sample failure, or <c>null</c>.</summary>
    public string? Message
    {
        get => message;
        internal set
        {
            message = value;
            Raise();
        }
    }

    public void ChooseFile() => owner.ChooseFile(this);

    public void RemoveSound() => owner.RemoveSound(this);

    public void Sample() => owner.Sample(this);
}
