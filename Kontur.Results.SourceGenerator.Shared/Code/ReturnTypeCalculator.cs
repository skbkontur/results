using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ReturnTypeCalculator
    {
        internal static TypeSyntax Get(TypeSyntax returnType, IEnumerable<SimpleNameSyntax> args)
        {
            var hasValueTask = false;
            foreach (var identifier in args.Select(nameSyntax => nameSyntax.Identifier))
            {
                if (identifier.IsTask())
                {
                    return TypeFactory.CreateTask(returnType);
                }

                if (identifier.IsValueTask())
                {
                    hasValueTask = true;
                }
            }

            return hasValueTask
                ? TypeFactory.CreateValueTask(returnType)
                : returnType;
        }
    }
}
