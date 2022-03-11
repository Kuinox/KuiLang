using System.Collections.Generic;

namespace KuiLang
{
    public class MethodDeclaration
    {
        public SignatureDeclaration Signature { get; set; }
        public List<Statement> Statements { get; set; }

        public MethodDeclaration(SignatureDeclaration signature, List<Statement> statements)
        {
            Signature = signature;
            Statements = statements;
        }
    }
}