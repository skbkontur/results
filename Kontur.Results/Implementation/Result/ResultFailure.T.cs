using System;

namespace Kontur.Results
{
    public sealed class ResultFailure<T> : Result<T>
    {
        internal ResultFailure(T fault)
        {
            Fault = fault;
        }

        internal T Fault { get; }

        public static Result<TFault, T> Create<TFault>(TFault fault)
        {
            return Result<TFault, T>.Fail(fault);
        }

        public override TResult Match<TResult>(Func<T, TResult> onFailure, Func<TResult> onSuccess)
        {
            return onFailure(Fault);
        }
    }
}