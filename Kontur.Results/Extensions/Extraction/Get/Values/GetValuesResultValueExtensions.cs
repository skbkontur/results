using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetValuesResultValueExtensions
    {
        /// <summary>Yields the value as a single-element sequence, or empty if failed.</summary>
        /// <example><code>
        /// Result&lt;Exception, string&gt; result = ...;
        /// IEnumerable&lt;string&gt; values = result.GetValues().ToArray();
        /// // [value] if Success, [] if Failure
        /// </code></example>
        [Pure]
        public static IEnumerable<TValue> GetValues<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.Match(Enumerable.Empty<TValue>, value => new[] { value });
        }
    }
}
