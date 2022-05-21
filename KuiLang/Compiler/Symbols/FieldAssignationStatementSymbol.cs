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
    public class FieldAssignationStatementSymbol : StatementSymbolBase<Ast.Statement.FieldAssignation>
    {
        public FieldAssignationStatementSymbol( SingleOrMultiStatementSymbol parent, Ast.Statement.FieldAssignation symbolAst )
            : base( parent, symbolAst )
        {
        }

        public IExpressionSymbol NewFieldValue { get; internal set; } = null!;
        public FieldSymbol AssignedField { get; internal set; } = null!;
    }
}
