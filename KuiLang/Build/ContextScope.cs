using System.Collections.Generic;

namespace KuiLang.Build
{
    public class ContextScope
    {
        public IReadOnlyDictionary<string,BuiltField> Fields { get; set; }
    }
}