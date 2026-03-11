using System;

namespace Kontur.Results
{
    public static class SwitchResultValueExtensions
    {
        /// <inheritdoc cref="Switch{TFault, TValue}(IResult{TFault, TValue}, Action{TFault}, Action{TValue})"/>
        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action onFailure,
            Action<TValue> onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

        /// <inheritdoc cref="Switch{TFault, TValue}(IResult{TFault, TValue}, Action{TFault}, Action{TValue})"/>
        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TFault> onFailure,
            Action onSuccess)
        {
            return result.Switch(onFailure, _ => onSuccess());
        }

        /// <inheritdoc cref="Switch{TFault, TValue}(IResult{TFault, TValue}, Action{TFault}, Action{TValue})"/>
        public static Result<TFault, TValue> Switch<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action onFailure,
            Action onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

        /// <summary>Executes one of two actions depending on the result state.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
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
