
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Farkle;
using Farkle.Builder;

namespace KuiLang.Tests
{
    public class BaseTests
    {

        static readonly RuntimeFarkle<FieldLocation> FullNameRuntime = KuiLang.FullNameDesigntime.Build();
        static readonly RuntimeFarkle<Expression> ExpressionRuntime = KuiLang.ExpressionDesigntime.Build();
        static readonly RuntimeFarkle<SignatureDeclaration> MethodSignatureDeclarationRuntime = KuiLang.MethodSignatureDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<MethodDeclaration> MethodDeclarationRuntime = KuiLang.MethodDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<TypeDeclaration> TypeDeclarationRuntime = KuiLang.TypeDeclarationDesigntime.Build();
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = FullNameRuntime.Parse(fullname);
            res.ResultValue.Parts.Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        [TestCase("Bar methodName()")]
        [TestCase("Foo.Bar methodName(AnArg arg)")]
        public void can_parse_argument(string declaration)
        {
            var res = MethodSignatureDeclarationRuntime.Parse(declaration);

            res.ResultValue.Name.Should().Be("methodName");
        }

        [TestCase("Access.Public void DoNothing() { }")]
        [TestCase("Access.Public void DoSomething() { Something(); }")]
        public void can_parse_method(string method)
        {
            var res = MethodDeclarationRuntime.Parse(method);
            res.IsOk.Should().BeTrue();
            res.ResultValue.Signature.Name.Should().Contain("Do");
        }

        [TestCase(
@"public type Foo
{
    number localField;
    public void Test()
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