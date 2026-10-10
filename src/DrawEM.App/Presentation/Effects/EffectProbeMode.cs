namespace DrawEM.App.Presentation.Effects;

public enum EffectProbePlacement
{
    Monitor,
    Cursor,
}

/// <summary>
/// What the S4-03.1 probe measures, read from <see cref="Variable"/> at startup: <c>monitor</c> (default),
/// <c>monitor-half</c> (Monitor at half resolution, the roadmap's first fallback), or <c>cursor</c>.
/// </summary>
/// <param name="RenderScale">Bitmap pixels per physical window pixel; WPF scales the bitmap up to the window.</param>
public sealed record EffectProbeMode(EffectProbePlacement Placement, double RenderScale, string Name)
{
    public const string Variable = "DRAWEM_EFFECT_PROBE_PLACEMENT";

    public static EffectProbeMode Parse(string? value) => value?.ToLowerInvariant() switch
    {
        null or "" or "monitor" => new(EffectProbePlacement.Monitor, 1.0, "monitor"),
        "monitor-half" => new(EffectProbePlacement.Monitor, 0.5, "monitor-half"),
        "cursor" => new(EffectProbePlacement.Cursor, 1.0, "cursor"),
        _ => throw new ArgumentException(
            $"{Variable} must be monitor, monitor-half, or cursor; got '{value}'.", nameof(value)),
    };
}
