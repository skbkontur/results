using System;

namespace Kontur.Results
{
    /// <summary>
    /// Thrown when attempting to ensure None on an Optional that has a value.
    /// </summary>
    public class ValueExistsException : InvalidOperationException
    {
        internal ValueExistsException(string message)
            : base(message)
        {
        }
    }
}
