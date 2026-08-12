using System;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnFailure
{
    internal class OnFailureResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OnFailureResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var factoryInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var factoryPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var returnType = TypeFactory.CreateResult(faultType, valueType);
            return this.methodDescriptionFactory.CreatePassOnly(
                "ResultValue",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.OnFailure,
                new[] { fault, value },
                TypeFactory.CreateInterfaceResult(faultType, valueType),
                new[] { TypeFactory.CreateResult(faultType, valueType) },
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
