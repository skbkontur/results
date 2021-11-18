using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
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
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var returnType = TypeFactory.CreateOptional(valueType);

            return methodDescriptionFactory.CreatePass(
                "OptionalResultPlain",
                returnType,
                MethodNames.Then,
                new[] { value, fault },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                valueType,
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "onSomeResult"));
        }
    }
}
