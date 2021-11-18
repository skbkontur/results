using System;
using System.ComponentModel;

namespace Kontur.Results
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IResult<out TFault, out TValue>
    {
        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess);

        internal TResult Match<TResult>(Func<TResult> onFailure, Func<TValue, TResult> onSuccess);

        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess);
    }
}