using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class EnsureHasValueOptionalFactoryInternal
    {
        internal static void EnsureHasValue<TValue>(
            IOptional<TValue> optional,
            Func<Exception> exceptionFactory)
        {
            optional.OnNone(new Action(() => throw exceptionFactory()));
        }

        internal static async Task EnsureHasValue<TValue>(
            IOptional<TValue> optional,
            Func<Task<Exception>> exceptionFactory)
        {
            await optional.Match<Task>(
                async () => throw await exceptionFactory().ConfigureAwait(false),
                _ => Task.CompletedTask).ConfigureAwait(false);
        }

        internal static async ValueTask EnsureHasValue<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask<Exception>> exceptionFactory)
        {
            await optional.Match<ValueTask>(
                async () => throw await exceptionFactory().ConfigureAwait(false),
                _ => default).ConfigureAwait(false);
        }
    }
}
