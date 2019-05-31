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

    }
}