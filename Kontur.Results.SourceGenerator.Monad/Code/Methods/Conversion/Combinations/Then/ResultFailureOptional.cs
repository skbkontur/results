using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Then
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
            var t = Identifiers.TypeT;
            var typeT = SyntaxFactory.IdentifierName(t);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var returnType = TypeFactory.CreateResultFailure(typeT);

            return methodDescriptionFactory.CreateFactory(
                "ResultFailureOptional",
                returnType,
                MethodNames.Then,
                new[] { t, value },
                returnType,
                Array.Empty<GenericNameSyntax>(),
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "onSuccessOptional"));
        }
    }
}
