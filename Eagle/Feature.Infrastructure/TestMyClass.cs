using System;
using NUnit.Framework;

namespace Feature.Infrastructure
{
    public class TestMyClass
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void MyTest(int i)
        {

        }

        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }

        [Test]
        public void FailingTest()
        {
            Assert.Fail();
        }

        [Test]
        public void InconclusiveTest()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void FlickeringTests()
        {
            var second = DateTime.Now.Second;
            var x = second / 10;
            if (x % 3 == 0) Assert.Pass();
            if (x % 3 == 1) Assert.Fail();
            if (x % 3 == 2) Assert.Inconclusive();
        }
    }
}