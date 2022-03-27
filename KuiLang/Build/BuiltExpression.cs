namespace KuiLang.Build
{
    public class BuiltExpression
    {
        public BuiltField? Field { get; }
        public BuiltFunctionCall? FunctionCall { get; }

        public BuiltExpression(BuiltField? field)
        {
            FieldLocation = field;
        }


        public BuiltExpression(BuiltFunctionCall? functionCall)
        {
            FunctionCall = functionCall;
        }
    }
}