using System;

namespace Kontur.Results
{
    public static class SwitchResultValueExtensions
    {
        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action onFailure,
            Action<TValue> onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TFault> onFailure,
            Action onSuccess)
        {
            return result.Switch(onFailure, _ => onSuccess());
        }

        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action onFailure,
            Action onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
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
    }
}