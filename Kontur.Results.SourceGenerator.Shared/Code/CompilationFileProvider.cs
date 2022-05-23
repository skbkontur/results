using System.Collections.Generic;
using System.Linq;
using Kontur.Results.SourceGenerator.Code.Internal;
using Kontur.Results.SourceGenerator.Code.Methods;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class CompilationFileProvider
    {
        private static readonly SyntaxToken[] ExtensionMethodAccessModifiers = AccessModifierFactory.Create(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword);

        private static readonly AttributeListSyntax EditorBrowsableAttributeList = SyntaxFactory.AttributeList(SyntaxFactory.SeparatedList(new[]
        {
            Attributes.EditorBrowsable,
        }));

        private readonly MethodsDescriptionProvider provider;
        private readonly ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator;
        private readonly CompilationFileProviderSettings compilationFileProviderSettings;

        public CompilationFileProvider(
            MethodsDescriptionProvider provider,
            ITypeParameterGenericMethodSyntaxGenerator typeParameterGenericMethodSyntaxGenerator,
            CompilationFileProviderSettings compilationFileProviderSettings)
        {
            this.provider = provider;
            this.typeParameterGenericMethodSyntaxGenerator = typeParameterGenericMethodSyntaxGenerator;
            this.compilationFileProviderSettings = compilationFileProviderSettings;
        }

        internal IEnumerable<CompilationUnit> GetAll()
        {
            var methodsDescriptions = this.provider.Get().ToArray();

            var internalPartialCompilationFile = methodsDescriptions
                .Select(methods => methods.Internal.Partial)
                .ToLookup(methods => methods.ClassName, methods => methods.Methods)
                .Select(CreateInternalPartialCompilationFile);

            return methodsDescriptions
                .SelectMany(all => all.Internal.Methods)
                .Select(CreateInternalCompilationFile)
                .Concat(internalPartialCompilationFile)
                .Append(this.CreateLocalExtensionCompilationFile(methodsDescriptions.SelectMany(all => all.Public.Extensions)))
                .Append(this.CreateGlobalExtensionCompilationFile(methodsDescriptions.SelectMany(all => all.Public.ExtensionsGlobal)))
                .Select(this.CreateCompilationUnit);
        }

        private static CompilationFile CreateInternalCompilationFile(InternalStandardMethodsDescription internalMethods)
        {
            var (className, usingDirectiveSyntax, methods) = internalMethods;

            var classSyntax = SyntaxFactory.ClassDeclaration(className)
                .AddModifiers(AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword))
                .AddMembers(methods.ToArray<MemberDeclarationSyntax>());

            return new(
                className,
                usingDirectiveSyntax,
                PutIntoNamespace(classSyntax));
        }

        private static CompilationFile CreateInternalPartialCompilationFile(IGrouping<string, IEnumerable<MethodDeclarationSyntax>> group)
        {
            var className = group.Key;

            var classSyntax = SyntaxFactory.ClassDeclaration(className)
                .AddModifiers(AccessModifierFactory.Create(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword, SyntaxKind.PartialKeyword))
                .AddMembers(CreateMemberDeclarationSyntax(group));

            return new(
                className,
                new[] { Using.System, Using.Tasks },
                PutIntoNamespace(classSyntax));
        }

        private static MemberDeclarationSyntax[] CreateMemberDeclarationSyntax(IEnumerable<IEnumerable<MethodDeclarationSyntax>> methods)
        {
            return methods.SelectMany(method => method).ToArray<MemberDeclarationSyntax>();
        }

        private static NamespaceDeclarationSyntax PutIntoNamespace(ClassDeclarationSyntax member)
        {
            return SyntaxFactory
                .NamespaceDeclaration(Using.ResultsName)
                .AddMembers(member);
        }

        private CompilationUnit CreateCompilationUnit(CompilationFile compilationFile)
        {
            var (fileName, usingDirectiveSyntax, memberDeclarationSyntax) = compilationFile;

            var compilationUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(usingDirectiveSyntax.Select(u => u).ToArray())
                .AddMembers(memberDeclarationSyntax)
                .NormalizeWhitespace();

            var indentationFixed = this.typeParameterGenericMethodSyntaxGenerator.FixMethodTypeParameterIndentation(compilationUnit);
            return new(fileName, indentationFixed);
        }

        private CompilationFile CreateGlobalExtensionCompilationFile(IEnumerable<MemberDeclarationSyntax> methods)
        {
            var className = this.compilationFileProviderSettings.GlobalExtensionsFileName;
            var classSyntax = SyntaxFactory.ClassDeclaration(className)
                .AddAttributeLists(EditorBrowsableAttributeList)
                .AddModifiers(ExtensionMethodAccessModifiers)
                .AddMembers(methods.ToArray());

            return new(
                className,
                new[] { Using.System, Using.ComponentModel, Using.Contracts, Using.Tasks, Using.Results },
                classSyntax.WithLeadingTrivia(SyntaxFactory.Comment("// ReSharper disable once CheckNamespace")));
        }

        private CompilationFile CreateLocalExtensionCompilationFile(IEnumerable<MemberDeclarationSyntax> methods)
        {
            var className = this.compilationFileProviderSettings.LocalExtensionsFileName;
            var classSyntax = SyntaxFactory.ClassDeclaration(className)
                .AddAttributeLists(EditorBrowsableAttributeList)
                .AddModifiers(ExtensionMethodAccessModifiers)
                .AddMembers(methods.ToArray());

            return new(
                className,
                new[] { Using.System, Using.ComponentModel, Using.Contracts, Using.Tasks },
                PutIntoNamespace(classSyntax));
        }

        private record CompilationFile(
            string FileName,
            IEnumerable<UsingDirectiveSyntax> UsingDirectives,
            MemberDeclarationSyntax Class);
    }
}