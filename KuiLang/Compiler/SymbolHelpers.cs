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
            return parent is IExpressionSymbol expr ? GetContainingStatement( expr ) : (StatementSymbol)parent;
        }

        public static FunctionExpressionSymbol GetContainingFunction( this ISymbol symbol )
        {
            if( symbol is FunctionExpressionSymbol method ) return method;
            return GetContainingFunction( symbol.Parent! );
        }

        public static ISymbolWithFields GetContainingType( this ISymbol symbol )
        {
            if( symbol.Parent is ISymbolWithFields s ) return s;
            return GetContainingType( symbol.Parent );
        }

        public static ProgramRootSymbol GetRoot( this ISymbol symbol )
        {
            if( symbol is null ) throw new InvalidOperationException();
            if( symbol is ProgramRootSymbol root ) return root;
            return GetRoot( symbol.Parent! );
        }

        public static TypeSymbol FindType( this ProgramRootSymbol root, Identifier typeIdentifier )
        {
            root.TypesSymbols.TryGetValue( typeIdentifier.Name, out var val );
            return val;
            // no namespace/subtype yet.
        }

        public static TypeSymbol? FindType( this ISymbol? symbol, Identifier typeIdentifier )
        {
            if( typeIdentifier.TryGetLanguageType( symbol.GetRoot().HardcodedSymbols, out var typeSymbol ) )
                return typeSymbol;
            if( symbol is null ) return null;
            if( symbol is ProgramRootSymbol root ) return root.FindType( typeIdentifier );
            if( symbol is FunctionExpressionSymbol method ) return method.Parent.FindType( typeIdentifier );
            return FindType( symbol?.Parent, typeIdentifier );
        }

        public static TypeSymbol? FindType( this TypeSymbol parent, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, parent.GetRoot().HardcodedSymbols, out var typeSymbol ) )
                return typeSymbol;
            if( typeIdentifier.Parts.Length > 1 )
                throw new NotSupportedException( "Right now we do not support namespace or nested types." );
            var curr = typeIdentifier.SubParts.Parts.Span[0];
            if( parent.Ast.Name == curr ) return parent;
            return parent.Parent.FindType( typeIdentifier );
        }

        public static TypeSymbol? FindType( this FunctionExpressionSymbol symbol, Identifier typeIdentifier )
        {
            if( TryGetLanguageType( typeIdentifier, symbol.GetRoot().HardcodedSymbols, out var typeSymbol ) )
                return typeSymbol;
            return symbol.Parent switch
            {
                ProgramRootSymbol parent => parent.FindType( typeIdentifier ),
                TypeSymbol parent => parent.FindType( typeIdentifier ),
                _ => throw new InvalidOperationException( "Unknown parent type." )
            };
        }


        public static bool TryGetLanguageType( this Identifier typeName, HardcodedSymbols hardcodedSymbols,
            [NotNullWhen( true )] out TypeSymbol? typeSymbol )
        {
            typeSymbol = null;
            if( typeName.Parts.Length == 1 && typeName.Name == "number" )
            {
                typeSymbol = hardcodedSymbols.NumberType;
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


        public static ITypedSymbol FindIdentifierValueDeclaration( this IdentifierValueExpressionSymbol symbol,
            DiagnosticChannel diagnostics, Identifier identifier )
        {
            ISymbolWithFields? methodType = null;
            if( identifier.Parts.Length == 1 )
            {
                // looking up the identifier in the statement.
                var statement = symbol.GetContainingStatement();
                var varDec =
                    (VariableSymbol)statement.CrawlStatements(
                        ( s ) => s is VariableSymbol v && v.Name == identifier.Name, true )!;
                var method = symbol.GetContainingFunction();
                ParameterSymbol? found = null;
                foreach( var parameter in method.Parameters )
                {
                    if( parameter.Key == identifier.Name )
                    {
                        diagnostics.CompilerErrorIfTrue( found != null,
                            "Variable with same name already declared previously." );
                        found = parameter.Value;
                    }
                }

                diagnostics.CompilerErrorIfTrue( varDec != null, "Local variable conflict with parameter." );
                if( found != null || varDec != null ) return (ITypedSymbol)found! ?? varDec!;

                methodType = method.GetContainingType();
            }

            var targetedType = methodType.FindType( identifier );
            if( targetedType != null ) return targetedType;
            methodType ??= symbol.FindType( identifier.ParentLocation );
            if( methodType!.Fields.TryGetValue( identifier.Name, out var field ) )
            {
                return field;
            }

            return methodType.Fields[identifier.Name];
        }

        public static StatementSymbol? CrawlStatements( this StatementSymbol symbol,
            Func<StatementSymbol, bool> crawler, bool backward = false )
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
