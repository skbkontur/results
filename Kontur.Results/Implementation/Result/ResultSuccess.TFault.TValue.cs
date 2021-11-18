using System;

namespace Kontur.Results
{
    internal sealed class ResultSuccess<TFault, TValue> : Result<TFault, TValue>
    {
        private readonly TValue value;

        internal ResultSuccess(TValue value)
        {
            this.value = value;
        }

        public override TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess)
        {
            return onSuccess(value);
        }
    }
}