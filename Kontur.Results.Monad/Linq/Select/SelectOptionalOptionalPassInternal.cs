using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class SelectOptionalOptionalPassInternal
    {
        internal static Optional<TResult> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Optional<TResult>> optionalFactory)
        {
            return optional.Then(optionalFactory);
        }

        internal static Task<Optional<TResult>> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, Task<Optional<TResult>>> optionalFactory)
        {
            return optional.Then(optionalFactory);
        }

        internal static ValueTask<Optional<TResult>> Select<TValue, TResult>(
            Optional<TValue> optional,
            Func<TValue, ValueTask<Optional<TResult>>> optionalFactory)
        {
            return optional.Then(optionalFactory);
        }
    }
}