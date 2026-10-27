using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using DrawEM.App.Application.Settings;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;
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


    public bool BezierSmoothing
    {
        get => editor.Smoothing == StrokeSmoothing.Bezier;
        set
        {
            editor.SetSmoothing(value ? StrokeSmoothing.Bezier : StrokeSmoothing.None);
            Refresh();
        }
    }
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

    /// <summary>
    /// Disabled only at the limit itself, as <see cref="StepWidth"/> is: for invalid or out-of-range text both
    /// buttons stay enabled, and a step brings the width back into range.
    /// </summary>
    public bool CanIncreaseWidth => editor.Width != StrokeWidth.Max;

    /// <inheritdoc cref="CanIncreaseWidth"/>
    public bool CanDecreaseWidth => editor.Width != StrokeWidth.Min;

    /// <summary>
    /// Changes the width by <paramref name="pixels"/>, kept within the allowed range. Text that is not a whole
    /// number becomes the minimum width.
    /// </summary>
    public void StepWidth(int pixels)
    {
        var stepped = int.TryParse(widthText.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out var current)
            ? Math.Clamp((long)current + pixels, StrokeWidth.Min, StrokeWidth.Max)
            : StrokeWidth.Min;
        WidthText = stepped.ToString(CultureInfo.InvariantCulture);
    }

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
            slot.EmptyKind = SlotKind.Sound;
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

    /// <summary>
    /// Switches the slot between sound and effect. An empty slot only shows the other selector. A sound slot
    /// takes the first effect; an effect slot asks for a file and stays unchanged if none is copied. Either
    /// keeps the slot's shortcut. Always notifies, so a type selector the model refused shows the kept type.
    /// </summary>
    internal void ChangeKind(SlotViewModel slot, SlotKind kind)
    {
        if (kind == slot.Kind)
        {
            return;
        }

        if (!slot.HasAction)
        {
            EndCapture();
            slot.EmptyKind = kind;
            slot.Message = null;
        }
        else if (kind == SlotKind.Effect)
        {
            SelectEffect(slot, EffectOption.All[0]);
        }
        else
        {
            ChooseFile(slot);
        }

        Refresh();
    }

    internal void SelectEffect(SlotViewModel slot, EffectOption effect)
    {
        EndCapture();
        editor.SelectEffect(slot.Number, effect.Id);
        slot.Message = null;
        Refresh();
    }

    internal void SetEffectLoop(SlotViewModel slot, bool loop)
    {
        editor.SetEffectLoop(slot.Number, loop);
        Refresh();
    }

    internal void SetEffectMaxLength(SlotViewModel slot, TimeSpan? maxLength)
    {
        editor.SetEffectMaxLength(slot.Number, maxLength);
        Refresh();
    }

    /// <summary>Empties the slot and its shortcut; the row keeps showing the same action type.</summary>
    internal void ClearSlot(SlotViewModel slot)
    {
        slot.Shortcut.EndCapture();
        slot.EmptyKind = slot.Kind;
        editor.ClearSlot(slot.Number);
        slot.Message = null;
        slot.Shortcut.Message = null;
        Refresh();
    }

    internal void Sample(SlotViewModel slot)
    {
        slot.Message = null;
        editor.Sample(
            slot.Number,
            reason => slot.Message = $"Sample failed: {reason}",
            slot.SetSampleRunning);
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

/// <summary>What a slot does when filled.</summary>
public enum SlotKind
{
    Sound,
    Effect,
}

/// <summary>One choice in a slot's effect list. <see cref="Group"/> is the effect's placement.</summary>
public sealed record EffectOption(EffectId Id, string Name, string Group)
{
    /// <summary>Every built-in effect, Monitor effects first, then Cursor effects.</summary>
    public static IReadOnlyList<EffectOption> All { get; } = EffectCatalog.BuiltIn
        .OrderBy(effect => effect.Placement)
        .Select(effect => new EffectOption(effect.Id, effect.DisplayName, effect.Placement.ToString()))
        .ToArray();
}

/// <summary>One numbered action slot.</summary>
public sealed class SlotViewModel : BindableBase
{
    private readonly SettingsViewModel owner;
    private string? message;
    private bool sampleRunning;

    internal SlotViewModel(SettingsViewModel owner, int number)
    {
        this.owner = owner;
        Number = number;
        Shortcut = new ShortcutFieldViewModel(owner, SettingsCommand.ForSlot(number));
    }

    public static IReadOnlyList<SlotKind> Kinds { get; } = [SlotKind.Sound, SlotKind.Effect];

    public int Number { get; }

    public string Label => Number.ToString("00", CultureInfo.InvariantCulture);

    public ShortcutFieldViewModel Shortcut { get; }

    /// <summary>The filled slot's action type, or the type chosen for an empty slot.</summary>
    public SlotKind Kind => Action switch
    {
        SoundAction => SlotKind.Sound,
        EffectAction => SlotKind.Effect,
        _ => EmptyKind,
    };

    public bool IsSound => Kind == SlotKind.Sound;

    public bool IsEffect => Kind == SlotKind.Effect;

    public bool HasEffect => Action is EffectAction;

    public bool HasAction => Action is not null;

    public bool HasSound => Action is SoundAction;

    public string FileName => (Action as SoundAction)?.Sound.DisplayName ?? "No sound selected";

    public string ChooseText => HasSound ? "Change file…" : "Choose file…";

    /// <summary>The slot's effect, or <c>null</c>. Setting a different one changes only the draft.</summary>
    public EffectOption? Effect
    {
        get => Action is EffectAction effect ? EffectOption.All.FirstOrDefault(option => option.Id == effect.Effect) : null;
        set
        {
            if (value is not null && value != Effect)
            {
                owner.SelectEffect(this, value);
            }
        }
    }

    public bool Loop
    {
        get => EffectSettings?.Loop ?? false;
        set
        {
            if (HasEffect && value != Loop)
            {
                owner.SetEffectLoop(this, value);
            }
        }
    }

    public bool NoLimit
    {
        get => HasEffect && MaxLength is null;
        set
        {
            if (!HasEffect || Loop || value == NoLimit)
            {
                return;
            }

            owner.SetEffectMaxLength(this, value ? null : DrawEM.App.Domain.Settings.EffectAction.DefaultMaxLength);
        }
    }

    public string MaxLengthText => MaxLength is { } length
        ? $"{(int)length.TotalSeconds} s"
        : "No limit";

    public bool CanSetNoLimit => HasEffect && !Loop;

    public bool CanEditMaxLength => HasEffect && !Loop && MaxLength is not null;

    public bool CanIncreaseMaxLength =>
        CanEditMaxLength && MaxLength!.Value.TotalSeconds != DrawEM.App.Domain.Settings.EffectAction.MaxSeconds;

    public bool CanDecreaseMaxLength =>
        CanEditMaxLength && MaxLength!.Value.TotalSeconds != DrawEM.App.Domain.Settings.EffectAction.MinSeconds;

    public string SampleText => sampleRunning ? "Stop" : "Sample";

    public void StepMaxLength(int seconds)
    {
        if (!CanEditMaxLength)
        {
            return;
        }

        var current = (int)MaxLength!.Value.TotalSeconds;
        var stepped = Math.Clamp(
            (long)current + seconds,
            DrawEM.App.Domain.Settings.EffectAction.MinSeconds,
            DrawEM.App.Domain.Settings.EffectAction.MaxSeconds);
        owner.SetEffectMaxLength(this, TimeSpan.FromSeconds(stepped));
    }

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

    /// <summary>The type an empty slot shows.</summary>
    internal SlotKind EmptyKind { get; set; }

    private SlotAction? Action => owner.SlotOf(Number).Action;

    private EffectAction? EffectSettings => Action as EffectAction;

    private TimeSpan? MaxLength => EffectSettings?.MaxLength;


    internal void SetSampleRunning(bool running)
    {
        sampleRunning = running;
        Raise(nameof(SampleText));
    }

    public void ChangeKind(SlotKind kind) => owner.ChangeKind(this, kind);

    public void ChooseFile() => owner.ChooseFile(this);

    public void ClearSlot() => owner.ClearSlot(this);

    public void Sample() => owner.Sample(this);
}
