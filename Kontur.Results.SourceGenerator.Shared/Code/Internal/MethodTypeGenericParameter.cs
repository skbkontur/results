using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record MethodTypeGenericParameter(
        SyntaxToken Identifier,
        TypeSyntax? UpperBound);
}