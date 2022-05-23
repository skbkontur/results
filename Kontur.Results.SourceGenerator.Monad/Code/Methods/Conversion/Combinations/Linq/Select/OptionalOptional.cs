using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Select
{
    internal class OptionalOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            var returnType = TypeFactory.CreateOptional(resultType);

            return this.methodDescriptionFactory.CreateSelect(
                "OptionalOptional",
                returnType,
                MethodNames.Select,
                new[] { value, result },
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                valueType,
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "optional"));
        }
    }
}