using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrNull
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetFaultOrNull = new(nameof(GetFaultOrNull), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the fault as nullable. Constrained to value types where <c>default</c> may be a valid fault.")));
    }
}
