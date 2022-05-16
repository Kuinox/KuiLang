using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    class FieldAssignationStatementSymbol : StatementSymbolBase<Ast.Statement.FieldAssignation>
    {
        public FieldAssignationStatementSymbol( OneOf<ISymbolWithAStatement, StatementBlockSymbol> parent, Ast.Statement.FieldAssignation symbolAst )
            : base( parent, symbolAst )
        {
        }
    }
}
