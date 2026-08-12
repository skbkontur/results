using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureSuccess
{
    internal static class MethodNames
    {
        internal static readonly MethodName EnsureSuccess = new(nameof(EnsureSuccess), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Throws if the result is a failure.")));
    }
}
