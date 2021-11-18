using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class ResultFailureOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultFailureOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return methodDescriptionFactory.CreatePass(
                "ResultFailureOptional",
                TypeFactory.CreateResult(faultType, valueType),
                MethodNames.OrElse,
                new[] { fault, value },
                TypeFactory.CreateResultFailure(faultType),
                Array.Empty<GenericNameSyntax>(),
                faultType,
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onFailureOptional"));
        }
    }
}
