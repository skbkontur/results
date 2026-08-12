using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SwitchResultPlainPassInternal
    {
        internal static Result<TFault> Switch<TFault>(
            IResult<TFault> result,
            Action<TFault> onFailure,
            Action onSuccess)
        {
            return result.Match(
                fault =>
                {
                    onFailure(fault);
                    return result;
                },
                () =>
                {
                    onSuccess();
                    return result;
                })
                .Upcast();
        }

        internal static async Task<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Action<TFault> onFailure,
            Func<Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault>>>(
                fault =>
                {
                    onFailure(fault);
                    return Task.FromResult(result.Upcast());
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Action<TFault> onFailure,
            Func<ValueTask> onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault>>>(
                fault =>
                {
                    onFailure(fault);
                    return new(result.Upcast());
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, Task> onFailure,
            Action onSuccess)
        {
            return await result.Match<Task<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                () =>
                {
                    onSuccess();
                    return Task.FromResult(result.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, Task> onFailure,
            Func<Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, Task> onFailure,
            Func<ValueTask> onSuccess)
        {
            return await result.Match<Task<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, ValueTask> onFailure,
            Action onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                () =>
                {
                    onSuccess();
                    return new(result.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, ValueTask> onFailure,
            Func<Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault>> Switch<TFault>(
            IResult<TFault> result,
            Func<TFault, ValueTask> onFailure,
            Func<ValueTask> onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async () =>
                {
                    await onSuccess().ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
