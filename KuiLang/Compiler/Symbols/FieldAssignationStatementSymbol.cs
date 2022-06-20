using KuiLang.Semantic;
using KuiLang.Syntax;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class FieldAssignationStatementSymbol : StatementSymbol
    {
        public FieldAssignationStatementSymbol( StatementSymbol parent, Ast.Statement.FieldAssignation ast )
            : base( parent )
        {
            Ast = ast;
        }

        public IExpressionSymbol NewFieldValue { get; internal set; } = null!;
        public FieldSymbol AssignedField { get; internal set; } = null!;
        public Ast.Statement.FieldAssignation Ast { get; }
    }
}
