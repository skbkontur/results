using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.Where
{
    internal class Optional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public Optional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var boolean = Identifiers.Bool;
            var booleanType = SyntaxFactory.IdentifierName(boolean);

            return methodDescriptionFactory.CreateSelect(
                "Optional",
                TypeFactory.CreateOptional(valueType),
                MethodNames.Where,
                new[] { value },
                TypeFactory.CreateOptional(valueType),
                Array.Empty<GenericNameSyntax>(),
                valueType,
                booleanType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "predicate"));
        }
    }
}
