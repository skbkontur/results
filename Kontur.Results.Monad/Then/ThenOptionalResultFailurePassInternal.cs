using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenOptionalResultFailurePassInternal
    {
        internal static Optional<TValue> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, ResultFailure<TFault>> onSomeResultFactory)
        {
            _ = optional;
            _ = onSomeResultFactory;
            return Optional<TValue>.None();
        }

        internal static Task<Optional<TValue>> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, Task<ResultFailure<TFault>>> onSomeResultFactory)
        {
            _ = optional;
            _ = onSomeResultFactory;
            return Task.FromResult(Optional<TValue>.None());
        }

        internal static ValueTask<Optional<TValue>> Then<TValue, TFault>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<ResultFailure<TFault>>> onSomeResultFactory)
        {
            _ = optional;
            _ = onSomeResultFactory;
            return new(Optional<TValue>.None());
        }
    }
}