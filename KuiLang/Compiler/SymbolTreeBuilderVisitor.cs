using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using KuiLang.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang.Compiler
{
    public class SymbolTableBuilderVisitor : AstVisitor<object>
    {
        ISymbolBase<Ast> _current = null!;
        public override ProgramRootSymbol Visit( Ast ast )
        {
            var root = new ProgramRootSymbol( ast );
            _current = root;
            base.Visit( ast );
            _current = null!;
            return root;
        }

        protected override object Visit( MethodDeclaration method )
        {
            var current = (TypeSymbol)_current;
            var symbol = new MethodSymbol( current, method );
            current.Add( symbol );
            _current = (ISymbolBase<Ast>)symbol;
            base.Visit( method );
            _current = (ISymbolBase<Ast>)symbol.Parent;
            return default!;
        }

        protected override object Visit( TypeDeclaration type )
        {
            var current = (ProgramRootSymbol)_current;
            var symbol = new TypeSymbol( current, type );
            current.Add( symbol );
            _current = (ISymbolBase<Ast>)symbol;
            base.Visit( type );
            _current = symbol.Parent;
            return default!;
        }

        protected override object Visit( FieldDeclaration field )
        {
            if( _current is TypeSymbol type )
            {
                type.Add( new FieldSymbol( field, type ) );
            }
            else
            {
                throw new NotImplementedException();
            }

            base.Visit( field );
            return default!;
        }

        override 
    }
}
