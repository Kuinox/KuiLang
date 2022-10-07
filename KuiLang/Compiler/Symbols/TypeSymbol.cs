using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KuiLang.Syntax.Ast;

namespace KuiLang.Semantic
{
    public class TypeSymbol : ISymbolWithFields, ITypedSymbol
    {
        public FunctionExpressionSymbol? FunctionExpressionSymbol { get; }

        public TypeSymbol( ISymbol? parent, Ast.Statement.Definition.Type? ast, FunctionExpressionSymbol? functionExpressionSymbol )
        {
            FunctionExpressionSymbol = functionExpressionSymbol;
            Parent = parent;
            Ast = ast;
        }


        public ISymbol Parent { get; }

        public Ast.Statement.Definition.Type? Ast { get; }

        public OrderedDictionary<string, FieldSymbol> Fields { get; } = new();

        public FunctionExpressionSymbol Constructor { get; internal set; } = null!;

        public Identifier Identifier => new( Ast.Name );

        [MemberNotNullWhen( true, nameof(FunctionExpressionSymbol) )]
        public bool IsMethod => FunctionExpressionSymbol != null;

        public override string ToString() => Identifier.ToString();

        TypeSymbol ITypedSymbol.Type => this;
    }
}
