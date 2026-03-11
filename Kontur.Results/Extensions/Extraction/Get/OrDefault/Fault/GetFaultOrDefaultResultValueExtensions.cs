using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrDefaultResultValueExtensions
    {
        /// <summary>Extracts the fault if failed.</summary>
        /// <returns>The contained fault, or <c>default(TFault)</c> if successful.</returns>
        [Pure]
        public static TFault? GetFaultOrDefault<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetFaultOrElse(default(TFault));
        }
    }
}