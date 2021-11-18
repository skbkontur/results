using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class MapValueResultValuePassInternal
    {
        internal static Result<TFault, TResult> MapValue<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TValue, TResult> valueFactory)
        {
            return result.Match(Result<TFault, TResult>.Fail, value => Result<TFault, TResult>.Succeed(valueFactory(value)));
        }

        internal static Task<Result<TFault, TResult>> MapValue<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TValue, Task<TResult>> valueFactory)
        {
            return result.Match(
                fault => Task.FromResult(Result<TFault, TResult>.Fail(fault)),
                async value =>
                {
                    var createdValue = await valueFactory(value).ConfigureAwait(false);
                    return Result<TFault, TResult>.Succeed(createdValue);
                });
        }

        internal static ValueTask<Result<TFault, TResult>> MapValue<TFault, TValue, TResult>(
            IResult<TFault, TValue> result,
            Func<TValue, ValueTask<TResult>> valueFactory)
        {
            return result.Match<ValueTask<Result<TFault, TResult>>>(
                fault => new(Result<TFault, TResult>.Fail(fault)),
                async value =>
                {
                    var createdValue = await valueFactory(value).ConfigureAwait(false);
                    return Result<TFault, TResult>.Succeed(createdValue);
                });
        }
    }
}