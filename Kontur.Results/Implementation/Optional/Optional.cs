using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    /// <summary>Factory methods for creating <see cref="Optional{TValue}"/> instances.</summary>
    public static class Optional
    {
        /// <summary>Creates a None marker that implicitly converts to <see cref="Optional{TValue}"/>. For a value, use <see cref="Some{TValue}(TValue)"/>.</summary>
        [Pure]
        public static NoneMarker None()
        {
            return default;
        }

        /// <inheritdoc cref="Optional{TValue}.None"/>
        [Pure]
        public static Optional<TValue> None<TValue>()
        {
            return Optional<TValue>.None();
        }

        /// <inheritdoc cref="Optional{TValue}.Some"/>
        [Pure]
        public static Optional<TValue> Some<TValue>(TValue value)
        {
            return Optional<TValue>.Some(value);
        }
    }
}
