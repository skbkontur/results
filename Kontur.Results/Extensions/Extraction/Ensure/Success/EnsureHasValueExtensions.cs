using System;

namespace Kontur.Results
{
    public static class EnsureHasValueExtensions
    {
        /// <summary>Throws if the optional is None.</summary>
        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional, Func<Exception> exceptionFactory)
        {
            optional.OnNone(() => throw exceptionFactory());
        }

        /// <inheritdoc cref="EnsureHasValue{TValue}(IOptional{TValue}, Func{Exception})"/>
        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional, Exception exception)
        {
            optional.EnsureHasValue(() => exception);
        }

        /// <inheritdoc cref="EnsureHasValue{TValue}(IOptional{TValue}, Func{Exception})"/>
        /// <exception cref="ValueMissingException"/>
        public static void EnsureHasValue<TValue>(this IOptional<TValue> optional)
        {
            optional.EnsureHasValue(() => new ValueMissingException($"No value in {optional}"));
        }
    }
}