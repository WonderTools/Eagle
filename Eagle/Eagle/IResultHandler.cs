using System.Collections.Generic;
using System.Threading.Tasks;
using WonderTools.Eagle.Contract;

namespace WonderTools.Eagle
{
    public interface IResultHandler
    {
        Task OnTestCompletion(List<TestResult> result);
    }
}