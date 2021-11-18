using Microsoft.CodeAnalysis;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class IdentifierExtensions
    {
        internal static bool IsValueTask(this SyntaxToken identifier)
        {
            return identifier.IsEquivalentTo(Identifiers.ValueTaskIdentifier);
        }

        internal static bool IsTask(this SyntaxToken identifier)
        {
            return identifier.IsEquivalentTo(Identifiers.TaskIdentifier);
        }
    }
}
