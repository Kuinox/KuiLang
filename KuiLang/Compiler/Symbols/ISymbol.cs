using Farkle.Grammar;
using KuiLang.Syntax;
using System.Collections.Generic;

namespace KuiLang.Semantic
{
    public interface ISymbol
    {
        public ISymbol? Parent { get; }
    }
}
