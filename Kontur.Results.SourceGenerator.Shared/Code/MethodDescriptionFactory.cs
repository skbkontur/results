using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Methods;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class MethodDescriptionFactory
    {
        private readonly InternalMethodDescriptionFactories internalMethodDescriptionFactories;
        private readonly PublicMethodDescriptionFactories publicMethodDescriptionFactories;

        public MethodDescriptionFactory(InternalMethodDescriptionFactories internalMethodDescriptionFactories, PublicMethodDescriptionFactories publicMethodDescriptionFactories)
        {
            this.internalMethodDescriptionFactories = internalMethodDescriptionFactories;
            this.publicMethodDescriptionFactories = publicMethodDescriptionFactories;
        }

        internal MethodsDescription CreateFactory(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            IEnumerable<UsingDirectiveSyntax> valueUsings,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            ClassNamesFactory classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create2(
                classNames,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                parametersName,
                attributes,
                valueUsings,
                wrapResult);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create2(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                parametersName,
                classNames,
                attributes,
                wrapResult);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreateFactory(
            string parameterTypes,
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            return CreateFactory(
                parameterTypes,
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                method,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                parametersName,
                new[] { Attributes.Pure },
                new[] { Using.Contracts, Using.Tasks },
                SyntaxFactory.ReturnStatement);
        }

        internal MethodsDescription CreateFactoryOnly(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            InternalTypeParameter[] otherInternalParams,
            MethodTypeGenerator.ParameterInfo[] otherPublicParams,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            ClassNamesFactory classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);

            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.CreateFactoryOnly(
                classNames.Factory,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                otherInternalParams,
                parametersName.Self,
                parametersName.Value,
                wrapResult);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.CreateFactoryOnly(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                otherPublicParams,
                parametersName.Self,
                parametersName.Value,
                classNames.FactoryNameSyntax(),
                attributes);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreatePass(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax passType,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            IEnumerable<UsingDirectiveSyntax> valueUsings,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            ClassNamesPass classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create3(
                classNames,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                passType,
                parametersName,
                attributes,
                valueUsings,
                wrapResult);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create3(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                passType,
                parametersName,
                classNames,
                attributes,
                wrapResult);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreatePass(
            string parameterTypes,
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax passType,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            return CreatePass(
                parameterTypes,
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                method,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                passType,
                parameterOtherType,
                parameterOtherTypeTasks,
                parametersName,
                new[] { Attributes.Pure },
                new[] { Using.Contracts, Using.Tasks },
                SyntaxFactory.ReturnStatement);
        }

        internal MethodsDescription CreatePassOnly(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            InternalTypeParameter[] factoryInternalParams,
            InternalTypeParameter[] passInternalParams,
            MethodTypeGenerator.ParameterInfo[] factoryPublicParams,
            MethodTypeGenerator.ParameterInfo[] passPublicParams,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            ClassNamesPass classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);

            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.CreatePassOnly(
                classNames,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                factoryInternalParams,
                passInternalParams,
                parametersName.Self,
                parametersName.Value,
                wrapResult);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.CreatePassOnly(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                factoryPublicParams,
                passPublicParams,
                parametersName.Self,
                parametersName.Value,
                classNames,
                attributes);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreateDefault(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SelfParameter parametersName,
            TypeParameterConstraintClauseSyntax[] additionalConstraints,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var defaultClassName = $"{method.Name}{parameterTypes}DefaultInternal";
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);

            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create1(
                defaultClassName,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                parametersName,
                additionalConstraints,
                wrapResult);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create1(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parametersName,
                defaultClassName,
                additionalConstraints,
                attributes,
                wrapResult);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreateDefault(
            string parameterTypes,
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SelfParameter parametersName,
            TypeParameterConstraintClauseSyntax[] additionalConstraints)
        {
            return CreateDefault(
                parameterTypes,
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                method,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parametersName,
                additionalConstraints,
                new[] { Attributes.Pure },
                SyntaxFactory.ReturnStatement);
        }

        internal MethodsDescription CreateSelect(
            string parameterTypes,
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax passType,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            ClassNamesPass classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.CreateSelect(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                passType,
                parametersName);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.CreateSelect(
                returnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                passType,
                parametersName,
                classNames);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreateSelectMany(
            string parameterTypes,
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax nextPassType,
            NameSyntax nextParameterOtherType,
            GenericNameSyntax[] nextParameterOtherTypeTasks,
            IdentifierNameSyntax passType,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames3 parametersName,
            bool local)
        {
            ClassNamesPass classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.CreateSelectMany(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(nextParameterOtherType, nextParameterOtherTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                nextPassType,
                passType,
                parametersName);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.CreateSelectMany(
                returnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                nextParameterOtherType,
                nextParameterOtherTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                nextPassType,
                passType,
                parametersName,
                classNames,
                local);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
        }

        internal MethodsDescription CreateSwitchOnly(
            string parameterTypes,
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SelfParameter self,
            SwitchParameterForm nextPassForm,
            SwitchParameterForm[] nextFactoryForms,
            SwitchParameterForm otherPassForm,
            SwitchParameterForm[] otherFactoryForms)
        {
            ClassNamesSwitch classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var tiers = SwitchTierGenerator.Create(classNames, nextPassForm, nextFactoryForms, otherPassForm, otherFactoryForms);

            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.CreateSwitch(
                classNames.Pass,
                tiers,
                computeReturnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                self,
                nextPassForm,
                otherPassForm);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.CreateSwitch(
                computeReturnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                self,
                tiers,
                nextPassForm,
                otherPassForm,
                classNames.PassNameSyntax());

            return new(internalMethodsDescriptions, publicMethodsDescriptions);
        }

        private static MethodTypeGeneratorParameter CreateParameterSelfType(
            NameSyntax upperBound,
            IEnumerable<GenericNameSyntax> taskTypes)
        {
            return new(upperBound, !taskTypes.Any());
        }
    }
}
