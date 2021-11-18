using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal record MethodGenericParameterDescription(
        SyntaxToken Identifier,
        TypeSyntax? UpperBound);
}