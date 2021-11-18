using System.Collections.Generic;

namespace Kontur.Results.SourceGenerator.Code.Internal
{
    internal record InternalMethodsDescription(
        IEnumerable<InternalStandardMethodsDescription> Methods,
        InternalPartialMethodDescription Partial);
}
