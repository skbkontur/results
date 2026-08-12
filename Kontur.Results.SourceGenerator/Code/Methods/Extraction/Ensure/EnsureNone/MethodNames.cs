using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureNone
{
    internal static class MethodNames
    {
        internal static readonly MethodName EnsureNone = new(nameof(EnsureNone), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Throws if the optional has a value.")));
    }
}
