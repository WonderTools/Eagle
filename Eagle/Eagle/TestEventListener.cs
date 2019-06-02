using System;
using NUnit.Engine;

namespace Eagle
{
    public class TestEventListener : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            Console.WriteLine(report);    
        }
    }
}