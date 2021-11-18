namespace Kontur.Results
{
    public sealed class SuccessMarker<TValue>
    {
        internal SuccessMarker(TValue value)
        {
            Value = value;
        }

        internal TValue Value { get; }
    }
}
