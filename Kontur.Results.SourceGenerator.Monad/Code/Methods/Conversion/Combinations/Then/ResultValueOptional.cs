using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultValueOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueOptional(MethodDescriptionFactory methodDescriptionFactory)
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

            var returnType = TypeFactory.CreateOptional(resultType);

            return methodDescriptionFactory.CreatePass(
                "ResultValueOptional",
                returnType,
                MethodNames.Then,
                new[] { fault, value, result },
                TypeFactory.CreateResult(faultType, valueType),
                Array.Empty<GenericNameSyntax>(),
                valueType,
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessOptional"));
        }
    }
}
