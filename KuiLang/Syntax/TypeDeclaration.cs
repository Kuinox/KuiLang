using System.Collections.Generic;

namespace KuiLang
{
    public class TypeDeclaration
    {
        public TypeDeclaration(bool isInterface, FieldLocation? accessLevel, string typeName, List<MethodOrFieldDeclaration> fields)
        {
            IsInterface = isInterface;
            AccessLevel = accessLevel;
            TypeName = typeName;
            Fields = fields;
        }

        public bool IsInterface { get; }
        public FieldLocation? AccessLevel { get; }
        public string TypeName { get; }
        public IReadOnlyCollection<MethodOrFieldDeclaration> Fields { get; }
    }
}