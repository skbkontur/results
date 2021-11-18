using System;

namespace Kontur.Results
{
    internal sealed class ResultSuccess<TValue> : Result<TValue>
    {
        private static readonly Lazy<ResultSuccess<TValue>> Provider = new(() => new());

        private ResultSuccess()
        {
        }

        internal static Result<TValue> Instance => Provider.Value;

        public override TResult Match<TResult>(Func<TValue, TResult> onFailure, Func<TResult> onSuccess)
        {
            return onSuccess();
        }
    }
}