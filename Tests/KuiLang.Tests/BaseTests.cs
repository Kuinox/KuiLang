
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
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = FullNameRuntime.Parse(fullname);
            res.ResultValue.Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        [TestCase("Bar methodName()")]
        [TestCase("Foo.Bar methodName(AnArg arg)")]
        public void can_parse_argument(string declaration)
        {
            var res = MethodSignatureDeclarationRuntime.Parse(declaration);

            res.ResultValue.Name.Should().Be("methodName");
        }
    }
}