using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetValueOrElseResultValuePassInternal
    {
        internal static TValue GetValueOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, TValue> valueFactory)
        {
            return result.Match(valueFactory, value => value);
        }

        internal static async Task<TValue> GetValueOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task<TValue>> valueFactory)
        {
            return await result.Match(valueFactory, value => Task.FromResult(value)).ConfigureAwait(false);
        }

        internal static async ValueTask<TValue> GetValueOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask<TValue>> valueFactory)
        {
            return await result.Match<ValueTask<TValue>>(valueFactory, value => new(value)).ConfigureAwait(false);
        }
    }
}
