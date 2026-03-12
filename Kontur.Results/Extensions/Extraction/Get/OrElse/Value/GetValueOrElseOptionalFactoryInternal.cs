using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class GetValueOrElseOptionalFactoryInternal
    {
        internal static TValue GetValueOrElse<TValue>(
            IOptional<TValue> optional,
            Func<TValue> valueFactory)
        {
            return optional.Match(valueFactory, value => value);
        }

        internal static async Task<TValue> GetValueOrElse<TValue>(
            IOptional<TValue> optional,
            Func<Task<TValue>> valueFactory)
        {
            return await optional.Match(valueFactory, value => Task.FromResult(value)).ConfigureAwait(false);
        }

        internal static async ValueTask<TValue> GetValueOrElse<TValue>(
            IOptional<TValue> optional,
            Func<ValueTask<TValue>> valueFactory)
        {
            return await optional.Match<ValueTask<TValue>>(valueFactory, value => new(value)).ConfigureAwait(false);
        }
    }
}
