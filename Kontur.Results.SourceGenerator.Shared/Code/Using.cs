using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class Using
    {
        internal static readonly IdentifierNameSyntax ResultsName = SyntaxFactory.IdentifierName("Kontur.Results");
        internal static readonly UsingDirectiveSyntax Results = SyntaxFactory.UsingDirective(ResultsName);

        internal static readonly UsingDirectiveSyntax System = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System"));
        internal static readonly UsingDirectiveSyntax ComponentModel = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ComponentModel"));
        internal static readonly UsingDirectiveSyntax Contracts = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Diagnostics.Contracts"));
        internal static readonly UsingDirectiveSyntax Tasks = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks"));
    }
}
