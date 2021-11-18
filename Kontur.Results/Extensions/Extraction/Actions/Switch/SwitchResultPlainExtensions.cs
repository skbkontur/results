using System;

namespace Kontur.Results
{
    public static class SwitchResultPlainExtensions
    {
        public static Result<TFault> Switch<TFault>(this IResult<TFault> result, Action onFailure, Action onSuccess)
        {
            return result.Switch(_ => onFailure(), onSuccess);
        }

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