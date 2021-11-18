using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ClassNamesFactoryExtensions
    {
        internal static IdentifierNameSyntax FactoryNameSyntax(this ClassNamesFactory classNames)
        {
            return SyntaxFactory.IdentifierName(classNames.Factory);
        }

        internal static IdentifierNameSyntax ValueNameSyntax(this ClassNamesFactory classNames)
        {
            return SyntaxFactory.IdentifierName(classNames.Value);
        }
    }
}
