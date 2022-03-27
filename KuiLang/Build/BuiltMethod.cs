using System.Collections.Generic;

namespace KuiLang.Build
{
    public class BuiltMethod
    {
        public BuiltMethod(string fullName, bool isStatic, BuiltType returnType, BuiltType typeOwner)
        {
            FullName = fullName;
            IsStatic = isStatic;
            ReturnType = returnType;
        }

        public BuiltType TypeOwner { get; }
        public string FullName { get; }
        public bool IsStatic { get; set; }
        public BuiltType ReturnType { get; set; } = null!;
        public ICollection<BuiltStatement> Statements { get; set; }
    }
}