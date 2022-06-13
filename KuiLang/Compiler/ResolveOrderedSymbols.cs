using KuiLang.Compiler.Symbols;
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
        protected override TypeSymbol Visit( IExpressionSymbol statement ) => (TypeSymbol)base.Visit( statement );
        protected override TypeSymbol Visit( AddExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidProgramException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( DivideExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidProgramException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( SubtractExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidProgramException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( MultiplyExpressionSymbol symbol )
        {
            var typeLeft = Visit( symbol.Left );
            var typeRight = Visit( symbol.Right );
            if( typeLeft != typeRight ) throw new InvalidProgramException();
            symbol.ReturnType = typeLeft;
            return typeLeft;
        }

        protected override object Visit( NumberLiteralSymbol numberLiteral ) => numberLiteral.ReturnType;

        protected override object Visit( MethodCallExpressionSymbol symbol ) => symbol.ReturnType;

        protected override object Visit( FieldReferenceExpressionSymbol symbol ) => symbol.ReturnType;

     

    }
}
