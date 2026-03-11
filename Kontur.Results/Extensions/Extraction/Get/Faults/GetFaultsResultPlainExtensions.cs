using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kontur.Results
{
    public static class GetFaultsResultPlainExtensions
    {
        /// <summary>Yields the fault as a single-element sequence, or empty if successful.</summary>
        /// <example><code>
        /// Result&lt;Exception&gt; result = ...;
        /// IEnumerable&lt;Exception&gt; faults = result.GetFaults().ToArray();
        /// // [exception] if Failure, [] if Success
        /// </code></example>
        [Pure]
        public static IEnumerable<TFault> GetFaults<TFault>(this IResult<TFault> result)
        {
            return result.Match(fault => new[] { fault }, Enumerable.Empty<TFault>);
        }
    }
}
