using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class Attributes
    {
        internal static readonly AttributeSyntax Pure = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Pure"));
        internal static readonly AttributeSyntax EditorBrowsable = SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName("EditorBrowsable"),
                SyntaxFactory.AttributeArgumentList(SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.AttributeArgument(SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("EditorBrowsableState"),
                        SyntaxFactory.Token(SyntaxKind.DotToken),
                        SyntaxFactory.IdentifierName("Never"))),
                })));
    }
}
