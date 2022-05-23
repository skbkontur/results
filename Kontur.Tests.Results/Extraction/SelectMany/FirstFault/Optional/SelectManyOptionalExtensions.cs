using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional
{
    public static class SelectManyOptionalExtensions
    {
        public static Optional<IEnumerable<TResult>> SelectMany<TItem, TValue, TResult>(
            this IEnumerable<TItem> collection,
            Func<TItem, IOptional<TValue>> optionalSelector,
            Func<TItem, TValue, TResult> resultSelector)
        {
            List<TResult> results = new();
            foreach (var item in collection)
            {
                var optional = optionalSelector(item).Upcast();
                if (!optional.TryGetValue(out var value))
                {
                    return Optional<IEnumerable<TResult>>.None();
                }

                results.Add(resultSelector(item, value));
            }

            return Optional<IEnumerable<TResult>>.Some(results);
        }

        public static Optional<IEnumerable<TResult>> SelectMany<TItem1, TItem2, TResult>(
            this IOptional<IEnumerable<TItem1>> optional,
            Func<TItem1, IEnumerable<TItem2>> collectionSelector,
            Func<TItem1, TItem2, TResult> resultSelector)
        {
            return optional.MapValue(values => values.SelectMany(value => collectionSelector(value).Select(item => resultSelector(value, item))));
        }

        public static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
            this IOptional<IEnumerable<TValue>> optional,
            Func<TValue, IOptional<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return optional
                .Upcast()
                .Match(
                    Optional<IEnumerable<TResult>>.None,
                    values => SelectMany(values, collectionSelector, resultSelector));
        }
    }
}
