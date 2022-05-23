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

        internal InternalMethodsDescription Create2(
            ClassNamesFactory classNames,
            GenericNameSyntax returnType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            MethodTypeGeneratorParameter parameterSelfType,
            MethodTypeGeneratorParameter parameterOtherType,
            ParameterNames parametersName)
        {
            var methodTypes = MethodTypeInternalGenerator
                .Create2Parameters(returnType, parameterSelfType, parameterOtherType)
                .ToArray();

            var valueMethods = methodTypes
                .Select(method => this.CreateValue(
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Value,
                    classNames.FactoryNameSyntax()));

            InternalStandardMethodsDescription valueMethodsDescription = new(
                classNames.Value,
                new[]
                {
                    Using.Contracts,
                    Using.Tasks,
                },
                valueMethods);

            var factoryMethods = methodTypes
                .Where(IsFirstParameterTask)
                .Select(method => this.CreateFactoryPartial(
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Factory));

            InternalPartialMethodDescription factoryMethodsDescription = new(
                classNames.Factory,
                factoryMethods);

            return new(new[] { valueMethodsDescription }, factoryMethodsDescription);
        }

        internal InternalMethodsDescription Create3(
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

            var valueMethods = methodTypes
                .Select(method => this.CreateValue(
                    method,
                    methodName,
                    genericParameters,
                    parametersName.Self,
                    parametersName.Value,
                    classNames.FactoryNameSyntax()));

            InternalStandardMethodsDescription valueMethodsDescription = new(
                classNames.Value,
                new[]
                {
                    Using.Contracts,
                    Using.Tasks,
                },
                valueMethods);

            var factoryMethods = methodTypes.Select(method => this.CreateFactory(
                method,
                methodName,
                genericParameters,
                parametersName.Self,
                parametersName.Factory,
                classNames.PassNameSyntax()));

            InternalStandardMethodsDescription factoryMethodsDescription = new(
                classNames.Factory,
                new[]
                {
                    Using.System,
                    Using.Tasks,
                },
                factoryMethods);

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

            return new(new[] { valueMethodsDescription, factoryMethodsDescription }, passMethodsDescription);
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
                parameterOther);
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
                parameterOther);
        }

        internal MethodDeclarationSyntax CreateFactoryPartial(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther)
        {
            return this.CreatePartial(
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                CreateFactoryParameter(methodType),
                parameterOther);
        }

        internal MethodDeclarationSyntax CreateFactory(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            return this.CreateDelegate(
                Enumerable.Empty<AttributeSyntax>(),
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                CreateFactoryParameter(methodType),
                parameterOther,
                delegateClassName,
                SyntaxFactory.SimpleLambdaExpression(
                    SyntaxFactory.Parameter(Identifiers.Discard),
                    SyntaxFactory.InvocationExpression(SyntaxFactory.IdentifierName(parameterOther))));
        }

        internal MethodDeclarationSyntax CreateValue(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            SelfParameter parameterSelf,
            SyntaxToken parameterOther,
            SimpleNameSyntax delegateClassName)
        {
            return this.CreateDelegate(
                new[] { Attributes.Pure },
                methodType,
                methodName,
                genericParameters,
                parameterSelf,
                methodType.Parameter2.Type,
                parameterOther,
                delegateClassName,
                SyntaxFactory.ParenthesizedLambdaExpression(
                    SyntaxFactory.ParameterList(),
                    SyntaxFactory.IdentifierName(parameterOther)));
        }

        private static bool IsFirstParameterTask(MethodTypeInternal2 method)
        {
            return method.Parameter1.IsTaskLike;
        }

        private static bool IsFirstParameterTask(MethodTypeInternal3 method)
        {
            return method.Parameter1.IsTaskLike;
        }

        private static GenericNameSyntax CreateFactoryParameter(MethodTypeInternal2 methodType)
        {
            return TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, methodType.Parameter2.Type);
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
           bool awaitDelegate)
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
                    SyntaxFactory.ReturnStatement(awaitDelegate ? WrapIntoAwait(methodCallExpression) : methodCallExpression),
                }));
        }

        private static BlockSyntax CreateInternalPartial(
            SyntaxToken methodName,
            IEnumerable<TypeSyntax> genericArguments,
            SelfParameter selfParameter,
            IdentifierNameSyntax argumentOtherNext,
            bool awaitDelegateNext,
            IdentifierNameSyntax argumentOther,
            bool awaitDelegate)
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
                    SyntaxFactory.ReturnStatement(awaitDelegateNext || awaitDelegate ? WrapIntoAwait(methodCallExpression) : methodCallExpression),
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
                });

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

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax);
        }

        private MethodDeclarationSyntax AddTypeConstraints(
            IEnumerable<MethodTypeGenericParameter> genericParameterTypes,
            MethodDeclarationSyntax methodDeclarationSyntax)
        {
            var typeParameterConstraints = this.typeParameterGenericMethodSyntaxGenerator
                .CreateTypeParameterConstraints(genericParameterTypes
                    .Select(param => new MethodGenericParameterDescription(param.Identifier, param.UpperBound)))
                .ToArray();

            return methodDeclarationSyntax.AddConstraintClauses(typeParameterConstraints);
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal2 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterType,
            SyntaxToken parameterOther)
        {
            var genericArguments = genericParameterNames
                .Concat(GetGenericParameters(methodType.Parameter2).Select(genericParameter => genericParameter.Identifier))
                .Select(SyntaxFactory.IdentifierName);

            var body = CreateInternalPartial(
                methodName,
                genericArguments,
                parameterSelf,
                SyntaxFactory.IdentifierName(parameterOther),
                methodType.Parameter2.IsTaskLike);

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

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax);
        }

        private MethodDeclarationSyntax CreatePartial(
            MethodTypeInternal3 methodType,
            SyntaxToken methodName,
            IReadOnlyCollection<SyntaxToken> genericParameterNames,
            SelfParameter parameterSelf,
            TypeSyntax parameterTypeNext,
            SyntaxToken parameterNext,
            TypeSyntax parameterType,
            SyntaxToken parameterOther)
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
                methodType.Parameter2.IsTaskLike);

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

            return this.AddTypeConstraints(genericParameterTypes, methodDeclarationSyntax);
        }
    }
}
