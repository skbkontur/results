using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record InternalPartialMethodDescription(
        string ClassName,
        IEnumerable<MethodDeclarationSyntax> Methods);
}
