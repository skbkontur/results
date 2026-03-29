using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrThrow
{
    internal class GetFaultOrThrowResultPlainDefault : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetFaultOrThrowResultPlainDefault(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultPlain",
                faultType,
                MethodNames.GetFaultOrThrow,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                Parameters.SelfResultIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>());
        }
    }
}
