using System;

namespace Kontur.Results
{
    public static class OnSuccessResultValueExtensions
    {
        /// <summary>Executes the action if the result is successful.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
        public static Result<TFault, TValue> OnSuccess<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TValue> action)
        {
            return result.Switch(() => { }, action);
        }

        /// <inheritdoc cref="OnSuccess{TFault, TValue}(IResult{TFault, TValue}, Action{TValue})"/>
        public static Result<TFault, TValue> OnSuccess<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action action)
        {
            return result.OnSuccess(_ => action());
        }
    }
}