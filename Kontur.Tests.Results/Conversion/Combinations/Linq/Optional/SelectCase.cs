using System.Collections.Generic;
using System.Linq;
using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional
{
    internal class SelectCase
    {
        internal SelectCase(IEnumerable<Optional<int>> args, Optional<int> result)
        {
            this.Args = args;
            this.Result = result;
        }

        internal IEnumerable<Optional<int>> Args { get; }

        internal Optional<int> Result { get; }

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
