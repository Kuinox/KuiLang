using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Visitors.Runtime
{
    public class RuntimeObject
    {
        public TypeSymbol Type { get; }

        public RuntimeObject(TypeSymbol type)
        {
            Type = type;
        }

        public Scope Scope { get; } = new();
    }
}
