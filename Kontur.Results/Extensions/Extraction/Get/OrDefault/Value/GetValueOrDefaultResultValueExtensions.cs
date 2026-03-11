using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrDefaultResultValueExtensions
    {
        /// <summary>Extracts the value if successful.</summary>
        /// <returns>The contained value, or <c>default(TValue)</c> if failed.</returns>
        [Pure]
        public static TValue? GetValueOrDefault<TFault, TValue>(this IResult<TFault, TValue> result)
        {
            return result.GetValueOrElse(default(TValue));
        }
    }
}