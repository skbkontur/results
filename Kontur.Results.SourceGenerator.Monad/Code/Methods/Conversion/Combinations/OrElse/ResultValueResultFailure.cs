using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class ResultValueResultFailure : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueResultFailure(MethodDescriptionFactory methodDescriptionFactory)
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

            return methodDescriptionFactory.CreatePass(
                "ResultValueResultFailure",
                TypeFactory.CreateResult(resultType, valueType),
                MethodNames.OrElse,
                new[] { fault, value, result },
                TypeFactory.CreateResult(faultType, valueType),
                Array.Empty<GenericNameSyntax>(),
                faultType,
                TypeFactory.CreateResultFailure(resultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onFailureResult"));
        }
    }
}
