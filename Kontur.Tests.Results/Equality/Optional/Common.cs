using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Optional
{
    internal static class Common
    {
        private static TestCaseData Create<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            return new(option1, option2);
        }

        private static TestCaseData CreateEqual<TValue>(Optional<TValue> optional)
        {
            return Create(optional, optional);
        }

        internal static IEnumerable<TestCaseData> CreateEqualsCases()
        {
            yield return CreateEqual(Optional<string>.None());
            yield return CreateEqual(Optional<int>.None());
            yield return CreateEqual(Optional<int>.Some(2));
            yield return CreateEqual(Optional<string?>.Some(null));
            yield return CreateEqual(Optional<string?>.Some("hello"));
            yield return CreateEqual(Optional<string>.Some("hello"));
        }

        private static IEnumerable<TestCaseData> CreateNotEqual<TValue1, TValue2>(Optional<TValue1> option1, Optional<TValue2> option2)
        {
            yield return new(option1, option2);
            yield return new(option2, option1);
        }

        private static IEnumerable<IEnumerable<TestCaseData>> CreateNonEqualsCaseTemplates()
        {
            yield return CreateNotEqual(Optional<string>.None(), Optional<int>.None());

            yield return CreateNotEqual(Optional<int>.None(), Optional<int>.Some(1));

            yield return CreateNotEqual(Optional<int>.Some(1), Optional<int>.Some(2));
            yield return CreateNotEqual(Optional<string>.Some("hello"), Optional<string>.Some("hi"));
            yield return CreateNotEqual(Optional<int>.Some(2), Optional<double>.Some(2.0));
            yield return CreateNotEqual(Optional<string>.Some("hello"), Optional<object>.Some("hello"));
        }

        internal static IEnumerable<TestCaseData> CreateNonEqualsCases()
        {
            return CreateNonEqualsCaseTemplates().SelectMany(c => c);
        }
    }
}
