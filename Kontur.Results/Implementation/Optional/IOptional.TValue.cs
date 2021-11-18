using System;
using System.ComponentModel;

namespace Kontur.Results
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IOptional<out TValue>
    {
        internal TResult Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome);
    }
}