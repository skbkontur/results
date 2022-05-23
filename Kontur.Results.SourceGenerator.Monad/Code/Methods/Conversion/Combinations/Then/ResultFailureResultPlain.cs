using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultFailureResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultFailureResultPlain(MethodDescriptionFactory methodDescriptionFactory)
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

            return this.methodDescriptionFactory.CreateFactory(
                "ResultFailureResultPlain",
                returnType,
                MethodNames.Then,
                new[] { t, fault },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessResult"));
        }
    }
}
