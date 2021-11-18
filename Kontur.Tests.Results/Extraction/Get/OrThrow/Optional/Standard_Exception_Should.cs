using System;
using FluentAssertions;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Optional
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

            Func<int> action = () => option.GetValueOrThrow();

            action.Should().Throw<TException>();
        }
    }
}
