using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler.Symbols
{
    public class VariableDeclarationSymbol : StatementSymbolBase<Ast.Statement.Definition.FieldDeclaration>
    {
        public VariableDeclarationSymbol(
            SingleOrMultiStatementSymbol parent,
            StatementBlockSymbol statementScope,
            Ast.Statement.Definition.FieldDeclaration symbolAst ) : base( parent, symbolAst )
        {
            StatementScope = statementScope;
        }

        public IExpressionSymbol? InitValue { get; internal set; } = null!;

        public TypeSymbol Type { get; internal set; } = null!;
        public StatementBlockSymbol StatementScope { get; }
    }
}
