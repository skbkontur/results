using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record MethodType2(
        SimpleNameSyntax Parameter1,
        SimpleNameSyntax Parameter2,
        IEnumerable<TypeSyntax> GenericArguments,
        TypeSyntax ReturnType);
}