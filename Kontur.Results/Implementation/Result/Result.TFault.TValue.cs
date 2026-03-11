using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.ResultValue;

namespace Kontur.Results
{
    /// <summary>Represents a result with a value: either Succeed(value) or Fail(fault).</summary>
    /// <typeparam name="TFault">The type of the fault.</typeparam>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    public abstract class Result<TFault, TValue> : IResult<TFault, TValue>
    {
        private static readonly Type FaultTypeArgument = typeof(TFault);
        private static readonly Type ValueTypeArgument = typeof(TValue);

        public bool Success => this.Match(false, true);

        public bool Failure => !this.Success;

        /// <summary>Converts to a plain result, discarding the value.</summary>
        public static implicit operator Result<TFault>(Result<TFault, TValue> result)
        {
            return result.Match(Result<TFault>.Fail, () => Result<TFault>.Succeed());
        }

        public static implicit operator Result<TFault, TValue>(TFault fault)
        {
            return Fail(fault);
        }

        public static implicit operator Result<TFault, TValue>(TValue value)
        {
            return Succeed(value);
        }

        public static implicit operator Result<TFault, TValue>(ResultFailure<TFault> marker)
        {
            return marker.Fault;
        }

        public static implicit operator Result<TFault, TValue>(SuccessMarker<TValue> marker)
        {
            return marker.Value;
        }

        [Pure]
        public static Result<TFault, TValue> Fail(TFault fault)
        {
            return new ResultFailure<TFault, TValue>(fault);
        }

        [Pure]
        public static Result<TFault, TValue> Succeed(TValue value)
        {
            return new ResultSuccess<TFault, TValue>(value);
        }

        TResult IResult<TFault, TValue>.Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess) => this.Match(onFailure, onSuccess);

        TResult IResult<TFault, TValue>.Match<TResult>(Func<TResult> onFailure, Func<TValue, TResult> onSuccess) => this.Match(onFailure, onSuccess);

        TResult IResult<TFault, TValue>.Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess) => this.Match(onFailure, onSuccess);

        /// <summary>Attempts to extract the fault.</summary>
        /// <returns><c>true</c> if the result is a failure and <paramref name="fault"/> is set; <c>false</c> if successful.</returns>
        [Pure]
        public bool TryGetFault([MaybeNullWhen(false)] out TFault fault)
        {
            return !this.TryGetValue(out _, out fault);
        }

        /// <summary>Attempts to extract the fault, providing both fault and value.</summary>
        /// <returns><c>true</c> if the result is a failure and <paramref name="fault"/> is set; <c>false</c> if successful and <paramref name="value"/> is set.</returns>
        [Pure]
        public bool TryGetFault(
            [MaybeNullWhen(false)] out TFault fault,
            [MaybeNullWhen(true)] out TValue value)
        {
            return !this.TryGetValue(out value, out fault);
        }

        /// <summary>Attempts to extract the value.</summary>
        /// <returns><c>true</c> if successful and <paramref name="value"/> is set; <c>false</c> if the result is a failure.</returns>
        [Pure]
        public bool TryGetValue([MaybeNullWhen(false)] out TValue value)
        {
            return this.TryGetValue(out value, out _);
        }

        /// <summary>Attempts to extract the value, providing both value and fault.</summary>
        /// <returns><c>true</c> if successful and <paramref name="value"/> is set; <c>false</c> if failed and <paramref name="fault"/> is set.</returns>
        [Pure]
        public bool TryGetValue(
            [MaybeNullWhen(false)] out TValue value,
            [MaybeNullWhen(true)] out TFault fault)
        {
            return this.Match<IContainer<TFault, TValue>>(
                    f => new FailureContainer<TFault, TValue>(f),
                    val => new SuccessContainer<TFault, TValue>(val))
                .TryGet(out value, out fault);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        [Pure]
        public TResult Match<TResult>(TResult onFailureValue, TResult onSuccessValue)
        {
            return this.Match(() => onFailureValue, onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(Func<TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match(_ => onFailure(), onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(Func<TFault, TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match<TResult>(onFailure, _ => onSuccessValue);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(TResult onFailureValue, Func<TResult> onSuccess)
        {
            return this.Match(() => onFailureValue, onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(Func<TResult> onFailure, Func<TResult> onSuccess)
        {
            return this.Match(_ => onFailure(), onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess)
        {
            return this.Match<TResult>(onFailure, _ => onSuccess());
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(TResult onFailureValue, Func<TValue, TResult> onSuccess)
        {
            return this.Match(() => onFailureValue, onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public TResult Match<TResult>(Func<TResult> onFailure, Func<TValue, TResult> onSuccess)
        {
            return this.Match<TResult>(_ => onFailure(), onSuccess);
        }

        /// <inheritdoc cref="IResult{TFault, TValue}.Match{TResult}(Func{TFault, TResult}, Func{TValue, TResult})"/>
        public abstract TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess);

        public sealed override string ToString()
        {
            var typeArguments = $"<{FaultTypeArgument.Name}, {ValueTypeArgument.Name}>";
            return this.Match(
                fault => $"{nameof(ResultFailure<TFault, TValue>)}{typeArguments} fault={fault}",
                value => $"{nameof(ResultSuccess<TFault, TValue>)}{typeArguments} value={value}");
        }

        public sealed override bool Equals(object? obj)
        {
            return obj is Result<TFault, TValue> other && other.GetState().Equals(this.GetState());
        }

        public sealed override int GetHashCode()
        {
            return (FaultTypeArgument, ValueTypeArgument, this.GetState()).GetHashCode();
        }

        [Pure]
        private (bool Success, object? Result) GetState()
        {
            return this.Match<(bool, object?)>(fault => (false, fault), value => (true, value));
        }
    }
}
