using System;
using NUnit.Engine;

namespace WonderTools.Eagle.Core
{
    public class TestEventListener : ITestEventListener
    {
        public void OnTestEvent(string report)
        {
            Console.WriteLine(report);    
        }
    }
}