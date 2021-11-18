using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class MethodDeclarationFactory : IMethodDeclarationFactory
    {
        public MethodDeclarationSyntax Create(
            IEnumerable<AttributeSyntax> methodAttributes,
            SyntaxToken[] accessModifiers,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            IEnumerable<ParameterSyntax> parameters,
            BlockSyntax body)
        {
            var attributes = SyntaxFactory.List(methodAttributes
                .Select(attribute => SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[] { attribute }))));

            return Create(
                    accessModifiers,
                    returnType,
                    methodName,
                    genericParameters,
                    parameters,
                    body)
                .WithAttributeLists(attributes);
        }

        public MethodDeclarationSyntax Create(
            SyntaxToken[] accessModifiers,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            IEnumerable<ParameterSyntax> parameters,
            BlockSyntax body)
        {
            var genericParametersInner = genericParameters.ToArray();
            var typeParameters = genericParametersInner
                .Select(SyntaxFactory.TypeParameter)
                .ToArray();

            return SyntaxFactory
                .MethodDeclaration(returnType, methodName)
                .AddModifiers(accessModifiers)
                .AddTypeParameterListParameters(typeParameters)
                .AddParameterListParameters(parameters.ToArray())
                .WithBody(body);
        }
    }
}
