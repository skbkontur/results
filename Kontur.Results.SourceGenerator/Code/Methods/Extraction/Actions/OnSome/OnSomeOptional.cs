using System;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.Actions.OnSome
{
    internal class OnSomeOptional : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public OnSomeOptional(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            var factoryInternalParams = MethodTypeInternalGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passInternalParams = MethodTypeInternalGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var factoryPublicParams = MethodTypeGenerator.CreateActionParameters(SyntaxFactory.IdentifierName(Identifiers.ActionIdentifier));

            var passPublicParams = MethodTypeGenerator.CreateActionParameters(TypeFactory.CreateClosedGeneric(Identifiers.ActionIdentifier, valueType), valueType);

            var returnType = TypeFactory.CreateOptional(valueType);
            return this.methodDescriptionFactory.CreatePassOnly(
                "Optional",
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                MethodNames.OnSome,
                new[] { value },
                TypeFactory.CreateInterfaceOptional(valueType),
                new[] { TypeFactory.CreateOptional(valueType) },
                factoryInternalParams,
                passInternalParams,
                factoryPublicParams,
                passPublicParams,
                new(Parameters.SelfOptionalIdentifier, "action"),
                Array.Empty<AttributeSyntax>(),
                SyntaxFactory.ReturnStatement);
        }
    }
}
