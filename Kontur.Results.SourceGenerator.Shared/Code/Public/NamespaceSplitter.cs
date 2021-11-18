using System;
using System.Collections.Generic;
using System.Linq;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal static class NamespaceSplitter
    {
        internal static (IEnumerable<TExtensionMethod> LocalMethods, IEnumerable<TExtensionMethod> GlobalMethods) Split<TExtensionMethod>(
            IEnumerable<MethodType2> methods,
            Func<MethodType2, TExtensionMethod> extensionMethodFactory)
        {
            return methods.Aggregate(
                (LocalMethods: Enumerable.Empty<TExtensionMethod>(), GlobalMethods: Enumerable.Empty<TExtensionMethod>()),
                (accumulator, method) =>
                {
                    var result = extensionMethodFactory(method);
                    var (localMethods, globalMethods) = accumulator;
                    return IsGlobal(method)
                        ? (localMethods, globalMethods.Append(result))
                        : (localMethods.Append(result), globalMethods);
                });
        }

        private static bool IsGlobal(MethodType2 method)
        {
            return method.Parameter2.Identifier.IsValueTask();
        }
    }
}
