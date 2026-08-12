using System;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnFailure
{
    internal class OnFailureResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OnFailureResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var factoryInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var factoryPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var returnType = TypeFactory.CreateResult(faultType);
            return this.methodDescriptionFactory.CreatePassOnly(
                "ResultPlain",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.OnFailure,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                factoryInternalParams,
                passInternalParams,
                factoryPublicParams,
                passPublicParams,
                new(Parameters.SelfResultIdentifier, "action"),
                Array.Empty<AttributeSyntax>(),
                SyntaxFactory.ReturnStatement);
        }
    }
}
