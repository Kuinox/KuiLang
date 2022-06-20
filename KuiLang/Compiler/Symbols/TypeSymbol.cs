using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Semantic
{
    public class TypeSymbol : ISymbol, ISymbolWithMethods
    {
        public TypeSymbol( ProgramRootSymbol parent, Ast.Statement.Definition.TypeDeclaration ast )
        {
            Parent = parent;
            Ast = ast;
            Name = ast.Name;
        }

        public ProgramRootSymbol Parent { get; }

        ISymbol? ISymbol.Parent => Parent;

        public Ast.Statement.Definition.TypeDeclaration Ast { get; }
        public string Name { get; }

        public Dictionary<string, MethodSymbol> Methods { get; } = new();

        public Dictionary<string, FieldSymbol> Fields { get; } = new();


    }
}
