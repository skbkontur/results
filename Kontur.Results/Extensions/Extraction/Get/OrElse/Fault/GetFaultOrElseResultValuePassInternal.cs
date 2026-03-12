using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetFaultOrElseResultValuePassInternal
    {
        internal static TFault GetFaultOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, TFault> faultFactory)
        {
            return result.Match(fault => fault, faultFactory);
        }

        internal static async Task<TFault> GetFaultOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Task<TFault>> faultFactory)
        {
            return await result.Match(fault => Task.FromResult(fault), faultFactory).ConfigureAwait(false);
        }

        internal static async ValueTask<TFault> GetFaultOrElse<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, ValueTask<TFault>> faultFactory)
        {
            return await result.Match<ValueTask<TFault>>(fault => new(fault), faultFactory).ConfigureAwait(false);
        }
    }
}
