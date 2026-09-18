namespace DrawEM.App.Infrastructure;

public sealed class WpfApplicationLifetime(System.Windows.Application application) : IApplicationLifetime
{
    public void Shutdown() => application.Shutdown();
}
