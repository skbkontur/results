using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrThrow
{
    internal static class MethodNames
    {
        internal static readonly MethodName GetValueOrThrow = new(nameof(GetValueOrThrow), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Extracts the value, or throws the exception from the factory.")));
    }
}
