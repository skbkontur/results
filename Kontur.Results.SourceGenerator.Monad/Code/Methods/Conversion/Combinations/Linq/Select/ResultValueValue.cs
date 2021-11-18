using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Select
{
    internal class ResultValueValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueValue(MethodDescriptionFactory methodDescriptionFactory)
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

            return methodDescriptionFactory.CreateSelect(
                "ResultValueValue",
                TypeFactory.CreateResult(faultType, resultType),
                MethodNames.Select,
                new[] { fault, value, result },
                TypeFactory.CreateResult(faultType, valueType),
                Array.Empty<GenericNameSyntax>(),
                valueType,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "value"));
        }
    }
}
