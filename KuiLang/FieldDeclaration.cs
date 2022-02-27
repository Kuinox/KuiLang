using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class FieldDeclaration
    {
        public FieldDeclaration(FieldLocation returnType, string fieldName)
        {
            Type = returnType;
            Name = fieldName;
        }

        public FieldLocation Type { get; }
        public string Name { get; }
    }
}
