using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Inheritance.Extraction.Get.OrThrow.Value
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = StringFaultResult.Fail<int>(new("foo"));

            Func<int> action = () => result.GetValueOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Failure()
        {
            var result = StringFaultResult.Fail<CustomValue>(new("err"));

            Func<CustomValue> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Value_If_Success()
        {
            CustomValue expected = new();
            var source = StringFaultResult.Succeed(expected);

            var result = source.GetValueOrThrow();

            result.Should().Be(expected);
        }
    }
}
