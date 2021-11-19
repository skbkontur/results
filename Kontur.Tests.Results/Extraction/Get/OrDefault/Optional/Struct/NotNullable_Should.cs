﻿using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrDefault.Optional.Struct
{
    [TestFixture]
    internal class NotNullable_Should
    {
        private static TestCaseData CreateCase(Optional<int> optional, int result)
        {
            return new(optional) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            CreateCase(Optional<int>.None(), 0),
            CreateCase(Optional<int>.Some(1), 1),
        };

        [TestCaseSource(nameof(Cases))]
        public int Process_Option(Optional<int> optional)
        {
            return optional.GetValueOrDefault();
        }
    }
}