using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Interpreter
{
    public class RuntimeObject
    {
        public TypeSymbol Type { get; }

        public RuntimeObject( TypeSymbol type )
        {
            Type = type;
        }

        public Dictionary<ISymbol, object> Fields { get; } = new();
    }
}
