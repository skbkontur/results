using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrElse
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetFaultOrElse = new(nameof(GetFaultOrElse), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the fault if failed, or computes a fallback.")));
    }
}
