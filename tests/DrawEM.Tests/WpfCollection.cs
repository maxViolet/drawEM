namespace DrawEM.Tests;

/// <summary>
/// Runs tests that load WPF pack:// resources one at a time.
/// WPF shares one resource package across threads, and System.IO.Packaging is not thread-safe.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class WpfCollection
{
    public const string Name = "Wpf";
}
