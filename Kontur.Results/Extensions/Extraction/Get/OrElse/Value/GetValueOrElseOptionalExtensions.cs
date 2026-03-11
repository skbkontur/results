using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrElseOptionalExtensions
    {
        /// <summary>Extracts the value if present, or computes a fallback lazily. The factory is only called when the optional is None, avoiding unnecessary allocation.</summary>
        public static TValue GetValueOrElse<TValue>(this IOptional<TValue> optional, Func<TValue> onNoneValueFactory)
        {
            return optional.Match(onNoneValueFactory, value => value);
        }

        /// <inheritdoc cref="GetValueOrElse{TValue}(IOptional{TValue}, Func{TValue})"/>
        [Pure]
        public static TValue GetValueOrElse<TValue>(this IOptional<TValue> optional, TValue onNoneValue)
        {
            return optional.GetValueOrElse(() => onNoneValue);
        }
    }
}