using System;
using FluentAssertions;
using Kontur.Results;
using Kontur.Tests.Results.LibraryNamespace;
using NUnit.Framework;

namespace Kontur.Tests.Results.Extraction.Ensure.Success.HasValue
{
    [TestFixture]
    internal class Override_Should
    {
        [Test]
        public void Import_Namespace_And_Do_Not_Override_Other_Values()
        {
            var option = Optional<string>.None();

            Action action = () => option.EnsureHasValue();

            action.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void Throw_If_None()
        {
            var option = Optional<CustomValue>.None();

            Action action = () => option.EnsureHasValue();

            action.Should()
                .Throw<Exception>()
                .WithMessage(LibraryException.Message);
        }

        [Test]
        public void Do_Not_Throw_If_Some()
        {
            var option = Optional<CustomValue>.Some(new());

            Action action = () => option.EnsureHasValue();

            action.Should().NotThrow();
        }
    }
}
