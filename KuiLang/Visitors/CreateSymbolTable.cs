using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast.Statement.Definition;

namespace KuiLang.Visitors
{
    public class SymbolTableBuilderVisitor : AstVisitor<object>
    {
        readonly Dictionary<string, ISymbol> _symbols = new();
        protected override object Visit(Method method)
        {
            _symbols.Add(method.Name, new MethodSymbol(method));
            return base.Visit(method);
        }
        public IReadOnlyDictionary<string, ISymbol> Symbols => _symbols;
    }
}
