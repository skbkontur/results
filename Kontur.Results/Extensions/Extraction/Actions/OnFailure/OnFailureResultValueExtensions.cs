using System;

namespace Kontur.Results
{
    public static class OnFailureResultValueExtensions
    {
        public static Result<TFault, TValue> OnFailure<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        public static Result<TFault, TValue> OnFailure<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action action)
        {
            return result.OnFailure(_ => action());
        }
    }
}