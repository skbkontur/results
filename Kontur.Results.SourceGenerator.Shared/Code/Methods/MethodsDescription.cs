using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;

namespace Kontur.Results.SourceGenerator.Code.Methods
{
    internal record MethodsDescription(
        InternalMethodsDescription Internal,
        PublicMethodsDescription Public);
}
