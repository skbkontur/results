using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetValueOrThrowResultValuePassInternal
    {
        internal static TValue GetValueOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Exception> exceptionFactory)
        {
            return result.Match(fault => throw exceptionFactory(fault), value => value);
        }

        internal static async Task<TValue> GetValueOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task<Exception>> exceptionFactory)
        {
            return await result.Match<Task<TValue>>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                value => Task.FromResult(value)).ConfigureAwait(false);
        }

        internal static async ValueTask<TValue> GetValueOrThrow<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask<Exception>> exceptionFactory)
        {
            return await result.Match<ValueTask<TValue>>(
                async fault => throw await exceptionFactory(fault).ConfigureAwait(false),
                value => new(value)).ConfigureAwait(false);
        }
    }
}
