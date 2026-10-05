using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Settings;
using DrawEM.App.Domain.Sound;

namespace DrawEM.App.Infrastructure.Settings;

/// <summary>The data cannot become a settings snapshot. The message is shown to the user.</summary>
public sealed class SettingsFormatException : Exception
{
    public SettingsFormatException(string message)
        : base(message)
    {
    }

    public SettingsFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Reads and writes the settings file schema described in <c>README.md</c>. Writing always produces
/// <see cref="CurrentSchemaVersion"/>.
/// </summary>
public static class SettingsJson
{
    public const int CurrentSchemaVersion = 1;

    public static byte[] Serialize(SettingsSnapshot snapshot)
    {
        using var buffer = new MemoryStream();
        using (var json = new Utf8JsonWriter(buffer, new JsonWriterOptions
        {
            Indented = true,
            // The file is not embedded in HTML; keep '+' and non-ASCII file names readable.
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        }))
        {
            json.WriteStartObject();
            json.WriteNumber("schemaVersion", CurrentSchemaVersion);

            json.WriteStartObject("drawing");
            json.WriteString("color", snapshot.Style.Color.ToString());
            json.WriteNumber("width", snapshot.Style.Width.Pixels);
            json.WriteString("drawShortcut", snapshot.DrawShortcut.ToString());
            json.WriteString("clearShortcut", snapshot.ClearShortcut.ToString());
            json.WriteEndObject();

            json.WriteStartArray("slots");
            foreach (var slot in snapshot.Slots)
            {
                json.WriteStartObject();
                json.WriteNumber("slot", slot.Number);
                switch (slot.Action)
                {
                    case null:
                        json.WriteNull("action");
                        break;
                    case SoundAction sound:
                        json.WriteStartObject("action");
                        json.WriteString("type", "sound");
                        json.WriteString("file", sound.Sound.LibraryFileName);
                        json.WriteString("name", sound.Sound.DisplayName);
                        json.WriteString("shortcut", sound.Shortcut!.ToString());
                        json.WriteEndObject();
                        break;
                    default:
                        throw new NotSupportedException($"Action type {slot.Action.GetType().Name} has no schema.");
                }

                json.WriteEndObject();
            }

            json.WriteEndArray();
            json.WriteEndObject();
        }

        return buffer.ToArray();
    }

    /// <exception cref="SettingsFormatException">
    /// The data is not JSON, has an unsupported schema version, or does not describe valid settings.
    /// </exception>
    public static SettingsSnapshot Deserialize(ReadOnlyMemory<byte> data)
    {
        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(data);
        }
        catch (JsonException failure)
        {
            throw new SettingsFormatException("Settings file is not valid JSON.", failure);
        }

        using (document)
        {
            var root = Object(document.RootElement, "settings");
            var version = Int(root, "schemaVersion");
            if (version != CurrentSchemaVersion)
            {
                throw new SettingsFormatException(
                    $"Settings schema version {version} is not supported; this drawEM reads version {CurrentSchemaVersion}.");
            }

            var drawing = Object(Property(root, "drawing"), "drawing");
            var colorText = String(drawing, "color");
            if (!HexColor.TryParse(colorText, out var color))
            {
                throw new SettingsFormatException($"drawing.color '{colorText}' is not a #RRGGBB color.");
            }

            var width = Int(drawing, "width");
            if (!StrokeWidth.IsValid(width))
            {
                throw new SettingsFormatException(
                    $"drawing.width {width} is outside {StrokeWidth.Min}–{StrokeWidth.Max}.");
            }

            var style = new DrawingStyle(color.Value, new StrokeWidth(width));
            var draw = ShortcutProperty(drawing, "drawShortcut");
            var clear = ShortcutProperty(drawing, "clearShortcut");

            var slotsElement = Property(root, "slots");
            if (slotsElement.ValueKind != JsonValueKind.Array)
            {
                throw new SettingsFormatException("slots must be an array.");
            }

            var slots = slotsElement.EnumerateArray().Select(ReadSlot).ToArray();
            var validation = SettingsSnapshot.Validate(style, draw, clear, slots);
            return validation.Snapshot ?? throw new SettingsFormatException(
                "Settings are invalid: " + string.Join(" ", validation.Errors));
        }
    }

    private static ActionSlot ReadSlot(JsonElement element)
    {
        var slot = Object(element, "slot entry");
        var number = Int(slot, "slot");
        if (number is < 1 or > ActionSlot.Count)
        {
            throw new SettingsFormatException($"Slot number {number} is outside 1–{ActionSlot.Count}.");
        }

        var action = Property(slot, "action");
        if (action.ValueKind == JsonValueKind.Null)
        {
            return ActionSlot.Empty(number);
        }

        Object(action, $"slot {number} action");
        var type = String(action, "type");
        if (type != "sound")
        {
            throw new SettingsFormatException($"Slot {number} action type '{type}' is not supported.");
        }

        SoundReference sound;
        try
        {
            sound = new SoundReference(String(action, "file"), String(action, "name"));
        }
        catch (ArgumentException failure)
        {
            throw new SettingsFormatException($"Slot {number} sound is invalid: {failure.Message}", failure);
        }

        return new ActionSlot(number, new SoundAction(sound, ShortcutProperty(action, "shortcut")));
    }

    private static Shortcut ShortcutProperty(JsonElement element, string name)
    {
        var text = String(element, name);
        return Shortcut.TryParse(text, out var shortcut, out var error)
            ? shortcut
            : throw new SettingsFormatException($"{name} '{text}' is not a valid shortcut ({error}).");
    }

    private static JsonElement Object(JsonElement element, string name) =>
        element.ValueKind == JsonValueKind.Object
            ? element
            : throw new SettingsFormatException($"{name} must be an object.");

    private static JsonElement Property(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value)
            ? value
            : throw new SettingsFormatException($"Missing property '{name}'.");

    private static string String(JsonElement element, string name)
    {
        var value = Property(element, name);
        return value.ValueKind == JsonValueKind.String
            ? value.GetString()!
            : throw new SettingsFormatException($"'{name}' must be a string.");
    }

    private static int Int(JsonElement element, string name)
    {
        var value = Property(element, name);
        return value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number)
            ? number
            : throw new SettingsFormatException($"'{name}' must be an integer.");
    }
}
