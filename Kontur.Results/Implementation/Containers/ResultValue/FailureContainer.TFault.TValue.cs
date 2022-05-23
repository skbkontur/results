using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.ResultValue
{
    internal sealed class FailureContainer<TFault, TValue> : IContainer<TFault, TValue>
    {
        private readonly TFault storedFault;

        internal FailureContainer(TFault fault)
        {
            this.storedFault = fault;
        }

        [Pure]
        public bool TryGet(
            [MaybeNullWhen(false)] out TValue value,
            out TFault fault)
        {
            fault = this.storedFault;
            value = default;
            return false;
        }
    }
}
