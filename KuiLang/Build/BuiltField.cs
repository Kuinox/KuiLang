namespace KuiLang.Build
{
    public class BuiltField
    {
        public BuiltField(ContextScope scope, string fieldName)
        {
            Scope = scope;
            FieldName = fieldName;
        }

        public ContextScope Scope { get; }
        public string FieldName { get; }
    }
}