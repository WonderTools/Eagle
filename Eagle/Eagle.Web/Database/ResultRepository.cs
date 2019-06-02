using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eagle.Web.Database
{
    public class ResultRepository : IResultRepository
    {
        private readonly ResultContext _resultContext;

        public ResultRepository(ResultContext resultContext)
        {
            _resultContext = resultContext;
        }

        public async Task<List<TestResult>> GetRecentTestResults(string id, int limit)
        {

            var result = _resultContext.TestResults.AsQueryable().Where(x => x.Id == id).OrderByDescending(x => x.SerialNumber)
                .Take(limit);
            return await result.ToListAsync();
        }

        public async Task<List<TestResult>> GetAllRecentTestResults()
        {
            return await _resultContext.TestResults.AsQueryable().GroupBy(x => x.Id)
                .Select(x => x.OrderByDescending(u => u.SerialNumber).FirstOrDefault()).ToListAsync();
        }

        public async Task InsertTestResult(TestResult testResult)
        {
            _resultContext.TestResults.Add(testResult);
            await _resultContext.SaveChangesAsync();
        }
    }
}
