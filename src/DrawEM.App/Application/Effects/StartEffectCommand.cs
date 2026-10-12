using DrawEM.App.Domain.Drawing;
using DrawEM.App.Domain.Effects;

namespace DrawEM.App.Application.Effects;

/// <summary>
/// Request to show one effect on the effect channel. The target monitor and cursor point are captured at
/// invocation and never resampled. <see cref="ConfigurationGeneration"/> names the configuration that built
/// the request, so later steps can discard requests a Save invalidated.
/// </summary>
public sealed record StartEffectCommand(
    EffectId Effect,
    EffectPlacement Placement,
    MonitorBounds Monitor,
    ScreenPoint CursorPoint,
    long ConfigurationGeneration);
