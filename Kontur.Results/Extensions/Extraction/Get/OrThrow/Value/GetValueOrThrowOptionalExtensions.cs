using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrThrowOptionalExtensions
    {
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            return optional.Match(() => throw exceptionFactory(), value => value);
        }

        [Pure]
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            return optional.GetValueOrThrow(() => exception);
        }

        [Pure]
        public static TValue GetValueOrThrow<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValueOrThrow(() => new ValueMissingException($"Can not get value from {optional}"));
        }
    }
}