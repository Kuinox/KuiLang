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
    public class IfStatementSymbol : StatementSymbol, ISymbolWithAStatement
    {
        public IfStatementSymbol( StatementSymbol parent, Ast.Statement.If symbolAst )
            : base( parent )
        {
            SymbolAst = symbolAst;
        }

        public IExpression Condition { get; set; } = null!;
        public StatementSymbol Statement { get; set; } = null!;

        public Ast.Statement.If SymbolAst { get; }
    }
}
