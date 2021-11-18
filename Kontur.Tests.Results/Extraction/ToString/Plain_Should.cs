using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ToString
{
    [TestFixture]
    internal class Plain_Should
    {
        [Test]
        public void Failure()
        {
            var expected = Guid.NewGuid();
            var result = Result<Guid>.Fail(expected);

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(Guid), "fault", expected.ToString());
        }

        [Test]
        public void Success()
        {
            var result = Result<Guid>.Succeed();

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(Guid), "Success");
        }
    }
}
