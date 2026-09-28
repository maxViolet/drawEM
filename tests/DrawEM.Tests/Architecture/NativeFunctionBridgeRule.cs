using Mono.Cecil;
using NetArchTest.Rules;

namespace DrawEM.Tests.Architecture;

internal sealed class NativeFunctionBridgeRule : ICustomRule
{
    public bool MeetsRule(TypeDefinition type)
    {
        foreach (var method in type.Methods.Where(method => method.HasBody))
        {
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.Operand is MethodReference reference &&
                    reference.DeclaringType.FullName == "System.Runtime.InteropServices.Marshal" &&
                    reference.Name is "GetDelegateForFunctionPointer" or "GetFunctionPointerForDelegate")
                {
                    return false;
                }
            }
        }

        // Include closures, iterators, and async state machines even when the library's
        // default type selection omits compiler-generated nested types.
        return type.NestedTypes.All(MeetsRule);
    }
}
