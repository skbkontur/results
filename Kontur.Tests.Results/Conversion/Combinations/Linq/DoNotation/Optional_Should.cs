using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.DoNotation
{
    [TestFixture]
    internal class Optional_Should
    {
        private static Task<int> GetCurrentIndex() => Task.FromResult(10);

        private static Task<Optional<Product>> GetCurrentProduct() => Task.FromResult(Optional<Product>.Some(new("Pizza")));

        private static Task<Optional<string>> GetMessage(Guid userId, int index, Product product)
        {
            var result = $"{userId.ToString().Substring(0, 2)}-{index}-{product.Name}";
            return Task.FromResult(Optional<string>.Some(result));
        }

        private static Task<Format> GetFormat(int index, string message)
        {
            var prefix = message.Last() + index.ToString(CultureInfo.InvariantCulture);
            Format result = new(prefix);
            return Task.FromResult(result);
        }

        private static Optional<ConvertResult> Convert(string message, Format format)
        {
            var result = format.Prefix + ": " + message;
            return Optional<ConvertResult>.Some(new(result));
        }

        private static TestCaseData Create(Optional<Guid> user, Optional<string> result)
        {
            return new(user) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Optional<Guid>.None(), Optional<string>.None()),
            Create(Optional<Guid>.Some(Guid.Empty), Optional<string>.None()),
            Create(Optional<Guid>.Some(Guid.Parse("00dc7316-e772-479f-9f6e-f6a776b17e00")), Optional<string>.Some("a11: 00-11-Pizza")),
            Create(Optional<Guid>.Some(Guid.Parse("e2dc7316-e772-479f-9f6e-f6a776b17ebb")), Optional<string>.Some("a11: e2-11-Pizza")),
        };

        [TestCaseSource(nameof(Cases))]
        public async Task<Optional<string>> Process_Chain(Optional<Guid> user)
        {
            Optional<Guid> GetCurrentUserId() => user;

            var task =
                from userId in GetCurrentUserId()
                where userId != Guid.Empty
                from index in GetCurrentIndex()
                from product in GetCurrentProduct()
                let nextIndex = index + 1
                where nextIndex > 0
                from message in GetMessage(userId, nextIndex, product)
                from format in GetFormat(nextIndex, message)
                from converted in Convert(message, format)
                select converted.Result;

            return await task.ConfigureAwait(false);
        }

        private class Product
        {
            internal Product(string name)
            {
                Name = name;
            }

            internal string Name { get; }
        }

        private class Format
        {
            internal Format(string prefix)
            {
                Prefix = prefix;
            }

            internal string Prefix { get; }
        }

        public class ConvertResult
        {
            internal ConvertResult(string result)
            {
                Result = result;
            }

            internal string Result { get; }
        }
    }
}