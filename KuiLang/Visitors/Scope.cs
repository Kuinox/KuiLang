using System;
using System.Collections.Generic;

namespace KuiLang.Visitors
{
    public class Scope
    {
        readonly Dictionary<string, object?> _variables = new();
        public void AddVariable(FieldLocation type, string name)
        {
            _variables.Add(name, null);
        }

        public void SetVariable(string name, object v)
        {
            if (!_variables.ContainsKey(name)) throw new InvalidOperationException("Trying to assign an undeclared variable");
            _variables[name] = v;
        }

        public object GetVariableValue(string name)
        {
            if (!_variables.TryGetValue(name, out object? val)) throw new InvalidOperationException("Trying to assign an undeclared variable");
            if (val == null) throw new InvalidOperationException("Accessing unassigned variable.");
            return val;
        }

        public object? TryGetVariable(string name) => _variables.GetValueOrDefault(name);
    }
}