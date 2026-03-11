namespace Kontur.Results
{
    /// <summary>
    /// Typed variant of <see cref="ResultFailedException"/> that carries the fault value.
    /// </summary>
    /// <typeparam name="TFault">The type of the fault.</typeparam>
    public sealed class ResultFailedException<TFault> : ResultFailedException
    {
        internal ResultFailedException(TFault fault, string message)
            : base(message)
        {
            this.Fault = fault;
        }

        /// <summary>
        /// Gets the fault value that caused the failure.
        /// </summary>
        public TFault Fault { get; }
    }
}
