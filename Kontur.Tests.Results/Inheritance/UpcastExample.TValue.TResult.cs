namespace Kontur.Tests.Results.Inheritance
{
    internal class UpcastExample<TValue, TResult>
    {
        internal UpcastExample(StringFaultResult<TValue> source, TResult result)
        {
            Source = source;
            Result = result;
        }

        internal StringFaultResult<TValue> Source { get; }

        internal TResult Result { get; }
    }
}
