using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
{
    internal class ResultValueResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var returnType = TypeFactory.CreateResult(faultType, valueType);

            return methodDescriptionFactory.CreatePass(
                "ResultValueResultPlain",
                returnType,
                MethodNames.Then,
                new[] { fault, value },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                valueType,
                TypeFactory.CreateResult(faultType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessResult"));
        }
    }
}
