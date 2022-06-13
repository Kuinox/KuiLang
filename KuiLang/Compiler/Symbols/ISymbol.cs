using KuiLang.Syntax;

namespace KuiLang.Semantic
{
    public interface ISymbol
    {
        ISymbol? Parent { get; }
    }
}
