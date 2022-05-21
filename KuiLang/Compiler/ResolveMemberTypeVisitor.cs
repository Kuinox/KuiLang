using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public class ResolveReferenceVisitor : SymbolVisitor<object>
    {
        protected override object Visit( MethodSymbol symbol )
        {
            var typeName = symbol.SymbolAst.ReturnType;
            var type = ResolveTypeSymbol( symbol.Parent, typeName);
            symbol.ReturnType = type;
            return base.Visit( symbol );
        }

        protected override object Visit( MethodParameterSymbol symbol )
        {
            var typeName = symbol.SymbolAst.SignatureType;
            var type = ResolveTypeSymbol( symbol.Parent.Parent, typeName );
            symbol.Type = type;
            return base.Visit( symbol );
        }

        protected override object Visit( FieldSymbol symbol )
        {
            var typeName = symbol.SymbolAst.SignatureType;
            var type = ResolveTypeSymbol( symbol.Parent, typeName );
            symbol.Type = type;
            return base.Visit( symbol );
        }

        static TypeSymbol ResolveTypeSymbol( TypeSymbol parent, FieldLocation typeName )
        {
            if( typeName.Parts.Length > 1 ) throw new NotSupportedException( "Right now we do not support namespace or nested types." );
            var curr = typeName.SubParts.Parts.Span[0];
            if( parent.Name == curr ) return parent;
            return parent.Parent.TypesSymbols[curr];
        }
    }
}
