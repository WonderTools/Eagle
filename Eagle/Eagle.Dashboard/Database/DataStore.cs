using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WonderTools.Eagle.Contract;
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


        public async Task CreateNode(NodeUpsertParameters creationParameters)
        {
             
            _context.Nodes.Add(new DbNode()
            {
                NodeName = creationParameters.NodeName,
                Uri = creationParameters.Uri,
                ClientSecret = creationParameters.ClientSecret,
                ExecutionIntervalInMinutes = creationParameters.ExecutionIntervalInMinutes
            });
            await _context.SaveChangesAsync();
        }



        public async Task<List<Node>> GetNodes()
        {
            var nodes = _context.Nodes !=null ? 
                _context.Nodes.Select(x => new Node()
                {
                    NodeName = x.NodeName,
                    Uri = x.Uri,
                    ExecutionIntervalInMinutes = x.ExecutionIntervalInMinutes
                }).ToListAsync()
                : null;
            return await nodes;
            
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
                NodeName = resultNodeName,
            });
            _context.Results.AddRange(results);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, List<TestSuite>>> GetLatestTestSuites()
        {
            var r1 = await _context.NodePackages.GroupBy(x => x.NodeName)
                .Select(g => g.OrderByDescending(x => x.SerialNumber).FirstOrDefault()).ToListAsync();

            return r1.ToDictionary(x => x.NodeName, x => JsonConvert.DeserializeObject<List<TestSuite>>(x.Package));
        }

        public async Task<List<TestResult>> GetLatestTestResults()
        {
            var r1 = await  _context.Results.GroupBy(x => x.Id, y=> y).ToListAsync();
            var r2 = r1.Select(x => x.ToList()).ToList();
            var r3 = r2.Select(x => x.OrderByDescending(y => y.SerialNumber).FirstOrDefault());
            var r4 = r3.Where(x => x != null).ToList();

            return r4.Select(x => new TestResult()
            {
                Id = x.Id,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Result = x.Result,
                DurationInMs = x.DurationInMs,
            }).ToList();
        }

        public async Task<string> GetUri(string nodeName)
        {
            return await _context.Nodes.Where(x => x.NodeName == nodeName).Select(x => x.Uri).FirstAsync();
        }

        public async Task<Node> UpdateNode(string nodeName, NodeUpsertParameters nodeUpsertParameters)
        {
            DbNode node = await GetNode(nodeName);

            if (node != null)
            {

                node.NodeName = nodeUpsertParameters.NodeName;
                node.Uri = nodeUpsertParameters.Uri;
                node.ClientSecret = nodeUpsertParameters.ClientSecret;
                node.ExecutionIntervalInMinutes = nodeUpsertParameters.ExecutionIntervalInMinutes;
                //_context.Nodes.Attach(nodechanged);
                _context.Entry(node).State = EntityState.Modified;
                _context.Update(node);
                await _context.SaveChangesAsync();
                
            }
            Node nodechanged = await _context.Nodes.Where(x => x.NodeName == nodeUpsertParameters.NodeName).Select(x => new Node()
            {
                Uri = x.Uri,
                NodeName = x.NodeName,
                ExecutionIntervalInMinutes = x.ExecutionIntervalInMinutes,
            }).FirstAsync();
            return nodechanged;
        }

        private async Task<DbNode> GetNode(string nodeName)
        {
            return await _context.Nodes.FirstOrDefaultAsync(x => x.NodeName == nodeName);
        }

        public async Task<bool> DeleteNode(string nodeName)
        {
            DbNode node = await GetNode(nodeName);
            if (node !=null)
            {
                _context.Remove(node);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}