using Farkle;
using Farkle.Builder;
using KuiLang.Syntax;
using KuiLang.Visitors;
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
        public static int RunScript(string scriptText)
        {
            var res = StatementListParser.Parse(scriptText);
            if (!res.IsOk)
            {
                var prevColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(res.ErrorValue);
                Console.ForegroundColor = prevColor;
                return -1;
            }
            var statements = res.ResultValue;
            var symbolsBuilder = new SymbolTableBuilderVisitor();
            symbolsBuilder.Visit(statements);
            var interpreter = new InterpreterVisitor(symbolsBuilder.Symbols);
            var val = interpreter.Visit(statements); //Thats where all the magic happens.
            Console.WriteLine($"Execution returned value: {val}");
            return 0;
        }
    }
}
