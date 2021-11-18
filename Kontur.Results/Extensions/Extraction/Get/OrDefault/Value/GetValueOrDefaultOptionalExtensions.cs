using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class GetValueOrDefaultOptionalExtensions
    {
        [Pure]
        public static TValue? GetValueOrDefault<TValue>(this IOptional<TValue> optional)
        {
            return optional.GetValueOrElse(default(TValue));
        }
    }
}