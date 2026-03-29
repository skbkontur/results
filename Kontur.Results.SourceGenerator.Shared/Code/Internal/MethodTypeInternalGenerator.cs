using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal static class MethodTypeInternalGenerator
    {
        internal static IEnumerable<MethodTypeInternal3> Create3Parameters(
            TypeSyntax returnType,
            MethodTypeGeneratorParameter selfParameter,
            MethodTypeGeneratorParameter nextParameter,
            MethodTypeGeneratorParameter otherParameter)
        {
            var parameters1 = CreateParameters(selfParameter, 1);
            var parametersNext = CreateParameters(nextParameter, 2);
            var parameters2 = CreateParameters(otherParameter, 3);

            return
                from parameter1 in parameters1
                from parameterNext in parametersNext
                from parameter2 in parameters2
                let allParameters = new[] { parameter1, parameterNext, parameter2 }
                select new MethodTypeInternal3(
                    parameter1,
                    parameterNext,
                    parameter2,
                    ReturnTypeCalculator.Get(returnType, allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        internal static IEnumerable<MethodTypeInternal1> Create1Parameter(
            TypeSyntax returnType,
            MethodTypeGeneratorParameter selfParameter)
        {
            var parameters1 = CreateParameters(selfParameter, 1);

            return
                from parameter1 in parameters1
                select new MethodTypeInternal1(
                    parameter1,
                    ReturnTypeCalculator.Get(returnType, parameter1.IsTask, parameter1.IsValueTask));
        }

        internal static IEnumerable<MethodTypeInternal2> Create2Parameters(
            TypeSyntax returnType,
            MethodTypeGeneratorParameter selfParameter,
            MethodTypeGeneratorParameter otherParameter)
        {
            var parameters1 = CreateParameters(selfParameter, 1);
            var parameters2 = CreateParameters(otherParameter, 2);

            return
                from parameter1 in parameters1
                from parameter2 in parameters2
                let allParameters = new[] { parameter1, parameter2 }
                select new MethodTypeInternal2(
                    parameter1,
                    parameter2,
                    ReturnTypeCalculator.Get(returnType, allParameters.Any(p => p.IsTask), allParameters.Any(p => p.IsValueTask)));
        }

        private static IEnumerable<InternalTypeParameter> CreateParameters(MethodTypeGeneratorParameter parameter, int argumentNumber)
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
