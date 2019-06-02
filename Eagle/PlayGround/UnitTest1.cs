using System.Collections.Generic;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        
    }

    public class TestSuite1
    {
        public string FullName { get; set; }

        public TestSuite1[][] TestCase { get; set; }

    }
}