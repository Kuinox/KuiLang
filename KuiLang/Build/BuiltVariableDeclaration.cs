namespace KuiLang.Build
{
    public class BuiltVariableDeclaration
    {
        public BuiltVariableDeclaration(BuiltType variableType, string variableName, BuiltExpression? initValue)
        {
            VariableType = variableType;
            VariableName = variableName;
            InitValue = initValue;
        }

        public BuiltType VariableType { get; }
        public string VariableName { get; }
        public BuiltExpression? InitValue { get; }
    }
}