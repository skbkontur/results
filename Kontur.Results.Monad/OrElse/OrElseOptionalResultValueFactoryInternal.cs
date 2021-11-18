using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseOptionalResultValueFactoryInternal
    {
        internal static Result<TFault, TValue> OrElse<TFault, TValue>(
            Optional<TValue> optional,
            Func<Result<TFault, TValue>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                Result<TFault, TValue>.Succeed);
        }

        internal static Task<Result<TFault, TValue>> OrElse<TFault, TValue>(
            Optional<TValue> optional,
            Func<Task<Result<TFault, TValue>>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                value => Task.FromResult(Result<TFault, TValue>.Succeed(value)));
        }

        internal static ValueTask<Result<TFault, TValue>> OrElse<TFault, TValue>(
            Optional<TValue> optional,
            Func<ValueTask<Result<TFault, TValue>>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                value => new(Result<TFault, TValue>.Succeed(value)));
        }
    }
}