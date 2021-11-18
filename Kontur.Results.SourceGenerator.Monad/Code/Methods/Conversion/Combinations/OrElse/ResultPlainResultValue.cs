using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class ResultPlainResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultPlainResultValue(MethodDescriptionFactory methodDescriptionFactory)
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

            var returnType = TypeFactory.CreateResult(resultType);

            return methodDescriptionFactory.CreatePass(
                "ResultPlainResultValue",
                returnType,
                MethodNames.OrElse,
                new[] { fault, result, value },
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                faultType,
                TypeFactory.CreateResult(resultType, valueType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onFailureResult"));
        }
    }
}
