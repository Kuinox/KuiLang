using System;
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
        public static readonly DesigntimeFarkle<Identifier> FullNameDesigntime;
        public static readonly DesigntimeFarkle<Expression> ExpressionDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.Typed.Method> MethodDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Definition.Type> TypeDeclarationDesigntime;
        public static readonly DesigntimeFarkle<Statement.Block> StatementListDesignTime;


        static DesigntimeFarkle<bool> IsLiteralPresent( string name ) =>
            Nonterminal.Create( $"{name} Maybe",
                name.Appended().FinishConstant( true ),
                ProductionBuilder.Empty.FinishConstant( false ) );

        private static decimal ToNumber( ReadOnlySpan<char> data )
            => decimal.Parse( data, NumberStyles.AllowExponent | NumberStyles.Float, CultureInfo.InvariantCulture );

        static KuiLang()
        {
            var simpleNamePart = Terminal.Create( "Namespace Part", ( context, data ) => data.ToString(), FromRegexString( @"\p{All Letters}(\p{All Letters}|\d)*" ) );
            var fullName = Nonterminal.Create<Identifier>( "FullName" );
            fullName.SetProductions(
                fullName.Extended()
                .Append( "." )
                .Extend( simpleNamePart )
                .Finish( ( a, b ) => a.Append( b ) ),
                simpleNamePart.Finish( ( s ) => new Identifier( s ) )
            );
            var argument = Nonterminal.Create( "Argument Declaration",
              fullName.Extended().Extend( simpleNamePart ).Finish( ( type, argName ) => new Statement.Definition.Typed.Parameter( type, argName, null ) )
            );

            var argumentList = Nonterminal.Create<MyList<Statement.Definition.Typed.Parameter>>( "Argument List" );
            argumentList.SetProductions(
                argumentList.Extended()
                    .Append( "," )
                    .Extend( argument )
                    .Finish( ( xs, s ) => xs.Plus( s ) ),
                argument.Finish( ( s ) => new MyList<Statement.Definition.Typed.Parameter>() { s } )
            );

            var expression = Nonterminal.Create<Expression>( "Expression" );

            var argumentPassingList = Nonterminal.Create<MyList<Expression>>( "Expression List" );
            argumentPassingList.SetProductions(
                argumentPassingList.Extended()
                .Append( "," )
                .Extend( expression )
                .Finish( ( xs, s ) => xs.Plus( s ) ),
                expression.Finish( s => new MyList<Expression>() { s } )
            );

            var functionCall =
                Nonterminal.Create( "Method Call",
                simpleNamePart.Extended()
                    .Append( "(" )
                    .Extend( argumentPassingList.Optional() )
                    .Append( ")" )
                    .Finish( ( functionName, args ) => new Expression.FuncCall( functionName, args ?? new MyList<Expression>() ) )
            );

            var methodCall = Nonterminal.Create( "Method Call",
                expression.Extended()
                    .Append( "." )
                    .Extend( simpleNamePart )
                    .Append( "(" )
                    .Extend( argumentPassingList.Optional() )
                    .Append( ")" )
                    .Finish( ( expr, funcName, args ) =>
                    new Expression.FuncCall.MethodCall( expr, funcName, args ?? new MyList<Expression>() ) )
            );

            var assignation = Nonterminal.Create( "Assignation",
                    "=".Appended()

                    .Extend( expression ).AsIs()
            );

            var number = Terminal.Create( "Number", ( position, data ) => ToNumber( data ),
                Join(
                    Literal( '-' ).Optional(),
                    Literal( '0' ).Or( Regex.OneOf( "123456789" ).And( Regex.OneOf( PredefinedSets.Number ).ZeroOrMore() ) ),
                    Literal( '.' ).And( Regex.OneOf( PredefinedSets.Number ).AtLeast( 1 ) ).Optional(),
                    Join(
                        Regex.OneOf( "eE" ),
                        Regex.OneOf( "+-" ).Optional(),
                        Regex.OneOf( PredefinedSets.Number ).AtLeast( 1 ) ).Optional() ) );

            var variableAssign = Nonterminal.Create( "Variable Assignation",
                expression.Extended().Extend( assignation ).Finish( ( a, b ) => new Statement.FieldAssignation( a, b ) ) );

            var opScope = new OperatorScope(
              new LeftAssociative( "+", "-" ),
              new LeftAssociative( "*", "/" ) );

            var operators = Nonterminal.Create( "Operators",
                expression.Extended().Append( "*" ).Extend( expression ).Finish<Expression>( ( left, right ) => new Expression.FuncCall.MethodCall.Operator.Multiply( left, right ) ),
                expression.Extended().Append( "/" ).Extend( expression ).Finish<Expression>( ( left, right ) => new Expression.FuncCall.MethodCall.Operator.Divide( left, right ) ),
                expression.Extended().Append( "+" ).Extend( expression ).Finish<Expression>( ( left, right ) => new Expression.FuncCall.MethodCall.Operator.Add( left, right ) ),
                expression.Extended().Append( "-" ).Extend( expression ).Finish<Expression>( ( left, right ) => new Expression.FuncCall.MethodCall.Operator.Subtract( left, right ) )
            ).WithOperatorScope( opScope );

            expression.SetProductions(
                functionCall.AsIs<Expression>(),
                fullName.Finish<Identifier, Expression>( s => new Expression.IdentifierValue( s ) ),
                number.Finish<decimal, Expression>( s => new Expression.Literal.Number( s ) ),
                operators.AsIs()
            );


            var statement = Nonterminal.Create<Statement>( "Statement" );
            var statementList = Nonterminal.Create( "Statement List", statement
                .Many<Statement, MyList<Statement>>()
                .Finish( s => new Statement.Block( s ) )
            );
            var statementScope = Nonterminal.Create( "Statement Scope",
                "{".Appended()
                    .Extend( statementList )
                    .Append( "}" )
                    .AsIs() );

            var returnStatement = Nonterminal.Create( "Return Statement",
                "return".Appended().Extend( expression ).Finish( s => new Statement.Return( s ) )
            );

            var ifStatement = Nonterminal.Create( "If Statement",
                "if".Appended()
                    .Append( "(" )
                    .Extend( expression )
                    .Append( ")" )
                    .Extend( statement )
                    .Finish<Statement>( ( condition, statements ) => new Statement.If( condition, statements ) )
            );


            var methodDeclaration = Nonterminal.Create( "Method Declaration",
                fullName.Extended()
                    .Extend( simpleNamePart )
                    .Append( "(" )
                    .Extend( argumentList.Optional() )
                    .Append( ")" )
                    .Extend( statementScope )
                    .Finish( ( typeName, methodName, args, statements )
                        => new Statement.Definition.Typed.Method(
                            typeName,
                            methodName,
                            args ?? new MyList<Statement.Definition.Typed.Parameter>(),
                            statements
                        )
                    )
            );

            var variableDeclaration = Nonterminal.Create( "Variable Declaration",
                fullName.Extended().Extend( simpleNamePart )
                .Extend( assignation.Optional() )
                .Finish( ( typeName, fieldName, exprVal ) => new Statement.Definition.Typed.Field( typeName, fieldName, exprVal ) )
            );



            var fieldDeclaration = Nonterminal.Create( "Field Declaration",
                fullName.Extended()
                .Extend( simpleNamePart ).Append( ";" ).Finish(
                    ( typeName, fieldName ) => new Statement.Definition.Typed.Field( typeName, fieldName, null )
                )
            );



            var definition = Nonterminal.Create<Statement.Definition>( "Method or Field Declaration List" );
            var definitionBlock =
                Nonterminal.Create( "Definition Block",
                    "{".Appended()
                        .Extend( definition.Many<Statement.Definition, MyList<Statement.Definition>>() )
                        .Append( "}" )
                        .AsIs()
            );

            var typeDeclaration = Nonterminal.Create( "Type Declaration",
               "type".Appended()
                    .Extend( simpleNamePart ) // type name
                    .Extend( definitionBlock )
                    .Finish( ( typeName, fields ) => new Statement.Definition.Type( typeName, fields ) )
            );
            definition.SetProductions(
                methodDeclaration.AsIs<Statement.Definition>(),
                //fieldDeclaration.Extended().Append(";").Finish<Statement.Definition>(s => s),
                typeDeclaration.AsIs<Statement.Definition>()
            );

            var expressionStatements = Nonterminal.Create<Statement.ExpressionStatement>( "Expression statement.",
                    functionCall.Extended().Append( ";" ).Finish( ( s ) => new Statement.ExpressionStatement( s ) ),
                    methodCall.Extended().Append( ";" ).Finish( ( s ) => new Statement.ExpressionStatement( s ) )
                );

            statement.SetProductions(
                variableDeclaration.Extended().Append( ";" ).Finish<Statement>( s => s ),
                returnStatement.Extended().Append( ";" ).Finish<Statement>( s => s ),
                ifStatement.AsIs(),
                statementScope.AsIs<Statement>(),
                definition.AsIs<Statement>()
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
                .AddBlockComment( "/*", "*/" )
                .AddLineComment( "//" )
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
