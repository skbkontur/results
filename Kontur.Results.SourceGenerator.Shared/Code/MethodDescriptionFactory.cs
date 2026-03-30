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
            TypeSyntax returnType,
            MethodName method,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            ClassNamesFactory classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create2(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                parametersName);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create2(
                returnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                parametersName,
                classNames);

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
            ClassNamesPass classNames = new(method.Name, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);
            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create3(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                passType,
                parametersName);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create3(
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
            var defaultClassName = $"{method.Name}{parameterTypes}DefaultInternal";
            var methodNameSyntax = SyntaxFactory.Identifier(method.Name);

            var internalMethodsDescriptions = this.internalMethodDescriptionFactories.Create1(
                defaultClassName,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                parametersName,
                additionalConstraints);

            var publicMethodsDescriptions = this.publicMethodDescriptionFactories.Create1(
                returnType,
                methodNameSyntax,
                method.Documentation,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parametersName,
                defaultClassName,
                additionalConstraints);

            return new(
                internalMethodsDescriptions,
                publicMethodsDescriptions);
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

        private static MethodTypeGeneratorParameter CreateParameterSelfType(
            NameSyntax upperBound,
            IEnumerable<GenericNameSyntax> taskTypes)
        {
            return new(upperBound, !taskTypes.Any());
        }
    }
}
