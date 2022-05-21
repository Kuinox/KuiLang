using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using KuiLang.Visitors;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Expression;
using static KuiLang.Syntax.Ast.Expression.Literal;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang.Compiler
{
    public class SymbolTableBuilderVisitor : AstVisitor<object>
    {
        ISymbol<Ast> _current = null!;
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
            _current = (ISymbol<Ast>)symbol;
            base.Visit( method );
            _current = (ISymbol<Ast>)symbol.Parent;
            return default!;
        }

        protected override object Visit( TypeDeclaration type )
        {
            var current = (ProgramRootSymbol)_current;
            var symbol = new TypeSymbol( current, type );
            current.Add( symbol );
            _current = (ISymbol<Ast>)symbol;
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

        protected override object Visit( Ast.Statement.Block statementBlock )
        {
            return base.Visit( statementBlock );
        }

        protected override object Visit( Ast.Statement.FieldAssignation assignation )
        {
            var symbol = new FieldAssignationStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), assignation );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            base.Visit( assignation );
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.If @if )
        {
            var symbol = new IfStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), @if );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            base.Visit( @if );
            _current = prev;
            return default!;
        }

        protected override object Visit( Ast.Statement.Return returnStatement )
        {
            var symbol = new ReturnStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), returnStatement );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            base.Visit( returnStatement );
            _current = prev;
            return default!;
        }

        protected override NumberLiteralSymbol Visit( Number constant )
        {
            var symbol = new NumberLiteralSymbol( constant );
            base.Visit( constant );
            return symbol;
        }

        protected override IExpressionSymbol Visit( Ast.Expression expression )
            => (IExpressionSymbol)base.Visit( expression );

        protected override object Visit( Ast.Statement.ExpressionStatement expressionStatement )
        {
            var symbol = new ExpressionStatementSymbol( SingleOrMultiStatementSymbol.From( _current ), expressionStatement );
            var prev = _current;
            _current = (ISymbol<Ast>)symbol;
            symbol.Expression = Visit( expressionStatement.TheExpression );
            _current = prev;
            return default!;
        }

        protected override FieldReferenceExpressionSymbol Visit( FieldReference variable )
        {
            var symbol = new FieldReferenceExpressionSymbol( variable );
            base.Visit( variable );
            return symbol;
        }

        protected override object Visit( Literal literal )
        {
            return base.Visit( literal );
        }
    }
}
