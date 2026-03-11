using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Map
{
    internal static class MethodNames
    {
        internal static readonly MethodName MapFault = new(nameof(MapFault), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Transforms the fault if failed. Success passes through unchanged.")));

        internal static readonly MethodName MapValue = new(nameof(MapValue), MethodDocumentation.Create(
            MethodDocumentation.SummaryElement("Transforms the value if present. None/Failure passes through unchanged.")));
    }
}
