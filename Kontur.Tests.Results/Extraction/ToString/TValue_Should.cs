using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ToString
{
    [TestFixture]
    internal class TValue_Should
    {
        [Test]
        public void Succeed()
        {
            var expected = Guid.NewGuid();
            var result = Result<int, Guid>.Succeed(expected);

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(Int32), nameof(Guid), "value", expected.ToString());
        }

        [Test]
        public void Fail()
        {
            var expected = Guid.NewGuid();
            var result = Result<Guid, int>.Fail(expected);

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(Guid), nameof(Int32), "fault", expected.ToString());
        }
    }
}
