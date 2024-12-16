using System.Collections.Generic;

namespace launchpad
{
    internal class ConsoleParameters
    {
        private readonly Dictionary<string, List<string>> namedParameters = new Dictionary<string, List<string>>();
        private readonly List<string> mainParameters = new List<string>();

        public ConsoleParameters(string[] arguments)
        {
            if (arguments == null)
                return;

            var current = string.Empty;

            foreach (var argument in arguments)
            {
                if (argument.StartsWith("-"))
                {
                    namedParameters.Add(argument, new List<string>());
                    current = argument;
                }
                else
                {
                    if (string.IsNullOrEmpty(current))
                        mainParameters.Add(argument);
                    else
                    {
                        namedParameters[current].Add(argument);
                        current = string.Empty;
                    }
                }
            }
        }

        public int NamedParameterssCount => namedParameters.Count;

        public int MainParametersCount => mainParameters.Count;

        public bool HasParameter(string param)
        {
            return namedParameters.ContainsKey(param);
        }

        public bool HasParameterWithValue(string param)
        {
            return namedParameters.ContainsKey(param) && namedParameters[param].Count > 0;
        }

        public string GetValue(string name)
        {
            return GetValue(name, 0);
        }

        public string GetValue(string name, string defaultValue)
        {
            return HasParameterWithValue(name) ? GetValue(name) : defaultValue;
        }

        public string GetValue(string name, int index)
        {
            return namedParameters[name][index];
        }

        public List<string> GetValues(string name)
        {
            return namedParameters[name];
        }

        public string GetMainValue(int index)
        {
            return mainParameters[index];
        }

        public List<string> GetMainValues()
        {
            return mainParameters;
        }

        public int GetValuesCount(string name)
        {
            return namedParameters[name].Count;
        }
    }
}