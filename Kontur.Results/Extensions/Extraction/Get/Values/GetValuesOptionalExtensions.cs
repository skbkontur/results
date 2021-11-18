using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetValuesOptionalExtensions
    {
        [Pure]
        public static IEnumerable<TValue> GetValues<TValue>(this IOptional<TValue> optional)
        {
            return optional.Match(Enumerable.Empty<TValue>, value => new[] { value });
        }
    }
}
