using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class VariableAssignationStatementSymbol : StatementSymbolBase<Ast.Statement.FieldAssignation>
    {
        public VariableAssignationStatementSymbol( SingleOrMultiStatementSymbol parent, Ast.Statement.FieldAssignation symbolAst )
            : base( parent, symbolAst )
        {
        }

        public IExpressionSymbol NewFieldValue { get; internal set; } = null!;
        public VariableSymbol AssignedField { get; internal set; } = null!;
    }
}
