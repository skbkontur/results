using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class MapFaultResultFailurePassInternal
    {
        internal static ResultFailure<TResult> MapFault<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, TResult> faultFactory)
        {
            return Result.Fail(faultFactory(result.Fault));
        }

        internal static async Task<ResultFailure<TResult>> MapFault<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, Task<TResult>> faultFactory)
        {
            var fault = await faultFactory(result.Fault).ConfigureAwait(false);
            return Result.Fail(fault);
        }

        internal static async ValueTask<ResultFailure<TResult>> MapFault<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, ValueTask<TResult>> faultFactory)
        {
            var fault = await faultFactory(result.Fault).ConfigureAwait(false);
            return Result.Fail(fault);
        }
    }
}