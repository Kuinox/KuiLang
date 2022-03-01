namespace KuiLang
{
    public class MethodOrFieldDeclaration
    {

        public MethodOrFieldDeclaration(MethodDeclaration methodDeclaration)
        {
            MethodDeclaration = methodDeclaration;
        }

        public MethodOrFieldDeclaration(FieldDeclaration fieldDeclaration)
        {
            FieldDeclaration = fieldDeclaration;
        }

        public MethodDeclaration? MethodDeclaration { get; }
        public FieldDeclaration? FieldDeclaration { get; }
    }
}