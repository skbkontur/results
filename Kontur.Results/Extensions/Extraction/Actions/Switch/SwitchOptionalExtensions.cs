using System;

namespace Kontur.Results
{
    public static class SwitchOptionalExtensions
    {
        public static Optional<TValue> Switch<TValue>(this IOptional<TValue> optional, Action onNone, Action onSome)
        {
            return optional.Switch(onNone, _ => onSome());
        }

        public static Optional<TValue> Switch<TValue>(this IOptional<TValue> optional, Action onNone, Action<TValue> onSome)
        {
            return optional.Match(
                () =>
                {
                    onNone();
                    return optional;
                },
                value =>
                {
                    onSome(value);
                    return optional;
                })
                .Upcast();
        }
    }
}