using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.Plain;

namespace Kontur.Results
{
    public abstract class Optional<TValue> : IOptional<TValue>
    {
        private static readonly Type TypeArgument = typeof(TValue);

        private protected Optional()
        {
        }

        public bool HasSome => this.Match(false, true);

        public bool IsNone => !this.HasSome;

        public static implicit operator bool(Optional<TValue> optional)
        {
            return optional.HasSome;
        }

        public static implicit operator Optional<TValue>(NoneMarker none)
        {
            _ = none;
            return None();
        }

        public static implicit operator Optional<TValue>(TValue value)
        {
            return Some(value);
        }

        [Pure]
        public static Optional<TValue> None()
        {
            return None<TValue>.Instance;
        }

        [Pure]
        public static Optional<TValue> Some(TValue value)
        {
            return new Some<TValue>(value);
        }

        TResult IOptional<TValue>.Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome) => this.Match(onNone, onSome);

        [Pure]
        public bool TryGetValue([MaybeNullWhen(false)] out TValue value)
        {
            return this.Match(
                    () => EmptyContainer<TValue>.Instance,
                    val => new FilledContainer<TValue>(val))
                .TryGet(out value);
        }

        [Pure]
        public TResult Match<TResult>(TResult onNoneValue, TResult onSomeValue)
        {
            return this.Match(() => onNoneValue, onSomeValue);
        }

        public TResult Match<TResult>(TResult onNoneValue, Func<TResult> onSome)
        {
            return this.Match(() => onNoneValue, onSome);
        }

        public TResult Match<TResult>(TResult onNoneValue, Func<TValue, TResult> onSome)
        {
            return this.Match(() => onNoneValue, onSome);
        }

        public TResult Match<TResult>(Func<TResult> onNone, TResult onSomeValue)
        {
            return this.Match(onNone, () => onSomeValue);
        }

        public TResult Match<TResult>(Func<TResult> onNone, Func<TResult> onSome)
        {
            return this.Match(onNone, _ => onSome());
        }

        public abstract TResult Match<TResult>(Func<TResult> onNone, Func<TValue, TResult> onSome);

        public sealed override string ToString()
        {
            var typeArguments = $"<{TypeArgument.Name}>";
            return this.Match(
                () => $"{nameof(None<TValue>)}{typeArguments}",
                value => $"{nameof(Some<TValue>)}{typeArguments} value={value}");
        }

        public sealed override bool Equals(object obj)
        {
            return obj is Optional<TValue> other && other.GetState().Equals(this.GetState());
        }

        public sealed override int GetHashCode()
        {
            return (TypeArgument, this.GetState()).GetHashCode();
        }

        [Pure]
        private (bool Success, TValue? Value) GetState()
        {
            return this.Match<(bool, TValue?)>(() => (false, default), value => (true, value));
        }
    }
}