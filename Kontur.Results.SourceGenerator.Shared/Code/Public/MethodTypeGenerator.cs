using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal static class MethodTypeGenerator
    {
        internal static IEnumerable<MethodType3> Create3Parameters(
            TypeSyntax returnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            NameSyntax nextParameterUpperBound,
            IReadOnlyCollection<TypeSyntax> nextParameterTaskArguments,
            NameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> parameterTaskArguments)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);
            var parametersNext = CreateParameter(nextParameterUpperBound, nextParameterTaskArguments);
            var parameters2 = CreateParameter(parameterUpperBound, parameterTaskArguments);

            return
                from parameter1 in parameters1
                from parameterNext in parametersNext
                from parameter2 in parameters2
                let allParameters = new[] { parameter1, parameterNext, parameter2 }
                select new MethodType3(
                    parameter1.ProposedType,
                    parameterNext.ProposedType,
                    parameterNext.TaskLike,
                    parameter2.ProposedType,
                    allParameters.Select(arg => arg.GenericArgument).WhereNotNull(),
                    ReturnTypeCalculator.Get(returnType, allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static IEnumerable<MethodType1> Create1Parameter(
            TypeSyntax returnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);

            return
                from parameter1 in parameters1
                select new MethodType1(
                    parameter1.ProposedType,
                    new[] { parameter1.GenericArgument }.WhereNotNull(),
                    ReturnTypeCalculator.Get(returnType, parameter1.IsTask, parameter1.IsValueTask));
        }

        internal static IEnumerable<MethodType2> Create2Parameters(
            TypeSyntax returnType,
            NameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            NameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> parameterTaskArguments)
        {
            var parameters1 = CreateParameter(selfUpperBound, selfTaskArguments);
            var parameters2 = CreateParameter(parameterUpperBound, parameterTaskArguments);

            return
                from parameter1 in parameters1
                from parameter2 in parameters2
                let allParameters = new[] { parameter1, parameter2 }
                select new MethodType2(
                    parameter1.ProposedType,
                    parameter2.ProposedType,
                    parameter2.IsValueTask,
                    allParameters.Select(arg => arg.GenericArgument).WhereNotNull(),
                    ReturnTypeCalculator.Get(returnType, allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        private static IEnumerable<ParameterInfo> CreateParameter(
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

        private sealed record ParameterInfo(NameSyntax ProposedType, TypeSyntax? GenericArgument, bool IsTask, bool IsValueTask)
        {
            internal bool TaskLike => IsTask || IsValueTask;
        }
    }
}
