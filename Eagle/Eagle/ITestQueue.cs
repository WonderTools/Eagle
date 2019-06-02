using System;
using System.Collections.Generic;

namespace Eagle
{
    public interface ITestQueue
    {
        string AddToQueue(string id);
        List<ScheduledTest> GetQueueElements();
        ScheduledTest RemoveTopOfQueue();
    }
}