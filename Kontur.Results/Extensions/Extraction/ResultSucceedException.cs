using System;

namespace Kontur.Results
{
    /// <summary>
    /// Thrown when attempting to extract a fault from a successful result.
    /// </summary>
    public class ResultSucceedException : InvalidOperationException
    {
        internal ResultSucceedException(string message)
            : base(message)
        {
        }
    }
}
