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
        public static readonly PrecompilableDesigntimeFarkle<FieldDeclaration> RootDesignTime;
        public static readonly RuntimeFarkle<FieldDeclaration> RootRuntime;
        public static readonly Nonterminal<FieldLocation> FullNameDesignTime;
        public static readonly Nonterminal<Expression> ExpressionRuntime;
        public static readonly DesigntimeFarkle<SignatureDeclaration> MethodSignatureDeclarationRuntime;

        static KuiLang()
        {
            var typeKeyword = Terminal.Create("Type Keyword", Literal("type"));
            var interfaceKeyword = Terminal.Create("Interface Keyword", Literal("interface"));
            var space = Terminal.Create("Space", (context, data) => data.ToString(), FromRegexString(@"\p{All Space}+"));
            var comment = Terminal.Create("Comment", (context, data) => data.ToString(), FromRegexString("//[^\n]*\n"));
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
            FullNameDesignTime = fullName.AutoWhitespace(false);
            var argument = Nonterminal.Create("Argument Declaration",
              fullName.Extended().Extend(simpleNamePart).Finish((type, argName) => new Arg(type, argName))
            );

            var argumentList = Nonterminal.Create<List<Arg>>("ArgumentList");
            argumentList.SetProductions(
                argumentList.Extended()
                    .Append(",")
                    .Extend(argument)
                    .Finish((xs, s) => xs.Plus(s)),
                argument.Finish((s) => new List<Arg>() { s })
            );


            var fieldDeclaration = Nonterminal.Create("Field Declaration",
                fullName.Extended()
                .Extend(fullName.Optional())
                .Extend(simpleNamePart)
                .Finish((accessModifier, type, fieldName) => new FieldDeclaration(accessModifier, type, fieldName)));


            var methodSignatureDeclaration = Nonterminal.Create("MethodSignatureDeclaration", fieldDeclaration.Extended().Append("(")
                .Extend(argumentList.Optional())
                .Append(")")
                .Finish((field, args) => new SignatureDeclaration(field.Type, field.Name, args)));

            MethodSignatureDeclarationRuntime = methodSignatureDeclaration.AutoWhitespace(false);

            var expression = Nonterminal.Create<Expression>("Expression");

            ExpressionRuntime = expression.AutoWhitespace(false);

            var argumentPassingList = Nonterminal.Create<List<Expression>>("ExpressionList");
            argumentPassingList.SetProductions(
                argumentPassingList.Extended()
                .Append(",")
                .Extend(expression)
                .Finish((xs, s) => xs.Plus(s)),
                expression.Finish(s => new List<Expression>())
            );


            var functionCall = Nonterminal.Create("FunctionCall",
                fullName.Extended()
                    .Append("(")
                    .Extend(argumentPassingList)
                    .Append(")")
                    .Finish((functionRef, args) => new FunctionCall(functionRef, args))
            );

            var assignation = Nonterminal.Create("Assignation",
                    "=".Appended()

                    .Extend(expression).AsIs()
            );

            var variableDeclaration = Nonterminal.Create("Variable Declaration",
                simpleNamePart.Extended().Extend(simpleNamePart)
                .Extend(assignation.Optional())
                .Finish((a, b, c) => new VariableDeclaration(new FieldLocation(a), b, c))
            );

            var variableAssign = Nonterminal.Create("Variable Assignation",
                fullName.Extended().Extend(assignation).Finish((a, b) => new VariableAssignation(a, b)));

            expression.SetProductions(
                functionCall.Finish(s => new Expression(s)),
                fullName.Finish(s => new Expression(s))
            );

            var statement = Nonterminal.Create<Statement>("Statement");

            var statementList = statement.Many<Statement, List<Statement>>();

            var statementScope = Nonterminal.Create("StatementScope",
                "{".Appended()
                    .Extend(statementList)
                    .Append("}")

                    .Finish(s => s)
            );
            var statementContent = Nonterminal.Create("StatementContent",
                expression.Finish(s => new Statement(s)),
                variableDeclaration.Finish(s => new Statement(s))
            );
            statement.SetProductions(
                statementContent.Extended().Append(";").AsIs(),
                statementScope.Finish(s => new Statement(s))
            );



            RootDesignTime = fieldDeclaration
                .AddBlockComment("/*", "*/")
                .AddLineComment("//")
                .MarkForPrecompile();
            RootRuntime = RootDesignTime.Build();
        }
    }
}
