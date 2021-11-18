using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Fault
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = StringFaultResult.Succeed("bar");

            Func<StringFault> action = () => result.GetFaultOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Success()
        {
            var result = StringFaultResult.Succeed(new CustomValue());

            Func<StringFault> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Fault_If_Failure()
        {
            StringFault expected = new("bar");
            var source = StringFaultResult.Fail<CustomValue>(expected);

            var result = source.GetFaultOrThrow();

            result.Should().Be(expected);
        }
    }
}
