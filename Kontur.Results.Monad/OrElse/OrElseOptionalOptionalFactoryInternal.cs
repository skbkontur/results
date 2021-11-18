using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseOptionalOptionalFactoryInternal
    {
        internal static Optional<TValue> OrElse<TValue>(
            Optional<TValue> optional,
            Func<Optional<TValue>> onNoneOptionalFactory)
        {
            return optional.Match(onNoneOptionalFactory, Optional<TValue>.Some);
        }

        internal static Task<Optional<TValue>> OrElse<TValue>(
            Optional<TValue> optional,
            Func<Task<Optional<TValue>>> onNoneOptionalFactory)
        {
            return optional.Match(
                onNoneOptionalFactory,
                value => Task.FromResult(Optional<TValue>.Some(value)));
        }

        internal static ValueTask<Optional<TValue>> OrElse<TValue>(
            Optional<TValue> optional,
            Func<ValueTask<Optional<TValue>>> onNoneOptionalFactory)
        {
            return optional.Match(
                onNoneOptionalFactory,
                value => new(Optional<TValue>.Some(value)));
        }
    }
}