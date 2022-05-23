using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.TValue.Using
{
    public static class SelectManyResultValueExtensions
    {
        public static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
            this IResult<TFault, TValue> result,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return result.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
        }
    }
}
