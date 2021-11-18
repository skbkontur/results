using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetEnumeratorOptionalExtensions
    {
        [Pure]
        public static IEnumerator<TValue> GetEnumerator<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValues().GetEnumerator();
        }
    }
}
