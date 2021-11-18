using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal class PublicMethodDescriptionFactories
    {
        private readonly IMethodDeclarationFactory methodDeclarationFactory;
        private readonly IMethodBodyFactory bodyFactory;

        public PublicMethodDescriptionFactories(
            IMethodDeclarationFactory methodDeclarationFactory,
            IMethodBodyFactory bodyFactory)
        {
            this.methodDeclarationFactory = methodDeclarationFactory;
            this.bodyFactory = bodyFactory;
        }

        internal PublicMethodsDescription Create2(
           GenericNameSyntax returnType,
           SyntaxToken methodName,
           IReadOnlyCollection<SyntaxToken> genericParameters,
           GenericNameSyntax parameterSelfTypeUpperBound,
           GenericNameSyntax[] parameterSelfTypeTasks,
           GenericNameSyntax parameterOtherTypeUpperBound,
           GenericNameSyntax[] parameterOtherTypeTasks,
           ParameterNames parametersName,
           ClassNamesFactory classNames)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(returnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, parameterOtherTypeUpperBound, parameterOtherTypeTasks)
                .ToArray();

            var valueMethods = methodTypes
                .Select(method =>
                    CreateValue(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Value,
                        classNames.ValueNameSyntax()));

            var factoryMethods = NamespaceSplitter.Split(
                methodTypes,
                method =>
                    CreateFactory(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Factory,
                        classNames.FactoryNameSyntax()));

            return new(
                factoryMethods.LocalMethods.Concat(valueMethods),
                factoryMethods.GlobalMethods);
        }

        internal PublicMethodsDescription Create3(
            GenericNameSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SimpleNameSyntax parameterOtherTypeUpperBound,
            GenericNameSyntax[] parameterOtherTypeTasks,
            IdentifierNameSyntax parameterOtherTypeFuncInput,
            ParameterNames parametersName,
            ClassNamesPass classNames)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(returnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, parameterOtherTypeUpperBound, parameterOtherTypeTasks)
                .ToArray();

            var valueMethods = methodTypes
                .Select(method =>
                    CreateValue(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Value,
                        classNames.ValueNameSyntax()));

            var factoryMethods = NamespaceSplitter.Split(
                methodTypes,
                method =>
                    CreateFactory(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Factory,
                        classNames.FactoryNameSyntax()));

            var passMethods = NamespaceSplitter.Split(
                methodTypes,
                method =>
                    CreatePass(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Factory,
                        parameterOtherTypeFuncInput,
                        classNames.PassNameSyntax()));

            return new(
                passMethods.LocalMethods
                    .Concat(factoryMethods.LocalMethods)
                    .Concat(valueMethods),
                passMethods.GlobalMethods.Concat(factoryMethods.GlobalMethods));
        }

        internal PublicMethodsDescription CreateSelect(
            GenericNameSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SimpleNameSyntax parameterOtherTypeUpperBound,
            GenericNameSyntax[] parameterOtherTypeTasks,
            IdentifierNameSyntax parameterOtherTypeFuncInput,
            ParameterNames parametersName,
            ClassNamesPass classNames)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(returnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, parameterOtherTypeUpperBound, parameterOtherTypeTasks)
                .ToArray();

            var (localMethods, globalMethods) = NamespaceSplitter.Split(
                methodTypes,
                method =>
                    CreatePass(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Factory,
                        parameterOtherTypeFuncInput,
                        classNames.PassNameSyntax()));

            return new(localMethods, globalMethods);
        }

        internal PublicMethodsDescription CreateSelectMany(
            GenericNameSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SimpleNameSyntax nextParameterOtherTypeUpperBound,
            GenericNameSyntax[] nextParameterOtherTypeTasks,
            SimpleNameSyntax parameterOtherTypeUpperBound,
            GenericNameSyntax[] parameterOtherTypeTasks,
            IdentifierNameSyntax nextParameterOtherTypeFuncInput,
            IdentifierNameSyntax parameterOtherTypeFuncInput,
            ParameterNames3 parametersName,
            ClassNamesPass classNames,
            bool local)
        {
            var methodTypes = MethodTypeGenerator
                .Create3Parameters(
                    returnType,
                    parameterSelfTypeUpperBound,
                    parameterSelfTypeTasks,
                    nextParameterOtherTypeUpperBound,
                    nextParameterOtherTypeTasks,
                    parameterOtherTypeUpperBound,
                    parameterOtherTypeTasks)
                .Where(method => !local || method.ParameterNextIsTask)
                .Select(method =>
                    CreatePass(
                        method,
                        methodName,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.NextFactory,
                        parametersName.Factory,
                        nextParameterOtherTypeFuncInput,
                        parameterOtherTypeFuncInput,
                        classNames.PassNameSyntax()))
                .ToArray();

            return local
                ? new(methodTypes, Array.Empty<MethodDeclarationSyntax>())
                : new(Array.Empty<MethodDeclarationSyntax>(), methodTypes);
        }

        private static bool IsFirstParameterIsTaskLike(SimpleNameSyntax parameterSelfType, MethodType2 method)
        {
            return !method.Parameter1.Equals(parameterSelfType);
        }

        private static bool IsFirstParameterIsTaskLike(SimpleNameSyntax parameterSelfType, MethodType3 method)
        {
            return !method.Parameter1.Equals(parameterSelfType);
        }

        private MethodDeclarationSyntax CreatePass(
            MethodType3 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            SyntaxToken nextParameterOther,
            SyntaxToken parameterOther,
            TypeSyntax nextPassType,
            TypeSyntax passType,
            SimpleNameSyntax delegateClassName)
        {
            return CreateFull(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, nextPassType, methodType.ParameterNext),
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, nextPassType, passType, methodType.Parameter2),
                nextParameterOther,
                parameterOther,
                delegateClassName);
        }

        private MethodDeclarationSyntax CreatePass(
           MethodType2 methodType,
           SyntaxToken methodName,
           IReadOnlyCollection<SyntaxToken> genericParameters,
           bool firstParameterIsTask,
           SelfParameter parameterSelf,
           SyntaxToken parameterOther,
           TypeSyntax passType,
           SimpleNameSyntax delegateClassName)
        {
            return CreateFull(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, passType, methodType.Parameter2),
                parameterOther,
                delegateClassName);
        }

        private MethodDeclarationSyntax CreateFactory(
            MethodType2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            return CreateFull(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, methodType.Parameter2),
                parameterOther,
                delegateClassName);
        }

        private MethodDeclarationSyntax CreateValue(
            MethodType2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            return CreateFull(
                new[] { Attributes.Pure },
                methodType,
                methodName,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                methodType.Parameter2,
                parameterOther,
                delegateClassName);
        }

        private MethodDeclarationSyntax CreateFull(
            IEnumerable<AttributeSyntax> attributes,
            MethodType2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            var selfParameterName = firstParameterIsTask ? parameterSelf.TaskName : parameterSelf.Name;

            var body = bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters
                    .Select(SyntaxFactory.IdentifierName)
                    .Concat(methodType.GenericArguments),
                new[]
                {
                    SyntaxFactory.IdentifierName(selfParameterName),
                    SyntaxFactory.IdentifierName(parameterOther),
                }.Select(SyntaxFactory.Argument));

            return methodDeclarationFactory.Create(
                attributes,
                AccessModifierFactory.Create(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword),
                methodType.ReturnType,
                methodName,
                genericParameters,
                new[]
                {
                    SyntaxFactory.Parameter(selfParameterName).WithType(methodType.Parameter1).AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                    SyntaxFactory.Parameter(parameterOther).WithType(parameterType),
                },
                body);
        }

        private MethodDeclarationSyntax CreateFull(
            IEnumerable<AttributeSyntax> attributes,
            MethodType3 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            TypeSyntax nextParameterType,
            TypeSyntax parameterType,
            SyntaxToken nextParameterOther,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            var selfParameterName = firstParameterIsTask ? parameterSelf.TaskName : parameterSelf.Name;

            var body = bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters
                    .Select(SyntaxFactory.IdentifierName)
                    .Concat(methodType.GenericArguments),
                new[]
                {
                    SyntaxFactory.IdentifierName(selfParameterName),
                    SyntaxFactory.IdentifierName(nextParameterOther),
                    SyntaxFactory.IdentifierName(parameterOther),
                }.Select(SyntaxFactory.Argument));

            return methodDeclarationFactory.Create(
                attributes,
                AccessModifierFactory.Create(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword),
                methodType.ReturnType,
                methodName,
                genericParameters,
                new[]
                {
                    SyntaxFactory.Parameter(selfParameterName).WithType(methodType.Parameter1).AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                    SyntaxFactory.Parameter(nextParameterOther).WithType(nextParameterType),
                    SyntaxFactory.Parameter(parameterOther).WithType(parameterType),
                },
                body);
        }
    }
}