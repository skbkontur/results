using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Extraction.SelectMany
{
    internal static class WrapToSequenceEqualsExtensions
    {
        internal static Optional<IEnumerable<int>> Wrap(this Optional<IEnumerable<int>> optional) =>
            optional
                .MapValue(value => new EnumerableWrapper(value))
                .Upcast<IEnumerable<int>>();

        internal static Result<string, IEnumerable<int>> Wrap(this Result<string, IEnumerable<int>> result) =>
            result
                .MapValue(value => new EnumerableWrapper(value))
                .Upcast<string, IEnumerable<int>>();

        private class EnumerableWrapper : IEnumerable<int>
        {
            private readonly IEnumerable<int> implementation;

            internal EnumerableWrapper(IEnumerable<int> implementation) => this.implementation = implementation;

            public IEnumerator<int> GetEnumerator() => implementation.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => implementation.GetEnumerator();

            public override int GetHashCode() => 0;

            public override bool Equals(object? obj) =>
                obj is IEnumerable<int> other && implementation.SequenceEqual(other);
        }
    }
}
