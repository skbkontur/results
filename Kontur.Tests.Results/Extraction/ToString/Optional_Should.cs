using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.ToString
{
    [TestFixture]
    internal class Optional_Should
    {
        [Test]
        public void Some()
        {
            var expected = Guid.NewGuid();
            var option = Optional<Guid>.Some(expected);

            var result = option.ToString();

            result.Should().ContainAll(nameof(Guid), "Some", expected.ToString());
        }

        [Test]
        public void None()
        {
            var option = Optional<Guid>.None();

            var result = option.ToString();

            result.Should().ContainAll(nameof(Guid), "None");
        }
    }
}
