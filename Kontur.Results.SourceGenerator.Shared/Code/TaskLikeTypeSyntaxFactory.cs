using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class TaskLikeTypeSyntaxFactory
    {
        internal static IEnumerable<GenericNameSyntax> Create(TypeSyntax argument)
        {
            yield return TypeFactory.CreateTask(argument);
            yield return TypeFactory.CreateValueTask(argument);
        }
    }
}
