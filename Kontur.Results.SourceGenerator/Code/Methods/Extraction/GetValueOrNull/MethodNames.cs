using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrNull
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetValueOrNull = new(nameof(GetValueOrNull), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the value as nullable. Constrained to value types where <c>default</c> may be a valid value.")));
    }
}
