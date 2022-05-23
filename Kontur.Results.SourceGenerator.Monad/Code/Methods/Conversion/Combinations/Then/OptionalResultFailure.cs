using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
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
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var returnType = TypeFactory.CreateOptional(valueType);

            return this.methodDescriptionFactory.CreatePass(
                "OptionalResultFailure",
                returnType,
                MethodNames.Then,
                new[] { value, fault },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                valueType,
                TypeFactory.CreateResultFailure(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "onSomeResult"));
        }
    }
}
