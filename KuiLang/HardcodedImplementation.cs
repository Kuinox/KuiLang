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
    public static class HardcodedSymbols
    {
        static HardcodedSymbols()
        {
            NumberType = new( null!, new( "number", new List<Ast.Statement.Definition>() ) );
            NumberValueField = new( new Ast.Statement.Definition.Typed.Field(NumberIdentifier, "value", new Ast.Expression.Literal.Number( 0 ) ), NumberType );
            Number = new( NumberType );
            NumberType.Fields.Add( NumberValueField.Name, NumberValueField );

            var add = OperatorBuilder( "+", ( ret ) => new HardcodedExpressionsSymbol.NumberAddSymbol( ret, NumberType ) );
            var substract = OperatorBuilder( "-", ( ret ) => new HardcodedExpressionsSymbol.NumberSubstractSymbol( ret, NumberType ) );
            var multiply = OperatorBuilder( "*", ( ret ) => new HardcodedExpressionsSymbol.NumberMultiplySymbol( ret, NumberType ) );
            var divide = OperatorBuilder( "/", ( ret ) => new HardcodedExpressionsSymbol.NumberDivideSymbol( ret, NumberType ) );

            NumberType.Methods.Add( add.Name, add );
            NumberType.Methods.Add( substract.Name, substract );
            NumberType.Methods.Add( multiply.Name, multiply );
            NumberType.Methods.Add( divide.Name, divide );
        }

        public static readonly string NumberName = "number";
        public static readonly Identifier NumberIdentifier = new( NumberName );

        static MethodSymbol OperatorBuilder(
            string methodName,
            Func<ReturnStatementSymbol, IExpressionSymbol> funcBuilder
        )
        {
            var method = new MethodSymbol( NumberType, new Ast.Statement.Definition.Typed.Method(NumberIdentifier, methodName, new Ast.Statement.Definition.Typed.Parameter[]
            {
                new Ast.Statement.Definition.Typed.Parameter(NumberIdentifier, "right", null )
            }, null ) )
            {
                ReturnType = NumberType
            };
            var right = new MethodParameterSymbol( new Ast.Statement.Definition.Typed.Parameter(NumberIdentifier, "right", null), method )
            {
                Type = NumberType
            };
            var ret = new ReturnStatementSymbol( method, null )
            {
                ReturnType = NumberType
            };
            ret.ReturnedValue = funcBuilder( ret );
            method.ParameterSymbols.Add( right.Ast.Name, right );
            method.Statement = ret;
            return method;
        }

        public static readonly FieldSymbol NumberValueField;
        public static readonly TypeSymbol NumberType;


        public static readonly RuntimeObject Number;


    }
}
