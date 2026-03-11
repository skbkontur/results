using System;
using System.ComponentModel;

namespace Kontur.Results
{
    /// <summary>Internal covariant interface for <see cref="Result{TFault, TValue}"/>. Not intended for direct use — use <see cref="Result{TFault, TValue}"/> instead.</summary>
    /// <typeparam name="TFault">The type of the fault.</typeparam>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IResult<out TFault, out TValue>
    {
        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess);

        internal TResult Match<TResult>(Func<TResult> onFailure, Func<TValue, TResult> onSuccess);

        /// <summary>Applies one of two functions depending on whether the result is a failure or success.</summary>
        /// <returns>The result of <paramref name="onFailure"/> applied to the fault, or <paramref name="onSuccess"/> applied to the value.</returns>
        /// <example>
        /// <code>
        /// Result&lt;string, int&gt; result = Result&lt;string, int&gt;.Succeed(42);
        /// string text = result.Match(fault =&gt; fault, value =&gt; value.ToString()); // "42"
        /// </code>
        /// </example>
        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess);
    }
}
