using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.Plain
{
    internal static class Common
    {
        private static TestCaseData Create<TFault1, TFault2>(Result<TFault1> result1, Result<TFault2> result2)
        {
            return new(result1, result2);
        }

        private static TestCaseData CreateEqual<TFault>(Result<TFault> result)
        {
            return Create(result, result);
        }

        internal static IEnumerable<TestCaseData> CreateEqualsCases()
        {
            yield return CreateEqual(Result<string>.Succeed());
            yield return CreateEqual(Result<int>.Succeed());
            yield return CreateEqual(Result<int>.Fail(2));
            yield return CreateEqual(Result<string?>.Fail(null));
            yield return CreateEqual(Result<string?>.Fail("hello"));
            yield return CreateEqual(Result<string>.Fail("hello"));
        }

        private static IEnumerable<TestCaseData> CreateNotEqual<TFault1, TFault2>(Result<TFault1> result1, Result<TFault2> result2)
        {
            yield return new(result1, result2);
            yield return new(result2, result1);
        }

        private static IEnumerable<IEnumerable<TestCaseData>> CreateNonEqualsCaseTemplates()
        {
            yield return CreateNotEqual(Result<string>.Succeed(), Result<int>.Succeed());

            yield return CreateNotEqual(Result<int>.Succeed(), Result<int>.Fail(1));

            yield return CreateNotEqual(Result<int>.Fail(1), Result<int>.Fail(2));
            yield return CreateNotEqual(Result<int>.Fail(2), Result<double>.Fail(2.0));
            yield return CreateNotEqual(Result<string>.Fail("hello"), Result<string>.Fail("hi"));
            yield return CreateNotEqual(Result<string>.Fail("hello"), Result<object>.Fail("hello"));
        }

        internal static IEnumerable<TestCaseData> CreateNonEqualsCases()
        {
            return CreateNonEqualsCaseTemplates().SelectMany(c => c);
        }
    }
}
