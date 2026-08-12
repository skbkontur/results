using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.Switch
{
    internal static class MethodNames
    {
        internal static readonly MethodName SwitchOptional = new(nameof(Switch), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes one of two actions depending on the optional state."),
            MethodDocumentation.ReturnsElement("The original optional, enabling method chaining.")));

        internal static readonly MethodName SwitchResult = new(nameof(Switch), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Executes one of two actions depending on the result state."),
            MethodDocumentation.ReturnsElement("The original result, enabling method chaining.")));
    }
}
