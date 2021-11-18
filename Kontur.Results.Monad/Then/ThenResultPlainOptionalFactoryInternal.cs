using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class ThenResultPlainOptionalFactoryInternal
    {
        internal static Optional<TValue> Then<TFault, TValue>(
            Result<TFault> result,
            Func<Optional<TValue>> onSuccessOptionalFactory)
        {
            return result.Match(Optional<TValue>.None, onSuccessOptionalFactory);
        }

        internal static Task<Optional<TValue>> Then<TFault, TValue>(
            Result<TFault> result,
            Func<Task<Optional<TValue>>> onSuccessOptionalFactory)
        {
            return result.Match(
                () => Task.FromResult(Optional<TValue>.None()),
                onSuccessOptionalFactory);
        }

        internal static ValueTask<Optional<TValue>> Then<TFault, TValue>(
            Result<TFault> result,
            Func<ValueTask<Optional<TValue>>> onSuccessOptionalFactory)
        {
            return result.Match(
                () => new(Optional<TValue>.None()),
                onSuccessOptionalFactory);
        }
    }
}