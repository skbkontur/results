using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal interface ITypeParameterGenericMethodSyntaxGenerator
    {
        IEnumerable<TypeParameterConstraintClauseSyntax> CreateTypeParameterConstraints(IEnumerable<MethodGenericParameterDescription> genericParameters);

        CompilationUnitSyntax FixMethodTypeParameterIndentation(CompilationUnitSyntax compilationUnit);
    }
}