using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrElseOptionalExtensions
    {
        public static TValue GetValueOrElse<TValue>(this IOptional<TValue> optional, Func<TValue> onNoneValueFactory)
        {
            return optional.Match(onNoneValueFactory, value => value);
        }

        [Pure]
        public static TValue GetValueOrElse<TValue>(this IOptional<TValue> optional, TValue onNoneValue)
        {
            return optional.GetValueOrElse(() => onNoneValue);
        }
    }
}