using System;
using System.Threading.Tasks;
using Kontur.Results;

namespace Kontur.Tests.Results.Inheritance
{
    public static class StringFaultResultMapFaultExtensions
    {
        public static Task<Result<TResult, TValue>> MapFault<TValue, TResult>(
            this Task<StringFaultResult<TValue>> resultTask,
            TResult result)
        {
            return MapFault(resultTask, () => result);
        }

        public static Task<Result<TResult, TValue>> MapFault<TValue, TResult>(
            this Task<StringFaultResult<TValue>> resultTask,
            Func<TResult> resultFactory)
        {
            return MapFault(resultTask, _ => resultFactory());
        }

        public static async Task<Result<TResult, TValue>> MapFault<TValue, TResult>(
            this Task<StringFaultResult<TValue>> resultTask,
            Func<StringFault, TResult> projection)
        {
            var result = await resultTask.ConfigureAwait(false);
            return result.MapFault(projection);
        }
    }
}
