using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Kontur.Results.SourceGenerator.Code
{
    internal class SelfParameter
    {
        internal SelfParameter(string name)
        {
            this.Name = SyntaxFactory.Identifier(name);
            this.TaskName = SyntaxFactory.Identifier(name + "Task");
        }

        public SyntaxToken Name { get; }

        public SyntaxToken TaskName { get; }
    }
}
