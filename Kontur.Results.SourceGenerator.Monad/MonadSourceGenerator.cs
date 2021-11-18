using Microsoft.CodeAnalysis;

namespace Kontur.Results.SourceGenerator.Monad
{
    [Generator]
    public class MonadSourceGenerator : SourceGeneratorBase
    {
        public MonadSourceGenerator()
            : base(new("KonturResultMonadGlobalExtensions", "KonturResultMonadExtensions"))
        {
        }
    }
}