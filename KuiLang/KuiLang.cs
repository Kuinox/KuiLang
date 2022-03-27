using System;
using System.Collections.Generic;
using Farkle;
using Farkle.Builder;
using KuiLang.Syntax;
using static Farkle.Builder.Regex;
using static KuiLang.Syntax.Ast;
namespace KuiLang
{
    public static class KuiLang
    {
        public static readonly PrecompilableDesigntimeFarkle<Ast> RootDesigntime;
        public static readonly RuntimeFarkle<Ast> RootRuntime;

        public static readonly DesigntimeFarkle<FieldLocation> FullNameDesigntime;
        public static readonly DesigntimeFarkle<Expression> ExpressionDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.MethodSignature> MethodSignatureDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.Method> MethodDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.Type> TypeDeclarationDesigntime;

        static DesigntimeFarkle<bool> IsLiteralPresent(string name) =>
            Nonterminal.Create($"{name} Maybe",
                name.Appended().FinishConstant(true),
                ProductionBuilder.Empty.FinishConstant(false));

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
            FullNameDesigntime = fullName;
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
                    .Finish((functionRef, args) => new Expression.FunctionCall(functionRef, args ?? new List<Expression>()))
            );

            var assignation = Nonterminal.Create("Assignation",
                    "=".Appended()

                    .Extend(ExpressionDesigntime).AsIs()
            );

            var variableDeclaration = Nonterminal.Create("Variable Declaration",
                simpleNamePart.Extended().Extend(simpleNamePart)
                .Extend(assignation.Optional())
                .Finish((a, b, c) => new Statement.VariableDeclaration(new FieldLocation(a), b, c))
            );

            var variableAssign = Nonterminal.Create("Variable Assignation",
                fullName.Extended().Extend(assignation).Finish((a, b) => new Statement.VariableAssignation(a, b)));

            expressionNonterminal.SetProductions(
                functionCall.Finish(s => (Expression)s),
                fullName.Finish(s => (Expression)new Expression.Variable(s))
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
                ExpressionDesigntime.Finish(s => (Statement)new Statement.ExpressionStatement(s)),
                variableDeclaration.Finish(s => (Statement)s)
            );
            statement.SetProductions(
                statementContent.Extended().Append(";").AsIs(),
                statementScope.Finish(s => (Statement)new Statement.Block(s))
            );

            var fieldSignature = Nonterminal.Create("Field Signature",
                fullName.Extended()
                    .Extend(fullName)
                    .Extend(simpleNamePart)
                    .Finish((accessModifier, type, fieldName)
                        => new Statement.Definition.Field(false, accessModifier, new Statement.Definition.Argument(type, fieldName))),
                fullName.Extended()
                    .Extend(simpleNamePart)
                    .Finish((type, fieldName) => new Statement.Definition.Field(false, null, new Statement.Definition.Argument(type, fieldName)))
            );

            var methodSignature = Nonterminal.Create("Method Signature",
                IsLiteralPresent("static").Extended().Extend(fieldSignature).Append("(")
                    .Extend(argumentList.Optional())
                    .Append(")")
                    .Finish((isStatic, field, args) => new Statement.Definition.MethodSignature(
                        new Statement.Definition.Field(isStatic, field.AccessModifier, field.Signature),
                        args ?? new List<Statement.Definition.Argument>()
                    )
)
            );
            MethodSignatureDeclarationDesigntime = methodSignature;

            var methodSignatureDeclaration = Nonterminal.Create("Method Signature Declaration",
                methodSignature.Extended().Append(";").AsIs()
            );

            var methodDeclaration = Nonterminal.Create("Method Declaration",
                methodSignature.Extended()
                    .Extend(statementScope)
                    .Finish((a, b) => new Statement.Definition.Method(a, b))
            );
            MethodDeclarationDesigntime = methodDeclaration;

            var fieldDeclaration = Nonterminal.Create("Field Declaration",
                fieldSignature.Extended().Append(";").AsIs()
            );

            var definition = Nonterminal.Create("Method or Field Declaration List",
                methodDeclaration.Finish(s => (Statement.Definition)s),
                fieldDeclaration.Finish(s => (Statement.Definition)s)
            );
            var interfaceFieldDeclarations = Nonterminal.Create("Interface Field Declaration",
                fieldDeclaration.Finish(s => (Statement.Definition)s),
                methodSignatureDeclaration.Finish(s => (Statement.Definition)s)
            );

            var typeDeclaration = Nonterminal.Create("Type Declaration",
                fullName.Extended() // access level
                    .Append("type")
                    .Extend(simpleNamePart) // type name
                    .Append("{")
                    .Extend(definition.Many<Statement.Definition, List<Statement.Definition>>())
                    .Append("}").Finish((accesLevel, typeName, fields) => new Statement.Definition.Type(accesLevel, typeName, fields)),
               "type".Appended()
                    .Extend(simpleNamePart) // type name
                    .Append("{")
                    .Extend(definition.Many<Statement.Definition, List<Statement.Definition>>())
                    .Append("}").Finish((typeName, fields) => new Statement.Definition.Type(null, typeName, fields))
            );
            TypeDeclarationDesigntime = typeDeclaration;

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

            RootDesigntime = ((DesigntimeFarkle<Ast>)TypeDeclarationDesigntime)
                .AddBlockComment("/*", "*/")
                .AddLineComment("//")
                .MarkForPrecompile();
            RootRuntime = RootDesigntime.Build();
        }
    }
}
