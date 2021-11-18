using System;
using System.Collections.Generic;
using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal static class UpcastTValueExamples
    {
        internal static IEnumerable<UpcastTValueExample<Child, Child, Result<Base, Base>>> GetBoth()
            => GetBoth(Result<Base, Base>.Fail, Result<Base, Base>.Succeed);

        internal static IEnumerable<UpcastTValueExample<Child, Child, TResult>> GetBoth<TResult>(
            Func<Child, TResult> onFailureResultFactory,
            Func<Child, TResult> onSuccessResultFactory)
            => Create(
                new(),
                new(),
                onFailureResultFactory,
                onSuccessResultFactory);

        internal static IEnumerable<UpcastTValueExample<Child, string, Result<Base, string>>> GetFaults()
            => GetFaults(Result<Base, string>.Fail, Result<Base, string>.Succeed);

        internal static IEnumerable<UpcastTValueExample<Child, string, TResult>> GetFaults<TResult>(
            Func<Child, TResult> onFailureResultFactory,
            Func<string, TResult> onSuccessResultFactory)
            => Create(
                new(),
                "bar",
                onFailureResultFactory,
                onSuccessResultFactory);

        internal static IEnumerable<UpcastTValueExample<string, Child, Result<string, Base>>> GetValues()
            => GetValues(Result<string, Base>.Fail, Result<string, Base>.Succeed);

        internal static IEnumerable<UpcastTValueExample<string, Child, TResult>> GetValues<TResult>(
            Func<string, TResult> onFailureResultFactory,
            Func<Child, TResult> onSuccessResultFactory)
            => Create(
                "bar",
                new(),
                onFailureResultFactory,
                onSuccessResultFactory);

        private static IEnumerable<UpcastTValueExample<TFault, TValue, TResult>> Create<TFault, TValue, TResult>(
            TFault fault,
            TValue value,
            Func<TFault, TResult> onFailureResultFactory,
            Func<TValue, TResult> onSuccessResultFactory)
        {
            yield return new(Result<TFault, TValue>.Fail(fault), onFailureResultFactory(fault));
            yield return new(Result<TFault, TValue>.Succeed(value), onSuccessResultFactory(value));
        }
    }
}
