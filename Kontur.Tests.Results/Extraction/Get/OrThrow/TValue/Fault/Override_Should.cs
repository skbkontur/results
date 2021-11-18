using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Fault
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = Result<int, string>.Succeed("foo");

            Func<int> action = () => result.GetFaultOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Success_For_Overriden_Fault()
        {
            var result = Result<CustomFault, string>.Succeed("val");

            Func<CustomFault> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Fault_If_Failure_For_Overriden_Fault()
        {
            CustomFault expected = new();
            var source = Result<CustomFault, string>.Fail(expected);

            var result = source.GetFaultOrThrow();

            result.Should().Be(expected);
        }

        [Test]
        public void Throw_If_Success_For_Overriden_Value()
        {
            var result = Result<string, CustomValue>.Succeed(new());

            Func<string> action = () => result.GetFaultOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Fault_If_Failure_For_Overriden_Value()
        {
            const string expected = "foo";
            var source = Result<string, CustomValue>.Fail(expected);

            var result = source.GetFaultOrThrow();

            result.Should().Be(expected);
        }
    }
}
