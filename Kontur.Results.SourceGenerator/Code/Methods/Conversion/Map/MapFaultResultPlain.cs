using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Map
{
    internal class MapFaultResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public MapFaultResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            return methodDescriptionFactory.CreatePass(
                "ResultPlain",
                TypeFactory.CreateResult(resultType),
                MethodNames.MapFault,
                new[] { fault, result },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
                faultType,
                resultType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "fault"));
        }
    }
}
