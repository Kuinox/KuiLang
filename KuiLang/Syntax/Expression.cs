namespace KuiLang
{
    public class Expression
    {
        public Expression(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }

        public Expression(FieldLocation fieldLocation)
        {
            FieldLocation = fieldLocation;
        }

        public FieldLocation? FieldLocation { get; }
        public FunctionCall? FunctionCall { get; }
    }
}