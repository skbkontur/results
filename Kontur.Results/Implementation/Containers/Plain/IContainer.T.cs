using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.Plain
{
    internal interface IContainer<T>
    {
        [Pure]
        bool TryGet([MaybeNullWhen(false)] out T data);
    }
}
