using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    internal class SelectCase
    {
        internal SelectCase(IEnumerable<Optional<int>> args, Optional<int> result)
        {
            Args = args;
            Result = result;
        }

        internal IEnumerable<Optional<int>> Args { get; }

        internal Optional<int> Result { get; }

        public override bool Equals(object? obj)
        {
            return obj is SelectCase other && Equals(other);
        }

        private bool Equals(SelectCase other)
        {
            return Result.Equals(other.Result) && Args.SequenceEqual(other.Args);
        }

        public override int GetHashCode()
        {
            return (args: Args, Result).GetHashCode();
        }

        public override string ToString()
        {
            var serialized = string.Join("; ", Args.Select(a => $"({a})"));
            return $"Result: {Result}. Args: " + serialized;
        }
    }
}
