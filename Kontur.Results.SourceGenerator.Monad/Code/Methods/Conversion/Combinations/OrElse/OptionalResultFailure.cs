using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class OptionalResultFailure : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalResultFailure(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return methodDescriptionFactory.CreateFactory(
                "OptionalResultFailure",
                TypeFactory.CreateResult(faultType, valueType),
                MethodNames.OrElse,
                new[] { value, fault },
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateResultFailure(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "onNoneResult"));
        }
    }
}
