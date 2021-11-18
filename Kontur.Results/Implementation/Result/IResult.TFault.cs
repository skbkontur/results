using System;
using System.ComponentModel;

namespace Kontur.Results
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IResult<out TFault>
    {
        internal TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess);
    }
}