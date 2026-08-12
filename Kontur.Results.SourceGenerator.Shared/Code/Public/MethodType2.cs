using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record MethodType2(
        NameSyntax Parameter1,
        NameSyntax Parameter2,
        bool Parameter2IsTask,
        bool Parameter2IsValueTask,
        IEnumerable<TypeSyntax> GenericArguments,
        TypeSyntax ReturnType);
}