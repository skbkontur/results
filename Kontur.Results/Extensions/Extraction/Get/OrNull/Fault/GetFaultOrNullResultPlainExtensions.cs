using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrNullResultPlainExtensions
    {
        /// <summary>Extracts the fault as nullable. Constrained to value types where <c>default</c> may be a valid fault.</summary>
        /// <returns>The contained fault, or <c>null</c> if successful.</returns>
        [Pure]
        public static TFault? GetFaultOrNull<TFault>(this IResult<TFault> result)
            where TFault : struct
        {
            return result.Match<TFault?>(fault => fault, () => null);
        }
    }
}