using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Map
{
    internal class MapFaultResultFailure : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public MapFaultResultFailure(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var t = Identifiers.TypeT;
            var typeT = SyntaxFactory.IdentifierName(t);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            return methodDescriptionFactory.CreatePass(
                "ResultFailure",
                TypeFactory.CreateResultFailure(resultType),
                MethodNames.MapFault,
                new[] { t, result },
                TypeFactory.CreateResultFailure(typeT),
                Array.Empty<GenericNameSyntax>(),
                typeT,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "fault"));
        }
    }
}
