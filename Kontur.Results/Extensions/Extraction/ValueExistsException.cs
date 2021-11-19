using System;

namespace Kontur.Results
{
    public class ValueExistsException : InvalidOperationException
    {
        internal ValueExistsException(string message)
            : base(message)
        {
        }
    }
}
