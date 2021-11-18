using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record InternalStandardMethodsDescription(
        string ClassName,
        IEnumerable<UsingDirectiveSyntax> Using,
        IEnumerable<MethodDeclarationSyntax> Methods);
}