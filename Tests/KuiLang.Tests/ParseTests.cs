
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
        static readonly RuntimeFarkle<Ast.Statement.Definition.Method> MethodDeclarationRuntime = KuiLang.MethodDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Statement.Definition.TypeDef> TypeDeclarationRuntime = KuiLang.TypeDeclarationDesigntime.Build();
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = FullNameRuntime.Parse(fullname);
            res.ResultValue.Parts.ToArray().Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        //[TestCase("Bar methodName()")]
        //[TestCase("Foo methodName()")]
        //[TestCase("Foo.Bar methodName(AnArg arg)")]
        //[TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg)")]
        //[TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg1, Foo anotherArg2)")]
        //[TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg2, Foo anotherArg3)")]
        //public void can_parse_argument(string declaration)
        //{
        //    var res = MethodSignatureDeclarationRuntime.Parse(declaration);

        //    res.ResultValue.Name.Should().Be("methodName");
        //}

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