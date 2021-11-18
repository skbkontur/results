using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal class UpcastOptionalExample<TResult>
    {
        internal UpcastOptionalExample(Optional<Child> optional, TResult result)
        {
            Optional = optional;
            Result = result;
        }

        internal Optional<Child> Optional { get; }

        internal TResult Result { get; }
    }
}
