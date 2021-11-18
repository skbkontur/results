using System.Collections.Generic;
using System.Linq;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Equality.TValue
{
    internal static class Common
    {
        private static TestCaseData Create<TFault1, TValue1, TFault2, TValue2>(
            Result<TFault1, TValue1> result1,
            Result<TFault2, TValue2> result2)
        {
            return new(result1, result2);
        }

        private static TestCaseData CreateEqual<TFault, TValue>(Result<TFault, TValue> result)
        {
            return Create(result, result);
        }

        internal static IEnumerable<TestCaseData> CreateEqualsCases()
        {
            yield return CreateEqual(Result<string, int>.Succeed(1));
            yield return CreateEqual(Result<string, string?>.Succeed(null));
            yield return CreateEqual(Result<string, string?>.Succeed("foo"));
            yield return CreateEqual(Result<string, string>.Succeed("foo"));
            yield return CreateEqual(Result<int, int>.Succeed(1));
            yield return CreateEqual(Result<int, string?>.Succeed(null));
            yield return CreateEqual(Result<int, string?>.Succeed("foo"));
            yield return CreateEqual(Result<int, string>.Succeed("foo"));

            yield return CreateEqual(Result<string?, string>.Fail(null));
            yield return CreateEqual(Result<string?, string>.Fail("foo"));
            yield return CreateEqual(Result<string?, int>.Fail(null));
            yield return CreateEqual(Result<string?, int>.Fail("foo"));
            yield return CreateEqual(Result<string, int>.Fail("foo"));
            yield return CreateEqual(Result<string, string>.Fail("foo"));
            yield return CreateEqual(Result<int, int>.Fail(1));
            yield return CreateEqual(Result<int, string>.Fail(1));
        }

        private static IEnumerable<TestCaseData> CreateNotEqual<TFault1, TValue1, TFault2, TValue2>(
            Result<TFault1, TValue1> result1,
            Result<TFault2, TValue2> result2)
        {
            yield return new(result1, result2);
            yield return new(result2, result1);
        }

        private static IEnumerable<IEnumerable<TestCaseData>> CreateNonEqualsCaseTemplates()
        {
            yield return CreateNotEqual(Result<int, int>.Succeed(1), Result<int, int>.Succeed(2));
            yield return CreateNotEqual(Result<int, string>.Succeed("foo"), Result<int, string>.Succeed("bar"));
            yield return CreateNotEqual(Result<string, int>.Succeed(1), Result<string, int>.Succeed(2));
            yield return CreateNotEqual(Result<string, string>.Succeed("foo"), Result<string, string>.Succeed("bar"));

            yield return CreateNotEqual(Result<int, object>.Succeed(1), Result<int, int>.Succeed(1));
            yield return CreateNotEqual(Result<int, object>.Succeed("foo"), Result<int, string>.Succeed("foo"));
            yield return CreateNotEqual(Result<string, object>.Succeed(1), Result<string, int>.Succeed(1));
            yield return CreateNotEqual(Result<string, object>.Succeed("foo"), Result<string, string>.Succeed("foo"));

            yield return CreateNotEqual(Result<int, int>.Succeed(1), Result<int, double>.Succeed(1.0));
            yield return CreateNotEqual(Result<int, int>.Succeed(1), Result<int, string>.Succeed("1"));
            yield return CreateNotEqual(Result<string, int>.Succeed(1), Result<string, double>.Succeed(1.0));
            yield return CreateNotEqual(Result<string, int>.Succeed(1), Result<string, string>.Succeed("1"));

            yield return CreateNotEqual(Result<int, int>.Fail(1), Result<int, int>.Fail(2));
            yield return CreateNotEqual(Result<int, string>.Fail(1), Result<int, string>.Fail(2));
            yield return CreateNotEqual(Result<string, int>.Fail("foo"), Result<string, int>.Fail("bar"));
            yield return CreateNotEqual(Result<string, string>.Fail("foo"), Result<string, string>.Fail("bar"));

            yield return CreateNotEqual(Result<object, int>.Fail(1), Result<int, int>.Fail(1));
            yield return CreateNotEqual(Result<object, int>.Fail("foo"), Result<string, int>.Fail("foo"));
            yield return CreateNotEqual(Result<object, string>.Fail(1), Result<int, string>.Fail(1));
            yield return CreateNotEqual(Result<object, string>.Fail("foo"), Result<string, string>.Fail("foo"));

            yield return CreateNotEqual(Result<int, int>.Fail(1), Result<double, int>.Fail(1.0));
            yield return CreateNotEqual(Result<int, string>.Fail(1), Result<double, string>.Fail(1.0));
            yield return CreateNotEqual(Result<int, int>.Fail(1), Result<string, int>.Fail("1"));
            yield return CreateNotEqual(Result<int, string>.Fail(1), Result<string, string>.Fail("1"));

            yield return CreateNotEqual(Result<int, int>.Fail(1), Result<int, int>.Succeed(1));
            yield return CreateNotEqual(Result<int, string>.Fail(1), Result<string, int>.Succeed(1));
            yield return CreateNotEqual(Result<string, int>.Fail("foo"), Result<int, string>.Succeed("foo"));
            yield return CreateNotEqual(Result<string, string>.Fail("foo"), Result<string, string>.Succeed("foo"));
        }

        internal static IEnumerable<TestCaseData> CreateNonEqualsCases()
        {
            return CreateNonEqualsCaseTemplates().SelectMany(c => c);
        }
    }
}
