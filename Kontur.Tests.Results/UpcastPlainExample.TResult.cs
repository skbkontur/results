using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal class UpcastPlainExample<TResult>
    {
        internal UpcastPlainExample(Result<Child> source, TResult result)
        {
            this.Source = source;
            this.Result = result;
        }

        internal Result<Child> Source { get; }

        internal TResult Result { get; }
    }
}
