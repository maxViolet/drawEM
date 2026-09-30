using DrawEM.App.Domain.Drawing;
using NetArchTest.Rules;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DrawEM.Tests.Architecture;

[Trait("Category", "Architecture")]
public class LayerDependencyTests
{
    private const string Domain = "DrawEM.App.Domain";
    private const string Application = "DrawEM.App.Application";
    private const string Infrastructure = "DrawEM.App.Infrastructure";
    private const string Presentation = "DrawEM.App.Presentation";

    public static TheoryData<string, string[]> AllowedLayerDependencies => new()
    {
        { Domain, ["System", "Microsoft", Domain] },
        { Application, ["System", "Microsoft", Domain, Application] },
        { Infrastructure, ["System", "Microsoft", Domain, Application, Infrastructure] },
        // OverlayWindow implements this existing tray-lifecycle port. Other infrastructure
        // dependencies remain forbidden; do not exempt the whole window or namespace.
        { Presentation, ["System", "Microsoft", Domain, Application, Presentation,
            "DrawEM.App.Infrastructure.IOverlayLifetime"] },
    };

    [Theory]
    [MemberData(nameof(AllowedLayerDependencies))]
    public void Layers_OnlyDependOnAllowedNamespaces(string layer, string[] allowedDependencies)
    {
        var types = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().ResideInNamespaceStartingWith(layer);

        Assert.NotEmpty(types.GetTypes());
        var result = types.Should().OnlyHaveDependenciesOn(allowedDependencies).GetResult();

        Assert.True(result.IsSuccessful,
            $"{layer} may depend only on: {string.Join(", ", allowedDependencies)}. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData(Domain)]
    [InlineData(Application)]
    public void InnerLayers_DoNotDependOnWindowsFrameworks(string layer)
    {
        var result = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().ResideInNamespaceStartingWith(layer)
            .ShouldNot().HaveDependencyOnAny(
                "System.Windows", "System.Drawing", "Microsoft.Win32", "Microsoft.UI", "Windows", "WinRT")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{layer} must be independent of Windows UI and platform frameworks. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData(Domain)]
    [InlineData(Application)]
    public void InnerLayers_DoNotDependOnFileSystem(string layer)
    {
        // Settings and media ports pass names and paths as strings; file access stays in Infrastructure.
        var result = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().ResideInNamespaceStartingWith(layer)
            .ShouldNot().HaveDependencyOn("System.IO")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{layer} must not depend on filesystem types. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData(Domain)]
    [InlineData(Application)]
    public void InnerLayers_DoNotDeclareNativeImports(string layer)
    {
        // Runtime.InteropServices also contains managed helpers (for example CollectionsMarshal
        // emitted for collection expressions). Check native import declarations specifically.
        var violations = typeof(DrawingState).Assembly.GetTypes()
            .Where(type => type.Namespace == layer || type.Namespace?.StartsWith(layer + ".", StringComparison.Ordinal) == true)
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(method => (method.Attributes & MethodAttributes.PinvokeImpl) != 0 ||
                method.IsDefined(typeof(LibraryImportAttribute), inherit: false))
            .Select(method => $"{method.DeclaringType!.FullName}.{method.Name}")
            .ToArray();

        Assert.True(violations.Length == 0,
            $"{layer} must not declare native imports. Violating methods: {string.Join(", ", violations)}");
    }

    [Theory]
    [InlineData(Domain)]
    [InlineData(Application)]
    public void InnerLayers_DoNotLoadNativeLibraries(string layer)
    {
        var result = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().ResideInNamespaceStartingWith(layer)
            .ShouldNot().HaveDependencyOn("System.Runtime.InteropServices.NativeLibrary")
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{layer} must not load native libraries. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Theory]
    [InlineData(Domain)]
    [InlineData(Application)]
    public void InnerLayers_DoNotConvertNativeFunctionPointers(string layer)
    {
        var result = Types.InAssembly(typeof(DrawingState).Assembly)
            .That().ResideInNamespaceStartingWith(layer)
            .Should().MeetCustomRule(new NativeFunctionBridgeRule())
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"{layer} must not convert native function pointers to or from delegates. " +
            $"Violating types: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}
