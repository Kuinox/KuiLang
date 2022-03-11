using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class FieldDeclaration
    {
        public FieldDeclaration(FieldLocation? accessModifier, FieldLocation returnType, string fieldName)
        {
            AccessModifier = accessModifier;
            Type = returnType;
            Name = fieldName;
        }

        public FieldLocation? AccessModifier { get; }
        public FieldLocation Type { get; }
        public string Name { get; }
    }
}
