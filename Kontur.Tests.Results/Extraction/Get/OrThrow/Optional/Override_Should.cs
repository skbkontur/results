using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;
using GetValueOrThrowOptionalExtensions = Kontur.Tests.Results.LibraryNamespace.GetValueOrThrowOptionalExtensions;

namespace Kontur.Tests.Results.Extraction.Get.OrThrow.Optional
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var option = Optional<string>.None();

            Func<string> action = () => option.GetValueOrThrow();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_None()
        {
            var option = Optional<CustomValue>.None();

            Func<CustomValue> action = () => GetValueOrThrowOptionalExtensions.GetValueOrThrow(option);

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Return_Value_If_Some()
        {
            CustomValue expected = new();
            var option = Optional<CustomValue>.Some(expected);

            var result = GetValueOrThrowOptionalExtensions.GetValueOrThrow(option);

            result.Should().Be(expected);
        }
    }
}
