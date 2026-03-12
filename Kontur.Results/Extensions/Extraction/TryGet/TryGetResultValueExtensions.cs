using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using Kontur.Results.Containers.ResultValue;

namespace Kontur.Results
{
    public static class TryGetResultValueExtensions
    {
        /// <summary>Attempts to extract the fault.</summary>
        /// <returns><c>true</c> if the result is a failure and <paramref name="fault"/> is set; <c>false</c> if successful.</returns>
        [Pure]
        public static bool TryGetFault<TFault, TValue>(this Result<TFault, TValue> result, [MaybeNullWhen(false)] out TFault fault)
        {
            return !result.TryGetValue(out _, out fault);
        }

        /// <summary>Attempts to extract the fault, providing both fault and value.</summary>
        /// <returns><c>true</c> if the result is a failure and <paramref name="fault"/> is set; <c>false</c> if successful and <paramref name="value"/> is set.</returns>
        [Pure]
        public static bool TryGetFault<TFault, TValue>(
            this Result<TFault, TValue> result,
            [MaybeNullWhen(false)] out TFault fault,
            [MaybeNullWhen(true)] out TValue value)
        {
            return !result.TryGetValue(out value, out fault);
        }

        /// <summary>Attempts to extract the value.</summary>
        /// <returns><c>true</c> if successful and <paramref name="value"/> is set; <c>false</c> if the result is a failure.</returns>
        [Pure]
        public static bool TryGetValue<TFault, TValue>(this Result<TFault, TValue> result, [MaybeNullWhen(false)] out TValue value)
        {
            return result.TryGetValue(out value, out _);
        }

        /// <summary>Attempts to extract the value, providing both value and fault.</summary>
        /// <returns><c>true</c> if successful and <paramref name="value"/> is set; <c>false</c> if failed and <paramref name="fault"/> is set.</returns>
        [Pure]
        public static bool TryGetValue<TFault, TValue>(
            this Result<TFault, TValue> result,
            [MaybeNullWhen(false)] out TValue value,
            [MaybeNullWhen(true)] out TFault fault)
        {
            return result.Match<IContainer<TFault, TValue>>(
                    f => new FailureContainer<TFault, TValue>(f),
                    val => new SuccessContainer<TFault, TValue>(val))
                .TryGet(out value, out fault);
        }
    }
}
