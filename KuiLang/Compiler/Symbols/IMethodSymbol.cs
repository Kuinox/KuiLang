using KuiLang.Semantic;

namespace KuiLang.Compiler.Symbols
{
    public interface IMethodSymbol : ISymbol
    {
        OrderedDictionary<string, MethodParameterSymbol> ParameterSymbols { get; }
    }
}
