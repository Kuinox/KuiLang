using System;
using System.Collections.Generic;
using System.Linq;
using Farkle;
using Farkle.Builder;
using Farkle.Builder.OperatorPrecedence;
using Microsoft.FSharp.Collections;
using static Farkle.Builder.Regex;

namespace KuiLang
{
    public static class KuiLang
    {
        public static readonly PrecompilableDesigntimeFarkle<SignatureDeclaration> RootDesignTime;
        public static readonly RuntimeFarkle<SignatureDeclaration> RootRuntime;
        static KuiLang()
        {
            var typeKeyword = Terminal.Create("Type Keyword", Literal("type"));
            var interfaceKeyword = Terminal.Create("Interface Keyword", Literal("interface"));
            var space = Terminal.Create("Space", (context, data) => data.ToString(), FromRegexString(@"\p{All Space}+"));
            var comment = Terminal.Create("Comment", (context, data) => data.ToString(), FromRegexString("//[^\n]*\n"));
            var triviaPart = Nonterminal.Create("TriviaPart",
                space.AsIs(),
                comment.AsIs(),
                Terminal.Create("AnotherSpace", (context, s) => s.ToString(), Literal(' ')).AsIs()
            );
            var trivia = triviaPart.Many<string, List<string>>();
            var varKeyword = Terminal.Create("Var Keyword", Literal("var"));


            var simpleNamePart = Terminal.Create("Namespace Part", (context, data) => data.ToString(), FromRegexString(@"\p{All Letters}+"));
            var fullName = Nonterminal.Create<FieldLocation>("FullName");
            fullName.SetProductions(
                fullName.Extended()
                .Append(".")
                .Extend(simpleNamePart)
                .Finish((a, b) => a.Append(b)),
                simpleNamePart.Finish((s) => new FieldLocation(s))
            );

            var argument = Nonterminal.Create("Argument Declaration",
              fullName.Extended().Append(trivia).Extend(simpleNamePart).Finish((type, argName) => new Arg(type, argName))
            );

            var argumentList = Nonterminal.Create<List<Arg>>("ArgumentList");
            argumentList.SetProductions(
                argumentList.Extended()
                    .Append(",").Append(trivia)
                    .Extend(argument).Append(trivia)
                    .Finish((xs, s) => xs.Plus(s)),
                argument.Finish((s) => new List<Arg>() { s })
            );


            var fieldDeclaration = Nonterminal.Create("Field Declaration",
                fullName.Extended().Append(trivia)
                .Extend(simpleNamePart).Append(trivia).Finish((type, fieldName) => new FieldDeclaration(type, fieldName)));


            var methodSignatureDeclaration = Nonterminal.Create("MethodSignatureDeclaration", fieldDeclaration.Extended().Append("(").Append(trivia)
                .Extend(argumentList.Optional())
                .Append(")")
                .Finish((field, args) => new SignatureDeclaration(field.Type, field.Name, args)));

            var expression = Nonterminal.Create<Expression>("Expression");
            var argumentPassingList = Nonterminal.Create<List<Expression>>("ExpressionList");
            argumentPassingList.SetProductions(
                argumentPassingList.Extended()
                .Append(",").Append(trivia)
                .Extend(expression).Append(trivia)
                .Finish((xs, s) => xs.Plus(s)),
                expression.Finish(s => new List<Expression>())
            );


            var functionCall = Nonterminal.Create("FunctionCall",
                fullName.Extended().Append(trivia)
                    .Append("(").Append(trivia)
                    .Extend(argumentPassingList)
                    .Append(")")
                    .Finish((functionRef, args) => new FunctionCall(functionRef, args))
            );

            var assignation = Nonterminal.Create("Assignation",
                    "=".Appended()
                    .Append(trivia)
                    .Extend(expression).AsIs()
            );

            var variableDeclaration = Nonterminal.Create("Variable Declaration",
                simpleNamePart.Extended().Append(trivia).Extend(simpleNamePart)
                .Extend(assignation.Optional())
                .Finish((a, b, c) => new VariableDeclaration(new FieldLocation(a), b, c))
            );

            var variableAssign = Nonterminal.Create("Variable Assignation",
                fullName.Extended().Append(trivia).Extend(assignation).Finish((a, b) => new VariableAssignation(a, b)));

            expression.SetProductions(
                functionCall.Finish(s => new Expression(s)),
                fullName.Finish(s => new Expression(s))
            );

            var statement = Nonterminal.Create<Statement>("Statement");

            var statementList = statement.Many<Statement, List<Statement>>();

            var statementScope = Nonterminal.Create("StatementScope",
                "{".Appended().Append(trivia)
                    .Extend(statementList)
                    .Append("}")
                    .Append(trivia)
                    .Finish(s => s)
            );
            var statementContent = Nonterminal.Create("StatementContent",
                expression.Finish(s => new Statement(s)),
                variableDeclaration.Finish(s => new Statement(s))
            );
            statement.SetProductions(
                trivia.Appended()
                .Extend(statementContent).Append(trivia)
                .Append(";").Append(trivia).AsIs(),
                statementScope.Finish(s => new Statement(s))
            );



            RootDesignTime = methodSignatureDeclaration.AutoWhitespace(false).MarkForPrecompile();
            RootRuntime = RootDesignTime.Build();
        }
    }
}
