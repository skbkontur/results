using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetEnumeratorResultValueExtensions
    {
        [Pure]
        public static IEnumerator<TValue> GetEnumerator<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValues().GetEnumerator();
        }
    }
}
