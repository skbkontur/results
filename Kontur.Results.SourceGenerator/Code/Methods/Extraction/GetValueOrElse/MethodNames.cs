using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrElse
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetValueOrElse = new(nameof(GetValueOrElse), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the value if present, or computes a fallback.")));
    }
}
