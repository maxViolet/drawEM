namespace DrawEM.App.Domain.Effects;

/// <summary>Where an effect appears on its monitor. Fixed per built-in effect.</summary>
public enum EffectPlacement
{
    /// <summary>Fills the whole target monitor.</summary>
    Monitor,

    /// <summary>Fixed size, centered on the cursor point captured at start.</summary>
    Cursor,
}
