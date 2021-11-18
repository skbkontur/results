using System;
using System.Threading.Tasks;

namespace Kontur.Results
{
    internal static partial class OrElseOptionalResultPlainFactoryInternal
    {
        internal static Result<TFault> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<Result<TFault>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                () => Result<TFault>.Succeed());
        }

        internal static Task<Result<TFault>> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<Task<Result<TFault>>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                () => Task.FromResult(Result<TFault>.Succeed()));
        }

        internal static ValueTask<Result<TFault>> OrElse<TValue, TFault>(
            Optional<TValue> optional,
            Func<ValueTask<Result<TFault>>> onNoneResultFactory)
        {
            return optional.Match(
                onNoneResultFactory,
                () => new(Result<TFault>.Succeed()));
        }
    }
}