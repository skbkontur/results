using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.HasValue
{
    [TestFixture(typeof(InvalidOperationException))]
    [TestFixture(typeof(ValueMissingException))]
    internal class Standard_Exception_Should<TException>
        where TException : Exception
    {
        [Test]
        public void Throw_If_None()
        {
            var option = Optional<int>.None();

            Action action = () => option.EnsureHasValue();

            action.Should().Throw<TException>();
        }
    }
}
