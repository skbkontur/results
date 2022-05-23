using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.Container.Optional
{
    public static class SelectManyOptionalExtensions
    {
        public static IEnumerable<TResult> SelectMany<TValue, TItem, TResult>(
            this IOptional<TValue> optional,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return optional.GetValues().SelectMany(collectionSelector, resultSelector);
        }

        public static IEnumerable<TResult> SelectMany<TItem, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IOptional<TValue>> optionSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.SelectMany(value => optionSelector(value).GetValues(), resultSelector);
        }
    }
}
