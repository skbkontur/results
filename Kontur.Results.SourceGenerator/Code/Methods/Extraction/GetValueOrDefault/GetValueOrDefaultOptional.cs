using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrDefault
{
    internal class GetValueOrDefaultOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetValueOrDefaultOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "Optional",
                SyntaxFactory.NullableType(valueType),
                MethodNames.GetValueOrDefault,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                Parameters.SelfOptionalIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>());
        }
    }
}
