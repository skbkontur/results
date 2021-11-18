using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal class UpcastPlainExample<TResult>
    {
        internal UpcastPlainExample(Result<Child> source, TResult result)
        {
            Source = source;
            Result = result;
        }

        internal Result<Child> Source { get; }

        internal TResult Result { get; }
    }
}
