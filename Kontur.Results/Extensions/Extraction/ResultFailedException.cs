using System;

namespace Kontur.Results
{
    /// <summary>
    /// Thrown when attempting to extract a value from a failed result.
    /// </summary>
    public class ResultFailedException : InvalidOperationException
    {
        internal ResultFailedException(string message)
            : base(message)
        {
        }
    }
}
