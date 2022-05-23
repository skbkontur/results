using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultFailureResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultFailureResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var t = Identifiers.TypeT;
            var tType = SyntaxFactory.IdentifierName(t);

            var returnType = TypeFactory.CreateResultFailure(tType);

            return this.methodDescriptionFactory.CreateFactory(
                "ResultFailureResultValue",
                returnType,
                MethodNames.Then,
                new[] { t, fault, value },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateResult(faultType, valueType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier,  "onSuccessResult"));
        }
    }
}
