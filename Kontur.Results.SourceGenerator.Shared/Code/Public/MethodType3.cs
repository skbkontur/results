using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal record MethodType3(
        SimpleNameSyntax Parameter1,
        SimpleNameSyntax ParameterNext,
        bool ParameterNextIsTask,
        SimpleNameSyntax Parameter2,
        IEnumerable<TypeSyntax> GenericArguments,
        TypeSyntax ReturnType);
}