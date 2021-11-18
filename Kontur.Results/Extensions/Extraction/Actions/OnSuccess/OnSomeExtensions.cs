using System;

namespace Kontur.Results
{
    public static class OnSomeExtensions
    {
        public static Optional<TValue> OnSome<TValue>(this IOptional<TValue> optional, Action<TValue> action)
        {
            return optional.Switch(() => { }, action);
        }

        public static Optional<TValue> OnSome<TValue>(this IOptional<TValue> optional, Action action)
        {
            return optional.OnSome(_ => action());
        }
    }
}