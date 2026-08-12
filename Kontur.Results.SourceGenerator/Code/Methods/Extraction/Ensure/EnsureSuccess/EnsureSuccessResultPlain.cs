using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Ensure.EnsureSuccess
{
    internal class EnsureSuccessResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public EnsureSuccessResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            return this.methodDescriptionFactory.CreatePass(
                "ResultPlain",
                ReturnTypeCalculator.Get,
                MethodNames.EnsureSuccess,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                faultType,
                SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("System"), SyntaxFactory.IdentifierName("Exception")),
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "exception"),
                Array.Empty<AttributeSyntax>(),
                new[] { Using.Tasks },
                SyntaxFactory.ExpressionStatement);
        }
    }
}
