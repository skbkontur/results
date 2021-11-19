using System;

namespace Kontur.Results
{
    public class ResultFailedException : InvalidOperationException
    {
        internal ResultFailedException(string message)
            : base(message)
        {
        }
    }
}
