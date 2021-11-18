using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenOptionalOptionalPassInternal
    {
        internal static Optional<TResult> Then<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Optional<TResult>> onSomeOptionalFactory)
        {
            return optional.Match(Optional<TResult>.None, onSomeOptionalFactory);
        }

        internal static Task<Optional<TResult>> Then<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Task<Optional<TResult>>> onSomeOptionalFactory)
        {
            return optional.Match(
                () => Task.FromResult(Optional<TResult>.None()),
                onSomeOptionalFactory);
        }

        internal static ValueTask<Optional<TResult>> Then<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<Optional<TResult>>> onSomeOptionalFactory)
        {
            return optional.Match(
                () => new(Optional<TResult>.None()),
                onSomeOptionalFactory);
        }
    }
}