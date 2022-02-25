
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace KuiLang.Tests
{
    public class BaseTests
    {
        [Test]
        public void can_parse_full_name()
        {
            var fullname = @"Foo.Bar.Type";
            var res = KuiLang.Runtime.Parse(fullname);
            res.ResultValue.Should().BeEquivalentTo(new string[] { "Foo", "Bar", "Type" });
        }


        [TestCase("Bar methodName()")]
        [TestCase("Foo.Bar methodName(AnArg arg)")]
        public void can_parse_argument(string declaration)
        {
            var res = KuiLang.Runtime.Parse(declaration);

            res.ResultValue.Name.Should().Be("methodName");
        }
    }
}