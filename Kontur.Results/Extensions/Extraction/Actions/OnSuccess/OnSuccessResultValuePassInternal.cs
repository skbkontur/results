using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnSuccessResultValuePassInternal
    {
        internal static Result<TFault, TValue> OnSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Action<TValue> action)
        {
            return result.Switch(() => { }, action);
        }

        internal static async Task<Result<TFault, TValue>> OnSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, Task> action)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                _ => Task.FromResult(result.Upcast()),
                async value =>
                {
                    await action(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault, TValue>> OnSuccess<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TValue, ValueTask> action)
        {
            return await result.Match<ValueTask<Result<TFault, TValue>>>(
                _ => new(result.Upcast()),
                async value =>
                {
                    await action(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
