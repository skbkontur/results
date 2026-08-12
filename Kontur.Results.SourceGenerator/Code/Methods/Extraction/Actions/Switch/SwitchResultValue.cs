using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.Switch
{
    internal class SwitchResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public SwitchResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var passNextInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var passOtherInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var factoryNextInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var factoryOtherInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passNextPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, faultType), faultType);

            var passOtherPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var factoryNextPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var factoryOtherPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var nextPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onFailure"), passNextInternalParams, passNextPublicParams);

            var nextFactoryForms = new[]
            {
                new SwitchParameterForm(SyntaxFactory.Identifier("onFailure"), factoryNextInternalParams, factoryNextPublicParams),
            };

            var otherPassForm = new SwitchParameterForm(SyntaxFactory.Identifier("onSuccess"), passOtherInternalParams, passOtherPublicParams);

            var otherFactoryForms = new[]
            {
                new SwitchParameterForm(SyntaxFactory.Identifier("onSuccess"), factoryOtherInternalParams, factoryOtherPublicParams),
            };

            var returnType = TypeFactory.CreateResult(faultType, valueType);
            return this.methodDescriptionFactory.CreateSwitchOnly(
                "ResultValue",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.SwitchResult,
                new[] { fault, value },
                TypeFactory.CreateInterfaceResult(faultType, valueType),
                new[] { TypeFactory.CreateResult(faultType, valueType) },
                Parameters.SelfResultIdentifier,
                nextPassForm,
                nextFactoryForms,
                otherPassForm,
                otherFactoryForms);
        }
    }
}
