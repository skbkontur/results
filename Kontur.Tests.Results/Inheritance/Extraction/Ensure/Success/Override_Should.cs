using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Ensure.Success
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = StringFaultResult.Fail<int>(new("bar"));

            Action action = () => result.EnsureSuccess();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Failure()
        {
            var result = StringFaultResult.Fail<CustomValue>(new("err"));

            Action action = () => result.EnsureSuccess();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Do_Not_Throw_If_Success()
        {
            var result = StringFaultResult.Succeed(new CustomValue());

            Action action = () => result.EnsureSuccess();

            action.Should().NotThrow();
        }
    }
}
