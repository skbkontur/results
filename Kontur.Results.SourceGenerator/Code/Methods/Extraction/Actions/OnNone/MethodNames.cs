using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnNone
{
    internal static class MethodNames
    {
        internal static readonly MethodName OnNone = new(nameof(OnNone), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes the action if the optional is None."),
            MethodDocumentation.ReturnsElement("The original optional, enabling method chaining.")));
    }
}
