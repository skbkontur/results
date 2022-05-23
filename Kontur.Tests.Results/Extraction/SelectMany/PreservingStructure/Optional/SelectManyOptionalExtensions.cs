using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.Optional
{
    public static class SelectManyOptionalExtensions
    {
        public static IEnumerable<Optional<TResult>> SelectMany<TItem, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IOptional<TValue>> optionalSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.Select(item => optionalSelector(item).MapValue(value => resultSelector(item, value)));
        }

        public static IEnumerable<Optional<TResult>> SelectMany<TItem, TValue, TResult>(
            this IEnumerable<IOptional<TItem>> collection,
            Func<TItem, IOptional<TValue>> optionalSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.Select(optional => optional.Upcast().Match(
                Optional<TResult>.None,
                item => optionalSelector(item).MapValue(value => resultSelector(item, value))));
        }

        public static IEnumerable<Optional<IEnumerable<TResult>>> SelectMany<TValue, TItem, TResult>(
            this IEnumerable<IOptional<TValue>> collection,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return collection
                .Select(optional => optional
                    .MapValue(item => collectionSelector(item)
                        .Select(value => resultSelector(item, value))));
        }

        public static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
            this IOptional<TValue> optional,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return optional.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
        }

        public static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
            this IOptional<IEnumerable<TValue>> optional,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return optional.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));
        }

        public static Optional<IEnumerable<Optional<TResult>>> SelectMany<TItem, TValue, TResult>(
            this IOptional<IEnumerable<TItem>> optional,
            Func<TItem, IOptional<TValue>> collectionSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return optional.MapValue(values => values.Select(item => collectionSelector(item).MapValue(value => resultSelector(item, value))));
        }
    }
}
