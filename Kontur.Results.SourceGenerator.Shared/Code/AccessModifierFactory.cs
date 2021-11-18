using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class AccessModifierFactory
    {
        internal static SyntaxToken[] Create(params SyntaxKind[] tokens)
        {
            return tokens.Select(SyntaxFactory.Token).ToArray();
        }
    }
}
