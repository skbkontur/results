using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnFailure
{
    internal static class MethodNames
    {
        internal static readonly MethodName OnFailure = new(nameof(OnFailure), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes the action if the result is a failure."),
            MethodDocumentation.ReturnsElement("The original result, enabling method chaining.")));
    }
}
