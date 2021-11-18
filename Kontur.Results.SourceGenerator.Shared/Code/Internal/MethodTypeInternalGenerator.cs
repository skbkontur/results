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
                select new MethodTypeInternal3(
                    parameter1,
                    parameterNext,
                    parameter2,
                    ReturnTypeCalculator.Get(returnType, new[] { parameter1, parameterNext, parameter2 }.Select(arg => arg.Type)));
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
                select new MethodTypeInternal2(
                    parameter1,
                    parameter2,
                    ReturnTypeCalculator.Get(returnType, new[] { parameter1, parameter2 }.Select(arg => arg.Type)));
        }

        private static IEnumerable<InternalTypeParameter> CreateParameters(MethodTypeGeneratorParameter parameter, int argumentNumber)
        {
            var (upperBound, isSealed) = parameter;
            return isSealed
                ? CreateSealedParameter(upperBound)
                : CreateUpperBoundParameter(upperBound, argumentNumber);
        }

        private static IEnumerable<InternalTypeParameter> CreateUpperBoundParameter(SimpleNameSyntax upperBound, int argumentNumber)
        {
            var genericParameter = SyntaxFactory.Identifier("TInstance" + argumentNumber);
            return TaskLikeTypeSyntaxFactory.Create(SyntaxFactory.IdentifierName(genericParameter))
                .Select(wrapped => new InternalTypeParameter(wrapped, true, new(genericParameter, upperBound)))
                .Append(new(upperBound, false, null));
        }

        private static IEnumerable<InternalTypeParameter> CreateSealedParameter(SimpleNameSyntax type)
        {
            return TaskLikeTypeSyntaxFactory.Create(type)
                .Select(wrapped => new InternalTypeParameter(wrapped, true, null))
                .Append(new(type, false, null));
        }
    }
}
