using System;

namespace Kontur.Results
{
    public static class OnFailureResultPlainExtensions
    {
        /// <summary>Executes the action if the result is a failure.</summary>
        /// <returns>The original result, enabling method chaining.</returns>
        public static Result<TFault> OnFailure<TFault>(this IResult<TFault> result, Action<TFault> action)
        {
            return result.Switch(action, () => { });
        }

        /// <inheritdoc cref="OnFailure{TFault}(IResult{TFault}, Action{TFault})"/>
        public static Result<TFault> OnFailure<TFault>(this IResult<TFault> result, Action action)
        {
            return result.OnFailure(_ => action());
        }
    }
}