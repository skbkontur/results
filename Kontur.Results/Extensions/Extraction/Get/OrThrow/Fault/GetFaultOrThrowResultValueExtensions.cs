using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrThrowResultValueExtensions
    {
        /// <summary>Extracts the fault, or throws the exception from the factory if successful.</summary>
        public static TFault GetFaultOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TValue, Exception> exceptionFactory)
        {
            return result.Match(fault => fault, value => throw exceptionFactory(value));
        }

        /// <inheritdoc cref="GetFaultOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        public static TFault GetFaultOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<Exception> exceptionFactory)
        {
            return result.GetFaultOrThrow(_ => throw exceptionFactory());
        }

        /// <inheritdoc cref="GetFaultOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        [Pure]
        public static TFault GetFaultOrThrow<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            return result.GetFaultOrThrow(() => exception);
        }

        /// <inheritdoc cref="GetFaultOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, Exception})"/>
        /// <exception cref="ResultSucceedException{TValue}"/>
        [Pure]
        public static TFault GetFaultOrThrow<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetFaultOrThrow(value => new ResultSucceedException<TValue>(value, $"Can not get fault from {result}"));
        }
    }
}