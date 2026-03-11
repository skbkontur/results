using System;

namespace Kontur.Results
{
    public static class SwitchResultPlainExtensions
    {
        /// <inheritdoc cref="Switch{TFault}(IResult{TFault}, Action{TFault}, Action)"/>
        public static Result<TFault> Switch<TFault>(this IResult<TFault> result, Action onFailure, Action onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

        /// <summary>Executes one of two actions depending on the result state.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
        public static Result<TFault> Switch<TFault>(this IResult<TFault> result, Action<TFault> onFailure, Action onSuccess)
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
    }
}