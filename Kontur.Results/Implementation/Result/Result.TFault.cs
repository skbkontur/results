using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    /// <summary>Represents a result without a value: either Success or Failure(fault).</summary>
    /// <typeparam name="TFault">The type of the fault.</typeparam>
    public abstract class Result<TFault> : IResult<TFault>
    {
        private static readonly Type TypeArgument = typeof(TFault);

        private protected Result()
        {
        }

        public bool Success => this.Match(false, true);

        public bool Failure => !this.Success;

        public static implicit operator Result<TFault>(SuccessMarker success)
        {
            _ = success;
            return Succeed();
        }

        public static implicit operator Result<TFault>(TFault fault)
        {
            return Fail(fault);
        }

        /// <summary>Creates a successful <see cref="Result{TFault, TValue}"/> with the given value.</summary>
        [Pure]
        public static Result<TFault, TValue> Succeed<TValue>(TValue value)
        {
            return Result<TFault, TValue>.Succeed(value);
        }

        [Pure]
        public static Result<TFault> Succeed()
        {
            return ResultSuccess<TFault>.Instance;
        }

        [Pure]
        public static Result<TFault> Fail(TFault fault)
        {
            return new ResultFailure<TFault>(fault);
        }

        TResult IResult<TFault>.Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess) => this.Match(onFailure, onSuccess);

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        [Pure]
        public TResult Match<TResult>(TResult onFailureValue, TResult onSuccessValue)
        {
            return this.Match(onFailureValue, () => onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        public TResult Match<TResult>(Func<TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match(onFailure, () => onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        public TResult Match<TResult>(Func<TFault, TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match(onFailure, () => onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        public TResult Match<TResult>(TResult onFailureValue, Func<TResult> onSuccess)
        {
            return this.Match(() => onFailureValue, onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        public TResult Match<TResult>(Func<TResult> onFailure, Func<TResult> onSuccess)
        {
            return this.Match(_ => onFailure(), onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault}.Match{TResult}(Func{TFault, TResult}, Func{TResult})"/>
        public abstract TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess);

        public sealed override string ToString()
        {
            var typeArguments = $"<{TypeArgument.Name}>";
            return this.Match(
                fault => $"{nameof(ResultFailure<TFault>)}{typeArguments} fault={fault}",
                () => $"{nameof(ResultSuccess<TFault>)}{typeArguments}");
        }

        public sealed override bool Equals(object? obj)
        {
            return obj is Result<TFault> other && other.GetState().Equals(this.GetState());
        }

        public sealed override int GetHashCode()
        {
            return (TypeArgument, this.GetState()).GetHashCode();
        }

        [Pure]
        private (bool Success, TFault? Fault) GetState()
        {
            return this.Match<(bool, TFault?)>(fault => (false, fault), () => (true, default));
        }
    }
}
