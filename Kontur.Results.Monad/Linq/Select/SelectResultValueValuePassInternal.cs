using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectResultValueValuePassInternal
    {
        internal static Result<TFault, TResult> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, TResult> valueFactory)
        {
            return result.MapValue(valueFactory);
        }

        internal static Task<Result<TFault, TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Task<TResult>> valueFactory)
        {
            return result.MapValue(valueFactory);
        }

        internal static ValueTask<Result<TFault, TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<TResult>> valueFactory)
        {
            return KonturResultGlobalExtensions.MapValue(result, valueFactory);
        }
    }
}