using KuiLang.Compiler.Symbols;
using System.Collections.Generic;

namespace KuiLang.Semantic
{
    public interface ISymbolWithMethods : ISymbol
    {
        Dictionary<string, MethodSymbol> Methods { get; }
    }
}
