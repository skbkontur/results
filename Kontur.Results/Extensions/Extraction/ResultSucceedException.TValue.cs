namespace Kontur.Results
{
    /// <summary>
    /// Typed variant of <see cref="ResultSucceedException"/> that carries the success value.
    /// </summary>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    public sealed class ResultSucceedException<TValue> : ResultSucceedException
    {
        internal ResultSucceedException(TValue value, string message)
            : base(message)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the success value.
        /// </summary>
        public TValue Value { get; }
    }
}
