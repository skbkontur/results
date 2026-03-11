using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal static class MethodNames
    {
        internal static readonly MethodName Then = new(nameof(Then), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Chains an operation that may produce None/Failure. Short-circuits on None/Failure.")));
    }
}
