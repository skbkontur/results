namespace Kontur.Results.SourceGenerator.Code
{
    internal class ClassNamesSwitch : ClassNamesPass
    {
        internal ClassNamesSwitch(string methodName, string parameterTypes)
        : base(methodName, parameterTypes)
        {
        }

        internal string FactoryNext => this.Create(nameof(this.FactoryNext));

        internal string FactoryBoth => this.Create(nameof(this.FactoryBoth));
    }
}
