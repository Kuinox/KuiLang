using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class VariableAssignationStatementSymbol : StatementSymbol
    {
        public VariableAssignationStatementSymbol( ISymbol parent, Ast.Statement.FieldAssignation ast )
            : base( parent )
        {
            Ast = ast;
        }

        public IExpressionSymbol NewFieldValue { get; internal set; } = null!;
        public VariableSymbol AssignedField { get; internal set; } = null!;
        public Ast.Statement.FieldAssignation Ast { get; }
    }
}
