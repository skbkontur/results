using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal interface IMethodBodyFactory
    {
        BlockSyntax CreateDelegateToOtherClass(
            SyntaxToken methodName,
            SimpleNameSyntax delegateClassName,
            IEnumerable<TypeSyntax> genericArguments,
            IEnumerable<ArgumentSyntax> arguments);
    }
}