using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrThrowOptionalExtensions
    {
        /// <summary>Extracts the value, or throws the exception from the factory if None.</summary>
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            return optional.Match(() => throw exceptionFactory(), value => value);
        }

        /// <inheritdoc cref="GetValueOrThrow{TValue}(IOptional{TValue}, Func{Exception})"/>
        [Pure]
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            return optional.GetValueOrThrow(() => exception);
        }

        /// <inheritdoc cref="GetValueOrThrow{TValue}(IOptional{TValue}, Func{Exception})"/>
        /// <exception cref="ValueMissingException"/>
        [Pure]
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValueOrThrow(() => new ValueMissingException($"Can not get value from {optional}"));
        }
    }
}