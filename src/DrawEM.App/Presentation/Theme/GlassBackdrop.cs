using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace DrawEM.App.Presentation.Theme;

/// <summary>
/// Gives a window the glass look: the theme's brushes, and the system Acrylic backdrop where Windows has it.
/// Without the backdrop the window uses the theme's opaque surface instead.
/// </summary>
public static class GlassBackdrop
{
    /// <summary>Resource key of the window surface; translucent over the backdrop, opaque without it.</summary>
    public const string SurfaceKey = "Glass.Surface";

    private const string SolidSurfaceKey = "Glass.SurfaceSolid";

    /// <summary>System backdrops arrived in Windows 11 22H2, build 22621.</summary>
    public static bool IsSupported(Version osVersion) => osVersion.Major > 10 || osVersion.Build >= 22621;

    private static bool Supported => IsSupported(Environment.OSVersion.Version);

    /// <summary>
    /// Applies <paramref name="theme"/> to <paramref name="window"/> now and on every change, until the window
    /// closes. Call from the constructor, after InitializeComponent. The window's first merged dictionary is
    /// the theme's brushes.
    /// </summary>
    /// <param name="captionHeight">The height of the window's own title bar; 0 for a window without one.</param>
    public static void Attach(Window window, UiThemeState theme, double captionHeight, Thickness resizeBorder)
    {
        WindowChrome.SetWindowChrome(window, new WindowChrome
        {
            CaptionHeight = captionHeight,
            ResizeBorderThickness = resizeBorder,
            CornerRadius = default,
            // The frame extends over the whole window, so the backdrop shows through it.
            GlassFrameThickness = Supported ? new Thickness(-1) : default,
            UseAeroCaptionButtons = false,
        });
        window.Background = Supported ? Brushes.Transparent : window.Background;

        void Apply()
        {
            ApplyBrushes(window, theme.Current);
            ApplyFrame(window, theme.Current);
        }

        Apply();
        window.SourceInitialized += (_, _) =>
        {
            HideSystemButtons(window);
            ApplyFrame(window, theme.Current);
        };
        theme.Changed += Apply;
        window.Closed += (_, _) => theme.Changed -= Apply;
    }

    /// <summary>
    /// The frame extends under the window's own title bar, so the system would draw its window buttons
    /// behind the translucent surface. Without the system menu it draws none; the window has its own.
    /// </summary>
    private static void HideSystemButtons(Window window)
    {
        if (!Supported)
        {
            return;
        }

        var handle = new WindowInteropHelper(window).Handle;
        var style = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_STYLE);
        NativeMethods.SetWindowLong(handle, NativeMethods.GWL_STYLE, style & ~NativeMethods.WS_SYSMENU);
    }

    private static void ApplyBrushes(Window window, UiTheme theme)
    {
        ResourceDictionary brushes = theme == UiTheme.Light ? new GlassLight() : new GlassDark();
        window.Resources.MergedDictionaries[0] = brushes;
        if (!Supported)
        {
            window.Resources[SurfaceKey] = brushes[SolidSurfaceKey];
        }
    }

    private static void ApplyFrame(Window window, UiTheme theme)
    {
        var handle = new WindowInteropHelper(window).Handle;
        if (handle == IntPtr.Zero || !Supported)
        {
            return;
        }

        ClearCompositionBackground(handle);
        SetFrameAttributes(handle, theme);
    }

    private static void ClearCompositionBackground(IntPtr handle)
    {
        if (HwndSource.FromHwnd(handle)?.CompositionTarget is { } target)
        {
            target.BackgroundColor = Colors.Transparent;
        }
    }

    private static void SetFrameAttributes(IntPtr handle, UiTheme theme)
    {
        var dark = theme == UiTheme.Dark ? 1 : 0;
        var corners = NativeMethods.DWMWCP_ROUND;
        var backdrop = NativeMethods.DWMSBT_TRANSIENTWINDOW;
        // A failed call leaves the default frame; the window stays usable, so the result is not checked.
        NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));
        NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWA_WINDOW_CORNER_PREFERENCE, ref corners, sizeof(int));
        NativeMethods.DwmSetWindowAttribute(handle, NativeMethods.DWMWA_SYSTEMBACKDROP_TYPE, ref backdrop, sizeof(int));
    }
}
