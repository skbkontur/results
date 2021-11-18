using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectOptionalValuePassInternal
    {
        internal static Optional<TResult> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, TResult> valueFactory)
        {
            return optional.MapValue(valueFactory);
        }

        internal static Task<Optional<TResult>> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Task<TResult>> valueFactory)
        {
            return optional.MapValue(valueFactory);
        }

        internal static ValueTask<Optional<TResult>> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<TResult>> valueFactory)
        {
            return KonturResultGlobalExtensions.MapValue(optional, valueFactory);
        }
    }
}