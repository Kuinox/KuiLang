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
        public static object RunScript( string scriptText )
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

            var rootSymbol = symbolsBuilder.Visit( statements );
            resolver.Visit( rootSymbol );
            orderedResolver.Visit( rootSymbol );
            var val = interpreter.Visit( rootSymbol ); //Thats where all the magic happens.
            Console.WriteLine( $"Execution returned value: {val}" );
            return val;
        }
    }
}
