using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal static class MethodTypeInternalGenerator
    {
        internal static IEnumerable<MethodTypeInternal1> Create1Parameter(
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodTypeGeneratorParameter selfParameter)
        {
            var parameters1 = CreateParameters(selfParameter, 1);

            return
                from parameter1 in parameters1
                select new MethodTypeInternal1(
                    parameter1,
                    computeReturnType(parameter1.IsTask, parameter1.IsValueTask));
        }

        internal static IEnumerable<MethodTypeInternal2> Create2Parameters(
            TypeSyntax returnType,
            MethodTypeGeneratorParameter selfParameter,
            MethodTypeGeneratorParameter otherParameter)
        {
            return Create2Parameters(
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                selfParameter,
                CreateParameters(otherParameter, 2));
        }

        internal static IEnumerable<MethodTypeInternal2> Create2Parameters(
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodTypeGeneratorParameter selfParameter,
            IEnumerable<InternalTypeParameter> otherParameters)
        {
            var parameters1 = CreateParameters(selfParameter, 1);

            return
                from parameter1 in parameters1
                from parameter2 in otherParameters
                let allParameters = new[] { parameter1, parameter2 }
                select new MethodTypeInternal2(
                    parameter1,
                    parameter2,
                    computeReturnType(allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static IEnumerable<MethodTypeInternal3> Create3Parameters(
            TypeSyntax returnType,
            MethodTypeGeneratorParameter selfParameter,
            MethodTypeGeneratorParameter nextParameter,
            MethodTypeGeneratorParameter otherParameter)
        {
            return Create3Parameters(
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                selfParameter,
                CreateParameters(nextParameter, 2),
                CreateParameters(otherParameter, 3));
        }

        internal static IEnumerable<MethodTypeInternal3> Create3Parameters(
            Func<bool, bool, TypeSyntax> computeReturnType,
            MethodTypeGeneratorParameter selfParameter,
            IEnumerable<InternalTypeParameter> nextParameters,
            IEnumerable<InternalTypeParameter> otherParameters)
        {
            var parameters1 = CreateParameters(selfParameter, 1);

            return
                from parameter1 in parameters1
                from parameterNext in nextParameters
                from parameter2 in otherParameters
                let allParameters = new[] { parameter1, parameterNext, parameter2 }
                select new MethodTypeInternal3(
                    parameter1,
                    parameterNext,
                    parameter2,
                    computeReturnType(allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static InternalTypeParameter[] CreateActionParameters(NameSyntax actionType, params TypeSyntax[] argumentTypes)
        {
            return new InternalTypeParameter[]
            {
                new(actionType, false, false, null),
                new(TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, argumentTypes.Append(SyntaxFactory.IdentifierName(Identifiers.TaskIdentifier))), true, false, null),
                new(TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, argumentTypes.Append(SyntaxFactory.IdentifierName(Identifiers.ValueTaskIdentifier))), false, true, null),
            };
        }

        internal static IEnumerable<InternalTypeParameter> CreateParameters(MethodTypeGeneratorParameter parameter, int argumentNumber)
        {
            var (upperBound, isSealed) = parameter;
            return isSealed
                ? CreateSealedParameter(upperBound)
                : CreateUpperBoundParameter(upperBound, argumentNumber);
        }

        private static IEnumerable<InternalTypeParameter> CreateUpperBoundParameter(NameSyntax upperBound, int argumentNumber)
        {
            var genericParameter = SyntaxFactory.Identifier("TInstance" + argumentNumber);
            return TaskLikeTypeSyntaxFactory.Create(SyntaxFactory.IdentifierName(genericParameter))
                .Select(item => new InternalTypeParameter(item.Syntax, item.IsTask, item.IsValueTask, new(genericParameter, upperBound)))
                .Append(new(upperBound, false, false, null));
        }

        private static IEnumerable<InternalTypeParameter> CreateSealedParameter(NameSyntax type)
        {
            return TaskLikeTypeSyntaxFactory.Create(type)
                .Select(item => new InternalTypeParameter(item.Syntax, item.IsTask, item.IsValueTask, null))
                .Append(new(type, false, false, null));
        }
    }
}
