using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class OptionalResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var returnType = TypeFactory.CreateResult(faultType);

            return methodDescriptionFactory.CreateFactory(
                "OptionalResultPlain",
                returnType,
                MethodNames.OrElse,
                new[] { value, fault },
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "onNoneResult"));
        }
    }
}
