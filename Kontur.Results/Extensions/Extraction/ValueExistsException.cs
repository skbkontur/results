using System;

namespace Kontur.Results
{
    public sealed class ValueExistsException : InvalidOperationException
    {
        internal ValueExistsException(string message)
            : base(message)
        {
        }
    }
}
