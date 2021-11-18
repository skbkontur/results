using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal static class MethodTypeGenerator
    {
        internal static IEnumerable<MethodType3> Create3Parameters(
            TypeSyntax returnType,
            SimpleNameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            SimpleNameSyntax nextParameterUpperBound,
            IReadOnlyCollection<TypeSyntax> nextParameterTaskArguments,
            SimpleNameSyntax parameterUpperBound,
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
                    ReturnTypeCalculator.Get(returnType, allParameters.Select(arg => arg.ProposedType)));
        }

        internal static IEnumerable<MethodType2> Create2Parameters(
            TypeSyntax returnType,
            SimpleNameSyntax selfUpperBound,
            IReadOnlyCollection<TypeSyntax> selfTaskArguments,
            SimpleNameSyntax parameterUpperBound,
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
                    allParameters.Select(arg => arg.GenericArgument).WhereNotNull(),
                    ReturnTypeCalculator.Get(returnType, allParameters.Select(arg => arg.ProposedType)));
        }

        private static IEnumerable<ParameterInfo> CreateParameter(
            SimpleNameSyntax parameterUpperBound,
            IReadOnlyCollection<TypeSyntax> taskArguments)
        {
            if (taskArguments.Any())
            {
                return taskArguments
                    .Append(parameterUpperBound)
                    .SelectMany(
                        TaskLikeTypeSyntaxFactory.Create,
                        (original, wrapped) => new ParameterInfo(wrapped, original, true))
                    .Append(new(parameterUpperBound, null, false));
            }

            return TaskLikeTypeSyntaxFactory
                .Create(parameterUpperBound)
                .Select(type => new ParameterInfo(type, null, true))
                .Append(new(parameterUpperBound, null, false));
        }

        private record ParameterInfo(SimpleNameSyntax ProposedType, TypeSyntax? GenericArgument, bool TaskLike);
    }
}
