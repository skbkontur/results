using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnSome
{
    internal static class MethodNames
    {
        internal static readonly MethodName OnSome = new(nameof(OnSome), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes the action if the optional has a value."),
            MethodDocumentation.ReturnsElement("The original optional, enabling method chaining.")));
    }
}
