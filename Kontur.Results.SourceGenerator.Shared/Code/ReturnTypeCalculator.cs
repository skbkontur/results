using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class ReturnTypeCalculator
    {
        internal static TypeSyntax Get(TypeSyntax returnType, bool hasTask, bool hasValueTask)
        {
            if (hasTask)
            {
                return TypeFactory.CreateTask(returnType);
            }

            return hasValueTask
                ? TypeFactory.CreateValueTask(returnType)
                : returnType;
        }

        internal static TypeSyntax Get(bool hasTask, bool hasValueTask)
        {
            if (hasTask)
            {
                return SyntaxFactory.IdentifierName(Identifiers.TaskIdentifier);
            }

            return hasValueTask
                ? SyntaxFactory.IdentifierName(Identifiers.ValueTaskIdentifier)
                : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword));
        }

        internal static Func<ExpressionSyntax, StatementSyntax> GetWrapResult(Func<ExpressionSyntax, StatementSyntax> wrapResult, bool taskLike)
        {
            return taskLike
                ? SyntaxFactory.ReturnStatement
                : wrapResult;
        }
    }
}
