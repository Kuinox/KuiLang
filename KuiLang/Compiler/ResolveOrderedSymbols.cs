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
        protected override TypeSymbol Visit( AddExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidOperationException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( DivideExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidOperationException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( SubtractExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidOperationException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( MultiplyExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidOperationException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( NumberLiteralSymbol numberLiteral ) => numberLiteral.ReturnType;

        protected override object Visit( MethodCallExpressionSymbol symbol ) => symbol.ReturnType;

        protected override object Visit( IdentifierValueExpressionSymbol symbol ) => symbol.ReturnType;

    }
}
