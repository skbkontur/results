using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Select
{
    internal class OptionalResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            return this.methodDescriptionFactory.CreateSelect(
                "OptionalResultValue",
                TypeFactory.CreateOptional(resultType),
                MethodNames.Select,
                new[] { fault, value, result },
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                valueType,
                TypeFactory.CreateResult(faultType, resultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "result"));
        }
    }
}
