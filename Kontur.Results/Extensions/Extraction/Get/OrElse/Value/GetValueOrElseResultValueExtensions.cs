using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrElseResultValueExtensions
    {
        public static TValue GetValueOrElse<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TFault, TValue> valueFactory)
        {
            return result.Match(valueFactory, value => value);
        }

        public static TValue GetValueOrElse<TFault, TValue>(this IResult<TFault, TValue> result, Func<TValue> valueFactory)
        {
            return result.GetValueOrElse(_ => valueFactory());
        }

        [Pure]
        public static TValue GetValueOrElse<TFault, TValue>(this IResult<TFault, TValue> result, TValue value)
        {
            return result.GetValueOrElse(() => value);
        }
    }
}