using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SwitchOptionalPassInternal
    {
        internal static Optional<TValue> Switch<TValue>(
            IOptional<TValue> optional,
            Action onNone,
            Action<TValue> onSome)
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

        internal static async Task<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Action onNone,
            Func<TValue, Task> onSome)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                () =>
                {
                    onNone();
                    return Task.FromResult(optional.Upcast());
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Action onNone,
            Func<TValue, ValueTask> onSome)
        {
            return await optional.Match<ValueTask<Optional<TValue>>>(
                () =>
                {
                    onNone();
                    return new(optional.Upcast());
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<Task> onNone,
            Action<TValue> onSome)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                value =>
                {
                    onSome(value);
                    return Task.FromResult(optional.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<Task> onNone,
            Func<TValue, Task> onSome)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<Task> onNone,
            Func<TValue, ValueTask> onSome)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask> onNone,
            Action<TValue> onSome)
        {
            return await optional.Match<ValueTask<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                value =>
                {
                    onSome(value);
                    return new(optional.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask> onNone,
            Func<TValue, Task> onSome)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Optional<TValue>> Switch<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask> onNone,
            Func<TValue, ValueTask> onSome)
        {
            return await optional.Match<ValueTask<Optional<TValue>>>(
                async () =>
                {
                    await onNone().ConfigureAwait(false);
                    return optional.Upcast();
                },
                async value =>
                {
                    await onSome(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
