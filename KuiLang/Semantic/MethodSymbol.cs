using KuiLang.Syntax;

namespace KuiLang.Semantic
{
    public class MethodSymbol : ISymbol
    {
        public MethodSymbol(Ast.Statement.Definition.Method method)
        {
            Method = method;
        }

        public Ast.Statement.Definition.Method Method { get; }
    }
}