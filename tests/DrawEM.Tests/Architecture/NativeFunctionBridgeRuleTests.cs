using System.Runtime.InteropServices;
using NetArchTest.Rules;

namespace DrawEM.Tests.Architecture;

[Trait("Category", "Architecture")]
public class NativeFunctionBridgeRuleTests
{
    [Theory]
    [InlineData(nameof(PointerToDelegateProbe), false)]
    [InlineData(nameof(DelegateToPointerProbe), false)]
    [InlineData(nameof(ClosureProbe), false)]
    [InlineData(nameof(AsyncProbe), false)]
    [InlineData(nameof(IteratorProbe), false)]
    [InlineData(nameof(ManagedCollectionProbe), true)]
    public void Rule_DistinguishesNativeFunctionBridgesFromManagedHelpers(string typeName, bool expected)
    {
        var types = Types.InAssembly(typeof(NativeFunctionBridgeRuleTests).Assembly)
            .That().HaveName(typeName);
        Assert.Single(types.GetTypes());

        var result = types.Should().MeetCustomRule(new NativeFunctionBridgeRule()).GetResult();

        Assert.Equal(expected, result.IsSuccessful);
    }

    private sealed class PointerToDelegateProbe
    {
        public Action Convert(nint pointer) => Marshal.GetDelegateForFunctionPointer<Action>(pointer);
    }

    private sealed class DelegateToPointerProbe
    {
        public nint Convert(Action callback) => Marshal.GetFunctionPointerForDelegate(callback);
    }

    private sealed class ClosureProbe
    {
        public Func<Action> ConvertLater(nint pointer) => () => Marshal.GetDelegateForFunctionPointer<Action>(pointer);
    }

    private sealed class ManagedCollectionProbe
    {
        public int Count(List<int> values) => CollectionsMarshal.AsSpan(values).Length;
    }

    private sealed class AsyncProbe
    {
        public async Task<Action> Convert(nint pointer)
        {
            await Task.Yield();
            return Marshal.GetDelegateForFunctionPointer<Action>(pointer);
        }
    }

    private sealed class IteratorProbe
    {
        public IEnumerable<Action> Convert(nint pointer)
        {
            yield return Marshal.GetDelegateForFunctionPointer<Action>(pointer);
        }
    }
}
