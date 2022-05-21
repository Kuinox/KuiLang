using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public interface ISymbolWithAStatement
    {
        public IStatementSymbol Statement { get; set; }
    }
}
