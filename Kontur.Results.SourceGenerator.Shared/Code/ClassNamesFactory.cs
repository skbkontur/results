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

        internal string Value => Create(nameof(Value));

        internal string Factory => Create(nameof(Factory));

        protected string Create(string suffix)
        {
            return $"{methodName}{parameterTypes}{suffix}Internal";
        }
    }
}
