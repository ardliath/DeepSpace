using NUnit.Framework;
using System;

namespace DeepSpace.Tests
{
    public class TestClass
    {
        [Test]
        public void This_test_will_pass()
        {
            Assert.AreEqual(1, 1);
        }

        [Test]
        public void This_test_will_fail()
        {
            Assert.Fail();
        }
    }
}
