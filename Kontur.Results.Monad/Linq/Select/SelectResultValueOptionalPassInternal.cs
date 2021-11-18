using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectResultValueOptionalPassInternal
    {
        internal static Optional<TResult> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Optional<TResult>> optionalFactory)
        {
            return result.Then(optionalFactory);
        }

        internal static Task<Optional<TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Optional<TResult>>> optionalFactory)
        {
            return result.Then(optionalFactory);
        }

        internal static ValueTask<Optional<TResult>> Select<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Optional<TResult>>> optionalFactory)
        {
            return result.Then(optionalFactory);
        }
    }
}