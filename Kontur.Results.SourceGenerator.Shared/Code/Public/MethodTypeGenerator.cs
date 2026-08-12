using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal static class MethodTypeGenerator
    {
        internal static IEnumerable<MethodType1> Create1Parameter(
            Func<bool, bool, TypeSyntax> computeReturnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);

            return
                from parameter1 in parameters1
                select new MethodType1(
                    parameter1.ProposedType,
                    new[] { parameter1.GenericArgument }.WhereNotNull(),
                    computeReturnType(parameter1.IsTask, parameter1.IsValueTask));
        }

        internal static IEnumerable<MethodType2> Create2Parameters(
            TypeSyntax returnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            NameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> parameterTaskArguments)
        {
            return Create2Parameters(
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                selfUpperBound,
                selfTaskArguments,
                CreateParameter(parameterUpperBound, parameterTaskArguments));
        }

        internal static IEnumerable<MethodType2> Create2Parameters(
            Func<bool, bool, TypeSyntax> computeReturnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            IEnumerable<ParameterInfo> otherParameters)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);

            return
                from parameter1 in parameters1
                from parameter2 in otherParameters
                let allParameters = new[] { parameter1, parameter2 }
                select new MethodType2(
                    parameter1.ProposedType,
                    parameter2.ProposedType,
                    parameter2.IsTask,
                    parameter2.IsValueTask,
                    allParameters.Select(arg => arg.GenericArgument).WhereNotNull(),
                    computeReturnType(allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static IEnumerable<MethodType3> Create3Parameters(
            TypeSyntax returnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            NameSyntax nextParameterUpperBound,
            IReadOnlyCollection<TypeSyntax> nextParameterTaskArguments,
            NameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> parameterTaskArguments)
        {
            return Create3Parameters(
                (hasTask, hasValueTask) => ReturnTypeCalculator.Get(returnType, hasTask, hasValueTask),
                selfUpperBound,
                selfTaskArguments,
                CreateParameter(nextParameterUpperBound, nextParameterTaskArguments),
                CreateParameter(parameterUpperBound, parameterTaskArguments));
        }

        internal static IEnumerable<MethodType3> Create3Parameters(
            Func<bool, bool, TypeSyntax> computeReturnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            IEnumerable<ParameterInfo> nextParameters,
            IEnumerable<ParameterInfo> otherParameters)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);

            return
                from parameter1 in parameters1
                from parameterNext in nextParameters
                from parameter2 in otherParameters
                let allParameters = new[] { parameter1, parameterNext, parameter2 }
                select new MethodType3(
                    parameter1.ProposedType,
                    parameterNext.ProposedType,
                    parameterNext.IsTask,
                    parameterNext.IsValueTask,
                    parameter2.ProposedType,
                    parameter2.IsValueTask,
                    allParameters.Select(arg => arg.GenericArgument).WhereNotNull(),
                    computeReturnType(allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static ParameterInfo[] CreateActionParameters(NameSyntax actionType, params TypeSyntax[] argumentTypes)
        {
            return new ParameterInfo[]
            {
                new(actionType, null, false, false),
                new(TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, argumentTypes.Append(SyntaxFactory.IdentifierName(Identifiers.TaskIdentifier))), null, true, false),
                new(TypeFactory.CreateClosedGeneric(Identifiers.FuncIdentifier, argumentTypes.Append(SyntaxFactory.IdentifierName(Identifiers.ValueTaskIdentifier))), null, false, true),
            };
        }

        internal static IEnumerable<ParameterInfo> CreateParameter(
            NameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> taskArguments)
        {
            if (taskArguments.Any())
            {
                return taskArguments
                    .Append(parameterUpperBound)
                    .SelectMany(
                        TaskLikeTypeSyntaxFactory.Create,
                        (original, item) => new ParameterInfo(item.Syntax, original, item.IsTask, item.IsValueTask))
                    .Append(new(parameterUpperBound, null, false, false));
            }

            return TaskLikeTypeSyntaxFactory
                .Create(parameterUpperBound)
                .Select(item => new ParameterInfo(item.Syntax, null, item.IsTask, item.IsValueTask))
                .Append(new(parameterUpperBound, null, false, false));
        }

        internal sealed record ParameterInfo(NameSyntax ProposedType, TypeSyntax? GenericArgument, bool IsTask, bool IsValueTask)
        {
            internal bool TaskLike => IsTask || IsValueTask;
        }
    }
}
