using System;

namespace Kontur.Results
{
    public static class SwitchOptionalExtensions
    {
        /// <inheritdoc cref="Switch{TValue}(IOptional{TValue}, Action, Action{TValue})"/>
        public static Optional<TValue> Switch<TValue>(this IOptional<TValue> optional, Action onNone, Action onSome)
        {
            return optional.Switch(onNone, _ => onSome());
        }

        /// <summary>Executes one of two actions depending on the optional state.</summary>
        /// <returns>The original optional, enabling method chaining.</returns>
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