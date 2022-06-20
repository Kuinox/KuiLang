using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public class ResolveMemberTypeVisitor : SymbolVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;

        public ResolveMemberTypeVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        protected override object Visit( MethodSymbol symbol )
        {
            var typeName = symbol.Ast.ReturnType;
            symbol.ReturnType = ResolveTypeSymbol( symbol, typeName );
            return base.Visit( symbol );
        }

        protected override object Visit( MethodParameterSymbol symbol )
        {
            var typeName = symbol.Ast.SignatureType;
            var type = ResolveTypeSymbol( symbol.Parent, typeName );
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
            var typeName = symbol.Ast.SignatureType;
            var type = ResolveTypeSymbol( symbol, typeName );
            symbol.Type = type;
            return base.Visit( symbol );
        }

        protected override object Visit( MethodCallExpressionSymbol symbol )
        {
            symbol.TargetMethod = ResolveMethodSymbol( symbol, symbol.SymbolAst.FunctionToCall );
            return base.Visit( symbol );
        }


        // Name Resolution is very naive right now.

        static MethodSymbol ResolveMethodSymbol( ISymbol symbol, FieldLocation methodName )
        {
            //first we must crawl up to the type def.

            var hasMethods = ResolveType( symbol );
            if( methodName.Parts.Length == 1 )
            {
                return hasMethods.Methods[methodName.Parts.Span[0]];
            }
            // RootMetods cannot be referenced outside of itself.
            var targetedType = ResolveTypeSymbol( symbol, methodName.ParentLocation );

            return targetedType.Methods[methodName.FieldName];

            static ISymbolWithMethods ResolveType( ISymbol symbol )
            {
                if( symbol.Parent is ISymbolWithMethods parent ) return parent;
                return ResolveType( symbol.Parent! );
            }
        }
        static bool TryGetLanguageType( FieldLocation typeName, [NotNullWhen( true )] out TypeSymbol? typeSymbol )
        {
            typeSymbol = null;
            if( typeName.Parts.Length == 1 && typeName.FieldName == "number" )
            {
                typeSymbol = _number;
                return true;
            }
            return false;
        }
        static TypeSymbol ResolveTypeSymbol( ISymbol symbol, FieldLocation typeName )
        {
            if( TryGetLanguageType( typeName, out var typeSymbol ) ) return typeSymbol;
            if( symbol.Parent is MethodSymbol method ) return ResolveTypeSymbol( method.Parent, typeName );
            return ResolveTypeSymbol( symbol.Parent!, typeName );
        }

        static readonly TypeSymbol _number = new( null!, new( "number", new List<Ast.Statement.Definition>() ) );
        static TypeSymbol ResolveTypeSymbol( MethodSymbol symbol, FieldLocation typeName )
        {
            if( TryGetLanguageType( typeName, out var typeSymbol ) ) return typeSymbol;
            return symbol.Parent switch
            {
                ProgramRootSymbol parent => ResolveTypeSymbol( parent, typeName ),
                TypeSymbol parent => ResolveTypeSymbol( parent, typeName ),
                _ => throw new InvalidOperationException( "Unknown parent type." )
            };
        }

        static TypeSymbol ResolveTypeSymbol( TypeSymbol parent, FieldLocation typeName )
        {
            if( TryGetLanguageType( typeName, out var typeSymbol ) ) return typeSymbol;
            if( typeName.Parts.Length > 1 ) throw new NotSupportedException( "Right now we do not support namespace or nested types." );
            var curr = typeName.SubParts.Parts.Span[0];
            if( parent.Name == curr ) return parent;
            return ResolveTypeSymbol( parent.Parent, typeName );
        }

        static TypeSymbol ResolveTypeSymbol( ProgramRootSymbol root, FieldLocation typeName )
        {
            var curr = typeName.SubParts.Parts.Span[0]; // no namespace/subtype yet.
            return root.TypesSymbols[curr];
        }

    }
}
