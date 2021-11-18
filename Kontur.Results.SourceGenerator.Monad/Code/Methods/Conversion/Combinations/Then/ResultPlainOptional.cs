using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultPlainOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultPlainOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var returnType = TypeFactory.CreateOptional(valueType);

            return methodDescriptionFactory.CreateFactory(
                "ResultPlainOptional",
                returnType,
                MethodNames.Then,
                new[] { fault, value },
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessOptional"));
        }
    }
}
