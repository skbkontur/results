using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.ResultValue
{
    internal sealed class SuccessContainer<TFault, TValue> : IContainer<TFault, TValue>
    {
        private readonly TValue storedValue;

        internal SuccessContainer(TValue value)
        {
            this.storedValue = value;
        }

        [Pure]
        public bool TryGet(
            out TValue value,
            [MaybeNullWhen(true)] out TFault fault)
        {
            fault = default;
            value = this.storedValue;
            return true;
        }
    }
}
