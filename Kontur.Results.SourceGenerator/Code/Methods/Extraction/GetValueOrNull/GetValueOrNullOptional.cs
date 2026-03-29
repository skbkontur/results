using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrNull
{
    internal class GetValueOrNullOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetValueOrNullOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "Optional",
                SyntaxFactory.NullableType(valueType),
                MethodNames.GetValueOrNull,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                Parameters.SelfOptionalIdentifier,
                new[]
                {
                    SyntaxFactory.TypeParameterConstraintClause(
                        SyntaxFactory.IdentifierName(value))
                        .AddConstraints(SyntaxFactory.ClassOrStructConstraint(SyntaxKind.StructConstraint)),
                });
        }
    }
}
