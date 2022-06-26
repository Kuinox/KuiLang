using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Expression.Literal;

namespace KuiLang.Compiler
{
    public static class SymbolHelpers
    {

        /// <summary>
        /// return the statement that contain this expression
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static StatementSymbol FindContainingStatement( this IExpression symbol )
        {
            var parent = symbol.Parent;
            return parent is IExpression expr ? FindContainingStatement( expr ) : (StatementSymbol)parent;
        }

        public static TypeSymbol FindType( this ProgramRootSymbol root, Identifier typeIdentifier )
        {
            var curr = typeIdentifier.SubParts.Parts.Span[0]; // no namespace/subtype yet.
            return root.TypesSymbols[curr];
        }

        public static TypeSymbol FindType( this ISymbol symbol, Identifier typeName )
        {
            if( typeName.TryGetLanguageType( out var typeSymbol ) ) return typeSymbol;
            if( symbol.Parent is MethodSymbol method ) return method.Parent.FindType( typeName );
            return FindType( symbol.Parent!, typeName );
        }

        public static TypeSymbol FindType( this TypeSymbol parent, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, out var typeSymbol ) ) return typeSymbol;
            if( typeIdentifier.Parts.Length > 1 ) throw new NotSupportedException( "Right now we do not support namespace or nested types." );
            var curr = typeIdentifier.SubParts.Parts.Span[0];
            if( parent.Name == curr ) return parent;
            return FindType( (ISymbol)parent.Parent, typeIdentifier );
        }

        public static TypeSymbol FindType( this MethodSymbol symbol, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, out var typeSymbol ) ) return typeSymbol;
            return symbol.Parent switch
            {
                ProgramRootSymbol parent => parent.FindType( typeIdentifier ),
                TypeSymbol parent => parent.FindType( typeIdentifier ),
                _ => throw new InvalidOperationException( "Unknown parent type." )
            };
        }

        static readonly TypeSymbol _number = new( null!, new( "number", new List<Ast.Statement.Definition>() ) );

        public static bool TryGetLanguageType( this Identifier typeName, [NotNullWhen( true )] out TypeSymbol? typeSymbol )
        {
            typeSymbol = null;
            if( typeName.Parts.Length == 1 && typeName.Name == "number" )
            {
                typeSymbol = _number;
                return true;
            }
            return false;
        }

        public static MethodSymbol FindMethod( this ISymbol symbol, Identifier methodIdentifier )
        {
            //first we must crawl up to the type def.
            var hasMethods = ResolveType( symbol );
            if( methodIdentifier.Parts.Length == 1 )
            {
                return hasMethods.Methods[methodIdentifier.Parts.Span[0]];
            }
            // RootMetods cannot be referenced outside of itself.
            var targetedType = symbol.FindType( methodIdentifier.ParentLocation );

            return targetedType.Methods[methodIdentifier.Name];

            static ISymbolWithMethods ResolveType( ISymbol symbol )
            {
                if( symbol.Parent is ISymbolWithMethods parent ) return parent;
                return ResolveType( symbol.Parent! );
            }
        }


        public static ITypedSymbol FindIdentifierValueDeclaration( this IdentifierValueExpressionSymbol symbol, Identifier identifier )
        {
            if( identifier.Parts.Length == 1 ) //if there is multiple it must be a field.
            {
                var statement = symbol.FindContainingStatement();
                var varDec = (VariableSymbol)statement.CrawlStatements( ( s ) => s is VariableSymbol v && v.Name == identifier.Name )!;
                if( varDec is not null ) return varDec;
            }
            var type = symbol.FindType( identifier.ParentLocation );
            return type.Fields[identifier.Name];
        }

        public static StatementSymbol? CrawlStatements( this StatementSymbol symbol, Func<StatementSymbol, bool> crawler, bool backward = false )
        {
            if( symbol.Parent is not StatementSymbol statement ) return null;
            if( symbol.Parent is not StatementBlockSymbol block ) return statement.CrawlStatements( crawler, backward );

            var idx = block.Statements.IndexOf( symbol );
            if( backward )
            {
                for( int i = idx - 1; i >= 0; i++ )
                {
                    if( crawler( block.Statements[i] ) ) return block.Statements[i];
                }
            }
            else
            {
                for( int i = idx; idx < block.Statements.Count; i++ )
                {
                    if( crawler( block.Statements[i] ) ) return block.Statements[i];
                }
            }
            return block.CrawlStatements( crawler, backward );
        }
    }
}
