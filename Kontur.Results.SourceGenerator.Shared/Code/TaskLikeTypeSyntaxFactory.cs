using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class TaskLikeTypeSyntaxFactory
    {
        internal static IEnumerable<(GenericNameSyntax Syntax, bool IsTask, bool IsValueTask)> Create(TypeSyntax argument)
        {
            yield return (TypeFactory.CreateTask(argument), true, false);
            yield return (TypeFactory.CreateValueTask(argument), false, true);
        }
    }
}
