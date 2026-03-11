using System;

namespace Kontur.Results
{
    /// <summary>
    /// Thrown when attempting to extract a value from None.
    /// </summary>
    public sealed class ValueMissingException : InvalidOperationException
    {
        internal ValueMissingException(string message)
            : base(message)
        {
        }
    }
}
