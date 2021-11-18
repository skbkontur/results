using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultFailureResultFailure : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultFailureResultFailure(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var t = Identifiers.TypeT;
            var tType = SyntaxFactory.IdentifierName(t);

            var returnType = TypeFactory.CreateResultFailure(tType);

            return methodDescriptionFactory.CreateFactory(
                "ResultFailureResultFailure",
                returnType,
                MethodNames.Then,
                new[] { t, fault },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateResultFailure(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessResult"));
        }
    }
}
