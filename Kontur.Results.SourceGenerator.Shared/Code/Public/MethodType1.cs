using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record MethodType1(
        NameSyntax Parameter1,
        IEnumerable<TypeSyntax> GenericArguments,
        TypeSyntax ReturnType);
}
