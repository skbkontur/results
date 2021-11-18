using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kontur.Results;
using NUnit.Framework;

namespace Kontur.Tests.Results.Conversion.Combinations.Linq.DoNotation
{
    [TestFixture]
    internal class TValue_Should
    {
        private static Task<int> GetCurrentIndex() => Task.FromResult(10);

        private static Task<Result<Error, Product>> GetCurrentProduct() => Task.FromResult(Result<Error, Product>.Succeed(new("Pizza")));

        private static Task<Result<Error, string>> GetMessage(Guid userId, int index, Product product)
        {
            if (userId == Guid.Empty)
            {
                return Task.FromResult(Result<Error, string>.Fail(Error.NoMessageForDefaultUser));
            }

            var result = $"{userId.ToString().Substring(0, 2)}-{index}-{product.Name}";
            return Task.FromResult(Result<Error, string>.Succeed(result));
        }

        private static Task<Format> GetFormat(int index, string message)
        {
            var prefix = message.Last() + index.ToString(CultureInfo.InvariantCulture);
            Format result = new(prefix);
            return Task.FromResult(result);
        }

        private static Result<Error, ConvertResult> Convert(string message, Format format)
        {
            var result = format.Prefix + ": " + message;
            return Result<Error, ConvertResult>.Succeed(new(result));
        }

        private static TestCaseData Create(Result<Error, Guid> user, Result<Error, string> result)
        {
            return new(user) { ExpectedResult = result };
        }

        private static readonly TestCaseData[] Cases =
        {
            Create(Result<Error, Guid>.Fail(Error.NoCurrentUser), Result<Error, string>.Fail(Error.NoCurrentUser)),
            Create(Result<Error, Guid>.Succeed(Guid.Empty), Result<Error, string>.Fail(Error.NoMessageForDefaultUser)),
            Create(Result<Error, Guid>.Succeed(Guid.Parse("00dc7316-e772-479f-9f6e-f6a776b17e00")),  Result<Error, string>.Succeed("a11: 00-11-Pizza")),
            Create(Result<Error, Guid>.Succeed(Guid.Parse("e2dc7316-e772-479f-9f6e-f6a776b17ebb")), Result<Error, string>.Succeed("a11: e2-11-Pizza")),
        };

        [TestCaseSource(nameof(Cases))]
        public async Task<Result<Error, string>> Process_Chain(Result<Error, Guid> user)
        {
            Result<Error, Guid> GetCurrentUserId() => user;

            var task =
                from userId in GetCurrentUserId()
                from index in GetCurrentIndex()
                from product in GetCurrentProduct()
                let nextIndex = index + 1
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

        public enum Error
        {
            NoCurrentUser,
            NoMessageForDefaultUser,
        }
    }
}