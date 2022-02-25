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
        public static readonly PrecompilableDesigntimeFarkle<SignatureDeclaration> Designtime;
        public static readonly RuntimeFarkle<SignatureDeclaration> Runtime;
        static KuiLang()
        {
            var typeKeyword = Terminal.Create("TypeKeyword", Literal("type"));

            var openBracket = Terminal.Create("OpenBracket", Literal("{"));
            var closeBracket = Terminal.Create("CloseBracket", Literal("}"));
            var space = Terminal.Create("Space", (context, data) => data.ToString(), FromRegexString(@"\p{All Space}+"));
            var comment = Terminal.Create("Comment", (context, data) => data.ToString(), FromRegexString("//[^\n]*\n"));
            var triviaPart = Nonterminal.Create("TriviaPart",
                space.AsIs(),
                comment.AsIs(),
                Terminal.Create("AnotherSpace", (context, s) => s.ToString(), Literal(' ')).AsIs()
            );
            var trivia = triviaPart.Many<string, List<string>>();
            var simpleNamePart = Terminal.Create("NamespacePart", (context, data) => data.ToString(), FromRegexString(@"\p{All Letters}+"));
            var fullName = Nonterminal.Create<FullName>("FullName");
            fullName.SetProductions(
                fullName.Extended()
                .Append(".")
                .Extend(simpleNamePart)
                .Finish((a, b) => a.Append(b)),
                simpleNamePart.Finish((s) => new FullName(s))
            );

            var argument = Nonterminal.Create("ArgumentDeclaration",
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


            var fieldDeclaration = Nonterminal.Create("FieldDeclaration",
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
                .Finish((xs, s) => xs.Add(s)),
                expression.Finish(s=> new List<Expression>())
            );


            var functionCall = Nonterminal.Create("FunctionCall",
                fullName.Extended().Append(trivia)
                    .Append("(").Append(trivia)
                    .Extend(argumentPassingList)
                    .Append(")")
                    .Finish((functionRef, args) => new FunctionCall(functionRef, args) )
            );

            expression.SetProductions(
                functionCall.AsIs()
            );

            var declarationList = declaration.Many<>();

            var codeScope = Nonterminal.Create(codeScope, 
                openBracket.Appended().Append(trivia)
                .Append()
            );


            Designtime = methodSignatureDeclaration.AutoWhitespace(false).MarkForPrecompile();
            Runtime = Designtime.Build();
            // public|private Type fieldName;
            // public|private Type fieldName => expression;
            // public|private Type methodName() { declarations }
            // public|private Type methodName() => expression;

            //var typeDeclaration = Nonterminal.Create("TypeDeclaration", typeKeyword.Appended().Extend(openBracket).));
        }
    }
}
