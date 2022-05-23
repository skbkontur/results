using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue
{
    public static class SelectManyResultValueExtensions
    {
        public static Result<TFault, IEnumerable<TResult>> SelectMany<TItem, TFault, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IResult<TFault, TValue>> selector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            List<TResult> results = new();
            foreach (var item in collection)
            {
                var result = selector(item).Upcast();
                if (result.TryGetFault(out var fault, out var value))
                {
                    return Result<TFault, IEnumerable<TResult>>.Fail(fault);
                }

                results.Add(resultSelector(item, value));
            }

            return Result<TFault, IEnumerable<TResult>>.Succeed(results);
        }

        public static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TItem1, TItem2, TResult>(
            this IResult<TFault, IEnumerable<TItem1>> result,
            Func<TItem1, IEnumerable<TItem2>> collectionSelector,
            Func<TItem1, TItem2, TResult> resultSelector)
        {
            return result.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));
        }

        public static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
            this IResult<TFault, IEnumerable<TValue>> result,
            Func<TValue, IResult<TFault, TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return result
                .Upcast()
                .Match(
                    Result<TFault, IEnumerable<TResult>>.Fail,
                    values => SelectMany(values, collectionSelector, resultSelector));
        }
    }
}
