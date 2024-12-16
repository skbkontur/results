using System;
using System.Collections.Generic;
using System.IO;

namespace launchpad
{
    internal class VariableFiller
    {
        public Dictionary<string, string> FillVariables(IEnumerable<VariableDefinition> definitions)
        {
            Console.WriteLine(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "launchpad-logo.txt")));

            var variables = new Dictionary<string, string>();

            foreach (var variableDefinition in definitions)
            {
                if(!variables.TryAdd(variableDefinition.Name, string.Empty))
                    throw new ArgumentException($"Variable '${variableDefinition.Name}' is defined twice.");

                Console.Out.WriteLine($"Enter value for '{variableDefinition.Name}' ({variableDefinition.Description}):");

                variables[variableDefinition.Name] = Console.ReadLine();

                Console.Out.WriteLine();
            }

            return variables;
        }
    }
}
