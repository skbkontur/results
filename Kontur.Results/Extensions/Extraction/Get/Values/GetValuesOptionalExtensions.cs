using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetValuesOptionalExtensions
    {
        /// <summary>Yields the value as a single-element sequence, or empty if None.</summary>
        /// <example><code>
        /// Optional&lt;string&gt; optional = ...;
        /// IEnumerable&lt;string&gt; values = optional.GetValues().ToArray();
        /// // [value] if Some, [] if None
        /// </code></example>
        [Pure]
        public static IEnumerable<TValue> GetValues<TValue>(this IOptional<TValue> optional)
        {
            return optional.Match(Enumerable.Empty<TValue>, value => new[] { value });
        }
    }
}
