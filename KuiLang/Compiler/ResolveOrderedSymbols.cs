using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    class ResolveOrderedSymbols : SymbolVisitor<object>
    {
        DiagnosticChannel _diagnostics;

        public ResolveOrderedSymbols( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        protected override TypeSymbol Visit( IExpression statement ) => (TypeSymbol)base.Visit( statement );

        protected override object Visit( NumberLiteralSymbol numberLiteral ) => numberLiteral.ReturnType;

        protected override object Visit( FunctionCallExpressionSymbol symbol ) => symbol.ReturnType;

        protected override object Visit( IdentifierValueExpressionSymbol symbol ) => symbol.ReturnType;

    }
}
