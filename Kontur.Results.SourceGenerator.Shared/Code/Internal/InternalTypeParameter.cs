using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record InternalTypeParameter(
        SimpleNameSyntax Type,
        bool IsTaskLike,
        MethodTypeGenericParameter? Generic);
}
