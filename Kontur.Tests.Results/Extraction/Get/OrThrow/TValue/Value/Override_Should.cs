using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.TValue.Value
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var result = Result<string, int>.Fail("foo");

            Func<int> action = () => result.GetValueOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_Failure_For_Overriden_Fault()
        {
            var result = Result<CustomFault, int>.Fail(new());

            Func<int> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Value_If_Success_For_Overriden_Fault()
        {
            const int expected = 5;
            var source = Result<CustomFault, int>.Succeed(expected);

            var result = source.GetValueOrThrow();

            result.Should().Be(expected);
        }

        [Test]
        public void Throw_If_Failure_Overriden_Value()
        {
            var result = Result<string, CustomValue>.Fail("bar");

            Func<CustomValue> action = () => result.GetValueOrThrow();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Value_If_Success_Overriden_Value()
        {
            CustomValue expected = new();
            var source = Result<string, CustomValue>.Succeed(expected);

            var result = source.GetValueOrThrow();

            result.Should().Be(expected);
        }
    }
}
