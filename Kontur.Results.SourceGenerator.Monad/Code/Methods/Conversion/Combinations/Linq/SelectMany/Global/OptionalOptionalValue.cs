using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.SelectMany.Global
{
    internal class OptionalOptionalValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalOptionalValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value1 = Identifiers.TypeValue1;
            var value1Type = SyntaxFactory.IdentifierName(value1);

            var value2 = Identifiers.TypeValue2;
            var value2Type = SyntaxFactory.IdentifierName(value2);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            return this.methodDescriptionFactory.CreateSelectMany(
                "OptionalOptionalValue",
                TypeFactory.CreateOptional(resultType),
                MethodNames.SelectMany,
                new[] { value1, value2, result },
                TypeFactory.CreateOptional(value1Type),
                Array.Empty<GenericNameSyntax>(),
                value1Type,
                TypeFactory.CreateOptional(value2Type),
                Array.Empty<GenericNameSyntax>(),
                value2Type,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "next", "result"),
                false);
        }
    }
}
