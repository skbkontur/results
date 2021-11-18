using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultPlainResultFailure : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultPlainResultFailure(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var returnType = TypeFactory.CreateResultFailure(faultType);

            return methodDescriptionFactory.CreateFactory(
                "ResultPlainResultFailure",
                returnType,
                MethodNames.Then,
                new[] { fault },
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessResult"));
        }
    }
}
