using System;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vostok.Tools.Publicize.Roslyn
{
    /// <summary>
    /// MsBuild task type
    /// </summary>
    public class PublicizeRoslyn : Task
    {
        /// <summary>
        /// Entry point of MsBuild task
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            var rewriter = new Rewriter(PublicApiAttributes, Log.LogMessage);
            var destinationAsPath = new Uri(DestinationDirectory).AbsolutePath;
            if (!destinationAsPath.EndsWith("/"))
                destinationAsPath += "/";
            var projDir = new Uri(Path.GetFullPath(ProjectDirectory));
            foreach (var file in SourceFiles)
            {
                var relativePath = projDir.MakeRelativeUri(new Uri(Path.GetFullPath(file)));
                var destination = Uri.UnescapeDataString(Path.Combine(destinationAsPath, relativePath.ToString()));
                var before = File.ReadAllText(file);
                var after = Publicizer.Process(before, rewriter);
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
                if (!string.Equals(before, after, StringComparison.Ordinal))
                    Log.LogMessage("Successfully publicized members in file '{0}'", relativePath);
                File.WriteAllText(destination, after, Encoding.UTF8);
            }

            return true;
        }
        
        /// <summary>
        /// $(ProjectDir)
        /// </summary>
        [Required] public string ProjectDirectory { get; set; }
        /// <summary>
        /// *.cs files to publicize
        /// </summary>
        [Required] public string[] SourceFiles { get; set; }
        /// <summary>
        /// $(ProjectDir)obj\
        /// </summary>
        [Required] public string DestinationDirectory { get; set; }
        /// <summary>
        /// PublicAPI
        /// </summary>
        [Required] public string[] PublicApiAttributes { get; set; }
    }

    [StringFormatMethod("format")]
    delegate void Logger(string format, params object[] args);

    internal static class Publicizer
    {
        public static string Process(string sourceText, Rewriter rewriter=null)
        {
            var tree = CSharpSyntaxTree.ParseText(sourceText, new CSharpParseOptions(LanguageVersion.Latest));
            var newNode = (rewriter ?? new Rewriter(new[] {"PublicAPI"}, Console.WriteLine)).Visit(tree.GetRoot());
            return newNode.ToString();
        }
    }

    internal class Rewriter : CSharpSyntaxRewriter
    {
        private readonly string[] publicApiAttributes;
        private readonly Logger logger;

        public Rewriter(string[] publicApiAttributes, Logger logger)
        {
            this.publicApiAttributes = publicApiAttributes;
            this.logger = logger;
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitMethodDeclaration(node);
            logger("Publicize method '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitConstructorDeclaration(node);
            logger("Publicize constructor '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitPropertyDeclaration(node);
            logger("Publicize property '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitEventDeclaration(node);
            logger("Publicize event '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitDelegateDeclaration(node);
            logger("Publicize delegate '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitConversionOperatorDeclaration(node);
            logger("Publicize conversion operator '{0}'", node.OperatorKeyword.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitFieldDeclaration(node);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitClassDeclaration(node);
            logger("Publicize class '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitStructDeclaration(node);
            logger("Publicize struct '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitInterfaceDeclaration(node);
            logger("Publicize interface '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (!ShouldPublicize(node.AttributeLists))
                return base.VisitEnumDeclaration(node);
            logger("Publicize enum '{0}'", node.Identifier.Text);
            return node
                .WithModifiers(MakePublic(node.Modifiers));
        }

        private static SyntaxTokenList MakePublic(SyntaxTokenList list)
        {
            var firstToken = list.FirstOrDefault();

            var publicKeyword = SyntaxFactory
                .Token(SyntaxKind.PublicKeyword)
                .WithLeadingTrivia(firstToken.LeadingTrivia)
                .WithTrailingTrivia(SyntaxFactory.Space);
         
            var newList = SyntaxTokenList.Create(publicKeyword);

            foreach (var token in list)
            {
                switch (token.ValueText)
                {
                    case "internal":
                    case "protected":
                    case "private":
                    case "public":
                        continue;
                    default:
                        newList = newList.Add(token);
                        break;
                }
            }

            return newList;
        }

        private bool ShouldPublicize(SyntaxList<AttributeListSyntax> list)
        {
            foreach (var attributeListSyntax in list)
            {
                foreach (var attribute in attributeListSyntax.Attributes)
                {
                    var name = attribute.Name.ToString();
                    if (publicApiAttributes.Contains(name, StringComparer.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }
    }
}