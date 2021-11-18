using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetFaultsResultValueExtensions
    {
        [Pure]
        public static IEnumerable<TFault> GetFaults<TFault, TResult>(this IResult<TFault, TResult> result)
        {
            return result.Match(fault => new[] { fault }, Enumerable.Empty<TFault>);
        }
    }
}
