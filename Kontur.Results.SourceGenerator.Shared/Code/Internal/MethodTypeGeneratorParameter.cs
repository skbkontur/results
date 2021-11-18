using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record MethodTypeGeneratorParameter(
        SimpleNameSyntax UpperBound,
        bool Sealed);
}
