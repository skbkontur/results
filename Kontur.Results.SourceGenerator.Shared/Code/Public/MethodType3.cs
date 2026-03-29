using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record MethodType3(
        NameSyntax Parameter1,
        NameSyntax ParameterNext,
        bool ParameterNextIsTask,
        NameSyntax Parameter2,
        IEnumerable<TypeSyntax> GenericArguments,
        TypeSyntax ReturnType);
}