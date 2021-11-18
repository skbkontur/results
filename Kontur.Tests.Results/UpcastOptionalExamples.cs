using System;
using System.Collections.Generic;
using Kontur.Results;

namespace Kontur.Tests.Results
{
    internal static class UpcastOptionalExamples
    {
        internal static IEnumerable<UpcastOptionalExample<Optional<Base>>> Get()
            => Get(Optional<Base>.None(), Optional<Base>.Some);

        internal static IEnumerable<UpcastOptionalExample<TResult>> Get<TResult>(TResult noneResult, Func<Child, TResult> someResultFactory)
        {
            Child child = new();
            yield return new(Optional<Child>.Some(child), someResultFactory(child));
            yield return new(Optional<Child>.None(), noneResult);
        }
    }
}
