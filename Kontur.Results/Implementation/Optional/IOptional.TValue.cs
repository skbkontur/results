using System;
using System.ComponentModel;

namespace Kontur.Results
{
    /// <summary>Internal covariant interface for <see cref="Optional{TValue}"/>. Not intended for direct use — use <see cref="Optional{TValue}"/> instead.</summary>
    /// <typeparam name="TValue">The type of the contained value.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IOptional<out TValue>
    {
        /// <summary>Applies one of two functions depending on whether the optional contains a value.</summary>
        /// <returns>The result of <paramref name="onNone"/> if None, or <paramref name="onSome"/> applied to the value.</returns>
        /// <example>
        /// <code>
        /// Optional&lt;int&gt; opt = Optional.Some(42);
        /// string text = opt.Match(() =&gt; "none", value =&gt; $"got {value}"); // "got 42"
        /// </code>
        /// </example>
        internal TResult Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome);
    }
}
