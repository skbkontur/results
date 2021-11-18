using Kontur.Results;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.Optional.OptionalOptional
{
    internal class SomeFixtureCase : IFixtureCase
    {
        public Optional<TValue> GetOption<TValue>(TValue value, TValue constant) => Optional<TValue>.Some(value);
    }
}
