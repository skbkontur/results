using System;

namespace Kontur.Results
{
    public static class OnFailureResultValueExtensions
    {
        /// <summary>Executes the action if the result is a failure.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
        public static Result<TFault, TValue> OnFailure<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        /// <inheritdoc cref="OnFailure{TFault, TValue}(IResult{TFault, TValue}, Action{TFault})"/>
        public static Result<TFault, TValue> OnFailure<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Action action)
        {
            return result.OnFailure(_ => action());
        }
    }
}