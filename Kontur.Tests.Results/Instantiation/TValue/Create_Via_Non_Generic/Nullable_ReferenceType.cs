using System;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ReferenceType
    {
        private static TestCaseData CreateHasValueCase(Result<string?, string?> result, bool success)
        {
            return Common.CreateHasValueCase(result, success);
        }

        private static readonly TestCaseData[] HasValueNullableReferenceTypeCases =
        {
            CreateHasValueCase(ResultFailure<string?>.Create<string?>(null), false),
            CreateHasValueCase(ResultFailure<string?>.Create<string?>("hi"), false),
            CreateHasValueCase(Result<string?>.Succeed<string?>(null), true),
            CreateHasValueCase(Result<string?>.Succeed<string?>("hi"), true),
        };

        [TestCaseSource(nameof(HasValueNullableReferenceTypeCases))]
        public bool HasValue(Result<string?, string?> result)
        {
            return result.Success;
        }

        private static TestCaseData CreateStoreCase(string? data)
        {
            return new(data) { ExpectedResult = data };
        }

        private static readonly TestCaseData[] StoreCases =
        {
            CreateStoreCase(null),
            CreateStoreCase("foo"),
        };

        [TestCaseSource(nameof(StoreCases))]
        public string? Store_Value(string? value)
        {
            var result = Result<Guid>.Succeed(value);

            return result.GetValueOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public string? Store_Value_For_Same_Types(string? value)
        {
            var result = Result<string?>.Succeed(value);

            return result.GetValueOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public string? Store_Fault(string? value)
        {
            var result = ResultFailure<Guid>.Create(value);

            return result.GetFaultOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public string? Store_Fault_For_Same_Types(string? value)
        {
            var result = ResultFailure<string?>.Create(value);

            return result.GetFaultOrThrow();
        }
    }
}
