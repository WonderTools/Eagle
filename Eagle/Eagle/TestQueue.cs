using System.Collections.Generic;
using System.Linq;

namespace Eagle
{
    public class TestQueue
    {
        public int _id = 1;
        private List<ScheduledFeature> QueueElements { get; set; } = new List<ScheduledFeature>();


        public string Add(string id, string name)
        {
            var myId = _id.ToString();
            _id++;
            QueueElements.Add(new ScheduledFeature(){Id = id, Name = name, TestId = myId});
            return myId;
        }


        public List<ScheduledFeature> GetElements()
        {
            return QueueElements.ToList();
        }

        public ScheduledFeature RemoveTopQueueElement()
        {
            if (QueueElements.Count != 0)
            {
                var element = QueueElements.ElementAt(0);
                QueueElements.RemoveAt(0);
                return element;
            }
            return null;
        }

        public void AdvanceQueueElement(int elementId)
        {

        }

        public void AdvanceQueueElementToTop(int elementId)
        {

        }

        public void DelayQueueElement(int elementId)
        {

        }

        public void DelayQueueElementToBottom(int elementId)
        {
                
        }
    }
}