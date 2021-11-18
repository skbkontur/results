using System;

namespace Kontur.Results
{
    public static class OnNoneExtensions
    {
        public static Optional<TValue> OnNone<TValue>(this IOptional<TValue> optional, Action action)
        {
            return optional.Switch(action, () => { });
        }
    }
}