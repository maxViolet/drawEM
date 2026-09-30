namespace DrawEM.App.Domain.Drawing;

/// <summary>
/// Stroke style applied to every stroke. Single source of truth for color and thickness;
/// not user-configurable in this version (see docs/v1/ROADMAP-v1.md).
/// </summary>
public static class DrawingDefaults
{
    /// <summary>Stroke color: OrangeRed #FF4500 (RGB 255, 69, 0).</summary>
    public const DrawingColor StrokeColor = DrawingColor.OrangeRed;

    /// <summary>Stroke width interpreted by the current WPF renderer as DIPs; a single-point dot has this diameter.</summary>
    public const int StrokeThickness = 4;
}
