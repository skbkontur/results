using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class MapFaultResultPlainPassInternal
    {
        internal static Result<TResult> MapFault<TFault, TResult>(
            IResult<TFault> result,
            Func<TFault, TResult> faultFactory)
        {
            return result.Match(fault => Result<TResult>.Fail(faultFactory(fault)), Result<TResult>.Succeed);
        }

        internal static Task<Result<TResult>> MapFault<TFault, TResult>(
            IResult<TFault> result,
            Func<TFault, Task<TResult>> faultFactory)
        {
            return result.Match(
                async fault =>
                {
                    var createdFault = await faultFactory(fault).ConfigureAwait(false);
                    return Result<TResult>.Fail(createdFault);
                },
                () => Task.FromResult(Result<TResult>.Succeed()));
        }

        internal static ValueTask<Result<TResult>> MapFault<TFault, TResult>(
            IResult<TFault> result,
            Func<TFault, ValueTask<TResult>> faultFactory)
        {
            return result.Match<ValueTask<Result<TResult>>>(
               async fault =>
                {
                    var createdFault = await faultFactory(fault).ConfigureAwait(false);
                    return Result<TResult>.Fail(createdFault);
                },
               () => new(Result<TResult>.Succeed()));
        }
    }
}