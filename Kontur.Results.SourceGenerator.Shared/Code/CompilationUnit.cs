using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal record CompilationUnit(string Hint, CompilationUnitSyntax Content);
}
