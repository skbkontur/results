using System.Diagnostics.Contracts;

namespace Kontur.Results
{
    public static class UpcastOptionalExtensions
    {
        [Pure]
        public static Optional<TResult> Upcast<TResult>(this IOptional<TResult> optional)
        {
            return optional.Match(Optional<TResult>.None, Optional<TResult>.Some);
        }
    }
}
