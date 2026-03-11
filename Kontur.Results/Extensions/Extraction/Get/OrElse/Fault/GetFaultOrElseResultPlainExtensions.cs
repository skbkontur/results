using System;
using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrElseResultPlainExtensions
    {
        /// <summary>Extracts the fault if failed, or computes a fallback lazily. The factory is only called when the result is successful, avoiding unnecessary allocation.</summary>
        public static TFault GetFaultOrElse<TFault>(this IResult<TFault> result, Func<TFault> onSuccessValueFactory)
        {
            return result.Match(fault => fault, onSuccessValueFactory);
        }

        /// <inheritdoc cref="GetFaultOrElse{TFault}(IResult{TFault}, Func{TFault})"/>
        [Pure]
        public static TFault GetFaultOrElse<TFault>(this IResult<TFault> result, TFault onSuccessValue)
        {
            return result.GetFaultOrElse(() => onSuccessValue);
        }
    }
}