using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetEnumeratorOptionalExtensions
    {
        /// <summary>
        /// Allows using Optional in foreach and LINQ expressions as a single-element or empty collection.
        /// </summary>
        /// <example><code>
        /// foreach (var value in optional) { /* runs if Some */ }
        ///
        /// // Mix with other collections:
        /// from value in optional
        /// from i in new[] { 1, 2 }
        /// select value + i;
        /// </code></example>
        [Pure]
        public static IEnumerator<TValue> GetEnumerator<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValues().GetEnumerator();
        }
    }
}
