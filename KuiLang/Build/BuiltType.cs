using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Build
{
    public class BuiltType
    {
        public BuiltType(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; set; }
        public IReadOnlyDictionary<string, BuiltMethod> Methods { get; set; }
        public IReadOnlyDictionary<string, BuiltField> Fields { get; set; }
    }
}
