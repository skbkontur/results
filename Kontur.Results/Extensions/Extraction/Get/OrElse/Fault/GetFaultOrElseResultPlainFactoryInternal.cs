using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetFaultOrElseResultPlainFactoryInternal
    {
        internal static TFault GetFaultOrElse<TFault>(
            IResult<TFault> result,
            Func<TFault> faultFactory)
        {
            return result.Match(fault => fault, faultFactory);
        }

        internal static async Task<TFault> GetFaultOrElse<TFault>(
            IResult<TFault> result,
            Func<Task<TFault>> faultFactory)
        {
            return await result.Match(fault => Task.FromResult(fault), faultFactory).ConfigureAwait(false);
        }

        internal static async ValueTask<TFault> GetFaultOrElse<TFault>(
            IResult<TFault> result,
            Func<ValueTask<TFault>> faultFactory)
        {
            return await result.Match<ValueTask<TFault>>(fault => new(fault), faultFactory).ConfigureAwait(false);
        }
    }
}
