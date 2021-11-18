using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.Plain
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = Result<string>.Fail("bar");

            Action action = () => result.EnsureSuccess();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Failure()
        {
            var result = Result<CustomFault>.Fail(new());

            Action action = () => result.EnsureSuccess();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Do_Not_Throw_If_Succeed()
        {
            var result = Result<CustomFault>.Succeed();

            Action action = () => result.EnsureSuccess();

            action.Should().NotThrow();
        }
    }
}
