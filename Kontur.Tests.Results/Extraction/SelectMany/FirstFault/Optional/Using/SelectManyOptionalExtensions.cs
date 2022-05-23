using System;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany.FirstFault.Optional.Using
{
    public static class SelectManyOptionalExtensions
    {
        public static Optional<IEnumerable<TResult>> SelectMany<TValue, TItem, TResult>(
            this IOptional<TValue> optional,
            Func<TValue, IEnumerable<TItem>> collectionSelector,
            Func<TValue, TItem, TResult> resultSelector)
        {
            return optional.MapValue(value => collectionSelector(value).Select(item => resultSelector(value, item)));
        }
    }
}
