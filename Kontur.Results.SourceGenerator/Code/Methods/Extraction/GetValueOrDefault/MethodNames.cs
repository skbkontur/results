using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrDefault
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetValueOrDefault = new(nameof(GetValueOrDefault), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the value if present, or returns default.")));
    }
}
