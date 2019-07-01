using System.Collections.Generic;
using System.Threading.Tasks;
using WonderTools.Eagle.Contract;

namespace WonderTools.Eagle.NUnit
{
    public interface IResultHandler
    {
        Task OnTestCompletion(List<TestResult> result);
    }
}