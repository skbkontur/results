using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Result.SelectCaseGenerators
{
    internal class SelectCase
    {
        internal SelectCase(IEnumerable<Result<string, int>> args, Result<string, int> result)
        {
            this.Args = args;
            this.Result = result;
        }

        internal IEnumerable<Result<string, int>> Args { get; }

        internal Result<string, int> Result { get; }

        public override bool Equals(object? obj)
        {
            return obj is SelectCase other && this.Equals(other);
        }

        private bool Equals(SelectCase other)
        {
            return this.Result.Equals(other.Result) && this.Args.SequenceEqual(other.Args);
        }

        public override int GetHashCode()
        {
            return (args: this.Args, this.Result).GetHashCode();
        }

        public override string ToString()
        {
            var serialized = string.Join("; ", this.Args.Select(a => $"({a})"));
            return $"Result: {this.Result}. Args: " + serialized;
        }
    }
}
