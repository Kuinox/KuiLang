using System.Collections.Generic;

namespace KuiLang
{
    public class TypeDeclaration
    {
        public TypeDeclaration(FieldLocation? accesLevel, string typeName, List<MethodOrFieldDeclaration> fields)
        {
            AccesLevel = accesLevel;
            TypeName = typeName;
            Fields = fields;
        }

        public FieldLocation? AccesLevel { get; }
        public string TypeName { get; }
        public IReadOnlyCollection<MethodOrFieldDeclaration> Fields { get; }
    }
}