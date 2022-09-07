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
            var ctor = new MethodSymbol( symbol, new Ast.Statement.Definition.Typed.Method( symbol.Identifier, symbol.Ast.Name, null, null ) );
            var ret = new ReturnStatementSymbol(
                ctor,
                null
            );
            ret.ReturnedValue = new InstantiateObjectExpression( ret );
            ctor.Statement = ret;
            symbol.Constructor = ctor;
            ctor.ReturnType = symbol;
            var res = base.Visit( symbol );//after this, all fields types should be resolved.

            foreach( var item in symbol.Fields.Select( s => new MethodParameterSymbol(
                new Ast.Statement.Definition.Typed.Parameter( s.Value.Ast.TypeIdentifier, s.Value.Ast.Name, s.Value.Ast.InitValue ),
                ctor )
            ) )
            {
                ctor.ParameterSymbols.Add( item.Ast.Name, item );
            }
            return res;
        }

        protected override object Visit( MethodSymbol symbol )
        {
            symbol.ReturnType = symbol.FindType( symbol.Ast!.ReturnTypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( MethodParameterSymbol symbol )
        {
            symbol.Type = symbol.Parent.FindType( symbol.Ast.TypeIdentifier );
            return base.Visit( symbol );
        }

        protected override object Visit( FieldSymbol symbol )
        {
            symbol.Type = symbol.Parent.FindType( symbol.Ast.TypeIdentifier );
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

            holder.Methods.TryGetValue( symbol.Ast.Name, out var localMethod );
            if( localMethod == null )
            {
                var targetType = symbol.FindType( new Identifier( symbol.Ast.Name ) );
                if( targetType == null )
                {
                    _diagnostics.EmitDiagnostic( new Diagnostic( Severity.Error, null, $"Could not find a function or type named {symbol.Ast.Name}", null ) );
                }
                else
                {
                    localMethod = targetType?.Constructor;
                    if( localMethod == null )
                    {
                        _diagnostics.EmitDiagnostic( new Diagnostic( Severity.Error, null, $"Type {targetType} does not have a constructor.", null ) );
                    }
                }
            }
            symbol.TargetMethod = localMethod!;
            return base.Visit( symbol );
        }

        protected override object Visit( IdentifierValueExpressionSymbol symbol )
        {
            symbol.Field = symbol.FindIdentifierValueDeclaration( symbol.Ast.Identifier );
            return base.Visit( symbol );
        }
    }
}
