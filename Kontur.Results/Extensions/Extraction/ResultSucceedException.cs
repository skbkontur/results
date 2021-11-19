using System;

namespace Kontur.Results
{
    public class ResultSucceedException : InvalidOperationException
    {
        internal ResultSucceedException(string message)
            : base(message)
        {
        }
    }
}
