using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrDefault
{
    internal class GetFaultOrDefaultResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetFaultOrDefaultResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultPlain",
                SyntaxFactory.NullableType(faultType),
                MethodNames.GetFaultOrDefault,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                Parameters.SelfResultIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>());
        }
    }
}
