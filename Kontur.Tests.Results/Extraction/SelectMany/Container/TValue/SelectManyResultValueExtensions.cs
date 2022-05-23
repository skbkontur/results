using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.Container.TValue
{
    public static class SelectManyResultValueExtensions
    {
        public static IEnumerable<TResult> SelectMany<TFault, TValue, TItem, TResult>(
            this IResult<TFault, TValue> result,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return result.GetValues().SelectMany(collectionSelector, resultSelector);
        }

        public static IEnumerable<TResult> SelectMany<TItem, TFault, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IResult<TFault, TValue>> selector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.SelectMany(value => selector(value).GetValues(), resultSelector);
        }
    }
}
