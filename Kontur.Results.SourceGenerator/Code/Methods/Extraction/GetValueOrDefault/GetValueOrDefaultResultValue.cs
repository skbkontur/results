using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetValueOrDefault
{
    internal class GetValueOrDefaultResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetValueOrDefaultResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultValue",
                SyntaxFactory.NullableType(valueType),
                MethodNames.GetValueOrDefault,
                new[] { fault, value },
                TypeFactory.CreateInterfaceResult(faultType, valueType),
                new[] { TypeFactory.CreateResult(faultType, valueType) },
                Parameters.SelfResultIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>());
        }
    }
}
