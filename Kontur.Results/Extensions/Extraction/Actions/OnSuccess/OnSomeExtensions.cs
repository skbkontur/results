using System;

namespace Kontur.Results
{
    public static class OnSomeExtensions
    {
        /// <summary>Executes the action if the optional has a value.</summary>
        /// <returns>The original optional, enabling method chaining.</returns>
        public static Optional<TValue> OnSome<TValue>(this IOptional<TValue> optional, Action<TValue> action)
        {
            return optional.Switch(() => { }, action);
        }

        /// <inheritdoc cref="OnSome{TValue}(IOptional{TValue}, Action{TValue})"/>
        public static Optional<TValue> OnSome<TValue>(this IOptional<TValue> optional, Action action)
        {
            return optional.OnSome(_ => action());
        }
    }
}