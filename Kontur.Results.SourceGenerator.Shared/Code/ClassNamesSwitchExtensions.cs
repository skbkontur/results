using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ClassNamesSwitchExtensions
    {
        internal static IdentifierNameSyntax FactoryNextNameSyntax(this ClassNamesSwitch classNames)
        {
            return SyntaxFactory.IdentifierName(classNames.FactoryNext);
        }

        internal static IdentifierNameSyntax FactoryBothNameSyntax(this ClassNamesSwitch classNames)
        {
            return SyntaxFactory.IdentifierName(classNames.FactoryBoth);
        }
    }
}
