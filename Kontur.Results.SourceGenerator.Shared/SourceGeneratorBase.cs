using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Kontur.Results.SourceGenerator.Code;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Methods;
using Kontur.Results.SourceGenerator.Code.Public;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Kontur.Results.SourceGenerator
{
    public abstract class SourceGeneratorBase : ISourceGenerator
    {
        private readonly CompilationFileProviderSettings settings;

        private protected SourceGeneratorBase(CompilationFileProviderSettings settings)
        {
            this.settings = settings;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        public void Execute(GeneratorExecutionContext context)
        {
            using var sp = GetServiceProvider();

            var provider = sp.GetRequiredService<CompilationFileProvider>();

            foreach (var (hint, compilationUnitSyntax) in provider.GetAll())
            {
                var text = compilationUnitSyntax.ToFullString();
                context.AddSource(hint, SourceText.From(text, Encoding.UTF8));
            }
        }

        private ServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton(_ => settings)
                .AddSingleton<CompilationFileProvider>()
                .AddSingleton<MethodsDescriptionProvider>()
                .AddSingleton<ITypeParameterGenericMethodSyntaxGenerator, TypeParameterGenericMethodSyntaxGenerator>()
                .AddSingleton<IMethodDeclarationFactory, MethodDeclarationFactory>()
                .AddSingleton<InternalMethodDescriptionFactories>()
                .AddSingleton<PublicMethodDescriptionFactories>()
                .AddSingleton<IMethodBodyFactory, MethodBodyFactory>()
                .AddSingleton<MethodDescriptionFactory>();

            var interfacesToRegister = new HashSet<Type>
            {
                typeof(IDescriptionProvider),
            };
            var registrations = Assembly.GetExecutingAssembly().DefinedTypes
                .SelectMany(type => type.ImplementedInterfaces.Select(inter => new { type, inter }))
                .Where(registration => interfacesToRegister.Contains(registration.inter));

            foreach (var registration in registrations)
            {
                serviceCollection.AddSingleton(registration.inter, registration.type);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}