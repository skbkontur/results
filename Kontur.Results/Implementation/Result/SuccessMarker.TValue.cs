namespace Kontur.Results
{
    public sealed class SuccessMarker<TValue>
    {
        internal SuccessMarker(TValue value)
        {
            this.Value = value;
        }

        internal TValue Value { get; }
    }
}
