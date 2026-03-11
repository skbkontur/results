using System;

namespace Kontur.Results
{
    public static class EnsureNoneExtensions
    {
        /// <summary>Throws if the optional has a value.</summary>
        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Func<TValue, Exception> exceptionFactory)
        {
            optional.OnSome(value => throw exceptionFactory(value));
        }

        /// <inheritdoc cref="EnsureNone{TValue}(IOptional{TValue}, Func{TValue, Exception})"/>
        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            optional.EnsureNone(_ => exceptionFactory());
        }

        /// <inheritdoc cref="EnsureNone{TValue}(IOptional{TValue}, Func{TValue, Exception})"/>
        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            optional.EnsureNone(() => exception);
        }

        /// <inheritdoc cref="EnsureNone{TValue}(IOptional{TValue}, Func{TValue, Exception})"/>
        /// <exception cref="ValueExistsException{TValue}"/>
        public static void EnsureNone<TValue>(this IOptional<TValue> optional)
        {
            optional.EnsureNone(value => new ValueExistsException<TValue>(value, $"{optional} has value"));
        }
    }
}