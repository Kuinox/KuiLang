using KuiLang.Compiler.Symbols;
using KuiLang.Diagnostics;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;

namespace KuiLang.Compiler
{
    public class ResolveMemberTypeVisitor : SymbolVisitor<object>
    {
        readonly DiagnosticChannel _diagnostics;

        public ResolveMemberTypeVisitor( DiagnosticChannel diagnostics )
        {
            _diagnostics = diagnostics;
        }

        protected override object Visit( TypeSymbol symbol )
        {
            var field = new FieldSymbol( new Ast.Statement.Definition.Typed.Field( default, symbol.Ast.Name, null ), symbol )
            {
                Type = symbol
            };
            symbol.Fields.Add( symbol.Ast.Name, field );
            var ctor = new FunctionExpressionSymbol( symbol, symbol.Ast.Name, null );
            field.InitValue = ctor;
            var ret = new ReturnStatementSymbol(
                ctor,
                null
            );
            ret.ReturnedValue = new InstantiateObjectExpression( ret );
            ctor.Statement = ret;
            symbol.Constructor = ctor;
            ctor.ReturnType = symbol;
            var res = base.Visit( symbol );//after this, all fields types should be resolved.

            foreach( var item in symbol.Fields.Select( s => new ParameterSymbol(
                new Ast.Statement.Definition.Typed.Parameter( s.Value.Ast.TypeIdentifier, s.Value.Ast.Name, s.Value.Ast.InitValue ),
                ctor )
            ) )
            {
                ctor.Parameters.Add( item.Ast.Name, item );
            }
            return res;
        }

        protected override object Visit( FunctionExpressionSymbol symbol )
        {
            if(symbol.ReturnType is null && symbol.FuncReturnTypeIdentifier is not null)
            {
                symbol.ReturnType = symbol.FindType( symbol.FuncReturnTypeIdentifier )!;
            }
            return base.Visit( symbol );
        }

        protected override object Visit( ParameterSymbol symbol )
        {
            
            symbol.Type ??= symbol.Parent.FindType( symbol.Ast.TypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( FieldSymbol symbol )
        {
            symbol.Type ??= symbol.Parent.FindType( symbol.Ast.TypeIdentifier )!;
            return base.Visit( symbol );
        }

        protected override object Visit( VariableSymbol symbol )
        {
            var type = symbol.FindType( symbol.Ast.TypeIdentifier );
            if( type == null ) _diagnostics.Error( $"Could not resolve type {symbol.Ast.TypeIdentifier}." );
            symbol.Type = type!;
            return base.Visit( symbol );
        }

        protected override object Visit( FunctionCallExpressionSymbol symbol )
        {
            //later will have signature resolution.
            return base.Visit( symbol );
        }

        protected override object Visit( IdentifierValueExpressionSymbol symbol )
        {
            symbol.Field = symbol.FindIdentifierValueDeclaration(_diagnostics, symbol.Ast.Identifier );
            return base.Visit( symbol );
        }
    }
}
