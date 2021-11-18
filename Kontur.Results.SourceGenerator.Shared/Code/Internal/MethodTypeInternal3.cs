using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record MethodTypeInternal3(
        InternalTypeParameter Parameter1,
        InternalTypeParameter ParameterNext,
        InternalTypeParameter Parameter2,
        TypeSyntax ReturnType);
}