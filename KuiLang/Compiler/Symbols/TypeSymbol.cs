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
    public class TypeSymbol : SymbolBase<Ast.Statement.Definition.TypeDeclaration>
    {
        readonly Dictionary<string, MethodSymbol> _methods = new();
        readonly Dictionary<string, FieldSymbol> _fields = new();

        public TypeSymbol( ProgramRootSymbol parent, Ast.Statement.Definition.TypeDeclaration typeDef ) : base( typeDef )
        {
            Parent = parent;
            Name = typeDef.Name;
        }

        public ProgramRootSymbol Parent { get; }
        public string Name { get; }

        public IReadOnlyDictionary<string, MethodSymbol> Methods => _methods;

        public IReadOnlyDictionary<string, FieldSymbol> Fields => _fields;

        public void Add( MethodSymbol symbol ) => _methods.Add( symbol.Name, symbol );

        public void Add( FieldSymbol symbol ) => _fields.Add( symbol.Name, symbol );
    }
}
