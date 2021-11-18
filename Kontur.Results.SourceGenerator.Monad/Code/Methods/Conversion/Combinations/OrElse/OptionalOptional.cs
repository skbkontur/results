using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.OrElse
{
    internal class OptionalOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OptionalOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var returnType = TypeFactory.CreateOptional(valueType);
            var parameterSelfType = TypeFactory.CreateOptional(valueType);

            return methodDescriptionFactory.CreateFactory(
                "OptionalOptional",
                returnType,
                MethodNames.OrElse,
                new[] { value },
                parameterSelfType,
                Array.Empty<GenericNameSyntax>(),
                parameterSelfType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "onNoneOptional"));
        }
    }
}
