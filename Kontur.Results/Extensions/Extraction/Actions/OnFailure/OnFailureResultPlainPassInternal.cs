using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnFailureResultPlainPassInternal
    {
        internal static Result<TFault> OnFailure<TFault>(
            IResult<TFault> result,
            Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        internal static async Task<Result<TFault>> OnFailure<TFault>(
            IResult<TFault> result,
            Func<TFault, Task> action)
        {
            return await result.Match<Task<Result<TFault>>>(
                async fault =>
                {
                    await action(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                () => Task.FromResult(result.Upcast()))
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault>> OnFailure<TFault>(
            IResult<TFault> result,
            Func<TFault, ValueTask> action)
        {
            return await result.Match<ValueTask<Result<TFault>>>(
                async fault =>
                {
                    await action(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                () => new(result.Upcast()))
                .ConfigureAwait(false);
        }
    }
}
