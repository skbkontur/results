using System;

namespace Kontur.Results
{
    public static class OnSuccessResultPlainExtensions
    {
        public static Result<TFault> OnSuccess<TFault>(this IResult<TFault> result, Action action)
        {
            return result.Switch(() => { }, action);
        }
    }
}