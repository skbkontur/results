using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class ParameterNames
    {
        private readonly string other;

        internal ParameterNames(SelfParameter self, string other)
        {
            this.other = other;
            Self = self;
        }

        internal SelfParameter Self { get; }

        internal SyntaxToken Value => SyntaxFactory.Identifier(other);

        internal SyntaxToken Factory => SyntaxFactory.Identifier(other + "Factory");
    }
}
