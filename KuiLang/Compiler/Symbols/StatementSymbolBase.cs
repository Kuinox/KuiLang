using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;

namespace KuiLang.Compiler.Symbols
{
    public abstract class StatementSymbolBase<T> : SymbolBase<T> where T : Ast.Statement
    {
        public StatementSymbolBase( OneOf<ISymbolWithAStatement, StatementBlockSymbol> parent, T symbolAst ) : base( symbolAst )
        {
            Parent = parent;
        }

        public OneOf<ISymbolWithAStatement, StatementBlockSymbol> Parent { get; }
    }
}
