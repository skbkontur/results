using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrThrow
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetFaultOrThrow = new(nameof(GetFaultOrThrow), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the fault, or throws the exception from the factory.")));
    }
}
