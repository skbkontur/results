using System;
using System.Collections.Generic;
using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal static class UpcastPlainExamples
    {
        internal static IEnumerable<UpcastPlainExample<Result<Base>>> Get()
            => Get(Result<Base>.Fail, Result<Base>.Succeed());

        internal static IEnumerable<UpcastPlainExample<TResult>> Get<TResult>(
            Func<Child, TResult> failureResultFactory,
            TResult successResult)
        {
            Child child = new();
            yield return new(Result<Child>.Fail(child), failureResultFactory(child));
            yield return new(Result<Child>.Succeed(), successResult);
        }
    }
}
