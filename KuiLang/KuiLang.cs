using System.Collections.Generic;
using Farkle;
using Farkle.Builder;
using static Farkle.Builder.Regex;

namespace KuiLang
{
    public static class KuiLang
    {
        public static readonly PrecompilableDesigntimeFarkle<TypeDeclaration> RootDesigntime;
        public static readonly RuntimeFarkle<TypeDeclaration> RootRuntime;

        public static readonly DesigntimeFarkle<FieldLocation> FullNameDesigntime;
        public static readonly DesigntimeFarkle<Expression> ExpressionDesigntime;
        public static readonly DesigntimeFarkle<SignatureDeclaration> MethodSignatureDeclarationDesigntime;
        public static readonly DesigntimeFarkle<MethodDeclaration> MethodDeclarationDesigntime;
        public static readonly DesigntimeFarkle<TypeDeclaration> TypeDeclarationDesigntime;
        static KuiLang()
        {
            var typeKeyword = Terminal.Create("Type Keyword", Literal("type"));
            var interfaceKeyword = Terminal.Create("Interface Keyword", Literal("interface"));
            var space = Terminal.Create("Space", (context, data) => data.ToString(), FromRegexString(@"\p{All Space}+"));
            var comment = Terminal.Create("Comment", (context, data) => data.ToString(), FromRegexString("//[^\n]*\n"));


            var simpleNamePart = Terminal.Create("Namespace Part", (context, data) => data.ToString(), FromRegexString(@"\p{All Letters}+"));
            var fullName = Nonterminal.Create<FieldLocation>("FullName");
            fullName.SetProductions(
                fullName.Extended()
                .Append(".")
                .Extend(simpleNamePart)
                .Finish((a, b) => a.Append(b)),
                simpleNamePart.Finish((s) => new FieldLocation(s))
            );
            FullNameDesigntime = fullName;
            var argument = Nonterminal.Create("Argument Declaration",
              fullName.Extended().Extend(simpleNamePart).Finish((type, argName) => new Arg(type, argName))
            );

            var argumentList = Nonterminal.Create<List<Arg>>("Argument List");
            argumentList.SetProductions(
                argumentList.Extended()
                    .Append(",")
                    .Extend(argument)
                    .Finish((xs, s) => xs.Plus(s)),
                argument.Finish((s) => new List<Arg>() { s })
            );







            var expressionNonterminal = Nonterminal.Create<Expression>("Expression");
            ExpressionDesigntime = expressionNonterminal;

            var argumentPassingList = Nonterminal.Create<List<Expression>>("Expression List");
            argumentPassingList.SetProductions(
                argumentPassingList.Extended()
                .Append(",")
                .Extend(ExpressionDesigntime)
                .Finish((xs, s) => xs.Plus(s)),
                ExpressionDesigntime.Finish(s => new List<Expression>())
            );


            var functionCall = Nonterminal.Create("Function Call",
                fullName.Extended()
                    .Append("(")
                    .Extend(argumentPassingList.Optional())
                    .Append(")")
                    .Finish((functionRef, args) => new FunctionCall(functionRef, args ?? new List<Expression>()))
            );

            var assignation = Nonterminal.Create("Assignation",
                    "=".Appended()

                    .Extend(ExpressionDesigntime).AsIs()
            );

            var variableDeclaration = Nonterminal.Create("Variable Declaration",
                simpleNamePart.Extended().Extend(simpleNamePart)
                .Extend(assignation.Optional())
                .Finish((a, b, c) => new VariableDeclaration(new FieldLocation(a), b, c))
            );

            var variableAssign = Nonterminal.Create("Variable Assignation",
                fullName.Extended().Extend(assignation).Finish((a, b) => new VariableAssignation(a, b)));

            expressionNonterminal.SetProductions(
                functionCall.Finish(s => new Expression(s)),
                fullName.Finish(s => new Expression(s))
            );

            var statement = Nonterminal.Create<Statement>("Statement");

            var statementList = statement.Many<Statement, List<Statement>>();

            var statementScope = Nonterminal.Create("Statement Scope",
                "{".Appended()
                    .Extend(statementList)
                    .Append("}")
                    .AsIs()
            );
            var statementContent = Nonterminal.Create("Statement Content",
                ExpressionDesigntime.Finish(s => new Statement(s)),
                variableDeclaration.Finish(s => new Statement(s))
            );
            statement.SetProductions(
                statementContent.Extended().Append(";").AsIs(),
                statementScope.Finish(s => new Statement(s))
            );

            var fieldSignature = Nonterminal.Create("Field Signature",
                fullName.Extended()
                    .Extend(fullName)
                    .Extend(simpleNamePart).Finish((accessModifier, type, fieldName) => new FieldDeclaration(accessModifier, type, fieldName)),
                fullName.Extended()
                    .Extend(simpleNamePart).Finish((type, fieldName) => new FieldDeclaration(null, type, fieldName))
            );
            var methodSignatureDeclaration = Nonterminal.Create("Method Signature Declaration",
                fieldSignature.Extended().Append("(")
                    .Extend(argumentList.Optional())
                    .Append(")")
                    .Finish((field, args) => new SignatureDeclaration(field.Type, field.Name, args)));
            MethodSignatureDeclarationDesigntime = methodSignatureDeclaration;


            var methodDeclaration = Nonterminal.Create("Method Declaration",
                MethodSignatureDeclarationDesigntime.Extended()
                    .Extend(statementScope)
                    .Finish((a, b) => new MethodDeclaration(a, b))
            );
            MethodDeclarationDesigntime = methodDeclaration;

            var fieldDeclaration = Nonterminal.Create("Field Declaration",
                fieldSignature.Extended().Append(";").AsIs()
            );

            var methodOrFieldDeclaration = Nonterminal.Create("Method or Field Declaration List",
                methodDeclaration.Finish(s => new MethodOrFieldDeclaration(s)),
                fieldSignature.Finish(s => new MethodOrFieldDeclaration(s))
            );


            TypeDeclarationDesigntime = Nonterminal.Create("Type Declaration",
                fullName.Optional().Extended()
                    .Extend(simpleNamePart)
                    .Append("{")
                    .Extend(methodOrFieldDeclaration.Many<MethodOrFieldDeclaration, List<MethodOrFieldDeclaration>>())
                    .Append("}").Finish((accesLevel, typeName, fields) => new TypeDeclaration(accesLevel, typeName, fields)
            ));

            RootDesigntime = TypeDeclarationDesigntime
                .AddBlockComment("/*", "*/")
                .AddLineComment("//")
                .MarkForPrecompile();
            RootRuntime = RootDesigntime.Build();
        }
    }
}
