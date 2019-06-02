using System.Collections.Generic;
using System.Linq;

namespace Eagle
{
    public class TestQueue : ITestQueue
    {
        public int _id = 1;
        private List<ScheduledTest> QueueElements { get; set; } = new List<ScheduledTest>();


        public string AddToQueue(string id)
        {
            var myId = _id.ToString();
            _id++;
            QueueElements.Add(new ScheduledTest(){Id = id, SerialNumber = myId});
            return myId;
        }

        public List<ScheduledTest> GetQueueElements()
        {
            return QueueElements.ToList();
        }

        public ScheduledTest RemoveTopOfQueue()
        {
            if (QueueElements.Count != 0)
            {
                var element = QueueElements.ElementAt(0);
                QueueElements.RemoveAt(0);
                return element;
            }
            return null;
        }
    }
}