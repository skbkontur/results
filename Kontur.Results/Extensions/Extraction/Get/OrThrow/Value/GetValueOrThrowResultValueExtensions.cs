using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrThrowResultValueExtensions
    {
        /// <summary>Extracts the value, or throws the exception from the factory if failed.</summary>
        public static TValue GetValueOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TFault, Exception> exceptionFactory)
        {
            return result.Match(fault => throw exceptionFactory(fault), value => value);
        }

        /// <inheritdoc cref="GetValueOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        public static TValue GetValueOrThrow<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<Exception> exceptionFactory)
        {
            return result.GetValueOrThrow(_ => throw exceptionFactory());
        }

        /// <inheritdoc cref="GetValueOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        [Pure]
        public static TValue GetValueOrThrow<TFault, TValue>(this IResult<TFault, TValue> result, Exception exception)
        {
            return result.GetValueOrThrow(() => exception);
        }

        /// <inheritdoc cref="GetValueOrThrow{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, Exception})"/>
        /// <exception cref="ResultFailedException{TFault}"/>
        [Pure]
        public static TValue GetValueOrThrow<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValueOrThrow(fault => new ResultFailedException<TFault>(fault, $"Can not get value from {result}"));
        }
    }
}