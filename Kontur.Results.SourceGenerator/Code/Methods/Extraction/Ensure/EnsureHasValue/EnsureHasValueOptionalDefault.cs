using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureHasValue
{
    internal class EnsureHasValueOptionalDefault : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public EnsureHasValueOptionalDefault(MethodDescriptionFactory methodDescriptionFactory)
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
                MethodNames.EnsureHasValue,
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
