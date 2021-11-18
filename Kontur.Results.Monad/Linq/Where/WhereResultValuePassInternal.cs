using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class WhereResultValuePassInternal
    {
        internal static Result<TFault, TValue> Where<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Result<TFault>> predicate)
        {
            return result.Match(
                result,
                value => predicate(value).Then(result));
        }

        internal static Task<Result<TFault, TValue>> Where<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, Task<Result<TFault>>> predicate)
        {
            return result.Match(
                () => Task.FromResult(result),
                async value =>
                {
                    var checkResult = await predicate(value).ConfigureAwait(false);
                    return checkResult.Then(result);
                });
        }

        internal static ValueTask<Result<TFault, TValue>> Where<TFault, TValue>(
            Result<TFault, TValue> result,
            Func<TValue, ValueTask<Result<TFault>>> predicate)
        {
            return result.Match<ValueTask<Result<TFault, TValue>>>(
                () => new(result),
                async value =>
                {
                    var checkResult = await predicate(value).ConfigureAwait(false);
                    return checkResult.Then(result);
                });
        }
    }
}