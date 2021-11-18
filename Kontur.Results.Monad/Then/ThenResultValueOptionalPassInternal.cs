using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultValueOptionalPassInternal
    {
        internal static Optional<TResult> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Optional<TResult>> onSuccessOptionalFactory)
        {
            return result.Match(Optional<TResult>.None, onSuccessOptionalFactory);
        }

        internal static Task<Optional<TResult>> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Optional<TResult>>> onSuccessOptionalFactory)
        {
            return result.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                onSuccessOptionalFactory);
        }

        internal static ValueTask<Optional<TResult>> Then<TFault, TValue, TResult>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Optional<TResult>>> onSuccessOptionalFactory)
        {
            return result.Match(
                () => new(Optional<TResult>.None()),
                onSuccessOptionalFactory);
        }
    }
}