using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Conversion.Combinations.Linq.SelectMany.Local
{
    internal class ResultValueValueResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public ResultValueValueResultValue(MethodDescriptionFactory methodDescriptionFactory)
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

            var result = Identifiers.TypeResult;
            var resultType = SyntaxFactory.IdentifierName(result);

            var returnType = TypeFactory.CreateResult(faultType, resultType);

            return methodDescriptionFactory.CreateSelectMany(
                "ResultValueValueResultValue",
                returnType,
                MethodNames.SelectMany,
                new[] { fault, value1, value2, result },
                TypeFactory.CreateResult(faultType, value1Type),
                Array.Empty<GenericNameSyntax>(),
                value1Type,
                value2Type,
                Array.Empty<GenericNameSyntax>(),
                value2Type,
                returnType,
                Array.Empty<GenericNameSyntax>(),
                new(Parameters.SelfResultIdentifier, "next", "result"),
                true);
        }
    }
}
