
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Farkle;
using Farkle.Builder;

namespace KuiLang.Tests
{
    public class ParseTests
    {

        static readonly RuntimeFarkle<FieldLocation> FullNameRuntime = KuiLang.FullNameDesigntime.Build();
        static readonly RuntimeFarkle<Expression> ExpressionRuntime = KuiLang.ExpressionDesigntime.Build();
        static readonly RuntimeFarkle<SignatureDeclaration> MethodSignatureDeclarationRuntime = KuiLang.MethodSignatureDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<MethodDeclaration> MethodDeclarationRuntime = KuiLang.MethodDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<TypeDeclaration> TypeDeclarationRuntime = KuiLang.TypeDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<TypeDeclaration> InterfaceDeclarationRuntime = KuiLang.InterfaceDeclarationDesigntime.Build();
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = FullNameRuntime.Parse(fullname);
            res.ResultValue.Parts.Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        [TestCase("Bar methodName()")]
        [TestCase("Foo methodName()")]
        [TestCase("Foo.Bar methodName(AnArg arg)")]
        [TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg)")]
        [TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg1, Foo anotherArg2)")]
        [TestCase("Foo.Bar methodName(AnArg arg, Foo anotherArg2, Foo anotherArg3)")]
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

        [TestCase(
@"public interface Foo
{
    number aProperty;
    void FooBar();
}
")]
        public void can_parse_interface(string interface_declaration)
        {
            var res = InterfaceDeclarationRuntime.Parse(interface_declaration);
            res.IsOk.Should().BeTrue();
        }
    }
}