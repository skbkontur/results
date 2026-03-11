using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrElseResultValueExtensions
    {
        /// <summary>Extracts the fault if failed, or computes a fallback from the value. The factory receives the value, allowing value-dependent conversion.</summary>
        public static TFault GetFaultOrElse<TFault, TValue>(
            this IResult<TFault, TValue> result,
            Func<TValue, TFault> faultFactory)
        {
            return result.Match(fault => fault, faultFactory);
        }

        /// <inheritdoc cref="GetFaultOrElse{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, TFault})"/>
        public static TFault GetFaultOrElse<TFault, TValue>(this IResult<TFault, TValue> result, Func<TFault> faultFactory)
        {
            return result.GetFaultOrElse(_ => faultFactory());
        }

        /// <inheritdoc cref="GetFaultOrElse{TFault, TValue}(IResult{TFault, TValue}, Func{TValue, TFault})"/>
        [Pure]
        public static TFault GetFaultOrElse<TFault, TValue>(this IResult<TFault, TValue> result, TFault fault)
        {
            return result.GetFaultOrElse(() => fault);
        }
    }
}