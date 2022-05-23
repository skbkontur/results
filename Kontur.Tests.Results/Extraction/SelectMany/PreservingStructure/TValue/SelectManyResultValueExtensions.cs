using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.PreservingStructure.TValue
{
    public static class SelectManyResultValueExtensions
    {
        public static IEnumerable<Result<TFault, TResult>> SelectMany<TItem, TFault, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IResult<TFault, TValue>> selector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.Select(item => selector(item).MapValue(value => resultSelector(item, value)));
        }

        public static IEnumerable<Result<TFault, TResult>> SelectMany<TItem, TFault, TValue, TResult>(
            this IEnumerable<IResult<TFault, TItem>> collection,
            Func<TItem, IResult<TFault, TValue>> selector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return collection.Select(result => result.Upcast().Match(
                Result<TFault, TResult>.Fail,
                item => selector(item).MapValue(value => resultSelector(item, value))));
        }

        public static IEnumerable<Result<TFault, IEnumerable<TResult>>> SelectMany<TValue, TFault, TItem, TResult>(
            this IEnumerable<IResult<TFault, TValue>> collection,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return collection
                .Select(result => result
                    .MapValue(item => collectionSelector(item)
                        .Select(value => resultSelector(item, value))));
        }

        public static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
            this IResult<TFault, TValue> result,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return result.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
        }

        public static Result<TFault, IEnumerable<TResult>> SelectMany<TFault, TValue, TItem, TResult>(
            this IResult<TFault, IEnumerable<TValue>> result,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return result.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));
        }

        public static Result<TFault1, IEnumerable<Result<TFault2, TResult>>> SelectMany<TFault1, TItem, TFault2, TValue, TResult>(
            this IResult<TFault1, IEnumerable<TItem>> result,
            Func<TItem, IResult<TFault2, TValue>> collectionSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            return result.MapValue(values => values.Select(item => collectionSelector(item).MapValue(value => resultSelector(item, value))));
        }
    }
}
