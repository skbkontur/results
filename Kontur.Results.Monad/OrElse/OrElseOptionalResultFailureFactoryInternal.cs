using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseOptionalResultFailureFactoryInternal
    {
        internal static Result<TFault, TValue> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<ResultFailure<TFault>> onNoneResultFactory)
        {
            return optional.Match(
                () =>
                {
                    Result<TFault, TValue> result = onNoneResultFactory();
                    return result;
                },
                Result<TFault, TValue>.Succeed);
        }

        internal static Task<Result<TFault, TValue>> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<Task<ResultFailure<TFault>>> onNoneResultFactory)
        {
            return optional.Match(
                async () =>
                {
                    Result<TFault, TValue> result = await onNoneResultFactory().ConfigureAwait(false);
                    return result;
                },
                value => Task.FromResult(Result<TFault, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TFault, TValue>> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<ValueTask<ResultFailure<TFault>>> onNoneResultFactory)
        {
            return optional.Match<ValueTask<Result<TFault, TValue>>>(
                async () =>
                {
                    Result<TFault, TValue> result = await onNoneResultFactory().ConfigureAwait(false);
                    return result;
                },
                value => new(Result<TFault, TValue>.Succeed(value)));
        }
    }
}