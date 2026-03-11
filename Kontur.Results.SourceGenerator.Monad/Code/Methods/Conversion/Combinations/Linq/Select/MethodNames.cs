using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Select
{
    internal static class MethodNames
    {
        internal static readonly MethodName Select = new(
            nameof(Select),
            MethodDocumentation.Create(
                MethodDocumentation.SummaryElement("Maps the contained value. Enables LINQ query syntax (do-notation)."),
                MethodDocumentation.CodeExampleElement(
                    "from userId in GetCurrentUserId()",
                    "select Process(userId);")));
    }
}
