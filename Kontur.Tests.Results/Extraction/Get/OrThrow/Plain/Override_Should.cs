using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Plain
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = Result<int>.Succeed();

            Func<int> action = () => result.GetFaultOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Success()
        {
            var result = Result<CustomFault>.Succeed();

            Func<CustomFault> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Fault_If_Failure()
        {
            CustomFault expected = new();
            var result = Result<CustomFault>.Fail(expected);

            var fault = result.GetFaultOrThrow();

            fault.Should().Be(expected);
        }
    }
}
