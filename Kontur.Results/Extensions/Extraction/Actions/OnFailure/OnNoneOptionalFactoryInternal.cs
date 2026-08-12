using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnNoneOptionalFactoryInternal
    {
        internal static Optional<TValue> OnNone<TValue>(
            IOptional<TValue> optional,
            Action action)
        {
            return optional.Switch(action, () => { });
        }

        internal static async Task<Optional<TValue>> OnNone<TValue>(
            IOptional<TValue> optional,
            Func<Task> action)
        {
            return await optional.Match<Task<Optional<TValue>>>(
                async () =>
                {
                    await action().ConfigureAwait(false);
                    return optional.Upcast();
                },
                _ => Task.FromResult(optional.Upcast()))
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Optional<TValue>> OnNone<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask> action)
        {
            return await optional.Match<ValueTask<Optional<TValue>>>(
                async () =>
                {
                    await action().ConfigureAwait(false);
                    return optional.Upcast();
                },
                _ => new(optional.Upcast()))
                .ConfigureAwait(false);
        }
    }
}
