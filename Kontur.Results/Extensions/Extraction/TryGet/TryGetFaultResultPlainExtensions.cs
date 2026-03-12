using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.Plain;

namespace Kontur.Results
{
    public static class TryGetFaultResultPlainExtensions
    {
        /// <summary>Attempts to extract the fault.</summary>
        /// <returns><c>true</c> if the result is a failure and <paramref name="fault"/> is set; <c>false</c> if successful.</returns>
        [Pure]
        public static bool TryGetFault<TFault>(this Result<TFault> result, [MaybeNullWhen(false)] out TFault fault)
        {
            return result.Match(f => new FilledContainer<TFault>(f), () => EmptyContainer<TFault>.Instance)
                .TryGet(out fault);
        }
    }
}
