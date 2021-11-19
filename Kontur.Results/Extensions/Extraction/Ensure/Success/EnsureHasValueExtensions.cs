using System;

namespace Kontur.Results
{
    public static class EnsureHasValueExtensions
    {
        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            optional.OnNone(() => throw exceptionFactory());
        }

        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            optional.EnsureHasValue(() => exception);
        }

        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional)
        {
            optional.EnsureHasValue(() => new ValueMissingException($"No value in {optional}"));
        }
    }
}