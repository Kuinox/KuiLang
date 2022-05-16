
using FluentAssertions;
using NUnit.Framework;
using Farkle;
using Farkle.Builder;
using KuiLang.Syntax;

namespace KuiLang.Tests
{
    public class ParseTests
    {

        static readonly RuntimeFarkle<FieldLocation> FullNameRuntime = KuiLang.FullNameDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Expression> ExpressionRuntime = KuiLang.ExpressionDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Statement.Definition.MethodDeclaration> MethodDeclarationRuntime = KuiLang.MethodDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Statement.Definition.TypeDeclaration> TypeDeclarationRuntime = KuiLang.TypeDeclarationDesigntime.Build();
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = FullNameRuntime.Parse(fullname);
            res.ResultValue.Parts.ToArray().Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        [TestCase("void DoNothing() { }")]
        [TestCase("void DoSomething() { Something(); }")]
        public void can_parse_method(string method)
        {
            var res = MethodDeclarationRuntime.Parse(method);
            res.IsOk.Should().BeTrue();
            res.ResultValue.Signature.Name.Should().Contain("Do");
        }

        [TestCase(
@"type Foo
{
    number localField;
    void Test()
    {
        doThings();
    }

}")]
        public void can_parse_type(string type)
        {
            var res = TypeDeclarationRuntime.Parse(type);
            res.IsOk.Should().BeTrue();
        }
    }
}