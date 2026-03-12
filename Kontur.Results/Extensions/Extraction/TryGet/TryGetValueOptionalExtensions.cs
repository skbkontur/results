using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.Plain;

namespace Kontur.Results
{
    public static class TryGetValueOptionalExtensions
    {
        /// <summary>Attempts to extract the value.</summary>
        /// <returns><c>true</c> if the optional contains a value and <paramref name="value"/> is set; <c>false</c> if None.</returns>
        [Pure]
        public static bool TryGetValue<TValue>(this Optional<TValue> optional, [MaybeNullWhen(false)] out TValue value)
        {
            return optional.Match(
                    () => EmptyContainer<TValue>.Instance,
                    val => new FilledContainer<TValue>(val))
                .TryGet(out value);
        }
    }
}
