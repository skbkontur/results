using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class Optional
    {
        [Pure]
        public static NoneMarker None()
        {
            return default;
        }

        [Pure]
        public static Optional<TValue> None<TValue>()
        {
            return Optional<TValue>.None();
        }

        [Pure]
        public static Optional<TValue> Some<TValue>(TValue value)
        {
            return Optional<TValue>.Some(value);
        }
    }
}