using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Methods.Extraction.GetFaultOrNull
{
    internal class GetFaultOrNullResultPlain : IDescriptionProvider
    {
        private readonly MethodDescriptionFactory methodDescriptionFactory;

        public GetFaultOrNullResultPlain(MethodDescriptionFactory methodDescriptionFactory)
        {
            this.methodDescriptionFactory = methodDescriptionFactory;
        }

        public MethodsDescription Get()
        {
            var fault = Identifiers.TypeFault;
            var faultType = SyntaxFactory.IdentifierName(fault);

            return this.methodDescriptionFactory.CreateDefault(
                "ResultPlain",
                SyntaxFactory.NullableType(faultType),
                MethodNames.GetFaultOrNull,
                new[] { fault },
                TypeFactory.CreateInterfaceResult(faultType),
                new[] { TypeFactory.CreateResult(faultType) },
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
