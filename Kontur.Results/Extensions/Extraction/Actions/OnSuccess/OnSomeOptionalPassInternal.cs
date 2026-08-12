using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnSomeOptionalPassInternal
    {
        internal static Optional<TValue> OnSome<TValue>(
            IOptional<TValue> optional,
            Action<TValue> action)
        {
            return optional.Switch(() => { }, action);
        }

        internal static async Task<Optional<TValue>> OnSome<TValue>(
            IOptional<TValue> optional,
            Func<TValue, Task> action)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                () => Task.FromResult(optional.Upcast()),
                async value =>
                {
                    await action(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Optional<TValue>> OnSome<TValue>(
            IOptional<TValue> optional,
            Func<TValue, ValueTask> action)
        {
            return await optional.Match<ValueTask<Optional<TValue>>>(
                () => new(optional.Upcast()),
                async value =>
                {
                    await action(value).ConfigureAwait(false);
                    return optional.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
