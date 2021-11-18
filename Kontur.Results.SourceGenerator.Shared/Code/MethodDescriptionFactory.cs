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
            GenericNameSyntax returnType,
            string methodName,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            GenericNameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            ClassNamesFactory classNames = new(methodName, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(methodName);
            var internalMethodsDescriptions = CreateInternal2(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                parameterOtherTypeTasks,
                parametersName);

            var publicMethodsDescriptions = publicMethodDescriptionFactories.Create2(
                returnType,
                methodNameSyntax,
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
            GenericNameSyntax returnType,
            string methodName,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax passType,
            SimpleNameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            ClassNamesPass classNames = new(methodName, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(methodName);
            var internalMethodsDescriptions = CreateInternal3(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                parameterSelfType,
                parameterSelfTypeTasks,
                parameterOtherType,
                passType,
                parameterOtherTypeTasks,
                parametersName);

            var publicMethodsDescriptions = publicMethodDescriptionFactories.Create3(
                returnType,
                methodNameSyntax,
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

        internal MethodsDescription CreateSelect(
            string parameterTypes,
            GenericNameSyntax returnType,
            string methodName,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax passType,
            SimpleNameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            ClassNamesPass classNames = new(methodName, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(methodName);
            var internalMethodsDescriptions = internalMethodDescriptionFactories.CreateSelect(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                passType,
                parametersName);

            var publicMethodsDescriptions = publicMethodDescriptionFactories.CreateSelect(
                returnType,
                methodNameSyntax,
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
            GenericNameSyntax returnType,
            string methodName,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IdentifierNameSyntax nextPassType,
            SimpleNameSyntax nextParameterOtherType,
            GenericNameSyntax[] nextParameterOtherTypeTasks,
            IdentifierNameSyntax passType,
            SimpleNameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames3 parametersName,
            bool local)
        {
            ClassNamesPass classNames = new(methodName, parameterTypes);
            var methodNameSyntax = SyntaxFactory.Identifier(methodName);
            var internalMethodsDescriptions = internalMethodDescriptionFactories.CreateSelectMany(
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

            var publicMethodsDescriptions = publicMethodDescriptionFactories.CreateSelectMany(
                returnType,
                methodNameSyntax,
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
            SimpleNameSyntax upperBound,
            IEnumerable<GenericNameSyntax> taskTypes)
        {
            return new(upperBound, !taskTypes.Any());
        }

        private InternalMethodsDescription CreateInternal2(
            ClassNamesFactory classNames,
            GenericNameSyntax returnType,
            SyntaxToken methodNameSyntax,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            GenericNameSyntax parameterOtherType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            return internalMethodDescriptionFactories.Create2(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                parametersName);
        }

        private InternalMethodsDescription CreateInternal3(
            ClassNamesPass classNames,
            GenericNameSyntax returnType,
            SyntaxToken methodNameSyntax,
            SyntaxToken[] genericParameters,
            GenericNameSyntax parameterSelfType,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SimpleNameSyntax parameterOtherType,
            IdentifierNameSyntax passType,
            GenericNameSyntax[] parameterOtherTypeTasks,
            ParameterNames parametersName)
        {
            return internalMethodDescriptionFactories.Create3(
                classNames,
                returnType,
                methodNameSyntax,
                genericParameters,
                CreateParameterSelfType(parameterSelfType, parameterSelfTypeTasks),
                CreateParameterSelfType(parameterOtherType, parameterOtherTypeTasks),
                passType,
                parametersName);
        }
    }
}
