using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record InternalTypeParameter(
        NameSyntax Type,
        bool IsTask,
        bool IsValueTask,
        MethodTypeGenericParameter? Generic)
    {
        internal bool IsTaskLike => IsTask || IsValueTask;
    }
}
