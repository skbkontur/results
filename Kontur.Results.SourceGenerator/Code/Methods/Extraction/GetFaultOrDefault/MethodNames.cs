using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrDefault
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetFaultOrDefault = new(nameof(GetFaultOrDefault), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the fault if failed, or returns default.")));
    }
}
