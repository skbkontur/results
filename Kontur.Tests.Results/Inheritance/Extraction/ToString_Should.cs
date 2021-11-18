using System;
using FluentAssertions;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction
{
    [TestFixture]
    internal class ToString_Should
    {
        [Test]
        public void Succeed()
        {
            var expected = Guid.NewGuid();
            var result = StringFaultResult.Succeed(expected);

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(String), nameof(Guid), "value", expected.ToString());
        }

        [Test]
        public void Fail()
        {
            const string expected = "message3";
            var result = StringFaultResult.Fail<Guid>(new(expected));

            var serialized = result.ToString();

            serialized.Should().ContainAll(nameof(String), nameof(Guid), "fault", expected);
        }
    }
}
