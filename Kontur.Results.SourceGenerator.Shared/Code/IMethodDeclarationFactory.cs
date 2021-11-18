using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal interface IMethodDeclarationFactory
    {
        MethodDeclarationSyntax Create(
            IEnumerable<AttributeSyntax> methodAttributes,
            SyntaxToken[] accessModifiers,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            IEnumerable<ParameterSyntax> parameters,
            BlockSyntax body);

        MethodDeclarationSyntax Create(
            SyntaxToken[] accessModifiers,
            TypeSyntax returnType,
            SyntaxToken methodName,
            IEnumerable<SyntaxToken> genericParameters,
            IEnumerable<ParameterSyntax> parameters,
            BlockSyntax body);
    }
}