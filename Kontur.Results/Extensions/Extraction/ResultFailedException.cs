using System;

namespace Kontur.Results
{
    public sealed class ResultFailedException : InvalidOperationException
    {
        internal ResultFailedException(string message)
            : base(message)
        {
        }
    }
}
