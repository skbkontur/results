using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal sealed class MethodDocumentation
    {
        private MethodDocumentation(SyntaxTriviaList trivia) => this.Trivia = trivia;

        internal SyntaxTriviaList Trivia { get; }

        internal static MethodDocumentation Create(params XmlNodeSyntax[] content) => new(CreateTrivia(content));

        internal static XmlNodeSyntax SummaryElement(string text) => SyntaxFactory.XmlSummaryElement(SyntaxFactory.XmlText(text));

        internal static XmlNodeSyntax ReturnsElement(string text) => SyntaxFactory.XmlReturnsElement(SyntaxFactory.XmlText(text));

        internal static XmlNodeSyntax CodeExampleElement(params string[] lines)
        {
            var tokens = lines.SelectMany(line => new[]
            {
                SyntaxFactory.XmlTextNewLine(
                    SyntaxFactory.TriviaList(),
                    "\n",
                    "\n",
                    SyntaxFactory.TriviaList()),
                SyntaxFactory.XmlTextLiteral(
                    SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("/// ")),
                    " " + line,
                    " " + line,
                    SyntaxFactory.TriviaList()),
            });
            var multiLineXmlText = SyntaxFactory.XmlText(SyntaxFactory.TokenList(tokens));
            return SyntaxFactory.XmlExampleElement(
                SyntaxFactory.XmlElement(
                    SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName("code")),
                    SyntaxFactory.SingletonList<XmlNodeSyntax>(multiLineXmlText),
                    SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName("code"))));
        }

        internal static SyntaxTriviaList CreateTrivia(params XmlNodeSyntax[] content)
        {
            var exteriorPrefix = SyntaxFactory.XmlText(
                SyntaxFactory.XmlTextLiteral(
                    SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("/// ")),
                    " ",
                    " ",
                    SyntaxFactory.TriviaList()));

            var newLineWithExterior = SyntaxFactory.XmlText(
                SyntaxFactory.XmlTextNewLine(
                    SyntaxFactory.TriviaList(),
                    "\n",
                    "\n",
                    SyntaxFactory.TriviaList()),
                SyntaxFactory.XmlTextLiteral(
                    SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior("/// ")),
                    " ",
                    " ",
                    SyntaxFactory.TriviaList()));

            var trailingNewLine = SyntaxFactory.XmlText(
                SyntaxFactory.XmlTextNewLine(
                    SyntaxFactory.TriviaList(),
                    "\n",
                    "\n",
                    SyntaxFactory.TriviaList()));

            var nodes = content
                .SelectMany((node, i) => i == 0
                    ? new XmlNodeSyntax[] { exteriorPrefix, node }
                    : new[] { newLineWithExterior, node })
                .Append(trailingNewLine);

            var doc = SyntaxFactory.DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                SyntaxFactory.List(nodes));
            return SyntaxFactory.TriviaList(SyntaxFactory.Trivia(doc));
        }
    }
}
