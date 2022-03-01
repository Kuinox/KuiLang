namespace KuiLang
{
    public class VariableDeclaration
    {

        public VariableDeclaration(FieldLocation type, string variableName, Expression? initValue)
        {
            Type = type;
            Name = variableName;
            InitValue = initValue;
        }

        public FieldLocation Type { get; }
        public string Name { get; }
        public Expression? InitValue { get; }
    }
}