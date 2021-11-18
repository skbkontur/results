using System.Collections.Generic;
using System.Linq;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq
{
    internal static class KPermutationOfBool
    {
        private static readonly bool[] Set = { false, true };

        internal static IEnumerable<IEnumerable<bool>> Create(int size)
        {
            return size < 1
                ? Enumerable.Empty<IEnumerable<bool>>()
                : CreateInternal(size);
        }

        private static IEnumerable<IEnumerable<bool>> CreateInternal(int size)
        {
            if (size < 1)
            {
                return new[] { Enumerable.Empty<bool>() };
            }

            return
                from permutation in CreateInternal(size - 1)
                from x in Set
                select permutation.Append(x);
        }
    }
}