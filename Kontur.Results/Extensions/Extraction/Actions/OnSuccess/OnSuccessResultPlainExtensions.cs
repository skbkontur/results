using System;

namespace Kontur.Results
{
    public static class OnSuccessResultPlainExtensions
    {
        /// <summary>Executes the action if the result is successful.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
        public static Result<TFault> OnSuccess<TFault>(this IResult<TFault> result, Action action)
        {
            return result.Switch(() => { }, action);
        }
    }
}