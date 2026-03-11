using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastOptionalExtensions
    {
        /// <summary>Widens the generic type parameter via covariance, e.g. <c>Optional&lt;Derived&gt;</c> to <c>Optional&lt;Base&gt;</c>.</summary>
        /// <example><code>
        /// Optional&lt;string&gt; specific = Optional.Some("hello");
        /// Optional&lt;object&gt; general = specific.Upcast&lt;object&gt;();
        /// </code></example>
        [Pure]
        public static Optional<TResult> Upcast<TResult>(this IOptional<TResult> optional)
        {
            return optional.Match(Optional<TResult>.None, Optional<TResult>.Some);
        }
    }
}
