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
    }
}
