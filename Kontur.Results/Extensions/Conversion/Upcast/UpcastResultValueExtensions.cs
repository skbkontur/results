using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastResultValueExtensions
    {
        /// <summary>Widens the generic type parameters via covariance, e.g. <c>Result&lt;DerivedFault, DerivedValue&gt;</c> to <c>Result&lt;BaseFault, BaseValue&gt;</c>.</summary>
        /// <example><code>
        /// Result&lt;ArgumentException, string&gt; specific = Result.Succeed&lt;ArgumentException, string&gt;("ok");
        /// Result&lt;Exception, object&gt; general = specific.Upcast&lt;Exception, object&gt;();
        /// </code></example>
        [Pure]
        public static Result<TFault, TValue> Upcast<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.Match(Result<TFault, TValue>.Fail, Result<TFault, TValue>.Succeed);
        }
    }
}
