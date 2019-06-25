using System.Collections.Generic;
using System.Threading.Tasks;
using Eagle.Contract;

namespace Eagle
{
    public interface IResultHandler
    {
        Task OnTestCompletion(List<TestResult> result);
    }
}