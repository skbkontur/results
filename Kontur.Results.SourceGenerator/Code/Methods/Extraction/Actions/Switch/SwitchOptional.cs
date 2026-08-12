using System;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.Switch
{
    internal class SwitchOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public SwitchOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var passNextInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passOtherInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var factoryOtherInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passNextPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passOtherPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var factoryOtherPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var nextPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onNone"), passNextInternalParams, passNextPublicParams);

            var otherPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onSome"), passOtherInternalParams, passOtherPublicParams);

            var otherFactoryForms = new[]
            {
                new SwitchParameterForm(SyntaxFactory.Identifier("onSome"), factoryOtherInternalParams, factoryOtherPublicParams),
            };

            var returnType = TypeFactory.CreateOptional(valueType);
            return this.methodDescriptionFactory.CreateSwitchOnly(
                "Optional",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.SwitchOptional,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                Parameters.SelfOptionalIdentifier,
                nextPassForm,
                Array.Empty<SwitchParameterForm>(),
                otherPassForm,
                otherFactoryForms);
        }
    }
}
