using System;

namespace Kontur.Results
{
    public static class OnFailureResultPlainExtensions
    {
        public static Result<TFault> OnFailure<TFault>(this IResult<TFault> result, Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        public static Result<TFault> OnFailure<TFault>(this IResult<TFault> result, Action action)
        {
            return result.OnFailure(_ => action());
        }
    }
}