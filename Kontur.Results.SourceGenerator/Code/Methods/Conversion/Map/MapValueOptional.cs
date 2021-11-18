using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Map
{
    internal class MapValueOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public MapValueOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            return methodDescriptionFactory.CreatePass(
                "Optional",
                TypeFactory.CreateOptional(resultType),
                MethodNames.MapValue,
                new[] { value, result },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                valueType,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "value"));
        }
    }
}
