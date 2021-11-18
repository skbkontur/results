using System;
using Kontur.Results;

namespace Kontur.Tests.Results.Inheritance
{
    public class StringFaultResult<TValue> : Result<StringFault, TValue>
    {
        private readonly Result<StringFault, TValue> result;

        public StringFaultResult(StringFault fault)
        {
            result = Fail(fault);
        }

        public StringFaultResult(TValue value)
        {
            result = Succeed(value);
        }

        public override TResult Match<TResult>(Func<StringFault, TResult> onFailure, Func<TValue, TResult> onSuccess)
        {
            return result.Match(onFailure, onSuccess);
        }
    }
}
