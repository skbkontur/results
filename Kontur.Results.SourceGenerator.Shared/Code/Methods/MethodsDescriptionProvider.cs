using System.Collections.Generic;
using System.Linq;

namespace Kontur.Results.SourceGenerator.Code.Methods
{
    internal class MethodsDescriptionProvider
    {
        private readonly IEnumerable<IDescriptionProvider> providers;

        public MethodsDescriptionProvider(IEnumerable<IDescriptionProvider> providers)
        {
            this.providers = providers;
        }

        internal IEnumerable<MethodsDescription> Get()
        {
            return providers.Select(provider => provider.Get());
        }
    }
}