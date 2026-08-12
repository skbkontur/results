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
        private readonly ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator;

        public PublicMethodDescriptionFactories(
            IMethodDeclarationFactory methodDeclarationFactory,
            IMethodBodyFactory bodyFactory,
            ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator)
        {
            this.methodDeclarationFactory = methodDeclarationFactory;
            this.bodyFactory = bodyFactory;
            this.typeParameterGenericMethodSyntaxGenerator = typeParameterGenericMethodSyntaxGenerator;
        }

        internal PublicMethodsDescription Create1(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SelfParameter parametersName,
            string defaultClassName,
            IReadOnlyCollection<TypeParameterConstraintClauseSyntax> additionalConstraints,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeGenerator
                .Create1Parameter(computeReturnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks)
                .ToArray();

            var methods = methodTypes
                .Select(method => this.CreateSelf(
                    method,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                    parametersName,
                    SyntaxFactory.IdentifierName(defaultClassName),
                    additionalConstraints,
                    attributes,
                    ReturnTypeCalculator.GetWrapResult(wrapResult, IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method))));

            return new(methods, Array.Empty<MethodDeclarationSyntax>());
        }

        internal PublicMethodsDescription Create2(
           Func<bool, bool, TypeSyntax> computeReturnType,
           SyntaxToken methodName,
           MethodDocumentation documentation,
           IReadOnlyCollection<SyntaxToken> genericParameters,
           GenericNameSyntax parameterSelfTypeUpperBound,
           GenericNameSyntax[] parameterSelfTypeTasks,
           NameSyntax parameterOtherTypeUpperBound,
           GenericNameSyntax[] parameterOtherTypeTasks,
           ParameterNames parametersName,
           ClassNamesFactory classNames,
           IEnumerable<AttributeSyntax> attributes,
           Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(
                    computeReturnType,
                    parameterSelfTypeUpperBound,
                    parameterSelfTypeTasks,
                    MethodTypeGenerator.CreateParameter(parameterOtherTypeUpperBound, parameterOtherTypeTasks))
                .ToArray();

            var valueMethods = methodTypes
                .Select(method => this.CreateFull(
                        attributes,
                        method,
                        methodName,
                        documentation,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        method.Parameter2,
                        parametersName.Value,
                        classNames.ValueNameSyntax(),
                        ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(parameterSelfTypeUpperBound, method))));

            return Assemble(
                methodTypes,
                method => this.CreateFull(
                    Enumerable.Empty<AttributeSyntax>(),
                    method,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                    parametersName.Self,
                    TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, method.Parameter2),
                    parametersName.Factory,
                    classNames.FactoryNameSyntax(),
                    ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(parameterSelfTypeUpperBound, method))),
                valueMethods);
        }

        internal PublicMethodsDescription Create3(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax parameterOtherTypeUpperBound,
            GenericNameSyntax[] parameterOtherTypeTasks,
            IdentifierNameSyntax parameterOtherTypeFuncInput,
            ParameterNames parametersName,
            ClassNamesPass classNames,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(
                    computeReturnType,
                    parameterSelfTypeUpperBound,
                    parameterSelfTypeTasks,
                    MethodTypeGenerator.CreateParameter(parameterOtherTypeUpperBound, parameterOtherTypeTasks))
                .ToArray();

            var valueMethods = methodTypes
                .Select(method => this.CreateFull(
                        attributes,
                        method,
                        methodName,
                        documentation,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        method.Parameter2,
                        parametersName.Value,
                        classNames.ValueNameSyntax(),
                        ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(parameterSelfTypeUpperBound, method))));

            return Assemble(
                methodTypes,
                method => this.CreateFull(
                        Enumerable.Empty<AttributeSyntax>(),
                        method,
                        methodName,
                        documentation,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, method.Parameter2),
                        parametersName.Factory,
                        classNames.FactoryNameSyntax(),
                        ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(parameterSelfTypeUpperBound, method))),
                methodTypes,
                method => this.CreateFull(
                    Enumerable.Empty<AttributeSyntax>(),
                    method,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                    parametersName.Self,
                    TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, parameterOtherTypeFuncInput, method.Parameter2),
                    parametersName.Factory,
                    classNames.PassNameSyntax(),
                    ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(parameterSelfTypeUpperBound, method))),
                valueMethods);
        }

        internal PublicMethodsDescription CreateSelect(
            TypeSyntax returnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax parameterOtherTypeUpperBound,
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
                method => this.CreatePass(
                        method,
                        methodName,
                        documentation,
                        genericParameters,
                        IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                        parametersName.Self,
                        parametersName.Factory,
                        parameterOtherTypeFuncInput,
                        classNames.PassNameSyntax()));

            return new(localMethods, globalMethods);
        }

        internal PublicMethodsDescription CreateSelectMany(
            TypeSyntax returnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            NameSyntax nextParameterOtherTypeUpperBound,
            GenericNameSyntax[] nextParameterOtherTypeTasks,
            NameSyntax parameterOtherTypeUpperBound,
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
                .Where(method => !local || method.ParameterNextIsTask || method.ParameterNextIsValueTask)
                .Select(method => this.CreatePass(
                        method,
                        methodName,
                        documentation,
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

        internal PublicMethodsDescription CreateFactoryOnly(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IEnumerable<MethodTypeGenerator.ParameterInfo> otherParameters,
            SelfParameter selfParameter,
            SyntaxToken parameterName,
            SimpleNameSyntax delegateClassName,
            IEnumerable<AttributeSyntax> attributes)
        {
            var methodTypes = MethodTypeGenerator
                .Create2Parameters(computeReturnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, otherParameters)
                .ToArray();

            return Assemble(
                methodTypes,
                methodType => this.CreateFull(
                    attributes,
                    methodType,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, methodType),
                    selfParameter,
                    methodType.Parameter2,
                    parameterName,
                    delegateClassName),
                Enumerable.Empty<MethodDeclarationSyntax>());
        }

        internal PublicMethodsDescription CreatePassOnly(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            IEnumerable<MethodTypeGenerator.ParameterInfo> factoryParameters,
            IEnumerable<MethodTypeGenerator.ParameterInfo> passParameters,
            SelfParameter selfParameter,
            SyntaxToken parameterName,
            ClassNamesPass classNames,
            IEnumerable<AttributeSyntax> attributes)
        {
            var factoryMethodTypes = MethodTypeGenerator
                .Create2Parameters(computeReturnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, factoryParameters)
                .ToArray();

            var passMethodTypes = MethodTypeGenerator
                .Create2Parameters(computeReturnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, passParameters)
                .ToArray();

            return Assemble(
                factoryMethodTypes,
                methodType => this.CreateFull(
                    attributes,
                    methodType,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, methodType),
                    selfParameter,
                    methodType.Parameter2,
                    parameterName,
                    classNames.FactoryNameSyntax()),
                passMethodTypes,
                methodType => this.CreateFull(
                    attributes,
                    methodType,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, methodType),
                    selfParameter,
                    methodType.Parameter2,
                    parameterName,
                    classNames.PassNameSyntax()),
                Enumerable.Empty<MethodDeclarationSyntax>());
        }

        internal PublicMethodsDescription CreateSwitch(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SelfParameter selfParameter,
            IEnumerable<SwitchTier> tiers,
            SwitchParameterForm nextPassForm,
            SwitchParameterForm otherPassForm,
            SimpleNameSyntax passClassName)
        {
            var tierMethods = tiers
                .Select(tier => this.CreateSwitchTierMethods(
                    computeReturnType,
                    methodName,
                    documentation,
                    genericParameters,
                    parameterSelfTypeUpperBound,
                    parameterSelfTypeTasks,
                    tier.NextForm,
                    tier.OtherForm,
                    selfParameter,
                    tier.ClassNameSyntax))
                .Prepend(this.CreateSwitchTierMethods(
                    computeReturnType,
                    methodName,
                    documentation,
                    genericParameters,
                    parameterSelfTypeUpperBound,
                    parameterSelfTypeTasks,
                    nextPassForm,
                    otherPassForm,
                    selfParameter,
                    passClassName))
                .ToArray();

            return new(
                tierMethods.SelectMany(methods => methods.LocalMethods),
                tierMethods.SelectMany(methods => methods.GlobalMethods));
        }

        internal MethodDeclarationSyntax CreateFull(
            IEnumerable<AttributeSyntax> attributes,
            MethodType2 methodType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            return this.CreateFull(
                attributes,
                methodType,
                methodName,
                documentation,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                parameterType,
                parameterOther,
                delegateClassName,
                SyntaxFactory.ReturnStatement);
        }

        internal MethodDeclarationSyntax CreateFull(
            IEnumerable<AttributeSyntax> attributes,
            MethodType2 methodType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var selfParameterName = firstParameterIsTask ? parameterSelf.TaskName : parameterSelf.Name;

            var body = this.bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters
                    .Select(SyntaxFactory.IdentifierName)
                    .Concat(methodType.GenericArguments),
                new[]
                {
                    SyntaxFactory.IdentifierName(selfParameterName),
                    SyntaxFactory.IdentifierName(parameterOther),
                }.Select(SyntaxFactory.Argument),
                wrapResult);

            var method = this.methodDeclarationFactory.Create(
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

            return method.WithLeadingTrivia(documentation.Trivia.AddRange(method.GetLeadingTrivia()));
        }

        private static PublicMethodsDescription Assemble(
            MethodType2[] factoryMethodTypes,
            Func<MethodType2, MethodDeclarationSyntax> createFactoryMethod,
            IEnumerable<MethodDeclarationSyntax> valueMethods)
        {
            var factoryMethods = NamespaceSplitter.Split(
                factoryMethodTypes,
                createFactoryMethod);

            return new(
                factoryMethods.LocalMethods.Concat(valueMethods),
                factoryMethods.GlobalMethods);
        }

        private static PublicMethodsDescription Assemble(
            MethodType2[] factoryMethodTypes,
            Func<MethodType2, MethodDeclarationSyntax> createFactoryMethod,
            MethodType2[] passMethodTypes,
            Func<MethodType2, MethodDeclarationSyntax> createPassMethod,
            IEnumerable<MethodDeclarationSyntax> valueMethods)
        {
            var factoryMethods = NamespaceSplitter.Split(
                factoryMethodTypes,
                createFactoryMethod);

            var passMethods = NamespaceSplitter.Split(
                passMethodTypes,
                createPassMethod);

            return new(
                passMethods.LocalMethods
                    .Concat(factoryMethods.LocalMethods)
                    .Concat(valueMethods),
                passMethods.GlobalMethods.Concat(factoryMethods.GlobalMethods));
        }

        private static bool IsFirstParameterIsTaskLike(NameSyntax parameterSelfType, MethodType1 method)
        {
            return !method.Parameter1.Equals(parameterSelfType);
        }

        private static bool IsFirstParameterIsTaskLike(NameSyntax parameterSelfType, MethodType2 method)
        {
            return !method.Parameter1.Equals(parameterSelfType);
        }

        private static bool IsFirstParameterIsTaskLike(NameSyntax parameterSelfType, MethodType3 method)
        {
            return !method.Parameter1.Equals(parameterSelfType);
        }

        private static bool HasAnyTaskParameter(NameSyntax parameterSelfType, MethodType2 method)
        {
            return IsFirstParameterIsTaskLike(parameterSelfType, method) || method.Parameter2IsTask || method.Parameter2IsValueTask;
        }

        private (IEnumerable<MethodDeclarationSyntax> LocalMethods, IEnumerable<MethodDeclarationSyntax> GlobalMethods) CreateSwitchTierMethods(
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            GenericNameSyntax parameterSelfTypeUpperBound,
            GenericNameSyntax[] parameterSelfTypeTasks,
            SwitchParameterForm nextForm,
            SwitchParameterForm otherForm,
            SelfParameter selfParameter,
            SimpleNameSyntax delegateClassName)
        {
            var methodTypes = MethodTypeGenerator
                .Create3Parameters(computeReturnType, parameterSelfTypeUpperBound, parameterSelfTypeTasks, nextForm.PublicTypes, otherForm.PublicTypes)
                .ToArray();

            return NamespaceSplitter.Split(
                methodTypes,
                method => this.CreateFull3(
                    Enumerable.Empty<AttributeSyntax>(),
                    method,
                    methodName,
                    documentation,
                    genericParameters,
                    IsFirstParameterIsTaskLike(parameterSelfTypeUpperBound, method),
                    selfParameter,
                    method.ParameterNext,
                    method.Parameter2,
                    nextForm.ParameterName,
                    otherForm.ParameterName,
                    delegateClassName));
        }

        private MethodDeclarationSyntax CreateSelf(
            MethodType1 methodType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            SimpleNameSyntax delegateClassName,
            IReadOnlyCollection<TypeParameterConstraintClauseSyntax> additionalConstraints,
            IEnumerable<AttributeSyntax> attributes,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var selfParameterName = firstParameterIsTask ? parameterSelf.TaskName : parameterSelf.Name;

            var body = this.bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters
                    .Select(SyntaxFactory.IdentifierName)
                    .Concat(methodType.GenericArguments),
                new[]
                {
                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(selfParameterName)),
                },
                wrapResult);

            var method = this.methodDeclarationFactory.Create(
                    attributes,
                    AccessModifierFactory.Create(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword),
                    methodType.ReturnType,
                    methodName,
                    genericParameters,
                    new[]
                    {
                        SyntaxFactory.Parameter(selfParameterName).WithType(methodType.Parameter1).AddModifiers(SyntaxFactory.Token(SyntaxKind.ThisKeyword)),
                    },
                    body);

            var annotatedConstraints = this.typeParameterGenericMethodSyntaxGenerator
                .CreateTypeParameterConstraints(
                    Enumerable.Empty<MethodGenericParameterDescription>(),
                    additionalConstraints)
                .ToArray();
            method = method.AddConstraintClauses(annotatedConstraints);

            return method.WithLeadingTrivia(documentation.Trivia.AddRange(method.GetLeadingTrivia()));
        }

        private MethodDeclarationSyntax CreatePass(
            MethodType3 methodType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            bool firstParameterIsTask,
            SelfParameter parameterSelf,
            SyntaxToken nextParameterOther,
            SyntaxToken parameterOther,
            TypeSyntax nextPassType,
            TypeSyntax passType,
            SimpleNameSyntax delegateClassName)
        {
            return this.CreateFull3(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                documentation,
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
           MethodDocumentation documentation,
           IReadOnlyCollection<SyntaxToken> genericParameters,
           bool firstParameterIsTask,
           SelfParameter parameterSelf,
           SyntaxToken parameterOther,
           TypeSyntax passType,
           SimpleNameSyntax delegateClassName)
        {
            return this.CreateFull(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                documentation,
                genericParameters,
                firstParameterIsTask,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, passType, methodType.Parameter2),
                parameterOther,
                delegateClassName);
        }

        private MethodDeclarationSyntax CreateFull3(
            IEnumerable<AttributeSyntax> attributes,
            MethodType3 methodType,
            SyntaxToken methodName,
            MethodDocumentation documentation,
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

            var body = this.bodyFactory.CreateDelegateToOtherClass(
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
                }.Select(SyntaxFactory.Argument),
                SyntaxFactory.ReturnStatement);

            var method = this.methodDeclarationFactory.Create(
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

            return method.WithLeadingTrivia(documentation.Trivia.AddRange(method.GetLeadingTrivia()));
        }
    }
}
