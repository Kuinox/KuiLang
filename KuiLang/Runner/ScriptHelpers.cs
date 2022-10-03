using Farkle;
using Farkle.Builder;
using KuiLang.Compiler;
using KuiLang.Interpreter;
using KuiLang.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Runner
{
    public static class ScriptHelpers
    {
        static readonly RuntimeFarkle<Ast.Statement.Block> StatementListParser = KuiLang.StatementListDesignTime.Build();
        public static RuntimeObject RunScript( string scriptText, bool debug = false )
        {
            var res = StatementListParser.Parse( scriptText );
            if( !res.IsOk )
            {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine( res.ErrorValue );
                Console.ForegroundColor = prevColor;
                throw new InvalidOperationException( res.ErrorValue.ToString() );
            }
            var statements = res.ResultValue;
            var diagnostics = new Diagnostics.DiagnosticChannel();
            var symbolsBuilder = new SymbolTreeBuilder( diagnostics );
            var resolver = new ResolveMemberTypeVisitor( diagnostics );
            var orderedResolver = new ResolveOrderedSymbols( diagnostics );
            var interpreter = new InterpreterVisitor( diagnostics );
            try
            {

                if( debug ) Console.WriteLine( "Ast:" );
                if( debug ) Console.WriteLine( statements );
                var rootSymbol = symbolsBuilder.Visit( statements );

                if( debug ) Console.WriteLine( "Symbols:" );
                if( debug ) Console.WriteLine( rootSymbol );
                resolver.Visit( rootSymbol );

                if( debug ) Console.WriteLine( "Unordered resolution:" );
                if( debug ) Console.WriteLine( rootSymbol );
                orderedResolver.Visit( rootSymbol );
                var validator = new ValidatorVisitor( diagnostics );
                validator.Visit( rootSymbol );
                if(diagnostics.HaveError)
                {
                    throw new InvalidProgramException();
                }
                if( debug )
                {
                    Console.WriteLine( "Ordered resolution:" );
                    Console.WriteLine( rootSymbol );
                }

                var val = interpreter.Visit( rootSymbol ); //Thats where all the magic happens.
                if( debug ) Console.WriteLine( $"Execution returned value: {val}" );
                return val.Owner.Fields[val.Field].AsT0;
            }
            finally
            {
                Console.WriteLine( "Compiler diagnostics:" );
                diagnostics.PrintDiagnostics( Console.Out );
            }


        }

        public static decimal AsNumber( this RuntimeObject @this )
            => (decimal)@this.Fields.Single().Value.AsT1;
    }
}
