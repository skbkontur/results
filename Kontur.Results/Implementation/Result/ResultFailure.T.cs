using System;

namespace Kontur.Results
{
    /// <summary>A failed <see cref="Result{T}"/> containing a fault value. Can be used with implicit conversion to create <see cref="Result{TFault, TValue}"/>.</summary>
    /// <typeparam name="T">The type of the fault.</typeparam>
    public sealed class ResultFailure<T> : Result<T>
    {
        internal ResultFailure(T fault)
        {
            this.Fault = fault;
        }

        internal T Fault { get; }

        /// <summary>Creates a failed <see cref="Result{TFault, TValue}"/> with the given fault.</summary>
        public static Result<TFault, T> Create<TFault>(TFault fault)
        {
            return Result<TFault, T>.Fail(fault);
        }

        /// <inheritdoc/>
        public override TResult Match<TResult>(Func<T, TResult> onFailure, Func<TResult> onSuccess)
        {
            return onFailure(this.Fault);
        }
    }
}
