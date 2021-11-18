using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseResultFailureResultFailurePassInternal
    {
        internal static ResultFailure<TResult> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, ResultFailure<TResult>> onFailureResultFactory)
        {
            Result<TFault, TFault> converted = result;
            return onFailureResultFactory(converted.Match(fault => fault, fault => fault));
        }

        internal static Task<ResultFailure<TResult>> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, Task<ResultFailure<TResult>>> onFailureResultFactory)
        {
            Result<TFault, TFault> converted = result;
            return onFailureResultFactory(converted.Match(fault => fault, fault => fault));
        }

        internal static ValueTask<ResultFailure<TResult>> OrElse<TFault, TResult>(
            ResultFailure<TFault> result,
            Func<TFault, ValueTask<ResultFailure<TResult>>> onFailureResultFactory)
        {
            Result<TFault, TFault> converted = result;
            return onFailureResultFactory(converted.Match(fault => fault, fault => fault));
        }
    }
}