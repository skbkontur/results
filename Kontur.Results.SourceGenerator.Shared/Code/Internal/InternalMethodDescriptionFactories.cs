using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal class InternalMethodDescriptionFactories
    {
        private readonly IMethodDeclarationFactory methodDeclarationFactory;
        private readonly IMethodBodyFactory bodyFactory;
        private readonly ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator;

        public InternalMethodDescriptionFactories(
            IMethodDeclarationFactory methodDeclarationFactory,
            IMethodBodyFactory bodyFactory,
            ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator)
        {
            this.methodDeclarationFactory = methodDeclarationFactory;
            this.bodyFactory = bodyFactory;
            this.typeParameterGenericMethodSyntaxGenerator = typeParameterGenericMethodSyntaxGenerator;
        }

        internal static GenericNameSyntax CreateFactoryParameter(MethodTypeInternal2 methodType)
        {
            return TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, methodType.Parameter2.Type);
        }

        internal InternalMethodsDescription Create1(
            string className,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            SelfParameter parametersName,
            IReadOnlyCollection<TypeParameterConstraintClauseSyntax> additionalConstraints,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create1Parameter(computeReturnType, parameterSelfType)
                .ToArray();

            var methods = methodTypes
                .Where(method => method.Parameter1.IsTaskLike)
                .Select(method => this.CreatePartial(
                    method,
                    methodName,
                    genericParameters,
                    parametersName,
                    additionalConstraints,
                    wrapResult));

            InternalPartialMethodDescription partialDescription = new(
                className,
                methods);

            return new(Enumerable.Empty<InternalStandardMethodsDescription>(), partialDescription);
        }

        internal InternalMethodsDescription Create2(
            ClassNamesFactory classNames,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            MethodTypeGeneratorParameter parameterOtherType,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            IEnumerable<UsingDirectiveSyntax> valueUsings,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create2Parameters(computeReturnType, parameterSelfType, MethodTypeInternalGenerator.CreateParameters(parameterOtherType, 2))
                .ToArray();

            var valueMethods = methodTypes
                .Select(method => this.CreateValue(
                    attributes,
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Value,
                    classNames.FactoryNameSyntax(),
                    ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(method))));

            InternalStandardMethodsDescription valueMethodsDescription = new(
                classNames.Value,
                valueUsings,
                valueMethods);

            return this.Assemble(
                methodTypes,
                new[] { valueMethodsDescription },
                classNames.Factory,
                methodName,
                genericParameters,
                parametersName.Self,
                CreateFactoryParameter,
                parametersName.Factory,
                wrapResult);
        }

        internal InternalMethodsDescription Create3(
            ClassNamesPass classNames,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            MethodTypeGeneratorParameter parameterOtherType,
            TypeSyntax passType,
            ParameterNames parametersName,
            IEnumerable<AttributeSyntax> attributes,
            IEnumerable<UsingDirectiveSyntax> valueUsings,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create2Parameters(computeReturnType, parameterSelfType, MethodTypeInternalGenerator.CreateParameters(parameterOtherType, 2))
                .ToArray();

            var valueMethods = methodTypes
                .Select(method => this.CreateValue(
                    attributes,
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Value,
                    classNames.FactoryNameSyntax(),
                    ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(method))));

            InternalStandardMethodsDescription valueMethodsDescription = new(
                classNames.Value,
                valueUsings,
                valueMethods);

            var factoryMethods = methodTypes.Select(method => this.CreateFactory(
                method,
                methodName,
                genericParameters,
                parametersName.Self,
                parametersName.Factory,
                classNames.PassNameSyntax(),
                CreateFactoryParameter,
                ReturnTypeCalculator.GetWrapResult(wrapResult, HasAnyTaskParameter(method))));

            InternalStandardMethodsDescription factoryMethodsDescription = new(
                classNames.Factory,
                new[]
                {
                    Using.System,
                    Using.Tasks,
                },
                factoryMethods);

            return this.Assemble(
                methodTypes,
                new[] { valueMethodsDescription, factoryMethodsDescription },
                classNames.Pass,
                methodName,
                genericParameters,
                parametersName.Self,
                method => TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, passType, method.Parameter2.Type),
                parametersName.Factory,
                wrapResult);
        }

        internal InternalMethodsDescription CreateFactoryOnly(
            string factoryClassName,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            IEnumerable<InternalTypeParameter> otherParameters,
            SelfParameter selfParameter,
            SyntaxToken parameterName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create2Parameters(computeReturnType, parameterSelfType, otherParameters)
                .ToArray();

            return this.Assemble(
                methodTypes,
                Enumerable.Empty<InternalStandardMethodsDescription>(),
                factoryClassName,
                methodName,
                genericParameters,
                selfParameter,
                method => method.Parameter2.Type,
                parameterName,
                wrapResult);
        }

        internal InternalMethodsDescription CreatePassOnly(
            ClassNamesPass classNames,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            IEnumerable<InternalTypeParameter> factoryParameters,
            IEnumerable<InternalTypeParameter> passParameters,
            SelfParameter selfParameter,
            SyntaxToken parameterName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var factoryMethodTypes = MethodTypeInternalGenerator
                .Create2Parameters(computeReturnType, parameterSelfType, factoryParameters)
                .ToArray();

            var factoryMethods = factoryMethodTypes.Select(factoryMethod => this.CreateFactory(
                factoryMethod,
                methodName,
                genericParameters,
                selfParameter,
                parameterName,
                classNames.PassNameSyntax(),
                method => method.Parameter2.Type));

            InternalStandardMethodsDescription factoryMethodsDescription = new(
                classNames.Factory,
                new[]
                {
                    Using.System,
                    Using.Tasks,
                },
                factoryMethods);

            var passMethodTypes = MethodTypeInternalGenerator
                .Create2Parameters(computeReturnType, parameterSelfType, passParameters)
                .ToArray();

            return this.Assemble(
                passMethodTypes,
                new[] { factoryMethodsDescription },
                classNames.Pass,
                methodName,
                genericParameters,
                selfParameter,
                method => method.Parameter2.Type,
                parameterName,
                wrapResult);
        }

        internal InternalMethodsDescription CreateSwitch(
            string passClassName,
            IEnumerable<SwitchTier> tiers,
            Func<bool, bool, TypeSyntax> computeReturnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            SelfParameter selfParameter,
            SwitchParameterForm nextPassForm,
            SwitchParameterForm otherPassForm)
        {
            var standardTiers = tiers
                .Select(tier => new InternalStandardMethodsDescription(
                    tier.ClassName,
                    new[]
                    {
                        Using.System,
                        Using.Tasks,
                    },
                    MethodTypeInternalGenerator
                        .Create3Parameters(computeReturnType, parameterSelfType, tier.NextForm.InternalTypes, tier.OtherForm.InternalTypes)
                        .Select(methodType => this.CreateSwitchFactory(methodType, methodName, genericParameters, selfParameter, tier))))
                .ToArray();

            var passMethodTypes = MethodTypeInternalGenerator
                .Create3Parameters(computeReturnType, parameterSelfType, nextPassForm.InternalTypes, otherPassForm.InternalTypes)
                .ToArray();

            return this.Assemble(
                passMethodTypes,
                standardTiers,
                passClassName,
                methodName,
                genericParameters,
                selfParameter,
                method => method.ParameterNext.Type,
                nextPassForm.ParameterName,
                method => method.Parameter2.Type,
                otherPassForm.ParameterName,
                SyntaxFactory.ReturnStatement);
        }

        internal InternalMethodsDescription CreateSelect(
            ClassNamesPass classNames,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            MethodTypeGeneratorParameter parameterOtherType,
            TypeSyntax passType,
            ParameterNames parametersName)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create2Parameters(returnType, parameterSelfType, parameterOtherType)
                .ToArray();

            var passMethods = methodTypes
                .Where(IsFirstParameterTask)
                .Select(method => this.CreatePassPartial(
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Factory,
                    passType));

            InternalPartialMethodDescription passMethodsDescription = new(
                classNames.Pass,
                passMethods);

            return new(Enumerable.Empty<InternalStandardMethodsDescription>(), passMethodsDescription);
        }

        internal InternalMethodsDescription CreateSelectMany(
            ClassNamesPass classNames,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            MethodTypeGeneratorParameter parameterNextType,
            MethodTypeGeneratorParameter parameterOtherType,
            TypeSyntax nextPassType,
            TypeSyntax passType,
            ParameterNames3 parametersName)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create3Parameters(returnType, parameterSelfType, parameterNextType, parameterOtherType)
                .ToArray();

            var passMethods = methodTypes
                .Where(IsFirstParameterTask)
                .Select(method => this.CreatePassPartial(
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.NextFactory,
                    parametersName.Factory,
                    nextPassType,
                    passType));

            InternalPartialMethodDescription passMethodsDescription = new(
                classNames.Pass,
                passMethods);

            return new(Enumerable.Empty<InternalStandardMethodsDescription>(), passMethodsDescription);
        }

        internal MethodDeclarationSyntax CreatePassPartial(
           MethodTypeInternal2 methodType,
           SyntaxToken methodName,
           IReadOnlyCollection<SyntaxToken> genericParameters,
           SelfParameter parameterSelf,
           SyntaxToken parameterOther,
           TypeSyntax passType)
        {
            return this.CreatePartial(
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, passType, methodType.Parameter2.Type),
                parameterOther,
                SyntaxFactory.ReturnStatement);
        }

        internal MethodDeclarationSyntax CreatePassPartial(
            MethodTypeInternal3 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterNext,
            SyntaxToken parameterOther,
            TypeSyntax nextPassType,
            TypeSyntax passType)
        {
            return this.CreatePartial(
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, nextPassType, methodType.ParameterNext.Type),
                parameterNext,
                TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, nextPassType, passType, methodType.Parameter2.Type),
                parameterOther,
                SyntaxFactory.ReturnStatement);
        }

        internal MethodDeclarationSyntax CreateFactory(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            Func<MethodTypeInternal2, TypeSyntax> parameterTypeSelector)
        {
            return this.CreateFactory(
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                parameterOther,
                delegateClassName,
                parameterTypeSelector,
                SyntaxFactory.ReturnStatement);
        }

        internal MethodDeclarationSyntax CreateFactory(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            Func<MethodTypeInternal2, TypeSyntax> parameterTypeSelector,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            return this.CreateDelegate(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                parameterTypeSelector(methodType),
                parameterOther,
                delegateClassName,
                SyntaxFactory.SimpleLambdaExpression(
                    SyntaxFactory.Parameter(Identifiers.Discard),
                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(parameterOther))),
                wrapResult);
        }

        internal MethodDeclarationSyntax CreateValue(
            IEnumerable<AttributeSyntax> attributes,
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            return this.CreateDelegate(
                attributes,
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                methodType.Parameter2.Type,
                parameterOther,
                delegateClassName,
                SyntaxFactory.ParenthesizedLambdaExpression(
                    SyntaxFactory.ParameterList(),
                    SyntaxFactory.IdentifierName(parameterOther)),
                wrapResult);
        }

        internal MethodDeclarationSyntax CreateSwitchFactory(
            MethodTypeInternal3 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SwitchTier tier)
        {
            return this.CreateDelegate(
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                methodType.ParameterNext.Type,
                tier.NextForm.ParameterName,
                methodType.Parameter2.Type,
                tier.OtherForm.ParameterName,
                tier.DelegateTarget,
                tier.NextArgument,
                tier.OtherArgument,
                SyntaxFactory.ReturnStatement);
        }

        private static bool IsFirstParameterTask(MethodTypeInternal2 method)
        {
            return method.Parameter1.IsTaskLike;
        }

        private static bool IsFirstParameterTask(MethodTypeInternal3 method)
        {
            return method.Parameter1.IsTaskLike;
        }

        private static bool HasAnyTaskParameter(MethodTypeInternal2 method)
        {
            return method.Parameter1.IsTaskLike || method.Parameter2.IsTaskLike;
        }

        private static IEnumerable<MethodTypeGenericParameter> GetGenericParameters(params InternalTypeParameter[] methodTypeGenericParameters)
        {
            return methodTypeGenericParameters.Select(parameter => parameter.Generic).WhereNotNull();
        }

        private static IReadOnlyCollection<MethodTypeGenericParameter> GetGenericParameters(MethodTypeInternal2 methodType)
        {
            return GetGenericParameters(methodType.Parameter1, methodType.Parameter2).ToArray();
        }

        private static IReadOnlyCollection<MethodTypeGenericParameter> GetGenericParameters(MethodTypeInternal3 methodType)
        {
            return GetGenericParameters(methodType.Parameter1, methodType.ParameterNext, methodType.Parameter2).ToArray();
        }

        private static BlockSyntax CreateInternalPartial(
           SyntaxToken methodName,
           IEnumerable<TypeSyntax> genericArguments,
           SelfParameter selfParameter,
           IdentifierNameSyntax argumentOther,
           bool awaitDelegate,
           Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var awaitCall = WrapIntoAwait(SyntaxFactory.IdentifierName(selfParameter.TaskName));

            StatementSyntax awaitAndStore = SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                   SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("var")),
                   SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(selfParameter.Name)
                            .WithInitializer(SyntaxFactory.EqualsValueClause(awaitCall)))));

            var methodCallExpression = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.GenericName(
                        methodName,
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(genericArguments))))
                .WithArgumentList(SyntaxFactory.ArgumentList(
                    SyntaxFactory.SeparatedList(new[]
                        {
                            SyntaxFactory.IdentifierName(selfParameter.Name),
                            argumentOther,
                        }
                        .Select(SyntaxFactory.Argument)
                        .ToArray())));

            return SyntaxFactory.Block()
                .WithStatements(SyntaxFactory.List(new[]
                {
                    awaitAndStore,
                    wrapResult(awaitDelegate ? WrapIntoAwait(methodCallExpression) : methodCallExpression),
                }));
        }

        private static BlockSyntax CreateInternalPartial(
            SyntaxToken methodName,
            IEnumerable<TypeSyntax> genericArguments,
            SelfParameter selfParameter,
            IdentifierNameSyntax argumentOtherNext,
            bool awaitDelegateNext,
            IdentifierNameSyntax argumentOther,
            bool awaitDelegate,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var awaitCall = WrapIntoAwait(SyntaxFactory.IdentifierName(selfParameter.TaskName));

            StatementSyntax awaitAndStore = SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("var")),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(selfParameter.Name)
                            .WithInitializer(SyntaxFactory.EqualsValueClause(awaitCall)))));

            var methodCallExpression = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.GenericName(
                        methodName,
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(genericArguments))))
                .WithArgumentList(SyntaxFactory.ArgumentList(
                    SyntaxFactory.SeparatedList(new[]
                        {
                            SyntaxFactory.IdentifierName(selfParameter.Name),
                            argumentOtherNext,
                            argumentOther,
                        }
                        .Select(SyntaxFactory.Argument)
                        .ToArray())));

            return SyntaxFactory.Block()
                .WithStatements(SyntaxFactory.List(new[]
                {
                    awaitAndStore,
                    wrapResult(awaitDelegateNext || awaitDelegate ? WrapIntoAwait(methodCallExpression) : methodCallExpression),
                }));
        }

        private static AwaitExpressionSyntax WrapIntoAwait(ExpressionSyntax expressionToAwait)
        {
            return SyntaxFactory.AwaitExpression(SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        expressionToAwait,
                        SyntaxFactory.IdentifierName(nameof(Task.ConfigureAwait))))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.FalseLiteralExpression))))));
        }

        private InternalMethodsDescription Assemble(
            MethodTypeInternal2[] partialMethodTypes,
            IEnumerable<InternalStandardMethodsDescription> standardTiers,
            string partialClassName,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            SelfParameter selfParameter,
            Func<MethodTypeInternal2, TypeSyntax> partialParameterTypeSelector,
            SyntaxToken partialParameterName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var partialMethods = partialMethodTypes
                .Where(IsFirstParameterTask)
                .Select(method => this.CreatePartial(
                    method,
                    methodName,
                    genericParameters,
                    selfParameter,
                    partialParameterTypeSelector(method),
                    partialParameterName,
                    wrapResult));

            InternalPartialMethodDescription partialMethodsDescription = new(
                partialClassName,
                partialMethods);

            return new(standardTiers, partialMethodsDescription);
        }

        private InternalMethodsDescription Assemble(
            MethodTypeInternal3[] partialMethodTypes,
            IEnumerable<InternalStandardMethodsDescription> standardTiers,
            string partialClassName,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            SelfParameter selfParameter,
            Func<MethodTypeInternal3, TypeSyntax> nextParameterTypeSelector,
            SyntaxToken nextParameterName,
            Func<MethodTypeInternal3, TypeSyntax> otherParameterTypeSelector,
            SyntaxToken otherParameterName,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var partialMethods = partialMethodTypes
                .Where(IsFirstParameterTask)
                .Select(method => this.CreatePartial(
                    method,
                    methodName,
                    genericParameters,
                    selfParameter,
                    nextParameterTypeSelector(method),
                    nextParameterName,
                    otherParameterTypeSelector(method),
                    otherParameterName,
                    wrapResult));

            InternalPartialMethodDescription partialMethodsDescription = new(
                partialClassName,
                partialMethods);

            return new(standardTiers, partialMethodsDescription);
        }

        private MethodDeclarationSyntax CreateDelegate(
            IEnumerable<AttributeSyntax> attributes,
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            ExpressionSyntax secondArgument)
        {
            return this.CreateDelegate(
                attributes,
                methodType,
                methodName,
                genericParameterNames,
                parameterSelf,
                parameterType,
                parameterOther,
                delegateClassName,
                secondArgument,
                SyntaxFactory.ReturnStatement);
        }

        private MethodDeclarationSyntax CreateDelegate(
            IEnumerable<AttributeSyntax> attributes,
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            ExpressionSyntax secondArgument,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var selfParameterName = methodType.Parameter1.IsTaskLike ? parameterSelf.TaskName : parameterSelf.Name;
            var genericParameterTypes = GetGenericParameters(methodType);
            var genericParameters = genericParameterNames
                .Concat(genericParameterTypes.Select(genericParameter => genericParameter.Identifier))
                .ToArray();

            var blockSyntax = this.bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters.Select(SyntaxFactory.IdentifierName),
                new[]
                {
                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(selfParameterName)),
                    SyntaxFactory.Argument(secondArgument),
                },
                wrapResult);

            var parameterSyntaxes = new[]
            {
                SyntaxFactory.Parameter(selfParameterName).WithType(methodType.Parameter1.Type),
                SyntaxFactory.Parameter(parameterOther).WithType(parameterType),
            };
            var methodDeclarationSyntax = this.methodDeclarationFactory.Create(
                attributes,
                AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword),
                methodType.ReturnType,
                methodName,
                genericParameters,
                parameterSyntaxes,
                blockSyntax);

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax, Enumerable.Empty<TypeParameterConstraintClauseSyntax>());
        }

        private MethodDeclarationSyntax CreateDelegate(
            MethodTypeInternal3 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterNextType,
            SyntaxToken parameterNext,
            TypeSyntax parameterOtherType,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName,
            ExpressionSyntax secondArgument,
            ExpressionSyntax thirdArgument,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var selfParameterName = methodType.Parameter1.IsTaskLike ? parameterSelf.TaskName : parameterSelf.Name;
            var genericParameterTypes = GetGenericParameters(methodType);
            var genericParameters = genericParameterNames
                .Concat(genericParameterTypes.Select(genericParameter => genericParameter.Identifier))
                .ToArray();

            var blockSyntax = this.bodyFactory.CreateDelegateToOtherClass(
                methodName,
                delegateClassName,
                genericParameters.Select(SyntaxFactory.IdentifierName),
                new[]
                {
                    SyntaxFactory.Argument(SyntaxFactory.IdentifierName(selfParameterName)),
                    SyntaxFactory.Argument(secondArgument),
                    SyntaxFactory.Argument(thirdArgument),
                },
                wrapResult);

            var parameterSyntaxes = new[]
            {
                SyntaxFactory.Parameter(selfParameterName).WithType(methodType.Parameter1.Type),
                SyntaxFactory.Parameter(parameterNext).WithType(parameterNextType),
                SyntaxFactory.Parameter(parameterOther).WithType(parameterOtherType),
            };
            var methodDeclarationSyntax = this.methodDeclarationFactory.Create(
                Enumerable.Empty<AttributeSyntax>(),
                AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword),
                methodType.ReturnType,
                methodName,
                genericParameters,
                parameterSyntaxes,
                blockSyntax);

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax, Enumerable.Empty<TypeParameterConstraintClauseSyntax>());
        }

        private MethodDeclarationSyntax AddTypeConstraints(
            IEnumerable<MethodTypeGenericParameter> genericParameterTypes,
            MethodDeclarationSyntax methodDeclarationSyntax,
            IEnumerable<TypeParameterConstraintClauseSyntax> additionalConstraints)
        {
            var typeParameterConstraints = this.typeParameterGenericMethodSyntaxGenerator
                .CreateTypeParameterConstraints(
                    genericParameterTypes
                        .Select(param => new MethodGenericParameterDescription(param.Identifier, param.UpperBound)),
                    additionalConstraints)
                .ToArray();

            return methodDeclarationSyntax.AddConstraintClauses(typeParameterConstraints);
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal1 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            IReadOnlyCollection<TypeParameterConstraintClauseSyntax> additionalConstraints)
        {
            return this.CreatePartial(
                methodType,
                methodName,
                genericParameterNames,
                parameterSelf,
                additionalConstraints,
                SyntaxFactory.ReturnStatement);
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal1 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            IReadOnlyCollection<TypeParameterConstraintClauseSyntax> additionalConstraints,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var genericArguments = genericParameterNames
                .Select(SyntaxFactory.IdentifierName);

            var awaitCall = WrapIntoAwait(SyntaxFactory.IdentifierName(parameterSelf.TaskName));

            StatementSyntax awaitAndStore = SyntaxFactory.LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("var")),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(parameterSelf.Name)
                            .WithInitializer(SyntaxFactory.EqualsValueClause(awaitCall)))));

            var methodCallExpression = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.GenericName(
                        methodName,
                        SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(genericArguments.Select(name => (TypeSyntax)name)))))
                .WithArgumentList(SyntaxFactory.ArgumentList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Argument(SyntaxFactory.IdentifierName(parameterSelf.Name)))));

            var body = SyntaxFactory.Block()
                .WithStatements(SyntaxFactory.List(new[]
                {
                    awaitAndStore,
                    wrapResult(methodCallExpression),
                }));

            var genericParameterTypes = GetGenericParameters(methodType.Parameter1).ToArray();

            var parameterSyntaxes = new[]
            {
                SyntaxFactory.Parameter(parameterSelf.TaskName).WithType(methodType.Parameter1.Type),
            };
            var methodDeclarationSyntax = this.methodDeclarationFactory.Create(
                AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword, SyntaxKind.AsyncKeyword),
                methodType.ReturnType,
                methodName,
                genericParameterNames
                    .Concat(genericParameterTypes.Select(genericParameter => genericParameter.Identifier)),
                parameterSyntaxes,
                body);

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax, additionalConstraints);
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var genericArguments = genericParameterNames
                .Concat(GetGenericParameters(methodType.Parameter2).Select(genericParameter => genericParameter.Identifier))
                .Select(SyntaxFactory.IdentifierName);

            var body = CreateInternalPartial(
                methodName,
                genericArguments,
                parameterSelf,
                SyntaxFactory.IdentifierName(parameterOther),
                methodType.Parameter2.IsTaskLike,
                wrapResult);

            var genericParameterTypes = GetGenericParameters(methodType);

            var parameterSyntaxes = new[]
            {
                SyntaxFactory.Parameter(parameterSelf.TaskName).WithType(methodType.Parameter1.Type),
                SyntaxFactory.Parameter(parameterOther).WithType(parameterType),
            };
            var methodDeclarationSyntax = this.methodDeclarationFactory.Create(
                AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword, SyntaxKind.AsyncKeyword),
                methodType.ReturnType,
                methodName,
                genericParameterNames
                    .Concat(genericParameterTypes.Select(genericParameter => genericParameter.Identifier)),
                parameterSyntaxes,
                body);

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax, Enumerable.Empty<TypeParameterConstraintClauseSyntax>());
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal3 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterTypeNext,
            SyntaxToken parameterNext,
            TypeSyntax parameterType,
            SyntaxToken parameterOther,
            Func<ExpressionSyntax, StatementSyntax> wrapResult)
        {
            var genericArguments = genericParameterNames
                .Concat(GetGenericParameters(methodType.ParameterNext).Select(genericParameter => genericParameter.Identifier))
                .Concat(GetGenericParameters(methodType.Parameter2).Select(genericParameter => genericParameter.Identifier))
                .Select(SyntaxFactory.IdentifierName);

            var body = CreateInternalPartial(
                methodName,
                genericArguments,
                parameterSelf,
                SyntaxFactory.IdentifierName(parameterNext),
                methodType.ParameterNext.IsTaskLike,
                SyntaxFactory.IdentifierName(parameterOther),
                methodType.Parameter2.IsTaskLike,
                wrapResult);

            var genericParameterTypes = GetGenericParameters(methodType);

            var parameterSyntaxes = new[]
            {
                SyntaxFactory.Parameter(parameterSelf.TaskName).WithType(methodType.Parameter1.Type),
                SyntaxFactory.Parameter(parameterNext).WithType(parameterTypeNext),
                SyntaxFactory.Parameter(parameterOther).WithType(parameterType),
            };
            var methodDeclarationSyntax = this.methodDeclarationFactory.Create(
                AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword, SyntaxKind.AsyncKeyword),
                methodType.ReturnType,
                methodName,
                genericParameterNames
                    .Concat(genericParameterTypes.Select(genericParameter => genericParameter.Identifier)),
                parameterSyntaxes,
                body);

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax, Enumerable.Empty<TypeParameterConstraintClauseSyntax>());
        }
    }
}
