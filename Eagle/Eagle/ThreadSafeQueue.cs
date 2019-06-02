using System.Collections.Generic;

namespace Eagle
{
    public class ThreadSafeQueue : ITestQueue
    {
        private ITestQueue _testQueue;
        public ThreadSafeQueue()
        {
            _testQueue = new TestQueue();
        }

        public string AddToQueue(string id)
        {
            lock (_testQueue)
            {
                return _testQueue.AddToQueue(id);
            }
        }

        public List<ScheduledTest> GetQueueElements()
        {
            lock (_testQueue)
            {
                return _testQueue.GetQueueElements();
            }
        }

        public ScheduledTest RemoveTopOfQueue()
        {
            lock (_testQueue)
            {
                return _testQueue.RemoveTopOfQueue();
            }
        }
    }
}