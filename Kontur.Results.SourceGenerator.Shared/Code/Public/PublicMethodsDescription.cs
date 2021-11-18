using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record PublicMethodsDescription(
        IEnumerable<MethodDeclarationSyntax> Extensions,
        IEnumerable<MethodDeclarationSyntax> ExtensionsGlobal);
}
