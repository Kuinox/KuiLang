using System;
using System.Collections.Generic;
using System.Globalization;
using Farkle;
using Farkle.Builder;
using Farkle.Builder.OperatorPrecedence;
using KuiLang.Syntax;
using static Farkle.Builder.Regex;
using static KuiLang.Syntax.Ast;
namespace KuiLang
{
    public static class KuiLang
    {
        public static readonly PrecompilableDesigntimeFarkle<Ast> RootDesigntime;
        public static readonly RuntimeFarkle<Ast> RootRuntime;

        public static readonly DesigntimeFarkle<Statement> StatementDesignTime;
        public static readonly DesigntimeFarkle<FieldLocation> FullNameDesigntime;
        public static readonly DesigntimeFarkle<Expression> ExpressionDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.Method> MethodDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.TypeDef> TypeDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Block> StatementListDesignTime;


        static DesigntimeFarkle<bool> IsLiteralPresent(string name) =>
            Nonterminal.Create($"{name} Maybe",
                name.Appended().FinishConstant(true),
                ProductionBuilder.Empty.FinishConstant(false));

        private static decimal ToNumber(ReadOnlySpan<char> data)
            => decimal.Parse(data, NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture);

        static KuiLang()
        {
            var simpleNamePart = Terminal.Create("Namespace Part", (context, data) => data.ToString(), FromRegexString(@"\p{All Letters}(\p{All Letters}|\d)*"));
            var fullName = Nonterminal.Create<FieldLocation>("FullName");
            fullName.SetProductions(
                fullName.Extended()
                .Append(".")
                .Extend(simpleNamePart)
                .Finish((a, b) => a.Append(b)),
                simpleNamePart.Finish((s) => new FieldLocation(s))
            );
            var argument = Nonterminal.Create("Argument Declaration",
              fullName.Extended().Extend(simpleNamePart).Finish((type, argName) => new Statement.Definition.Argument(type, argName))
            );

            var argumentList = Nonterminal.Create<List<Statement.Definition.Argument>>("Argument List");
            argumentList.SetProductions(
                argumentList.Extended()
                    .Append(",")
                    .Extend(argument)
                    .Finish((xs, s) => xs.Plus(s)),
                argument.Finish((s) => new List<Statement.Definition.Argument>() { s })
            );

            var expression = Nonterminal.Create<Expression>("Expression");

            var argumentPassingList = Nonterminal.Create<List<Expression>>("Expression List");
            argumentPassingList.SetProductions(
                argumentPassingList.Extended()
                .Append(",")
                .Extend(expression)
                .Finish((xs, s) => xs.Plus(s)),
                expression.Finish(s => new List<Expression>())
            );

            var functionCall = Nonterminal.Create("Function Call",
                fullName.Extended()
                    .Append("(")
                    .Extend(argumentPassingList.Optional())
                    .Append(")")
                    .Finish((functionRef, args) => new Expression.FunctionCall(functionRef, args ?? new List<Expression>()))
            );

            var assignation = Nonterminal.Create("Assignation",
                    "=".Appended()

                    .Extend(expression).AsIs()
            );

            var number = Terminal.Create("Number", (position, data) => ToNumber(data),
                Join(
                    Literal('-').Optional(),
                    Literal('0').Or(OneOf("123456789").And(OneOf(PredefinedSets.Number).ZeroOrMore())),
                    Literal('.').And(OneOf(PredefinedSets.Number).AtLeast(1)).Optional(),
                    Join(
                        OneOf("eE"),
                        OneOf("+-").Optional(),
                        OneOf(PredefinedSets.Number).AtLeast(1)).Optional()));

            var variableAssign = Nonterminal.Create("Variable Assignation",
                fullName.Extended().Extend(assignation).Finish((a, b) => new Statement.VariableAssignation(a, b)));

            var opScope = new OperatorScope(
              new LeftAssociative("+", "-"),
              new LeftAssociative("*", "/"));

            var operators = Nonterminal.Create("Operators",
                expression.Extended().Append("*").Extend(expression).Finish((left, right) => (Expression)new Expression.Multiply(left, right)),
                expression.Extended().Append("/").Extend(expression).Finish((left, right) => (Expression)new Expression.Divide(left, right)),
                expression.Extended().Append("+").Extend(expression).Finish((left, right) => (Expression)new Expression.Add(left, right)),
                expression.Extended().Append("-").Extend(expression).Finish((left, right) => (Expression)new Expression.Substract(left, right))
            ).WithOperatorScope(opScope);

            expression.SetProductions(
                functionCall.Finish(s => (Expression)s),
                fullName.Finish(s => (Expression)new Expression.Variable(s)),
                number.Finish(s => (Expression)new Expression.Constant(s)),
                operators.AsIs()
            );


            var statement = Nonterminal.Create<Statement>("Statement");
            var statementList = Nonterminal.Create("Statement Block", statement
                .Many<Statement, List<Statement>>()
                .Finish(s => new Statement.Block(s))
            );
            var statementScope = Nonterminal.Create("Statement Scope",
                "{".Appended()
                    .Extend(statementList)
                    .Append("}")
                    .AsIs());

            var returnStatement = Nonterminal.Create("Return Statement",
                "return".Appended().Extend(expression).Finish(s => new Statement.Return(s))
            );

            var ifStatement = Nonterminal.Create("If Statement",
                "if".Appended()
                    .Append("(")
                    .Extend(expression)
                    .Append(")")
                    .Extend(statementScope)
                    .Finish((condition, statements) => (Statement)new Statement.If(condition, statements))
            );


            var methodDeclaration = Nonterminal.Create("Method Declaration",
                fullName.Extended()
                    .Extend(simpleNamePart)
                    .Append("(")
                    .Extend(argumentList.Optional())
                    .Append(")")
                    .Extend(statementScope)
                    .Finish((typeName, methodName, args, statements)
                        => new Statement.Definition.Method(
                            new Statement.Definition.MethodSignature(
                                new Statement.Definition.Field(
                                    new Statement.Definition.Argument(typeName, methodName)
                                ), args ?? new()
                            ),
                            statements
                        )
                    )
            );

            var variableDeclaration = Nonterminal.Create("Variable Declaration",
                fullName.Extended().Extend(simpleNamePart)
                .Extend(assignation.Optional())
                .Finish((typeName, fieldName, exprVal) => new Statement.VariableDeclaration(typeName, fieldName, exprVal))
            );

            statement.SetProductions(
                expression.Extended().Append(";").Finish(s => (Statement)new Statement.ExpressionStatement(s)),
                variableDeclaration.Extended().Append(";").Finish(s => (Statement)s),
                returnStatement.Extended().Append(";").Finish(s => (Statement)s),
                ifStatement.AsIs(),
                statementScope.Finish(s => (Statement)s),
                methodDeclaration.Finish(s => (Statement)s)
            );

            var fieldDeclaration = Nonterminal.Create("Field Declaration",
                fullName.Extended()
                .Extend(simpleNamePart).Append(";").Finish(
                    (typeName, fieldName) => new Statement.Definition.Field(new Statement.Definition.Argument(typeName, fieldName))
                )
            );

            var definition = Nonterminal.Create("Method or Field Declaration List",
                methodDeclaration.Finish(s => (Statement.Definition)s),
                fieldDeclaration.Finish(s => (Statement.Definition)s)
            );

            var typeDeclaration = Nonterminal.Create("Type Declaration",
               "type".Appended()
                    .Extend(simpleNamePart) // type name
                    .Append("{")
                    .Extend(definition.Many<Statement.Definition, List<Statement.Definition>>())
                    .Append("}").Finish((typeName, fields) => new Statement.Definition.TypeDef(typeName, fields))
            );

            //var interfaceDeclaration = Nonterminal.Create("Interface Declaration",
            //    fullName.Extended() // access level
            //        .Append("interface")
            //        .Extend(simpleNamePart) // type name
            //        .Append("{")
            //        .Extend(interfaceFieldDeclarations.Many<MethodOrFieldDeclaration, List<MethodOrFieldDeclaration>>())
            //        .Append("}").Finish((accesLevel, typeName, fields) => new TypeDeclaration(true, accesLevel, typeName, fields)),
            //   "interface".Appended()
            //        .Extend(simpleNamePart) // type name
            //        .Append("{")
            //        .Extend(interfaceFieldDeclarations.Many<MethodOrFieldDeclaration, List<MethodOrFieldDeclaration>>())
            //        .Append("}").Finish((typeName, fields) => new TypeDeclaration(true, null, typeName, fields))
            //);

            //InterfaceDeclarationDesigntime = interfaceDeclaration;
            //var interfaceOrTypeDeclaration = Nonterminal.Create("Interface Or Type Declaration",
            //    interfaceDeclaration.AsIs(),
            //    typeDeclaration.AsIs()
            //);
            //InterfaceOrTypeDeclarationDesignTime = interfaceOrTypeDeclaration;

            RootDesigntime = ((DesigntimeFarkle<Ast>)statementList)
                .AddBlockComment("/*", "*/")
                .AddLineComment("//")
                .MarkForPrecompile();
            RootRuntime = RootDesigntime.Build();

            StatementDesignTime = statement;
            FullNameDesigntime = fullName;
            ExpressionDesigntime = expression;
            MethodDeclarationDesigntime = methodDeclaration;
            TypeDeclarationDesigntime = typeDeclaration;
            StatementListDesignTime = statementList;
        }
    }
}
