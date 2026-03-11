using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Where
{
    internal static class MethodNames
    {
        internal static readonly MethodName Where = new(
            nameof(Where),
            MethodDocumentation.Create(
                MethodDocumentation.SummaryElement("Filters by a predicate in do-notation. Short-circuits to None/Failure when the predicate is not satisfied."),
                MethodDocumentation.CodeExampleElement(
                    "from userId in GetCurrentUserId()",
                    "where userId != Guid.Empty",
                    "from index in GetCurrentIndex()",
                    "select Process(userId, index);")));
    }
}
