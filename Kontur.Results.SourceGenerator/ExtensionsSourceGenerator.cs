using Microsoft.CodeAnalysis;

namespace Kontur.Results.SourceGenerator
{
    [Generator]
    public class ExtensionsSourceGenerator : SourceGeneratorBase
    {
        public ExtensionsSourceGenerator()
            : base(new("KonturResultGlobalExtensions", "KonturResultExtensions"))
        {
        }
    }
}