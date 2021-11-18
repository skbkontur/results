using System;

namespace Kontur.Results
{
    public static class OnSuccessResultValueExtensions
    {
        public static Result<TFault, TValue> OnSuccess<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TValue> action)
        {
            return result.Switch(() => { }, action);
        }

        public static Result<TFault, TValue> OnSuccess<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action action)
        {
            return result.OnSuccess(_ => action());
        }
    }
}