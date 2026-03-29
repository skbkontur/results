using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetValueOrThrowOptionalFactoryInternal
    {
        internal static TValue GetValueOrThrow<TValue>(
            IOptional<TValue> optional,
            Func<Exception> exceptionFactory)
        {
            return optional.Match(() => throw exceptionFactory(), value => value);
        }

        internal static async Task<TValue> GetValueOrThrow<TValue>(
            IOptional<TValue> optional,
            Func<Task<Exception>> exceptionFactory)
        {
            return await optional.Match<Task<TValue>>(
                async () => throw await exceptionFactory().ConfigureAwait(false),
                value => Task.FromResult(value)).ConfigureAwait(false);
        }

        internal static async ValueTask<TValue> GetValueOrThrow<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask<Exception>> exceptionFactory)
        {
            return await optional.Match<ValueTask<TValue>>(
                async () => throw await exceptionFactory().ConfigureAwait(false),
                value => new(value)).ConfigureAwait(false);
        }
    }
}
