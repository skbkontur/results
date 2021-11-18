using System.Collections.Generic;

namespace Kontur.Results.SourceGenerator.Code
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source)
        {
            foreach (var element in source)
            {
                if (element != null)
                {
                    yield return element;
                }
            }
        }
    }
}
