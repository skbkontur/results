using System;

namespace Kontur.Results
{
    public static class EnsureNoneExtensions
    {
        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Func<TValue, Exception> exceptionFactory)
        {
            optional.OnSome(value => throw exceptionFactory(value));
        }

        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            optional.EnsureNone(_ => exceptionFactory());
        }

        public static void EnsureNone<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            optional.EnsureNone(() => exception);
        }

        public static void EnsureNone<TValue>(this IOptional<TValue> optional)
        {
            optional.EnsureNone(new ValueExistsException($"{optional} has value"));
        }
    }
}