using FluentAssertions;
using KuiLang.Runner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuiLang.Tests
{
    public class RunTests
    {
        [Test]
        public void ReturnTest()
        {
            ScriptHelpers.RunScript("return 1;").Should().Be(1);
        }

        [Test]
        public void VariableTest()
        {
            ScriptHelpers.RunScript(
@"number foo = 42;
return 42;"
            ).Should().Be(42);
        }

        [Test]
        public void FunctionTest()
        {
            ScriptHelpers.RunScript(
@"
number aFunction() { return 12; }
return aFunction();"
            ).Should().Be(12);
        }

        [Test]
        public void OperatorTest()
        {
            ScriptHelpers.RunScript( @"return 12*4;" ).Should().Be(48);
        }

        [Test]
        public void IfTest()
        {
            ScriptHelpers.RunScript(@"if(1) { return 2; } return 1;").Should().Be(2);
        }

        [Test]
        public void StackOverflow()
        {
            ScriptHelpers.RunScript(
@"
number StackOverflow() {StackOverflow();}
StackOverflow();
"
);
        }
    }
}
