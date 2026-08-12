using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnFailureResultValuePassInternal
    {
        internal static Result<TFault, TValue> OnFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        internal static async Task<Result<TFault, TValue>> OnFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task> action)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                async fault =>
                {
                    await action(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                _ => Task.FromResult(result.Upcast()))
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault, TValue>> OnFailure<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask> action)
        {
            return await result.Match<ValueTask<Result<TFault, TValue>>>(
                async fault =>
                {
                    await action(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                _ => new(result.Upcast()))
                .ConfigureAwait(false);
        }
    }
}
