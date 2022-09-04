using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
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
        public static StatementSymbol GetContainingStatement( this IExpressionSymbol symbol )
        {
            var parent = symbol.Parent;
            return parent is IExpressionSymbol expr ?
                GetContainingStatement( expr ) : (StatementSymbol)parent;
        }

        public static IMethodSymbol GetContainingFunction( this ISymbol symbol )
        {
            if( symbol.Parent is IMethodSymbol method ) return method;
            return GetContainingFunction( symbol.Parent! );
        }

        public static ISymbolWithMethods GetContainingMethodHolder( this ISymbol symbol )
        {
            if( symbol.Parent is ISymbolWithMethods s ) return s;
            return GetContainingMethodHolder( symbol.Parent );
        }

        public static TypeSymbol FindType( this ProgramRootSymbol root, Identifier typeIdentifier )
            => root.TypesSymbols[typeIdentifier.Name];// no namespace/subtype yet.

        public static TypeSymbol? FindType( this ISymbol symbol, Identifier typeIdentifier )
        {
            if( typeIdentifier.TryGetLanguageType( out var typeSymbol ) ) return typeSymbol;
            if( symbol.Parent is null ) return null;
            if( symbol.Parent is ProgramRootSymbol root ) return root.FindType( typeIdentifier );
            if( symbol.Parent is MethodSymbol method ) return method.Parent.FindType( typeIdentifier );
            return FindType( symbol.Parent!, typeIdentifier );
        }

        public static TypeSymbol? FindType( this TypeSymbol parent, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, out var typeSymbol ) ) return typeSymbol;
            if( typeIdentifier.Parts.Length > 1 ) throw new NotSupportedException( "Right now we do not support namespace or nested types." );
            var curr = typeIdentifier.SubParts.Parts.Span[0];
            if( parent.Ast.Name == curr ) return parent;
            return parent.Parent.FindType( typeIdentifier );
        }

        public static TypeSymbol? FindType( this MethodSymbol symbol, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, out var typeSymbol ) ) return typeSymbol;
            return symbol.Parent switch
            {
                ProgramRootSymbol parent => parent.FindType( typeIdentifier ),
                TypeSymbol parent => parent.FindType( typeIdentifier ),
                _ => throw new InvalidOperationException( "Unknown parent type." )
            };
        }


        public static bool TryGetLanguageType( this Identifier typeName, [NotNullWhen( true )] out TypeSymbol? typeSymbol )
        {
            typeSymbol = null;
            if( typeName.Parts.Length == 1 && typeName.Name == "number" )
            {
                typeSymbol = HardcodedSymbols.NumberType;
                return true;
            }
            return false;
        }

        //public static MethodSymbol? FindMethod( this ISymbol symbol, Identifier methodIdentifier )
        //{
        //    var type = symbol.GetContainingType();
        //    if( methodIdentifier.Parts.Length == 1 )
        //    {

        //    }
        //    if( type != null )
        //    {
        //        return type.Constructor;
        //    }

        //    //first we must crawl up to the type def.
        //    var hasMethods = FindMethodContainer( symbol );
        //    if( methodIdentifier.Parts.Length == 1 )
        //    {
        //        return hasMethods.Methods[methodIdentifier.Parts.Span[0]];
        //    }
        //    // RootMetods cannot be referenced outside of itself.
        //    var targetedType = symbol.FindType( methodIdentifier.ParentLocation );
        //    return targetedType?.Methods[methodIdentifier.Name];

        //    static ISymbolWithMethods FindMethodContainer( ISymbol symbol )
        //    {
        //        if( symbol.Parent is ISymbolWithMethods parent ) return parent;
        //        return FindMethodContainer( symbol.Parent! );
        //    }
        //}


        public static ITypedSymbol FindIdentifierValueDeclaration( this IdentifierValueExpressionSymbol symbol, Identifier identifier )
        {
            if( identifier.Parts.Length == 1 ) //if there is multiple it must be a field.
            {
                var statement = symbol.GetContainingStatement();
                var varDec = (VariableSymbol)statement.CrawlStatements( ( s ) => s is VariableSymbol v && v.Name == identifier.Name, true )!;
                if( varDec is not null ) return varDec;
                var method = symbol.GetContainingFunction();
                foreach( var parameter in method.ParameterSymbols )
                {
                    if( parameter.Key == identifier.Name ) return parameter.Value;
                }
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
                for( int i = idx - 1; i >= 0; i-- )
                {
                    if( crawler( block.Statements[i] ) ) return block.Statements[i];
                }
            }
            else
            {
                for( int i = idx; i < block.Statements.Count; i++ )
                {
                    if( crawler( block.Statements[i] ) ) return block.Statements[i];
                }
            }
            return block.CrawlStatements( crawler, backward );
        }
    }
}
