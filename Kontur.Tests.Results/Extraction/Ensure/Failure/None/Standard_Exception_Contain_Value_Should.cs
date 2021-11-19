using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.None
{
    [TestFixture]
    internal class Standard_Exception_Contain_Value_Should
    {
        [Test]
        public void Throw_If_Some()
        {
            var expected = Guid.NewGuid().ToString();
            var option = Optional<string>.Some(expected);

            Action action = () => option.EnsureNone();

            action.Should()
                .Throw<ValueExistsException<string>>()
                .WithMessage($"*{expected}*")
                .Which.Value.Should().Be(expected);
        }
    }
}
