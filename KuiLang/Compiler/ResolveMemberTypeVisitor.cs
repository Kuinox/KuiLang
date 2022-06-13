using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public class ResolveMemberTypeVisitor : SymbolVisitor<object>
    {
        protected override object Visit( MethodSymbol symbol )
        {
            var typeName = symbol.SymbolAst.ReturnType;
            var type = ResolveTypeSymbol( symbol.Parent, typeName );
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

        protected override object Visit( VariableSymbol symbol )
        {
            var typeName = symbol.SymbolAst.SignatureType;
            var type = ResolveTypeSymbol( symbol, typeName );
            symbol.Type = type;
            return base.Visit( symbol );
        }

        protected override object Visit( MethodCallExpressionSymbol symbol )
        {
            symbol.TargetMethod = ResolveMethodSymbol(symbol, symbol.SymbolAst.FunctionToCall);
            return base.Visit( symbol );
        }


        // Name Resolution is very naive right now.

        static MethodSymbol ResolveMethodSymbol( ISymbol symbol, FieldLocation methodName )
        {
            //first we must crawl up to the type def.

            var type = ResolveType( symbol );
            if( methodName.Parts.Length == 1 )
            {
                return type.Methods[methodName.Parts.Span[0]];
            }
            if( methodName.Parts.Length > 2 ) throw new InvalidOperationException( "Complex full name not supported yet." );
            return type.Parent.TypesSymbols[methodName.Parts.Span[0]].Methods[methodName.Parts.Span[1]];

            static TypeSymbol ResolveType( ISymbol symbol )
            {
                if( symbol.Parent is MethodSymbol method ) return method.Parent;
                return ResolveType( symbol.Parent! );
            }
        }

        static TypeSymbol ResolveTypeSymbol( IStatementSymbol symbol, FieldLocation typeName )
        {
            if( symbol.Parent.Value is MethodSymbol method ) return ResolveTypeSymbol( method.Parent, typeName );
            return ResolveTypeSymbol( (IStatementSymbol)symbol.Parent.Value, typeName );
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
