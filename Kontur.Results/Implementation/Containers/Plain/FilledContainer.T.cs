using System.Diagnostics.Contracts;

namespace Kontur.Results.Containers.Plain
{
    internal sealed class FilledContainer<T> : IContainer<T>
    {
        private readonly T stored;

        internal FilledContainer(T data)
        {
            stored = data;
        }

        [Pure]
        public bool TryGet(out T data)
        {
            data = stored;
            return true;
        }
    }
}
