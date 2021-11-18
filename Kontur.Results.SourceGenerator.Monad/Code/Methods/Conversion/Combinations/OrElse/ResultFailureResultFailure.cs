using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
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
            var t = Identifiers.TypeT;
            var tType = SyntaxFactory.IdentifierName(t);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            var returnType = TypeFactory.CreateResultFailure(resultType);

            return methodDescriptionFactory.CreatePass(
                "ResultFailureResultFailure",
                returnType,
                MethodNames.OrElse,
                new[] { t, result },
                TypeFactory.CreateResultFailure(tType),
                Array.Empty<GenericNameSyntax>(),
                tType,
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onFailureResult"));
        }
    }
}
