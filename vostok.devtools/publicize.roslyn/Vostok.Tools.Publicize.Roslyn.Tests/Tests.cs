using System;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Tools.Publicize.Roslyn.Tests
{
    public class Tests
    {
        private static readonly (string code, string expected)[] testData = {
            (
                @"using System;
                    [PublicAPI] internal class A{ }",
                @"using System;
                    [PublicAPI] public class A{ }"
            ),
            (
                @"using System;
                    [PublicAPI] internal interface A{ }",
                @"using System;
                    [PublicAPI] public interface A{ }" 
            ),
            (
                @"using System;
                    [PublicAPI] public class A{ }",
                @"using System;
                    [PublicAPI] public class A{ }" 
            ),
            (
                @"using System;
                    public class A {
                    [PublicAPI] internal class B{ }
                }",
                @"using System;
                    public class A {
                    [PublicAPI] public class B{ }
                }"
            ),
            (
                @"using System;
                    public class A {
                        [PublicAPI]
                        internal void M() {}
                }",
                @"using System;
                    public class A {
                        [PublicAPI]
                        public void M() {}
                }"
            ),
            (
                @"using System;
                    [PublicAPI] internal enum A {V1, V2}
                }",
                @"using System;
                    [PublicAPI] public enum A {V1, V2}
                }"
            )
        };
        
        
        [TestCaseSource(nameof(testData))]
        public void Should_Publicize((string code, string expected) data)
        {
            var actual = Publicizer.Process(data.code);
            Console.WriteLine(actual);
            actual.Should().BeEquivalentTo(data.expected);
        }
    }
}