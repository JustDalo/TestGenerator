using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestGeneratorLibrary;

namespace Class2Test
{
    [TestFixture]
    public class Class2
    {
        [Test]
        public void MyMethodTest()
        {
            List<int> list = ;
            Class1 actual = MyMethod(list);
            int expected = 0;
            Assert.That(actual, Is.EqualTo(expected));
            Assert.Fail("autogenerated");
        }
    }
}