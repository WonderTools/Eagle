using Eagle;
using NUnit.Framework;

namespace Feature.Infrastructure
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {

        }
    }


    public class NewTestSuite
    {
        [TestCase(4)]
        [TestCase(3)]
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        public void TestMe(int r)
        {

        }

        [Test]
        public void GoodTest()
        {
        }
    }

    [FeaturePackage]

    public class ConfigruationMy
    {

    }
}