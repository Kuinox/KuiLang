using KuiLang.Syntax;

namespace KuiLang.Semantic
{
    public abstract class SymbolBase<T> : ISymbolBase<T> where T : Ast
    {
        protected SymbolBase( T symbolAst )
        {
            SymbolAst = symbolAst;
        }
        public T SymbolAst { get; }
    }

    public interface ISymbolBase<T> where T : Ast
    {
        public T SymbolAst { get; }
    }
}
