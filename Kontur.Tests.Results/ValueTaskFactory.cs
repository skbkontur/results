using System.Threading.Tasks;

namespace Kontur.Tests.Results
{
    internal static class ValueTaskFactory
    {
        internal static ValueTask<T> Create<T>(T result)
        {
            return new(result);
        }
    }
}
