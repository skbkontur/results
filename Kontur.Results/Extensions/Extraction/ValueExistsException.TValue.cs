namespace Kontur.Results
{
    /// <summary>
    /// Typed variant of <see cref="ValueExistsException"/> that carries the value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public sealed class ValueExistsException<TValue> : ValueExistsException
    {
        internal ValueExistsException(TValue value, string message)
            : base(message)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value that was present.
        /// </summary>
        public TValue Value { get; }
    }
}
