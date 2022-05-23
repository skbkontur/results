using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class ParameterNames3
    {
        private readonly string next;
        private readonly string other;

        internal ParameterNames3(SelfParameter self, string next, string other)
        {
            this.Self = self;
            this.next = next;
            this.other = other;
        }

        internal SelfParameter Self { get; }

        internal SyntaxToken NextValue => SyntaxFactory.Identifier(this.next);

        internal SyntaxToken NextFactory => SyntaxFactory.Identifier(this.next + "Factory");

        internal SyntaxToken Value => SyntaxFactory.Identifier(this.other);

        internal SyntaxToken Factory => SyntaxFactory.Identifier(this.other + "Factory");
    }
}
