using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators
{
    internal class SelectCase
    {
        internal SelectCase(IEnumerable<Result<string, int>> args, Result<string, int> result)
        {
            Args = args;
            Result = result;
        }

        internal IEnumerable<Result<string, int>> Args { get; }

        internal Result<string, int> Result { get; }

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
