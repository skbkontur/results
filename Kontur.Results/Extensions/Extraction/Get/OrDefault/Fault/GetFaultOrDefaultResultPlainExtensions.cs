using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetFaultOrDefaultResultPlainExtensions
    {
        /// <summary>Extracts the fault if failed.</summary>
        /// <returns>The contained fault, or <c>default(TFault)</c> if successful.</returns>
        [Pure]
        public static TFault? GetFaultOrDefault<TFault>(this IResult<TFault> result)
        {
            return result.GetFaultOrElse(default(TFault));
        }
    }
}