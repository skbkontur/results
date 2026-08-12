using System;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.Switch
{
    internal class SwitchResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public SwitchResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var passNextInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var passOtherInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var factoryNextInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passNextPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var passOtherPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var factoryNextPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var nextPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onFailure"), passNextInternalParams, passNextPublicParams);

            var nextFactoryForms = new[]
            {
                new SwitchParameterForm(SyntaxFactory.Identifier("onFailure"), factoryNextInternalParams, factoryNextPublicParams),
            };

            var otherPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onSuccess"), passOtherInternalParams, passOtherPublicParams);

            var returnType = TypeFactory.CreateResult(faultType);
            return this.methodDescriptionFactory.CreateSwitchOnly(
                "ResultPlain",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.SwitchResult,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                Parameters.SelfResultIdentifier,
                nextPassForm,
                nextFactoryForms,
                otherPassForm,
                Array.Empty<SwitchParameterForm>());
        }
    }
}
