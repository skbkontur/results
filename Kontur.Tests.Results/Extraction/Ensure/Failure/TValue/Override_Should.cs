using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Failure.TValue
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = Result<int, string>.Succeed("foo");

            Action action = () => result.EnsureFailure();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Success_For_Overriden_Fault()
        {
            var result = Result<CustomFault, string>.Succeed("val");

            Action action = () => result.EnsureFailure();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Do_Not_Throw_If_Failure_For_Overriden_Fault()
        {
            var result = Result<CustomFault, string>.Fail(new());

            Action action = () => result.EnsureFailure();

            action.Should().NotThrow();
        }

        [Test]
        public void Throw_If_Success_For_Overriden_Value()
        {
            var result = Result<string, CustomValue>.Succeed(new());

            Action action = () => result.EnsureFailure();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Do_Not_Throw_If_Failure_For_Overriden_Value()
        {
            var result = Result<string, CustomValue>.Fail("bar");

            Action action = () => result.EnsureFailure();

            action.Should().NotThrow();
        }
    }
}
