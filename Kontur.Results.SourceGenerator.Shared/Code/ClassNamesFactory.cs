namespace Kontur.Results.SourceGenerator.Code
{
    internal class ClassNamesFactory
    {
        private readonly string methodName;
        private readonly string parameterTypes;

        internal ClassNamesFactory(string methodName, string parameterTypes)
        {
            this.methodName = methodName;
            this.parameterTypes = parameterTypes;
        }

        internal string Value => this.Create(nameof(this.Value));

        internal string Factory => this.Create(nameof(this.Factory));

        protected string Create(string suffix)
        {
            return $"{this.methodName}{this.parameterTypes}{suffix}Internal";
        }
    }
}
