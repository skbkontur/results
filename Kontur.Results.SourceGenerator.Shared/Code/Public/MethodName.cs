namespace Kontur.Results.SourceGenerator.Code.Public
{
    internal sealed class MethodName
    {
        internal MethodName(string name, MethodDocumentation documentation)
        {
            this.Name = name;
            this.Documentation = documentation;
        }

        internal string Name { get; }

        internal MethodDocumentation Documentation { get; }
    }
}
