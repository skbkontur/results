using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureNone
{
    internal class EnsureNoneOptionalDefault : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public EnsureNoneOptionalDefault(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "Optional",
                ReturnTypeCalculator.Get,
                MethodNames.EnsureNone,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                Parameters.SelfOptionalIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>(),
                Array.Empty<AttributeSyntax>(),
                SyntaxFactory.ExpressionStatement);
        }
    }
}
