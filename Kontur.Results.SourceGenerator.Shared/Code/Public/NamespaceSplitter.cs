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
            return Split(methods, extensionMethodFactory, IsGlobal);
        }

        internal static (IEnumerable<TExtensionMethod> LocalMethods, IEnumerable<TExtensionMethod> GlobalMethods) Split<TExtensionMethod>(
            IEnumerable<MethodType3> methods,
            Func<MethodType3, TExtensionMethod> extensionMethodFactory)
        {
            return Split(methods, extensionMethodFactory, IsGlobal);
        }

        private static (IEnumerable<TExtensionMethod> LocalMethods, IEnumerable<TExtensionMethod> GlobalMethods) Split<TMethod, TExtensionMethod>(
            IEnumerable<TMethod> methods,
            Func<TMethod, TExtensionMethod> extensionMethodFactory,
            Func<TMethod, bool> isGlobal)
        {
            return methods.Aggregate(
                (LocalMethods: Enumerable.Empty<TExtensionMethod>(), GlobalMethods: Enumerable.Empty<TExtensionMethod>()),
                (accumulator, method) =>
                {
                    var result = extensionMethodFactory(method);
                    var (localMethods, globalMethods) = accumulator;
                    return isGlobal(method)
                        ? (localMethods, globalMethods.Append(result))
                        : (localMethods.Append(result), globalMethods);
                });
        }

        private static bool IsGlobal(MethodType2 method)
        {
            return method.Parameter2IsValueTask;
        }

        private static bool IsGlobal(MethodType3 method)
        {
            return method.ParameterNextIsValueTask || method.Parameter2IsValueTask;
        }
    }
}
