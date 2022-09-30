
using FluentAssertions;
using NUnit.Framework;
using Farkle;
using KuiLang.Syntax;
using Farkle.Builder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KuiLang.Tests
{
    public class ParseTests
    {
        static readonly RuntimeFarkle<Identifier> _fullNameRuntime = KuiLang.FullNameDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Expression> _expressionRuntime = KuiLang.ExpressionDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Statement.Definition.Typed.Method> _methodDeclarationRuntime = KuiLang.MethodDeclarationDesigntime.Build();
        static readonly RuntimeFarkle<Ast.Statement.Definition.Type> _typeDeclarationRuntime = KuiLang.TypeDeclarationDesigntime.Build();
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = _fullNameRuntime.Parse( fullname );
            res.ResultValue.Parts.ToArray().Should().BeEquivalentTo( new string[] { "Foo", "Bar", "Type" } );
        }


        [TestCase( "void DoNothing() { }" )]
        [TestCase( "void DoSomething() { Something(); }" )]
        public void can_parse_method( string method )
        {
            var res = _methodDeclarationRuntime.Parse( method );
            res.IsOk.Should().BeTrue();
            //res.ResultValue.Signature.Name.Should().Contain( "Do" );
        }

        [TestCase(
    @"type Foo
{
    void Test()
    {
        doThings();
    }

}" )]
        public void can_parse_type( string type )
        {
            var res = _typeDeclarationRuntime.Parse( type );
            res.IsOk.Should().BeTrue();
        }
    }
}
