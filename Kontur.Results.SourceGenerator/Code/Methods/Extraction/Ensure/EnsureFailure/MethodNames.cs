using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureFailure
{
    internal static class MethodNames
    {
        internal static readonly MethodName EnsureFailure = new(nameof(EnsureFailure), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Throws if the result is successful.")));
    }
}
