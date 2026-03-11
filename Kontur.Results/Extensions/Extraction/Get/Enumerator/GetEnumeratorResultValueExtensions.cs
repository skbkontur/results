using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetEnumeratorResultValueExtensions
    {
        /// <summary>
        /// Allows using Result in foreach and LINQ expressions as a single-element or empty collection.
        /// </summary>
        /// <example><code>
        /// foreach (var value in result) { /* runs if Success */ }
        ///
        /// // Mix with other collections:
        /// from value in result
        /// from i in new[] { 1, 2 }
        /// select value + i;
        /// </code></example>
        [Pure]
        public static IEnumerator<TValue> GetEnumerator<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValues().GetEnumerator();
        }
    }
}
