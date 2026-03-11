using System;
using System.ComponentModel;

namespace Kontur.Results
{
    /// <summary>Internal covariant interface for <see cref="Result{TFault}"/>. Not intended for direct use — use <see cref="Result{TFault}"/> instead.</summary>
    /// <typeparam name="TFault">The type of the fault.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IResult<out TFault>
    {
        /// <summary>Applies one of two functions depending on whether the result is a failure or success.</summary>
        /// <returns>The result of <paramref name="onFailure"/> applied to the fault, or <paramref name="onSuccess"/>.</returns>
        /// <example>
        /// <code>
        /// Result&lt;string&gt; result = Result&lt;string&gt;.Succeed();
        /// int code = result.Match(fault =&gt; -1, () =&gt; 0); // 0
        /// </code>
        /// </example>
        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess);
    }
}
