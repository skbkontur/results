using System;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Instantiation.TValue.Create_Via_Non_Generic
{
    [TestFixture]
    internal class Nullable_ValueType_Should
    {
        private static TestCaseData CreateHasValueCase(Result<int?, int?> result, bool success)
        {
            return Common.CreateHasValueCase(result, success);
        }

        private static readonly TestCaseData[] HasValueNullableReferenceTypeCases =
        {
            CreateHasValueCase(ResultFailure<int?>.Create<int?>(null), false),
            CreateHasValueCase(ResultFailure<int?>.Create<int?>(10), false),
            CreateHasValueCase(Result<int?>.Succeed<int?>(null), true),
            CreateHasValueCase(Result<int?>.Succeed<int?>(10), true),
        };

        [TestCaseSource(nameof(HasValueNullableReferenceTypeCases))]
        public bool HasValue(Result<int?, int?> result)
        {
            return result.Success;
        }

        private static TestCaseData CreateStoreCase(int? data)
        {
            return new(data) { ExpectedResult = data };
        }

        private static readonly TestCaseData[] StoreCases =
        {
            CreateStoreCase(null),
            CreateStoreCase(10),
        };

        [TestCaseSource(nameof(StoreCases))]
        public int? Store_Value(int? value)
        {
            var result = Result<Guid>.Succeed(value);

            return result.GetValueOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public int? Store_Value_For_Same_Types(int? value)
        {
            var result = Result<int?>.Succeed(value);

            return result.GetValueOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public int? Store_Fault(int? value)
        {
            var result = ResultFailure<Guid>.Create(value);

            return result.GetFaultOrThrow();
        }

        [TestCaseSource(nameof(StoreCases))]
        public int? Store_Fault_For_Same_Types(int? value)
        {
            var result = ResultFailure<int?>.Create(value);

            return result.GetFaultOrThrow();
        }
    }
}