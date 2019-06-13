using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eagle.Dashboard.Models;
using Eagle.Dashboard.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Eagle.Dashboard.Database
{
    public class DashboardContext : DbContext
    {

        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {

        }

        public DbSet<DbNode> Nodes { get; set; }
        public DbSet<DbNodePackage> NodePackages { get; set; }
        public DbSet<DbResult> Results { get; set; }
    }


    public class DataStore : IDataStore
    {
        private readonly DashboardContext _context;

        public DataStore(DashboardContext context)
        {
            _context = context;
        }


        public async Task CreateNode(NodeCreationParameters creationParameters)
        {
            _context.Nodes.Add(new DbNode() {NodeName = creationParameters.NodeName, Uri = creationParameters.Uri});
            await _context.SaveChangesAsync();
        }

        public async Task AddRequest(string requestId, string nodeName, string testId, DateTime requestTime, bool isRequestSuccessful)
        {
        }

        public async Task AddDiscoveredTests(string resultNodeName, List<TestSuite> resultTestSuites)
        {
            var package = JsonConvert.SerializeObject(resultTestSuites);

            _context.NodePackages.Add(new DbNodePackage()
            {
                NodeName = resultNodeName,
                Package = package,
            });
            await _context.SaveChangesAsync();
        }



        public async Task AddTestResults(string resultNodeName, List<TestResult> resultTestResults)
        {
            var results = resultTestResults.Select(x => new DbResult()
            {
                Id = x.Id,
                DurationInMs = x.DurationInMs,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Result = x.Result,
            });
            _context.Results.AddRange(results);
            await _context.SaveChangesAsync();
        }

        public async Task<List<List<TestSuite>>> GetLatestTestSuites()
        {
            //var nodePackages = await _context.NodePackages.GroupBy(x => x.NodeName, y => y)
            //    .Select(y => y.ToList().OrderByDescending(a => a.SerialNumber).Select(x => x.Package).FirstOrDefault())
            //    .ToListAsync();

            var r1 = await _context.NodePackages.GroupBy(x => x.NodeName, y => y).ToListAsync();
            var r2 = r1.Select(x => x.ToList()).ToList();
            var r3 = r2.Select(x => x.OrderByDescending(y => y.SerialNumber).FirstOrDefault());
            var r4 = r3.Where(x => x != null).ToList();
            var nodePackages = r4.Select(x => x.Package).ToList();


            var result = nodePackages.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => JsonConvert.DeserializeObject<List<TestSuite>>(x)).ToList();
            return result;
        }

        public Task<List<TestResult>> GetLatestTestResults()
        {
            throw new NotImplementedException();
        }
    }
}