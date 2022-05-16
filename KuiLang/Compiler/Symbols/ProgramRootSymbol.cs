using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public class ProgramRootSymbol : SymbolBase<Ast>
    {
        readonly Dictionary<string, TypeSymbol> _typesSymbols = new();

        public ProgramRootSymbol( Ast symbolAst ) : base( symbolAst )
        {
        }

        public IReadOnlyDictionary<string, TypeSymbol> TypesSymbols => _typesSymbols;

        public void Add( TypeSymbol symbol ) => _typesSymbols.Add( symbol.Name, symbol );
    }
}
