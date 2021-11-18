using System;

namespace Kontur.Results
{
    public sealed class ResultSucceedException : InvalidOperationException
    {
        internal ResultSucceedException(string message)
            : base(message)
        {
        }
    }
}
