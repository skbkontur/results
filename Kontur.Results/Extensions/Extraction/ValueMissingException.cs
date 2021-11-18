using System;

namespace Kontur.Results
{
    public sealed class ValueMissingException : InvalidOperationException
    {
        internal ValueMissingException(string message)
            : base(message)
        {
        }
    }
}
