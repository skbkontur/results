using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastResultPlainExtensions
    {
        /// <summary>Widens the fault type parameter via covariance, e.g. <c>Result&lt;DerivedFault&gt;</c> to <c>Result&lt;BaseFault&gt;</c>.</summary>
        /// <example><code>
        /// Result&lt;ArgumentException&gt; specific = Result.Fail(new ArgumentException());
        /// Result&lt;Exception&gt; general = specific.Upcast&lt;Exception&gt;();
        /// </code></example>
        [Pure]
        public static Result<TFault> Upcast<TFault>(this IResult<TFault> result)
        {
            return result.Match(Result<TFault>.Fail, Result<TFault>.Succeed);
        }
    }
}
