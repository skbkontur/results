using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.Plain
{
    internal sealed class EmptyContainer<T> : IContainer<T>
    {
        private static readonly Lazy<EmptyContainer<T>> Provider = new(() => new());

        private EmptyContainer()
        {
        }

        internal static IContainer<T> Instance => Provider.Value;

        [Pure]
        public bool TryGet([MaybeNullWhen(false)] out T data)
        {
            data = default;
            return false;
        }
    }
}
