using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetValuesResultValueExtensions
    {
        [Pure]
        public static IEnumerable<TValue> GetValues<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.Match(Enumerable.Empty<TValue>, value => new[] { value });
        }
    }
}
