using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetFaultsResultValueExtensions
    {
        /// <summary>Yields the fault as a single-element sequence, or empty if successful.</summary>
        /// <example><code>
        /// Result&lt;Exception, string&gt; result = ...;
        /// IEnumerable&lt;Exception&gt; faults = result.GetFaults().ToArray();
        /// // [exception] if Failure, [] if Success
        /// </code></example>
        [Pure]
        public static IEnumerable<TFault> GetFaults<TFault, TResult>(this IResult<TFault, TResult> result)
        {
            return result.Match(fault => new[] { fault }, Enumerable.Empty<TFault>);
        }
    }
}
