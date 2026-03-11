using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrElseResultValueExtensions
    {
        /// <summary>Extracts the value if successful, or computes a fallback from the fault. The factory receives the fault, allowing error-dependent recovery.</summary>
        public static TValue GetValueOrElse<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TFault, TValue> valueFactory)
        {
            return result.Match(valueFactory, value => value);
        }

        /// <inheritdoc cref="GetValueOrElse{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, TValue})"/>
        public static TValue GetValueOrElse<TFault, TValue>(this IResult<TFault, TValue> result, Func<TValue> valueFactory)
        {
            return result.GetValueOrElse(_ => valueFactory());
        }

        /// <inheritdoc cref="GetValueOrElse{TFault, TValue}(IResult{TFault, TValue}, Func{TFault, TValue})"/>
        [Pure]
        public static TValue GetValueOrElse<TFault, TValue>(this IResult<TFault, TValue> result, TValue value)
        {
            return result.GetValueOrElse(() => value);
        }
    }
}