using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.ResultValue
{
    internal interface IContainer<TFault, TValue>
    {
        [Pure]
        bool TryGet(
            [MaybeNullWhen(false)] out TValue value,
            [MaybeNullWhen(true)] out TFault fault);
    }
}
