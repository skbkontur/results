using System;
using System.Collections.Generic;
using Kontur.Results;

namespace Kontur.Tests.Results.Inheritance
{
    internal static class UpcastExamples
    {
        internal static IEnumerable<UpcastExample<Child, Result<StringFaultBase, Base>>> GetBoth()
            => GetBoth(Result<StringFaultBase, Base>.Fail, Result<StringFaultBase, Base>.Succeed);

        internal static IEnumerable<UpcastExample<Child, TResult>> GetBoth<TResult>(
            Func<StringFault, TResult> onFailureResultFactory,
            Func<Child, TResult> onSuccessResultFactory)
            => Create(
                new(),
                onFailureResultFactory,
                onSuccessResultFactory);

        internal static IEnumerable<UpcastExample<string, Result<StringFaultBase, string>>> GetFaults()
            => GetFaults(Result<StringFaultBase, string>.Fail, Result<StringFaultBase, string>.Succeed);

        internal static IEnumerable<UpcastExample<string, TResult>> GetFaults<TResult>(
            Func<StringFault, TResult> onFailureResultFactory,
            Func<string, TResult> onSuccessResultFactory)
            => Create(
                "bar",
                onFailureResultFactory,
                onSuccessResultFactory);

        internal static IEnumerable<UpcastExample<Child, Result<StringFault, Base>>> GetValues()
            => GetValues(Result<StringFault, Base>.Fail, Result<StringFault, Base>.Succeed);

        internal static IEnumerable<UpcastExample<Child, TResult>> GetValues<TResult>(
            Func<StringFault, TResult> onFailureResultFactory,
            Func<Child, TResult> onSuccessResultFactory)
            => Create(
                new(),
                onFailureResultFactory,
                onSuccessResultFactory);

        private static IEnumerable<UpcastExample<TValue, TResult>> Create<TValue, TResult>(
            TValue value,
            Func<StringFault, TResult> onFailureResultFactory,
            Func<TValue, TResult> onSuccessResultFactory)
        {
            StringFault fault = new("bar");
            yield return new(StringFaultResult.Fail<TValue>(fault), onFailureResultFactory(fault));
            yield return new(StringFaultResult.Succeed(value), onSuccessResultFactory(value));
        }
    }
}
