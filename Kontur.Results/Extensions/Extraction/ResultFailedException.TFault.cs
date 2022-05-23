namespace Kontur.Results
{
    public sealed class ResultFailedException<TFault> : ResultFailedException
    {
        internal ResultFailedException(TFault fault, string message)
            : base(message)
        {
            this.Fault = fault;
        }

        public TFault Fault { get; }
    }
}
