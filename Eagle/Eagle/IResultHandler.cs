using System.Collections.Generic;
using System.Threading.Tasks;
using WonderTools.Eagle.Contract;

namespace WonderTools.Eagle.Core
{
    public interface IResultHandler
    {
        Task OnTestCompletion(List<TestResult> result);
    }
}