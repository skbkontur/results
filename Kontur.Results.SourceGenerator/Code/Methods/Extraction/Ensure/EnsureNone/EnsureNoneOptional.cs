using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureNone
{
    internal class EnsureNoneOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public EnsureNoneOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreatePass(
                "Optional",
                ReturnTypeCalculator.Get,
                MethodNames.EnsureNone,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                valueType,
                SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("System"), SyntaxFactory.IdentifierName("Exception")),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfOptionalIdentifier, "exception"),
                Array.Empty<AttributeSyntax>(),
                new[] { Using.Tasks },
                SyntaxFactory.ExpressionStatement);
        }
    }
}
