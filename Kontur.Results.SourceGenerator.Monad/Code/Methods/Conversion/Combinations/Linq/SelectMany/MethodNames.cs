using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.SelectMany
{
    internal static class MethodNames
    {
        internal static readonly MethodName SelectMany = new(
            nameof(SelectMany),
            MethodDocumentation.Create(
                MethodDocumentation.SummaryElement("Chains multiple operations. Enables do-notation via LINQ query syntax."),
                MethodDocumentation.CodeExampleElement(
                    "from userId  in GetCurrentUserId()",
                    "from index   in GetCurrentIndex()",
                    "from product in GetCurrentProduct()",
                    "select Process(userId, index, product);")));
    }
}
