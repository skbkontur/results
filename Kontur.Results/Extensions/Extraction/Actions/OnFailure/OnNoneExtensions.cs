using System;

namespace Kontur.Results
{
    public static class OnNoneExtensions
    {
        /// <summary>Executes the action if the optional is None.</summary>
        /// <returns>The original optional, enabling method chaining.</returns>
        public static Optional<TValue> OnNone<TValue>(this IOptional<TValue> optional, Action action)
        {
            return optional.Switch(action, () => { });
        }
    }
}