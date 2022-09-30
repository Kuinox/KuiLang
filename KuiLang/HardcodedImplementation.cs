using KuiLang.Compiler;
using KuiLang.Compiler.Symbols;
using KuiLang.Interpreter;
using KuiLang.Semantic;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang
{
    public class HardcodedSymbols
    {
        public readonly FieldSymbol NumberValueField;
        public readonly TypeSymbol NumberType;


        public HardcodedSymbols(ProgramRootSymbol root)
        {
            NumberType = new( root, new( "number", new MyList<Ast.Statement.Definition>() ) );
            root.Add( NumberType );
            NumberValueField = new( new Ast.Statement.Definition.Typed.Field( NumberIdentifier, "value", new Ast.Expression.Literal.Number( 0 ) ), NumberType );
            NumberType.Fields.Add( NumberValueField.Ast.Name, NumberValueField );

            var add = OperatorBuilder( "+", ( ret ) => new HardcodedExpressionsSymbol.NumberAddSymbol( ret, NumberType ) );
            var substract = OperatorBuilder( "-", ( ret ) => new HardcodedExpressionsSymbol.NumberSubstractSymbol( ret, NumberType ) );
            var multiply = OperatorBuilder( "*", ( ret ) => new HardcodedExpressionsSymbol.NumberMultiplySymbol( ret, NumberType ) );
            var divide = OperatorBuilder( "/", ( ret ) => new HardcodedExpressionsSymbol.NumberDivideSymbol( ret, NumberType ) );
        }

        public static readonly string NumberName = "number";
        public static readonly Identifier NumberIdentifier = new( NumberName );

        FieldSymbol OperatorBuilder(
            string methodName,
            Func<ReturnStatementSymbol, IExpressionSymbol> funcBuilder
        )
        {
            
            var method = new FunctionExpressionSymbol( NumberType, methodName, null )
            {
                ReturnType = NumberType
            };
            var field = new FieldSymbol( new Ast.Statement.Definition.Typed.Field( default, methodName, null ), NumberType )
            {
                InitValue = method,
                Type = method.ReturnType
            };
            NumberType.Fields.Add( methodName, field );
            var right = new ParameterSymbol( new Ast.Statement.Definition.Typed.Parameter( NumberIdentifier, "right", null ), method )
            {
                Type = NumberType
            };
            var ret = new ReturnStatementSymbol( method, null );
            ret.ReturnedValue = funcBuilder( ret );
            method.Parameters.Add( right.Ast.Name, right );
            method.Statement = ret;
            return field;
        }



    }
}
