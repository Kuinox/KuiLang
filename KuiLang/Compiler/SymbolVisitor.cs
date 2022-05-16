using KuiLang.Compiler.Symbols;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Compiler
{
    public abstract class SymbolVisitor<T>
    {
        public virtual T Visit( ProgramRootSymbol symbol )
        {
            foreach( var type in symbol.TypesSymbols.Values )
            {
                Visit( type );
            }
            return default!;
        }

        public virtual T Visit( TypeSymbol symbol )
        {
            foreach( var field in symbol.Fields.Values )
            {
                Visit( field );
            }

            foreach( var method in symbol.Methods.Values )
            {
                Visit( method );
            }
            return default!;
        }

        public virtual T Visit( FieldSymbol symbol ) => default!;

        public virtual T Visit( MethodSymbol symbol )
        {
            foreach( var arguments in symbol.ParameterSymbols.Values )
            {
                Visit( arguments );
            }

            Visit( symbol.StatementSymbol );
            return default!;
        }

        public virtual T Visit( MethodParameterSymbol symbol ) => default!;

        public virtual T Visit( StatementSymbolBase symbol ) => default!;
    }
}
