using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureFailure
{
    internal class EnsureFailureResultPlainDefault : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public EnsureFailureResultPlainDefault(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultPlain",
                ReturnTypeCalculator.Get,
                MethodNames.EnsureFailure,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                Parameters.SelfResultIdentifier,
                Array.Empty<TypeParameterConstraintClauseSyntax>(),
                Array.Empty<AttributeSyntax>(),
                SyntaxFactory.ExpressionStatement);
        }
    }
}
