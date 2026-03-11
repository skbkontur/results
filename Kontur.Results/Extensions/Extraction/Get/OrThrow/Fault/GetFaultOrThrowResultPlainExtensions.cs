using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrThrowResultPlainExtensions
    {
        /// <summary>Extracts the fault, or throws the exception from the factory if successful.</summary>
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result, Func<Exception> exceptionFactory)
        {
            return result.Match(fault => fault, () => throw exceptionFactory());
        }

        /// <inheritdoc cref="GetFaultOrThrow{TFault}(IResult{TFault}, Func{Exception})"/>
        [Pure]
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result, Exception exception)
        {
            return result.GetFaultOrThrow(() => exception);
        }

        /// <inheritdoc cref="GetFaultOrThrow{TFault}(IResult{TFault}, Func{Exception})"/>
        /// <exception cref="ResultSucceedException"/>
        [Pure]
        public static TFault GetFaultOrThrow<TFault>(this IResult<TFault> result)
        {
            return result.GetFaultOrThrow(() => new ResultSucceedException($"Can not get fault from {result}"));
        }
    }
}