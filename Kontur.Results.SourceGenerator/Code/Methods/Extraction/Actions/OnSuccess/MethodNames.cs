using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnSuccess
{
    internal static class MethodNames
    {
        internal static readonly MethodName OnSuccess = new(nameof(OnSuccess), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes the action if the result is successful."),
            MethodDocumentation.ReturnsElement("The original result, enabling method chaining.")));
    }
}
