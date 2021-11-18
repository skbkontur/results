using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Equality
{
    internal static class Common
    {
        private static TestCaseData Create<TValue1, TValue2>(StringFaultResult<TValue1> result1, StringFaultResult<TValue2> result2)
        {
            return new(result1, result2);
        }

        private static TestCaseData CreateEqual<TValue>(StringFaultResult<TValue> result)
        {
            return Create(result, result);
        }

        internal static IEnumerable<TestCaseData> CreateEqualsCases()
        {
            yield return CreateEqual(StringFaultResult.Succeed(1));
            yield return CreateEqual(StringFaultResult.Succeed<string?>(null));
            yield return CreateEqual(StringFaultResult.Succeed<string?>("foo"));
            yield return CreateEqual(StringFaultResult.Succeed("foo"));

            yield return CreateEqual(StringFaultResult.Fail<string?>(new("foo")));
            yield return CreateEqual(StringFaultResult.Fail<string>(new("foo")));
            yield return CreateEqual(StringFaultResult.Fail<int>(new("foo")));
        }

        private static IEnumerable<TestCaseData> CreateNotEqual<TValue1, TValue2>(StringFaultResult<TValue1> result1,  StringFaultResult<TValue2> result2)
        {
            yield return new(result1, result2);
            yield return new(result2, result1);
        }

        private static IEnumerable<IEnumerable<TestCaseData>> CreateNonEqualsCaseTemplates()
        {
            yield return CreateNotEqual(StringFaultResult.Succeed(1), StringFaultResult.Succeed(2));
            yield return CreateNotEqual(StringFaultResult.Succeed(1), StringFaultResult.Succeed(1.0));
            yield return CreateNotEqual(StringFaultResult.Succeed(1), StringFaultResult.Succeed("1"));
            yield return CreateNotEqual(StringFaultResult.Succeed("foo"), StringFaultResult.Succeed("bar"));

            yield return CreateNotEqual(StringFaultResult.Succeed<object>(1), StringFaultResult.Succeed(1));
            yield return CreateNotEqual(StringFaultResult.Succeed<object>("foo"), StringFaultResult.Succeed("foo"));

            yield return CreateNotEqual(StringFaultResult.Fail<int>(new("foo")), StringFaultResult.Fail<int>(new("bar")));
            yield return CreateNotEqual(StringFaultResult.Fail<string>(new("foo")), StringFaultResult.Fail<string>(new("bar")));

            yield return CreateNotEqual(StringFaultResult.Fail<int>(new("foo")), StringFaultResult.Succeed("foo"));
            yield return CreateNotEqual(StringFaultResult.Fail<string>(new("foo")), StringFaultResult.Succeed("foo"));
        }

        internal static IEnumerable<TestCaseData> CreateNonEqualsCases()
        {
            return CreateNonEqualsCaseTemplates().SelectMany(c => c);
        }
    }
}
