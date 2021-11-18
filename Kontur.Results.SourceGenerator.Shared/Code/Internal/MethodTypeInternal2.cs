using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record MethodTypeInternal2(
        InternalTypeParameter Parameter1,
        InternalTypeParameter Parameter2,
        TypeSyntax ReturnType);
}