using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestGeneratorLibrary;
using Moq;

namespace Class3Test
{
    [TestFixture]
    public class Class3Test
    {
        private Mock<IDisposable> _s;
        private Mock<ICloneable> _c;
        private Class3 _class3;
        [SetUp]
        public void SetUp()
        {
            _s = new Mock<IDisposable>();
            _c = new Mock<ICloneable>();
            var a = default(int);
            var str = default(string);
            _class3 = new Class3(_s.Object, _c.Object, a, str);
        }

        [Test]
        public void Method1Test()
        {
            int d = default(int);
            int e = default(int);
            int actual = _class3.Method1(d, e);
            int expected = 0;
            Assert.That(actual, Is.EqualTo(expected));
            Assert.Fail("autogenerated");
        }

        [Test]
        public void Method2Test()
        {
            _class3.Method2();
            Assert.Fail("autogenerated");
        }
    }
}