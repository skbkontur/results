using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrNull
{
    internal class GetFaultOrNullResultValue : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetFaultOrNullResultValue(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            var value = Identifiers.TypeValue;
            var valueType = SyntaxFactory.IdentifierName(value);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultValue",
                SyntaxFactory.NullableType(faultType),
                MethodNames.GetFaultOrNull,
                new[] { fault, value },
                TypeFactory.CreateInterfaceResult(faultType, valueType),
                new[] { TypeFactory.CreateResult(faultType, valueType) },
                Parameters.SelfResultIdentifier,
                new[]
                {
                    SyntaxFactory.TypeParameterConstraintClause(
                        SyntaxFactory.IdentifierName(fault))
                        .AddConstraints(SyntaxFactory.ClassOrStructConstraint(SyntaxKind.StructConstraint)),
                });
        }
    }
}
