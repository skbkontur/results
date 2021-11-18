using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class Identifiers
    {
        internal const string StringT = "T";
        internal const string StringFault = "TFault";
        internal const string StringValue = "TValue";
        internal const string StringValue1 = "TValue1";
        internal const string StringValue2 = "TValue2";

        internal static readonly SyntaxToken TypeT = SyntaxFactory.Identifier(StringT);
        internal static readonly SyntaxToken TypeFault = SyntaxFactory.Identifier(StringFault);
        internal static readonly SyntaxToken TypeValue = SyntaxFactory.Identifier(StringValue);
        internal static readonly SyntaxToken TypeValue1 = SyntaxFactory.Identifier(StringValue1);
        internal static readonly SyntaxToken TypeValue2 = SyntaxFactory.Identifier(StringValue2);
        internal static readonly SyntaxToken TypeResult = SyntaxFactory.Identifier("TResult");

        internal static readonly SyntaxToken Bool = SyntaxFactory.Identifier("bool");

        internal static readonly SyntaxToken Discard = SyntaxFactory.Identifier("_");

        internal static readonly SyntaxToken TaskIdentifier = SyntaxFactory.Identifier(nameof(Task));
        internal static readonly SyntaxToken ValueTaskIdentifier = SyntaxFactory.Identifier(nameof(ValueTask));

        internal static readonly SyntaxToken FuncIdentifier = SyntaxFactory.Identifier(nameof(Func<object>));
    }
}
