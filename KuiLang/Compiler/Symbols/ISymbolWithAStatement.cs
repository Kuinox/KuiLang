using KuiLang.Semantic;
using KuiLang.Syntax;

namespace KuiLang.Compiler.Symbols
{
    public interface ISymbolWithAStatement : ISymbol
    {
        public IStatementSymbol Statement { get; set; }
    }
}
