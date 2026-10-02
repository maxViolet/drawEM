namespace DrawEM.App.Infrastructure;

/// <summary>Queues a tagged neutral key-down and key-up while the shortcut modifiers are held.</summary>
public interface INeutralKeyEmitter
{
    void EmitNeutralKey();
}
