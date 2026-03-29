using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrThrow
{
    internal class GetValueOrThrowOptionalDefault : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetValueOrThrowOptionalDefault(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "Optional",
                valueType,
                MethodNames.GetValueOrThrow,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                Parameters.SelfOptionalIdentifier);
        }
    }
}
