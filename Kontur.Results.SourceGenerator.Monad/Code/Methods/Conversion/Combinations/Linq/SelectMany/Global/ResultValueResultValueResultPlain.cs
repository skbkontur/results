using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.SelectMany.Global
{
    internal class ResultValueResultValueResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueResultValueResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value1 = Identifiers.TypeValue1;
            var value1Type = SyntaxFactory.IdentifierName(value1);

            var value2 = Identifiers.TypeValue2;
            var value2Type = SyntaxFactory.IdentifierName(value2);

            var returnType = TypeFactory.CreateResult(faultType);

            return methodDescriptionFactory.CreateSelectMany(
                "ResultValueResultValueResultPlain",
                returnType,
                MethodNames.SelectMany,
                new[] { fault, value1, value2 },
                TypeFactory.CreateResult(faultType, value1Type),
                Array.Empty<GenericNameSyntax>(),
                value1Type,
                TypeFactory.CreateResult(faultType, value2Type),
                Array.Empty<GenericNameSyntax>(),
                value2Type,
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "next", "result"),
                false);
        }
    }
}
