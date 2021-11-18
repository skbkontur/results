using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.SelectMany.Local
{
    internal class OptionalValueValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalValueValue(MethodDescriptionFactory methodDescriptionFactory)
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

            return methodDescriptionFactory.CreateSelectMany(
                "OptionalValueValue",
                TypeFactory.CreateOptional(resultType),
                MethodNames.SelectMany,
                new[] { value1, value2, result },
                TypeFactory.CreateOptional(value1Type),
                Array.Empty<GenericNameSyntax>(),
                value1Type,
                value2Type,
                Array.Empty<GenericNameSyntax>(),
                value2Type,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "next", "result"),
                true);
        }
    }
}
