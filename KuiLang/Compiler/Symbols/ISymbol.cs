using KuiLang.Syntax;

namespace KuiLang.Semantic
{
    public interface ISymbol<T> where T : Ast
    {
        public T SymbolAst { get; }
    }
}
