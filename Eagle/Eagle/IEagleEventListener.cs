using System;
using System.Threading.Tasks;

namespace Eagle
{
    public interface IEagleEventListener
    {
        Task TestCompleted(string id, string result, DateTime startingTime, DateTime finishingTime, int duration);
    }
}