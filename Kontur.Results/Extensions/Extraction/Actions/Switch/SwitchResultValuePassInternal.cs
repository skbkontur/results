using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SwitchResultValuePassInternal
    {
        internal static Result<TFault, TValue> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Action<TFault> onFailure,
            Action<TValue> onSuccess)
        {
            return result.Match(
                fault =>
                {
                    onFailure(fault);
                    return result;
                },
                value =>
                {
                    onSuccess(value);
                    return result;
                })
                .Upcast();
        }

        internal static async Task<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Action<TFault> onFailure,
            Func<TValue, Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                fault =>
                {
                    onFailure(fault);
                    return Task.FromResult(result.Upcast());
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Action<TFault> onFailure,
            Func<TValue, ValueTask> onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault, TValue>>>(
                fault =>
                {
                    onFailure(fault);
                    return new(result.Upcast());
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task> onFailure,
            Action<TValue> onSuccess)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                value =>
                {
                    onSuccess(value);
                    return Task.FromResult(result.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task> onFailure,
            Func<TValue, Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, Task> onFailure,
            Func<TValue, ValueTask> onSuccess)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask> onFailure,
            Action<TValue> onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                value =>
                {
                    onSuccess(value);
                    return new(result.Upcast());
                })
                .ConfigureAwait(false);
        }

        internal static async Task<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask> onFailure,
            Func<TValue, Task> onSuccess)
        {
            return await result.Match<Task<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }

        internal static async ValueTask<Result<TFault, TValue>> Switch<TFault, TValue>(
            IResult<TFault, TValue> result,
            Func<TFault, ValueTask> onFailure,
            Func<TValue, ValueTask> onSuccess)
        {
            return await result.Match<ValueTask<Result<TFault, TValue>>>(
                async fault =>
                {
                    await onFailure(fault).ConfigureAwait(false);
                    return result.Upcast();
                },
                async value =>
                {
                    await onSuccess(value).ConfigureAwait(false);
                    return result.Upcast();
                })
                .ConfigureAwait(false);
        }
    }
}
