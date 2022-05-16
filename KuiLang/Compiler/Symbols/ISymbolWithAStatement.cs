using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public interface ISymbolWithAStatement
    {
        public StatementSymbolBase<Ast.Statement> Statement { get; set; }
    }
}
