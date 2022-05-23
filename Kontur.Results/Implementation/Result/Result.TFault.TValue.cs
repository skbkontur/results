using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.ResultValue;

namespace Kontur.Results
{
    public abstract class Result<TFault, TValue> : IResult<TFault, TValue>
    {
        private static readonly Type FaultTypeArgument = typeof(TFault);
        private static readonly Type ValueTypeArgument = typeof(TValue);

        public bool Success => this.Match(false, true);

        public bool Failure => !this.Success;

        public static implicit operator bool(Result<TFault, TValue> result)
        {
            return result.Success;
        }

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

        [Pure]
        public bool TryGetFault([MaybeNullWhen(false)] out TFault fault)
        {
            return !this.TryGetValue(out _, out fault);
        }

        [Pure]
        public bool TryGetFault(
            [MaybeNullWhen(false)] out TFault fault,
            [MaybeNullWhen(true)] out TValue value)
        {
            return !this.TryGetValue(out value, out fault);
        }

        [Pure]
        public bool TryGetValue([MaybeNullWhen(false)] out TValue value)
        {
            return this.TryGetValue(out value, out _);
        }

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

        [Pure]
        public TResult Match<TResult>(TResult onFailureValue, TResult onSuccessValue)
        {
            return this.Match(() => onFailureValue, onSuccessValue);
        }

        public TResult Match<TResult>(Func<TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match(_ => onFailure(), onSuccessValue);
        }

        public TResult Match<TResult>(Func<TFault, TResult> onFailure, TResult onSuccessValue)
        {
            return this.Match<TResult>(onFailure, _ => onSuccessValue);
        }

        public TResult Match<TResult>(TResult onFailureValue, Func<TResult> onSuccess)
        {
            return this.Match(() => onFailureValue, onSuccess);
        }

        public TResult Match<TResult>(Func<TResult> onFailure, Func<TResult> onSuccess)
        {
            return this.Match(_ => onFailure(), onSuccess);
        }

        public TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TResult> onSuccess)
        {
            return this.Match<TResult>(onFailure, _ => onSuccess());
        }

        public TResult Match<TResult>(TResult onFailureValue, Func<TValue, TResult> onSuccess)
        {
            return this.Match(() => onFailureValue, onSuccess);
        }

        public TResult Match<TResult>(Func<TResult> onFailure, Func<TValue, TResult> onSuccess)
        {
            return this.Match<TResult>(_ => onFailure(), onSuccess);
        }

        public abstract TResult Match<TResult>(Func<TFault, TResult> onFailure, Func<TValue, TResult> onSuccess);

        public sealed override string ToString()
        {
            var typeArguments = $"<{FaultTypeArgument.Name}, {ValueTypeArgument.Name}>";
            return this.Match(
                fault => $"{nameof(ResultFailure<TFault, TValue>)}{typeArguments} fault={fault}",
                value => $"{nameof(ResultSuccess<TFault, TValue>)}{typeArguments} value={value}");
        }

        public sealed override bool Equals(object obj)
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