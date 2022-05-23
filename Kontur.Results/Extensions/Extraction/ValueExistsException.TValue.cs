namespace Kontur.Results
{
    public sealed class ValueExistsException<TValue> : ValueExistsException
    {
        internal ValueExistsException(TValue value, string message)
            : base(message)
        {
            this.Value = value;
        }

        public TValue Value { get; }
    }
}
