using System;

namespace Kontur.Results
{
    internal sealed class ResultFailure<TFault, TValue> : Result<TFault, TValue>
    {
        private readonly TFault fault;

        internal ResultFailure(TFault fault)
        {
            this.fault = fault;
        }

        public override TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess)
        {
            return onFailure(fault);
        }
    }
}