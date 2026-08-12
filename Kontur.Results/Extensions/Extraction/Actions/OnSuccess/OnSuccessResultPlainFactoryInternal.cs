using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OnSuccessResultPlainFactoryInternal
    {
        internal static Result<TFault> OnSuccess<TFault>(
            IResult<TFault> result,
            Action action)
        {
            return result.Switch(() => { }, action);
        }

        internal static async Task<Result<TFault>> OnSuccess<TFault>(
            IResult<TFault> result,
            Func<Task> action)
        {
            return await result.Match<Task<Result<TFault>>>(
                _ => Task.FromResult(result.Upcast()),
                async () =>
                {
                    await action().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault>> OnSuccess<TFault>(
            IResult<TFault> result,
            Func<ValueTask> action)
        {
            return await result.Match<ValueTask<Result<TFault>>>(
                _ => new(result.Upcast()),
                async () =>
                {
                    await action().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
