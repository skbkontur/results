using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureNoneOptionalPassInternal
    {
        internal static void EnsureNone<TValue>(
            IOptional<TValue> optional,
            Func<TValue, Exception> exceptionFactory)
        {
            optional.OnSome(new Action<TValue>(value => throw exceptionFactory(value)));
        }

        internal static async Task EnsureNone<TValue>(
            IOptional<TValue> optional,
            Func<TValue, Task<Exception>> exceptionFactory)
        {
            await optional.Match<Task>(
                () => Task.CompletedTask,
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureNone<TValue>(
            IOptional<TValue> optional,
            Func<TValue, ValueTask<Exception>> exceptionFactory)
        {
            await optional.Match<ValueTask>(
                () => default,
                async value => throw await exceptionFactory(value).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
