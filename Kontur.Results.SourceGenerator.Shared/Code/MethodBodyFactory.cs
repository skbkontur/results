using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class MethodBodyFactory : IMethodBodyFactory
    {
        public BlockSyntax CreateDelegateToOtherClass(
            SyntaxToken methodName,
            SimpleNameSyntax delegateClassName,
            IEnumerable<TypeSyntax> genericArguments,
            IEnumerable<ArgumentSyntax> arguments)
        {
            var returnStatement = SyntaxFactory.ReturnStatement(
                SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                delegateClassName,
                                SyntaxFactory.GenericName(
                                    methodName,
                                    SyntaxFactory.TypeArgumentList(
                                        SyntaxFactory.SeparatedList(genericArguments))))
                            .WithOperatorToken(SyntaxFactory.Token(SyntaxKind.DotToken)))
                    .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(arguments))));

            return SyntaxFactory.Block()
                .WithStatements(SyntaxFactory.List<StatementSyntax>(new[]
                {
                    returnStatement,
                }));
        }
    }
}
