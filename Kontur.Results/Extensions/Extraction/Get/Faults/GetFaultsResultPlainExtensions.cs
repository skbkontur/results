using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetFaultsResultPlainExtensions
    {
        [Pure]
        public static IEnumerable<TFault> GetFaults<TFault>(this IResult<TFault> result)
        {
            return result.Match(fault => new[] { fault }, Enumerable.Empty<TFault>);
        }
    }
}
