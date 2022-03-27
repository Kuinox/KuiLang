using System.Collections.Generic;

namespace KuiLang.Build
{
    public class BuiltStatement
    {
        public BuiltStatement(BuiltVariableDeclaration variableDeclaration)
        {
            VariableDeclaration = variableDeclaration;
        }

        public BuiltStatement(BuiltExpression expression)
        {
            Expression = expression;
        }

        public BuiltStatement(IReadOnlyCollection<BuiltStatement> statementList)
        {
            StatementList = statementList;
        }

        public BuiltVariableDeclaration? VariableDeclaration { get; }
        public BuiltExpression? Expression { get; }
        public IReadOnlyCollection<BuiltStatement>? StatementList { get; }
    }
}