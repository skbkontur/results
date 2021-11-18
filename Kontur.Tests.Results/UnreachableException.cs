using NUnit.Framework;

namespace Kontur.Tests.Results
{
    internal class UnreachableException : AssertionException
    {
        public UnreachableException()
            : base("Branch should be unreachable")
        {
        }
    }
}
