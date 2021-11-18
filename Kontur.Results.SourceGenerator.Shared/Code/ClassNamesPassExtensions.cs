using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ClassNamesPassExtensions
    {
        internal static IdentifierNameSyntax PassNameSyntax(this ClassNamesPass classNames)
        {
            return SyntaxFactory.IdentifierName(classNames.Pass);
        }
    }
}