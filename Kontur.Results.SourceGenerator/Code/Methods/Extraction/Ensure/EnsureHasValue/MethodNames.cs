using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureHasValue
{
    internal static class MethodNames
    {
        internal static readonly MethodName EnsureHasValue = new(nameof(EnsureHasValue), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Throws if the optional is None.")));
    }
}
