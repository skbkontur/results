using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class MapFaultResultValuePassInternal
    {
        internal static Result<TResult, TValue> MapFault<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TFault, TResult> faultFactory)
        {
            return result.Match(fault => Result<TResult, TValue>.Fail(faultFactory(fault)), Result<TResult, TValue>.Succeed);
        }

        internal static Task<Result<TResult, TValue>> MapFault<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TFault, Task<TResult>> faultFactory)
        {
            return result.Match(
                async fault =>
                {
                    var createdFault = await faultFactory(fault).ConfigureAwait(false);
                    return Result<TResult, TValue>.Fail(createdFault);
                },
                value => Task.FromResult(Result<TResult, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TResult, TValue>> MapFault<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask<TResult>> faultFactory)
        {
            return result.Match<ValueTask<Result<TResult, TValue>>>(
               async fault =>
                {
                    var createdFault = await faultFactory(fault).ConfigureAwait(false);
                    return Result<TResult, TValue>.Fail(createdFault);
                },
               value => new(Result<TResult, TValue>.Succeed(value)));
        }
    }
}