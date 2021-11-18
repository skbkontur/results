using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class TypeParameterGenericMethodSyntaxGenerator : ITypeParameterGenericMethodSyntaxGenerator
    {
        private static readonly SyntaxAnnotation FirstWhere = new("where-first");
        private static readonly SyntaxAnnotation OtherWhere = new("where-other");

        public IEnumerable<TypeParameterConstraintClauseSyntax> CreateTypeParameterConstraints(IEnumerable<MethodGenericParameterDescription> genericParameters)
        {
            return genericParameters
                .Select(param => param.UpperBound == null ? null : new { param.Identifier, param.UpperBound })
                .WhereNotNull()
                .Select(param => SyntaxFactory.TypeParameterConstraintClause(
                    SyntaxFactory.IdentifierName(param.Identifier),
                    SyntaxFactory.SeparatedList<TypeParameterConstraintSyntax>(new[]
                    {
                        SyntaxFactory.TypeConstraint(param.UpperBound),
                    })))
                .Select((clause, i) => i == 0
                    ? clause.WithAdditionalAnnotations(FirstWhere)
                    : clause.WithAdditionalAnnotations(OtherWhere));
        }

        public CompilationUnitSyntax FixMethodTypeParameterIndentation(CompilationUnitSyntax compilationUnit)
        {
            return compilationUnit
                .ReplaceNodes(
                    compilationUnit.GetAnnotatedNodes(FirstWhere)
                        .Concat(compilationUnit.GetAnnotatedNodes(OtherWhere)),
                    (_, node) => node.HasAnnotation(FirstWhere)
                        ? node.WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed)
                        : node.WithLeadingTrivia(node.Parent?.GetLeadingTrivia().Add(SyntaxFactory.Whitespace("    "))).WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed));
        }
    }
}
